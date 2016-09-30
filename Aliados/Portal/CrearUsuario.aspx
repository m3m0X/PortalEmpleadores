<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="CrearUsuario.aspx.cs" Inherits="PortalTrabajadores.Portal.CrearUsuario" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Js/jquery-ui.css">
    <script src="../Js/jquery-ui.js"></script>
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
    <asp:UpdateProgress ID="upProgress" DynamicLayout="true" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="loader">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="Container_UpdatePanel1">
                <table id="TablaDatos">
                    <tr>
                        <th colspan="2">Busqueda de Usuario</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblIdentificacion" runat="server" Text="Digite el nombre:" />
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:TextBox ID="txtUser" runat="server" MaxLength="20" CssClass="MarcaAgua"
                                ToolTip="Usuario" placeholder="Nombre de Usuario"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCargo" ControlToValidate="txtUser" 
                                CssClass="MensajeError" Display="Dynamic" 
                                ValidationGroup="busquedaForm" runat="server" ErrorMessage="Digite el nombre de usuario">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="BotonTablaDatos">
                                <asp:Button ID="BtnBuscar" runat="server" 
                                    ValidationGroup="busquedaForm" Text="Buscar" 
                                    OnClick="BtnBuscar_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="Container_UpdatePanel2" runat="server" visible="false">
                <table id="TablaDatos2">
                    <tr>
                        <th colspan="2">Datos del Empleado</th>
                    </tr>
                    <tr class="ColorOscuro">
                        <td><asp:Label ID="lblUsuario" runat="server" Text="Nombre de Usuario:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtUser2" runat="server" MaxLength="100" 
                                CssClass="MarcaAgua" Enabled="false"/>
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblRol" runat="server" Text="Rol:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:DropDownList ID="ddlRol" runat="server">
                                <asp:ListItem Selected="True" Value="3">Admin Nomina</asp:ListItem>
                                <asp:ListItem Value="5">Admin SGD</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblActivo" runat="server" Text="Activo" />
                        </td>                         
                        <td class="CeldaTablaDatos">
                            <asp:CheckBox ID="cbActivo" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="BotonTablaDatos" colspan="2">
                            <asp:Button ID="BtnEditar" runat="server" Text="Guardar Información" OnClick="BtnEditar_Click"/>                        
                            <asp:Button ID="BtnPass" runat="server" Text="Restaurar contraseña" OnClick="BtnPass_Click"/>
                            <asp:Button ID="BtnCancel" runat="server" Text="Cancelar" OnClick="BtnCancel_Click" /></td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar"/>
        </Triggers>         
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Errores" runat="server">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <asp:Label ID="LblMsj" runat="server" Text="LabelMsjError" Visible="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
