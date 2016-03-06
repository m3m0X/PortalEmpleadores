<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="ModificarTercero.aspx.cs" Inherits="PortalTrabajadores.Portal.ModificarTercero" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="Container_UpdatePanel2" runat="server" visible="false">
                <table id="TablaDatos2">
                    <tr>
                        <th colspan="2">Datos del Cliente</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="LblDoc" runat="server" Text="Nit:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="TxtDoc" runat="server"/></td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td>
                            <asp:Label ID="LblNombres" runat="server" Text="Nombre Empresa Cliente:" /></td>
                        <td>
                            <asp:TextBox ID="TxtNombres" runat="server" MaxLength="100" onkeypress="return ValidaSoloLetras(event)" Style="text-transform: uppercase" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblCelular" runat="server" Text="Celular:" /></td>
                        <td>
                            <asp:TextBox ID="TxtCelular" runat="server" MaxLength="10" />
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                Enabled="True" TargetControlID="TxtCelular" FilterType="Numbers" />
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td>
                            <asp:Label ID="LblDir" runat="server" Text="Dirección:" /></td>
                        <td>
                            <asp:TextBox ID="TxtDir" runat="server" MaxLength="100" Style="text-transform: uppercase" /></td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td>
                            <asp:Label ID="LblCorreo" runat="server" Text="Correo:" /></td>
                        <td>
                            <asp:TextBox ID="TxtCorreo" runat="server" MaxLength="100" Style="text-transform: uppercase" /></td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td>
                            <asp:Label ID="LblPass" runat="server" Text="Contraseña:" Visible="false" /></td>
                        <td>
                            <asp:TextBox ID="TxtPass" runat="server" MaxLength="15" Visible="false" TextMode="Password" /><asp:RequiredFieldValidator ID="RequiredContrasena" ValidationGroup="form" ControlToValidate="TxtPass" runat="server" ErrorMessage="Debe Ingresar una contraseña. " ForeColor="#CC3300" Visible="false" Display="Dynamic" /><asp:CustomValidator ID="ValidatorPass" ValidationGroup="form" runat="server" ForeColor="#CC3300" ErrorMessage="Debe ingresar más de 4 carácteres" Visible="false" OnServerValidate="ValidatorPass_ServerValidate" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnEditar" runat="server" Text="Guardar Datos" ValidationGroup="form" OnClick="BtnEditar_Click" /></td>
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnPass" runat="server" Text="Cambiar Contraseña" OnClick="BtnContrasena_Click" /></td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Errores" runat="server">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="LblMsj" runat="server" Text="LabelMsjError" Visible="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
