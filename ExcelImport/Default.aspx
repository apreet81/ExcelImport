<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DemoFile.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <asp:Panel runat="server" ID="FileUploadPanel">
                <div class="row text-center">
                    <h3>Upload Excel data</h3>
                </div>
                <div class="col-xs-12">
                    <div class="col-xs-4">
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                    </div>
                    <div class="col-xs-3">
                        <asp:Button ID="btnUpload" runat="server" Text="Upload"
                            OnClick="btnUpload_Click" />
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="gridPanel" Visible="false">
                <div class="row">
                    <h3>Map excel and Database columns</h3>
                </div>
                <div class="col-xs-12">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                        OnPageIndexChanging="PageIndexChanging" AllowPaging="true" OnRowDataBound="GridView1_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="Columns" HeaderText="Excel Columns" />
                            <asp:TemplateField HeaderText="Database Columns">
                                <ItemTemplate>
                                    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label2" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <br />
                </div>
                <div class="row text-center">
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="successMessagePanel" Visible="false">
                <h3>Data successfully saved to database.</h3>
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Back to upload new data</asp:LinkButton>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
