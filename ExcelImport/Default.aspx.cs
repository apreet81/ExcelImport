using ExcelDataReader;
using ExcelImport;
using ExcelImport.Models;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace DemoFile
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                string FilePath = Server.MapPath(FolderPath + FileName);
                FileUpload1.SaveAs(FilePath);
                GridView1.Caption = FileName;
                GridView1.DataSource = ReadExcel(FilePath);
                GridView1.DataBind();
            }
        }

        private DataTable ReadExcel(string FilePath)
        {
            using (var stream = File.Open(FilePath, FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // Choose one of either 1 or 2:

                    // 1. Use the reader methods
                    do
                    {
                        while (reader.Read())
                        {
                            // reader.GetDouble(0);
                        }
                    } while (reader.NextResult());

                    // 2. Use the AsDataSet extension method
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    ViewState["CurrentTable"] = result.Tables[0];

                    DataTable dtCurrentTable = FlipDataTable(result.Tables[0]);
                    return dtCurrentTable;
                }
            }
        }

        private ArrayList GetDummyData()

        {
            ArrayList arr = new ArrayList();

            arr.Add(new ListItem("Select Column", "0"));
            arr.Add(new ListItem(Constants.DbColumns.Id_Document, "1"));
            arr.Add(new ListItem(Constants.DbColumns.Phone, "2"));
            arr.Add(new ListItem(Constants.DbColumns.Alternative_Id, "3"));
            arr.Add(new ListItem(Constants.DbColumns.Driving_License, "4"));
            arr.Add(new ListItem(Constants.DbColumns.First_Name, "5"));
            arr.Add(new ListItem(Constants.DbColumns.Last_Name, "6"));
            arr.Add(new ListItem(Constants.DbColumns.Sex, "7"));
            arr.Add(new ListItem(Constants.DbColumns.Education, "8"));
            arr.Add(new ListItem(Constants.DbColumns.Marital_Status, "9"));
            arr.Add(new ListItem(Constants.DbColumns.Children, "10"));

            return arr;
        }

        private void FillDropDownList(DropDownList ddl)
        {
            ArrayList arr = GetDummyData();

            foreach (ListItem item in arr)
            {
                ddl.Items.Add(item);
            }
        }

        public static DataTable FlipDataTable(DataTable dt)

        {
            DataTable table = new DataTable();
            table.Columns.Add("Columns");
            table.Columns.Add("Value");
            DataRow dr;

            //get all the columns and make it as rows
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                dr = table.NewRow();
                dr[0] = dt.Columns[j].ToString();
                table.Rows.Add(dr);
            }

            return table;
        }

        protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
            string FileName = GridView1.Caption;
            string FilePath = Server.MapPath(FolderPath + FileName);

            ReadExcel(FilePath);

            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl1 = e.Row.FindControl("DropDownList1") as DropDownList;
                FillDropDownList(ddl1);
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isValid = true;
            foreach (GridViewRow row in GridView1.Rows)
            {
                DropDownList ddl1 = row.FindControl("DropDownList1") as DropDownList;
                DropDownList currentDropdown = (DropDownList)sender;
                Label lbl = (Label)row.FindControl("Label2");
                lbl.Text = string.Empty;
                if (ddl1.SelectedItem.Value == currentDropdown.SelectedItem.Value && ddl1.ClientID != currentDropdown.ClientID)
                {
                    lbl.Text = "Invalid data";
                    isValid = false;
                }
            }
            if (!isValid)
            {
                DataControlFieldCell item = (DataControlFieldCell)((DropDownList)sender).Parent;
                Label lbl = (Label)item.FindControl("Label2");
                lbl.Text = "Invalid data";
            }
        }

        private bool ValidateData()
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                Label lbl = (Label)row.FindControl("Label2");
                if (!string.IsNullOrWhiteSpace(lbl.Text))
                {
                    return false;
                }
            }
            return true;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateData())
                {
                    DataTable actualData = (DataTable)ViewState["CurrentTable"];
                    DataTable gridData = new DataTable();
                    gridData.Columns.Add("ExcelColumn");
                    gridData.Columns.Add("DbColumn");

                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        DropDownList ddl1 = (DropDownList)row.FindControl("DropDownList1");
                        if (ddl1.SelectedItem.Value != "0")
                        {
                            DataRow dr = gridData.NewRow();
                            dr["ExcelColumn"] = row.Cells[0].Text;
                            dr["DbColumn"] = ddl1.SelectedItem.Text;
                            gridData.Rows.Add(dr);
                        }
                    }
                    using (AppDbContext context = new AppDbContext())
                    {
                        foreach (DataRow actualDataRow in actualData.Rows)
                        {
                            ImportedData importedData = new ImportedData();
                            if (gridData.AsEnumerable().Any(g => g.Field<string>("DbColumn") == Constants.DbColumns.Id_Document))
                            {
                                DataRow dataRow = gridData.AsEnumerable().Where(g => g.Field<string>("DbColumn") == Constants.DbColumns.Id_Document).FirstOrDefault();
                                string excelColumn = Convert.ToString(dataRow["ExcelColumn"]);
                                importedData.Id_Document = Convert.ToString(actualDataRow[excelColumn]);
                            }
                            if (gridData.AsEnumerable().Any(g => g.Field<string>("DbColumn") == Constants.DbColumns.Phone))
                            {
                                DataRow dataRow = gridData.AsEnumerable().Where(g => g.Field<string>("DbColumn") == Constants.DbColumns.Phone).FirstOrDefault();
                                string excelColumn = Convert.ToString(dataRow["ExcelColumn"]);
                                importedData.Phone = Convert.ToString(actualDataRow[excelColumn]);
                            }
                            if (gridData.AsEnumerable().Any(g => g.Field<string>("DbColumn") == Constants.DbColumns.Alternative_Id))
                            {
                                DataRow dataRow = gridData.AsEnumerable().Where(g => g.Field<string>("DbColumn") == Constants.DbColumns.Alternative_Id).FirstOrDefault();
                                string excelColumn = Convert.ToString(dataRow["ExcelColumn"]);
                                importedData.Alternative_Id = Convert.ToString(actualDataRow[excelColumn]);
                            }
                            if (gridData.AsEnumerable().Any(g => g.Field<string>("DbColumn") == Constants.DbColumns.Driving_License))
                            {
                                DataRow dataRow = gridData.AsEnumerable().Where(g => g.Field<string>("DbColumn") == Constants.DbColumns.Driving_License).FirstOrDefault();
                                string excelColumn = Convert.ToString(dataRow["ExcelColumn"]);
                                importedData.Driving_License = Convert.ToString(actualDataRow[excelColumn]);
                            }
                            if (gridData.AsEnumerable().Any(g => g.Field<string>("DbColumn") == Constants.DbColumns.First_Name))
                            {
                                DataRow dataRow = gridData.AsEnumerable().Where(g => g.Field<string>("DbColumn") == Constants.DbColumns.First_Name).FirstOrDefault();
                                string excelColumn = Convert.ToString(dataRow["ExcelColumn"]);
                                importedData.First_Name = Convert.ToString(actualDataRow[excelColumn]);
                            }
                            if (gridData.AsEnumerable().Any(g => g.Field<string>("DbColumn") == Constants.DbColumns.Last_Name))
                            {
                                DataRow dataRow = gridData.AsEnumerable().Where(g => g.Field<string>("DbColumn") == Constants.DbColumns.Last_Name).FirstOrDefault();
                                string excelColumn = Convert.ToString(dataRow["ExcelColumn"]);
                                importedData.Last_Name = Convert.ToString(actualDataRow[excelColumn]);
                            }
                            if (gridData.AsEnumerable().Any(g => g.Field<string>("DbColumn") == Constants.DbColumns.Sex))
                            {
                                DataRow dataRow = gridData.AsEnumerable().Where(g => g.Field<string>("DbColumn") == Constants.DbColumns.Sex).FirstOrDefault();
                                string excelColumn = Convert.ToString(dataRow["ExcelColumn"]);
                                importedData.Sex = Convert.ToString(actualDataRow[excelColumn]);
                            }
                            if (gridData.AsEnumerable().Any(g => g.Field<string>("DbColumn") == Constants.DbColumns.Education))
                            {
                                DataRow dataRow = gridData.AsEnumerable().Where(g => g.Field<string>("DbColumn") == Constants.DbColumns.Education).FirstOrDefault();
                                string excelColumn = Convert.ToString(dataRow["ExcelColumn"]);
                                importedData.Education = Convert.ToString(actualDataRow[excelColumn]);
                            }
                            if (gridData.AsEnumerable().Any(g => g.Field<string>("DbColumn") == Constants.DbColumns.Marital_Status))
                            {
                                DataRow dataRow = gridData.AsEnumerable().Where(g => g.Field<string>("DbColumn") == Constants.DbColumns.Marital_Status).FirstOrDefault();
                                string excelColumn = Convert.ToString(dataRow["ExcelColumn"]);
                                importedData.Marital_Status = Convert.ToString(actualDataRow[excelColumn]);
                            }
                            if (gridData.AsEnumerable().Any(g => g.Field<string>("DbColumn") == Constants.DbColumns.Children))
                            {
                                DataRow dataRow = gridData.AsEnumerable().Where(g => g.Field<string>("DbColumn") == Constants.DbColumns.Children).FirstOrDefault();
                                string excelColumn = Convert.ToString(dataRow["ExcelColumn"]);
                                importedData.Children = Convert.ToString(actualDataRow[excelColumn]);
                            }
                            context.ImportedDatas.Add(importedData);
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}