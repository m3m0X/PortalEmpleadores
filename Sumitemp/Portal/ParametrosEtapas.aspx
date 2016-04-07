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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="Container_UpdatePanel1">
                <table id="TablaDatos">
                    <tr>
                        <th colspan="2">Seleccione el año a parametrizar</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblAnio" runat="server" Text="Año:" />
                        </td>                        
                        <td class="BotonTablaDatos">
                            <asp:DropDownList ID="ddlAnio" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblEtapasDdl" runat="server" Text="Etapas:" />
                        </td>                        
                        <td class="BotonTablaDatos">
                            <asp:DropDownList ID="ddlEtapas" runat="server" DataSourceID="sqlEtapas" DataTextField="Etapa" DataValueField="idEtapas"></asp:DropDownList>
                            <asp:SqlDataSource ID="sqlEtapas" runat="server" ConnectionString='<%$ ConnectionStrings:CadenaConexioMySql3 %>' ProviderName='<%$ ConnectionStrings:CadenaConexioMySql3.ProviderName %>' SelectCommand="SELECT idEtapas, Etapa FROM etapas"></asp:SqlDataSource>
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
                <table id="TablaDatos2">
                    <tr>
                        <th colspan="5">Parametros Fecha Etapas</th>
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
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblEtapa" runat="server" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="jqCalendar"></asp:TextBox>
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="jqCalendar"></asp:TextBox>
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtCorteInicio" runat="server" CssClass="jqCalendar"></asp:TextBox>
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtCorteFin" runat="server" CssClass="jqCalendar"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="BotonTablaDatos" colspan="4"></td>
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnEditar" runat="server" Text="Guardar" OnClick="BtnEditar_Click"/>
                            <asp:Button ID="BtnCancel" runat="server" Text="Cancelar" OnClick="BtnCancel_Click"/></td>
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
