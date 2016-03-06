<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="Laboral.aspx.cs" Inherits="PortalTrabajadores.Portal.Laboral" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Css para la fecha -->
    <link href="../CSS/CSSCallapsePanel.css" rel="stylesheet" type="text/css" />
    <!-- Js De Los campos de Textos -->
    <script src="../Js/funciones.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContainerTitulo" runat="server">
     <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTitulo" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Container" runat="server">
    <div id="Container_UpdatePanel1">
        <table id="TablaDatos">
            <tr>
                <th colspan="2">Busqueda de Usuario</th>
            </tr>
            <tr>
                <td class="CeldaTablaDatos">
                    <asp:Label ID="LblCambio" runat="server" Text="Digite Identificación:" />
                </td>
                <td class="BotonTablaDatos">
                    <asp:TextBox ID="txtuser" runat="server" MaxLength="20" CssClass="MarcaAgua"
                        ToolTip="Usuario" placeholder="Identificación"  onkeypress="return ValidaSoloNumeros(event)"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="BotonTablaDatos">
                        <asp:Button ID="BtnBuscar" runat="server" Text="Buscar" OnClick="BtnBuscar_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>