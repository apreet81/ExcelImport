<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default2.aspx.cs" Inherits="ExcelImport.Default2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Untitled Page</title>
</head>

<body>

    <form id="form1" runat="server">

        <div>

            <asp:GridView ID="Gridview1" runat="server" ShowFooter="true" AutoGenerateColumns="false">

                <Columns>

                    <asp:BoundField DataField="RowNumber" HeaderText="Row Number" />

                    <asp:TemplateField HeaderText="Header 1">

                        <ItemTemplate>

                            <asp:DropDownList ID="DropDownList1" runat="server" AppendDataBoundItems="true">

                                <asp:ListItem Value="-1">Select</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Header 2">

                        <ItemTemplate>

                            <asp:DropDownList ID="DropDownList2" runat="server" AppendDataBoundItems="true">

                                <asp:ListItem Value="-1">Select</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Header 3">

                        <ItemTemplate>

                            <asp:DropDownList ID="DropDownList3" runat="server" AppendDataBoundItems="true">

                                <asp:ListItem Value="-1">Select</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>

                        <FooterStyle HorizontalAlign="Right" />

                        <FooterTemplate>

                            <asp:Button ID="ButtonAdd" runat="server" Text="Add New Row"
                                OnClick="ButtonAdd_Click" />
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>