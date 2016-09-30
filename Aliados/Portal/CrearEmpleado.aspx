<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="CrearEmpleado.aspx.cs" Inherits="PortalTrabajadores.Portal.CrearEmpleado" %>
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
                            <asp:Label ID="lblIdentificacion" runat="server" Text="Digite Identificación:" />
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:TextBox ID="txtUser" runat="server" MaxLength="20" CssClass="MarcaAgua"
                                ToolTip="Usuario" placeholder="Identificación"  onkeypress="return ValidaSoloNumeros(event)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCargo" ControlToValidate="txtUser" 
                                CssClass="MensajeError" Display="Dynamic" 
                                ValidationGroup="busquedaForm" runat="server" ErrorMessage="Digite una cedula"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="BotonTablaDatos">
                                <asp:Button ID="BtnBuscar" runat="server" ValidationGroup="busquedaForm" Text="Buscar" OnClick="BtnBuscar_Click" />
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
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblId" runat="server" Text="Tipo de documento y número" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:DropDownList ID="ddlTipoDocumento" runat="server" DataSourceID="sqlTipoDoc" DataTextField="Nombre_TipoID" DataValueField="idTipoIdentificacion"></asp:DropDownList>
                            <asp:SqlDataSource ID="sqlTipoDoc" runat="server" ConnectionString='<%$ ConnectionStrings:CadenaConexioMySql %>' ProviderName='<%$ ConnectionStrings:CadenaConexioMySql.ProviderName %>' SelectCommand="SELECT idTipoIdentificacion, Nombre_TipoID FROM tipoidentificacion WHERE (Activo = 1) AND (idTipoIdentificacion <> 1)"></asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtUser2" runat="server" MaxLength="100" 
                                onkeypress="return ValidaSoloNumeros(event)"
                                CssClass="MarcaAgua"
                                placeholder="Número" Enabled="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos"><asp:Label ID="lblExpedicion" runat="server" Text="Lugar de Expedición:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtExpedicion" runat="server" MaxLength="100" onkeypress="return ValidaSoloLetras(event)" Style="text-transform: uppercase" />
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos"><asp:Label ID="lblNombres" runat="server" Text="Nombres:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtNombres" runat="server" MaxLength="100" onkeypress="return ValidaSoloLetras(event)" Style="text-transform: uppercase" />
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos"></td>
                        <td class="CeldaTablaDatos">
                            <asp:RequiredFieldValidator ID="rfvNombres" 
                                ControlToValidate="txtNombres" CssClass="MensajeError" 
                                Display="Dynamic" ValidationGroup="userForm" 
                                runat="server" ErrorMessage="Digite Nombre"></asp:RequiredFieldValidator>
                        </td>
                    </tr>                    
                    <tr>
                        <td class="CeldaTablaDatos"><asp:Label ID="lblApellidos" runat="server" Text="Apellidos:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtPrimerApellido" runat="server" MaxLength="100" 
                                onkeypress="return ValidaSoloLetras(event)" 
                                Style="text-transform: uppercase" 
                                CssClass="MarcaAgua"
                                placeholder="Primer Apellido"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos"></td>
                        <td class="CeldaTablaDatos">
                            <asp:RequiredFieldValidator ID="rfvPrimerApellido" 
                                ControlToValidate="txtPrimerApellido" CssClass="MensajeError" 
                                Display="Dynamic" ValidationGroup="userForm" 
                                runat="server" ErrorMessage="Digite Primer Apellido"></asp:RequiredFieldValidator>
                        </td>
                    </tr> 
                    <tr>
                        <td></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtSegundoApellido" runat="server" MaxLength="100" 
                                onkeypress="return ValidaSoloLetras(event)" 
                                Style="text-transform: uppercase" 
                                CssClass="MarcaAgua"
                                placeholder="Segundo Apellido"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos"></td>
                        <td class="CeldaTablaDatos">
                            <asp:RequiredFieldValidator ID="rfvSegundoApellido" 
                                ControlToValidate="txtPrimerApellido" CssClass="MensajeError" 
                                Display="Dynamic" ValidationGroup="userForm" 
                                runat="server" ErrorMessage="Digite Segundo Apellido"></asp:RequiredFieldValidator>
                        </td>
                    </tr> 
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblSexo" runat="server" Text="Sexo:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:DropDownList ID="ddlSexo" runat="server">
                                <asp:ListItem Selected="True" Value="M">Masculino</asp:ListItem>
                                <asp:ListItem Value="F">Femenino</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblFechaNacimiento" runat="server" Text="Fecha de nacimiento:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="jqCalendar" onkeypress="return ValidaSoloNumerosFecha(event)"></asp:TextBox>                            
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos"></td>
                        <td class="CeldaTablaDatos">
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtFechaNacimiento"
                                ValidationExpression="\d{4}(?:/\d{1,2}){2}" Display="Dynamic"
                                CssClass="MensajeError" ErrorMessage="Formato Invalido." 
                                ValidationGroup="userForm" />
                        </td>
                    </tr> 
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos"><asp:Label ID="lblDireccion" runat="server" Text="Dirección:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtDireccion" runat="server" MaxLength="100" 
                                Style="text-transform: uppercase" 
                                CssClass="MarcaAgua"
                                placeholder="Dirección"/>
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtBarrio" runat="server" MaxLength="100" 
                                Style="text-transform: uppercase" 
                                CssClass="MarcaAgua"
                                placeholder="Barrio"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos"><asp:Label ID="lblTelefono" runat="server" Text="Teléfono:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtTelefono" runat="server" MaxLength="10" />
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                Enabled="True" TargetControlID="txtTelefono" FilterType="Numbers" />
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos"><asp:Label ID="lblCelular" runat="server" Text="Celular:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtCelular" runat="server" MaxLength="10" />
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                Enabled="True" TargetControlID="TxtCelular" FilterType="Numbers" />
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblCorreo" runat="server" Text="Correo:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtCorreo" runat="server" MaxLength="100" Style="text-transform: uppercase" />                            
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos"></td>
                        <td class="CeldaTablaDatos">
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCorreo"
                                ValidationExpression="^[a-z0-9][-a-z0-9.!#$%&'*+-=?^_`{|}~\/]+@([-a-z0-9]+\.)+[a-z]{2,5}$" 
                                Display="Dynamic" CssClass="MensajeError" 
                                ErrorMessage="Formato Invalido." ValidationGroup="userForm" />
                        </td>
                    </tr> 
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos"><asp:Label ID="lblEstadoCivil" runat="server" Text="Estado Civil:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:DropDownList ID="ddlEstadosCiviles" runat="server" DataSourceID="sdsEstadosCiviles" DataTextField="Nombre_EstadoCivil" DataValueField="idEstadosCiviles"></asp:DropDownList>
                            <asp:SqlDataSource ID="sdsEstadosCiviles" runat="server" ConnectionString='<%$ ConnectionStrings:CadenaConexioMySql %>' ProviderName='<%$ ConnectionStrings:CadenaConexioMySql.ProviderName %>' SelectCommand="SELECT idEstadosCiviles, Nombre_EstadoCivil FROM estadosciviles WHERE (Activo = 1)"></asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblEPS" runat="server" Text="EPS:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtEPS" runat="server" MaxLength="100" Style="text-transform: uppercase" /></td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblAFP" runat="server" Text="AFP:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtAFP" runat="server" MaxLength="100" Style="text-transform: uppercase" /></td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblCesantias" runat="server" Text="Cesantias:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtCesantias" runat="server" MaxLength="100" Style="text-transform: uppercase" /></td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblArea" runat="server" Text="Área" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:DropDownList ID="ddlArea" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblCargo" runat="server" Text="Cargo" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:DropDownList ID="ddlCargo" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                     <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblJefe" runat="server" Text="Rol Jefe" />
                        </td>                         
                        <td class="CeldaTablaDatos">
                            <asp:CheckBox ID="cbJefe" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnEditar" runat="server" Text="Guardar Información" ValidationGroup="userForm" OnClick="BtnEditar_Click"/></td>
                        <td class="BotonTablaDatos">
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