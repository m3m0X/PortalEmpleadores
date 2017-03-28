<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="PeriodoExtra.aspx.cs" Inherits="PortalTrabajadores.Portal.PeriodoExtra" %>

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
            <div id="Container_UpdatePanel1" runat="server" visible="true">
                <table id="TablaDatos">
                    <tr>
                        <th colspan="2">Busqueda Jefe/Empleado</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblJefe" runat="server" Text="Digite Identificación Jefe:" />
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:TextBox ID="txtJefe" runat="server" MaxLength="20" CssClass="MarcaAgua"
                                ToolTip="Usuario" placeholder="Identificación"  onkeypress="return ValidaSoloNumeros(event)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos"></td>
                        <td class="BotonTablaDatos">
                            <asp:RequiredFieldValidator ID="rfvJefe" ControlToValidate="txtJefe" 
                                CssClass="MensajeError" Display="Dynamic" 
                                ValidationGroup="busquedaForm" runat="server" ErrorMessage="Digite una cedula"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblUser" runat="server" Text="Digite Identificación Trabajador:" />
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:TextBox ID="txtUser" runat="server" MaxLength="20" CssClass="MarcaAgua"
                                ToolTip="Usuario" placeholder="Identificación"  onkeypress="return ValidaSoloNumeros(event)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos"></td>
                        <td class="BotonTablaDatos">
                            <asp:RequiredFieldValidator ID="rfvTrabajador" ControlToValidate="txtUser" 
                                CssClass="MensajeError" Display="Dynamic" 
                                ValidationGroup="busquedaForm" runat="server" ErrorMessage="Digite una cedula"></asp:RequiredFieldValidator>
                        </td>
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
                        <td colspan="2" class="BotonTablaDatos">
                            <asp:Button ID="BtnBuscar" runat="server" Text="Buscar" ValidationGroup="busquedaForm" OnClick="BtnBuscar_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="gvJefeEmpleado" runat="server" AutoGenerateColumns="false" OnRowCommand="gvJefeEmpleado_RowCommand">
                    <AlternatingRowStyle CssClass="ColorOscuro" />
                    <Columns>
                        <asp:BoundField DataField="Cedula_Empleado" HeaderText="Cedula" />
                        <asp:BoundField DataField="Nombres_Completos_Empleado" HeaderText="Nombre Empleado" />
                        <asp:TemplateField HeaderText="Generar" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnPeriodo" runat="server" ImageUrl="~/Img/calendar.gif" CommandArgument='<%#Eval("idJefeEmpleado")%>' CommandName="Periodos" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="Container_UpdatePanel2" runat="server" visible="false">
                <table id="TablaDatos2">
                    <tr>
                        <th colspan="2">Parametros Periodo Extra</th>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblFechaInicio" runat="server" Text="Fecha Inicio" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="jqCalendar" onkeypress="return ValidaSoloNumerosFecha(event)"></asp:TextBox>
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
                    </tr>
                    <tr>                        
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblFechaFin" runat="server" Text="Fecha Fin" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="jqCalendar" onkeypress="return ValidaSoloNumerosFecha(event)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos"></td>
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
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnGuardar" runat="server" Text="Guardar" ValidationGroup="objForm" OnClick="BtnGuardar_Click"/>
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnCancel" runat="server" Text="Cancelar" OnClick="BtnCancel_Click"/>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnBuscar"/>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Errores" runat="server">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <asp:Label ID="LblMsj" runat="server" Text="" Visible="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
