<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="PortalTrabajadores.Portal.index1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Container" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="Container_UpdatePanel1">
                <table id="TablaDatos">
                    <tr>
                        <th colspan="2">Seleccione el Proyecto</th>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="DropListProyecto" runat="server" Height="22px" ></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                         <td>
                                <asp:Button ID="BtnProyectos" runat="server" Text="Seleccionar" OnClick="BtnProyectos_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnProyectos" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>