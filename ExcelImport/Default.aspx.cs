using ExcelDataReader;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
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

                ReadExcel(FilePath);
            }
        }

        private void ReadExcel(string FilePath)
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

                    //GridView1.Caption = Path.GetFileName(FilePath);

                    DataTable dtCurrentTable = FlipDataTable(result.Tables[0]);

                    GridView1.DataSource = dtCurrentTable;

                    GridView1.DataBind();

                    // The result of each spreadsheet is in result.Tables


                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)

                    {
                        //extract the DropDownList Selected Items

                        DropDownList ddl1 = (DropDownList)GridView1.Rows[i].Cells[1].FindControl("DropDownList1");

                        FillDropDownList(ddl1);
                    }
                }
            }
        }

        private ArrayList GetDummyData()

        {
            ArrayList arr = new ArrayList();

            arr.Add(new ListItem("Item1", "1"));

            arr.Add(new ListItem("Item2", "2"));

            arr.Add(new ListItem("Item3", "3"));

            arr.Add(new ListItem("Item4", "4"));

            arr.Add(new ListItem("Item5", "5"));

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

            //Get all the rows and change into columns

            //for (int i = 0; i <= dt.Rows.Count; i++)

            //{

            //    table.Columns.Add(Convert.ToString(i));

            //}
            table.Columns.Add("Columns");
            table.Columns.Add("Value");
            DataRow dr;

            //get all the columns and make it as rows

            for (int j = 0; j < dt.Columns.Count; j++)

            {

                dr = table.NewRow();

                dr[0] = dt.Columns[j].ToString();

                //for (int k = 1; k <= dt.Rows.Count; k++)

                //    dr[k] = dt.Rows[k - 1][j];

                table.Rows.Add(dr);

            }



            return table;

        }
        protected void PageIndexChanging(object sender, GridViewPageEventArgs e)

        {
            string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

            string FileName = GridView1.Caption;

            string Extension = Path.GetExtension(FileName);

            string FilePath = Server.MapPath(FolderPath + FileName);

            ReadExcel(FilePath);

            GridView1.PageIndex = e.NewPageIndex;

            GridView1.DataBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}