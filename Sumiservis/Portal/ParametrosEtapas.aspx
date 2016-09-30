<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="ParametrosEtapas.aspx.cs" Inherits="PortalTrabajadores.Portal.ParametrosEtapas" %>

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
            <div id="Container_UpdatePanel1" runat="server">
                <table id="TablaDatos">
                    <tr>
                        <th colspan="2">Seleccione el año a parametrizar</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblAnio" runat="server" Text="Año:" />
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:DropDownList ID="ddlAnio" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAnio_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblEtapasDdl" runat="server" Text="Etapas:" />
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:DropDownList ID="ddlEtapas" runat="server"></asp:DropDownList>                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="BotonTablaDatos">
                            <asp:Button ID="BtnBuscar" runat="server" Text="Buscar" OnClick="BtnBuscar_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="Container_UpdatePanel2" runat="server" visible="false">
                <p></p>
                <table id="TablaDatos2">
                    <tr>
                        <th colspan="6">Parametros Fecha Etapas</th>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblTituloEtapas" runat="server" Text="Etapa" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblFechaInicio" runat="server" Text="Fecha Inicio" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblFechaFin" runat="server" Text="Fecha Fin" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblCorteInicio" runat="server" Text="Corte Inicio" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblCorteFin" runat="server" Text="Corte Fin" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblLimite" runat="server" Text="Limite Asignación" />
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblEtapa" runat="server" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="jqCalendar" onkeypress="return ValidaSoloNumerosFecha(event)"></asp:TextBox>
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="jqCalendar" onkeypress="return ValidaSoloNumerosFecha(event)"></asp:TextBox>
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtCorteInicio" runat="server" CssClass="jqCalendar" onkeypress="return ValidaSoloNumerosFecha(event)"></asp:TextBox>
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtCorteFin" runat="server" CssClass="jqCalendar" onkeypress="return ValidaSoloNumerosFecha(event)"></asp:TextBox>
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtLimite" runat="server" CssClass="jqCalendar" onkeypress="return ValidaSoloNumerosFecha(event)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos"></td>
                        <td class="CeldaTablaDatos">
                            <asp:RequiredFieldValidator ID="rfvFecha1" ControlToValidate="txtFechaInicio"
                                CssClass="MensajeError" Display="Dynamic"
                                ValidationGroup="objForm" runat="server"
                                ErrorMessage="Ingrese una fecha"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtFechaInicio"
                                ValidationExpression="\d{4}(?:/\d{1,2}){2}" Display="Dynamic"
                                CssClass="MensajeError" ErrorMessage="Formato Invalido." 
                                ValidationGroup="objForm" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:RequiredFieldValidator ID="rfvFecha2" ControlToValidate="txtFechaFin"
                                CssClass="MensajeError" Display="Dynamic"
                                ValidationGroup="objForm" runat="server"
                                ErrorMessage="Ingrese una fecha"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtFechaFin"
                                ValidationExpression="\d{4}(?:/\d{1,2}){2}" Display="Dynamic"
                                CssClass="MensajeError" ErrorMessage="Formato Invalido." 
                                ValidationGroup="objForm" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:RequiredFieldValidator ID="rfvFecha3" ControlToValidate="txtCorteInicio"
                                CssClass="MensajeError" Display="Dynamic"
                                ValidationGroup="objForm" runat="server"
                                ErrorMessage="Ingrese una fecha"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCorteInicio"
                                ValidationExpression="\d{4}(?:/\d{1,2}){2}" Display="Dynamic"
                                CssClass="MensajeError" ErrorMessage="Formato Invalido." 
                                ValidationGroup="objForm" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:RequiredFieldValidator ID="rfvFecha4" ControlToValidate="txtCorteFin"
                                CssClass="MensajeError" Display="Dynamic"
                                ValidationGroup="objForm" runat="server"
                                ErrorMessage="Ingrese una fecha"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCorteFin"
                                ValidationExpression="\d{4}(?:/\d{1,2}){2}" Display="Dynamic"
                                CssClass="MensajeError" ErrorMessage="Formato Invalido." 
                                ValidationGroup="objForm" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:RequiredFieldValidator ID="rfvFecha5" ControlToValidate="txtLimite"
                                CssClass="MensajeError" Display="Dynamic"
                                ValidationGroup="objForm" runat="server"
                                ErrorMessage="Ingrese una fecha"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtLimite"
                                ValidationExpression="\d{4}(?:/\d{1,2}){2}" Display="Dynamic"
                                CssClass="MensajeError" ErrorMessage="Formato Invalido." 
                                ValidationGroup="objForm" />
                        </td>
                    </tr>
                    <tr>
                        <td class="BotonTablaDatos" colspan="5"></td>
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnEditar" runat="server" Text="Guardar" ValidationGroup="objForm" OnClick="BtnEditar_Click" />
                            <asp:Button ID="BtnCancel" runat="server" Text="Cancelar" OnClick="BtnCancel_Click" /></td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
            <asp:AsyncPostBackTrigger ControlID="ddlAnio" EventName="selectedindexchanged" />            
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
