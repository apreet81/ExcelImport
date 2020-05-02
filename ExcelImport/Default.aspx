<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DemoFile.Default" %>

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

            <asp:Label ID="Label1" runat="server" Text="Has Header ?" />

            <asp:RadioButtonList ID="rbHDR" runat="server">

                <asp:ListItem Text="Yes" Value="Yes" Selected="True">
                </asp:ListItem>

                <asp:ListItem Text="No" Value="No"></asp:ListItem>
            </asp:RadioButtonList>

            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                OnPageIndexChanging="PageIndexChanging" AllowPaging="true" OnRowDataBound="GridView1_RowDataBound">
                 <Columns>
                      <asp:BoundField DataField="Columns" HeaderText="Level" />
                    <asp:TemplateField HeaderText="Qualification">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:DropDownList ID="DropDownList1" runat="server">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
