<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ExcelImport.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <asp:FileUpload ID="FileUpload1" runat="server" />

            <asp:Button ID="btnUpload" runat="server" Text="Upload"
                OnClick="btnUpload_Click" />

            <br />
            <asp:GridView ID="GridView1" runat="server"
                OnPageIndexChanging="PageIndexChanging" AllowPaging="true">
            </asp:GridView>
        </div>
    </form>
</body>
</html>