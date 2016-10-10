<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="ParametrosObjetivos.aspx.cs" Inherits="PortalTrabajadores.Portal.ParametrosObjetivos" %>

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
                        <th colspan="2">Seleccione el año a parametrizar</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblAnio" runat="server" Text="Año:"/>
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:DropDownList ID="ddlAnio" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="BotonTablaDatos">
                            <asp:Button ID="BtnCrear" runat="server" Text="Crear" OnClick="BtnCrear_Click" />
                            <asp:Button ID="BtnBuscar" runat="server" Text="Consultar" OnClick="BtnBuscar_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="Container_UpdatePanel2" runat="server" visible="false">
                <table id="TablaDatos2">
                    <tr class="ColorOscuro">
                        <th colspan="2">Parametros Objetivos</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblAño" runat="server" Text="Año:" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtAno" runat="server"
                                MaxLength="4" onkeypress="return ValidaSoloNumeros(event)"></asp:TextBox>
                        </td>                        
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblMin" runat="server" Text="Cantidad Minima de Objetivos" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:DropDownList ID="ddlMin" runat="server">
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblMax" runat="server" Text="Cantidad Máxima de Objetivos" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:DropDownList ID="ddlMax" runat="server">
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblSeguimiento" runat="server" Text="Periodo" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:DropDownList ID="ddlSeguimiento" runat="server">
                                <asp:ListItem Value="0" Selected="True">---Seleccione una opción---</asp:ListItem>
                                <asp:ListItem Value="1">Anual</asp:ListItem>
                                <asp:ListItem Value="2">Semestral</asp:ListItem>
                                <asp:ListItem Value="3">Trimestral</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>             
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblActivo" runat="server" Text="Activo" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:CheckBox ID="cbActivo" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos" colspan="2">
                            <asp:CompareValidator ID="cValidator" 
                                runat="server" 
                                ErrorMessage="CompareValidator"
                                ControlToValidate="ddlMin"
                                ControlToCompare="ddlMax"
                                CssClass="MensajeError" 
                                Display="Dynamic"
                                Operator="LessThanEqual"
                                Type="Integer"
                                Text="Error: El objetivo min no puede ser mayor a max"
                                ValidationGroup="objForm">
                            </asp:CompareValidator>
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnEditar" runat="server" Text="Guardar" ValidationGroup="objForm" OnClick="BtnEditar_Click" /></td>
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnCancel" runat="server" Text="Cerrar" OnClick="BtnCancel_Click" /></td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
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
