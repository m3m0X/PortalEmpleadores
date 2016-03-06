<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="Actualizadatos.aspx.cs" Inherits="PortalTrabajadores.Portal.Actualizadatos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Css para la fecha -->
    <link href="../CSS/CSSCallapsePanel.css" rel="stylesheet" type="text/css" />
    <!-- Js De Los campos de Textos -->
    <script src="../Js/funciones.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContainerTitulo" runat="server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTitulo" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Container" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="Container_UpdatePanel1">
                <table id="TablaDatos">
                    <tr>
                        <th colspan="2">Datos a actualizar</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos"><asp:Label ID="LblDoc" runat="server" Text="Documento:" /></td>
                        <td class="CeldaTablaDatos"><asp:Label ID="TxtDoc" runat="server"/></td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td><asp:Label ID="LblExp" runat="server" Text="Expedicion Documento:" /></td>
                        <td><asp:Label ID="TxtExp" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="LblFechNac" runat="server" Text="Fecha Nacimiento:" /></td>
                        <td><asp:TextBox ID="TxtFechNac" runat="server" Style="text-transform: uppercase" />
                            <asp:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="TxtFechNac" ControlExtender="TxtFechNac_MaskedEditExtender" IsValidEmpty="false" ForeColor="#CC3300" EmptyValueMessage="*Ingrese una fecha" InvalidValueMessage="*Formato de Fecha No Válido: yyyy/MM/dd"></asp:MaskedEditValidator>
                            <asp:MaskedEditExtender ID="TxtFechNac_MaskedEditExtender" runat="server"  Enabled="True" TargetControlID="TxtFechNac" Mask="9999/99/99" MaskType="Date" UserDateFormat="YearMonthDay" InputDirection="RightToLeft" MessageValidatorTip="true" />      
                            <asp:CalendarExtender ID="TxtFechNac_CalendarExtender" runat="server" DefaultView="Days" Enabled="True" PopupPosition="BottomLeft" TargetControlID="TxtFechNac" CssClass="cal_Theme1" Format="yyyy/MM/dd" /></td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td><asp:Label ID="LblNombres" runat="server" Text="Nombres:" /></td>
                        <td><asp:TextBox ID="TxtNombres" runat="server" MaxLength="100" onkeypress="return ValidaSoloLetras(event)" Style="text-transform: uppercase"/></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="LblPrimerApellido" runat="server" Text="Primer apellido:" /></td>
                        <td><asp:TextBox ID="TxtPrimerApellido" runat="server" MaxLength="100" onkeypress="return ValidaSoloLetras(event)" Style="text-transform: uppercase"/></td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td><asp:Label ID="LblSegundoApellido" runat="server" Text="Segundo apellido:" /></td>
                        <td><asp:TextBox ID="TxtSegundoApellido" runat="server" MaxLength="100" onkeypress="return ValidaSoloLetras(event)" Style="text-transform: uppercase"/></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblSexoEmp" runat="server" Text="Sexo:" /></td>
                        <td>
                            <asp:DropDownList ID="TxtSexoEmp" runat="server">
                                <asp:ListItem Value="M">Masculino</asp:ListItem>
                                <asp:ListItem Value="F">Femenino</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td><asp:Label ID="LblLugarNac" runat="server" Text="Lugar Nacimiento:" /></td>
                        <td>
                            <asp:DropDownList ID="ddlCiudades" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="LblEstCivil" runat="server" Text="Estado Civil:" /></td>
                        <td>
                            <asp:DropDownList ID="TxtEstCivil" runat="server">
                                <asp:ListItem Value="1">Soltero(a)</asp:ListItem>
                                <asp:ListItem Value="2">Casado(a)</asp:ListItem>
                                <asp:ListItem Value="3">Viudo(a)</asp:ListItem>
                                <asp:ListItem Value="4">Divorciado(a)</asp:ListItem>
                                <asp:ListItem Value="5">Unión Libre</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td><asp:Label ID="LblDir" runat="server" Text="Dirección:" /></td>
                        <td><asp:TextBox ID="TxtDir" runat="server" MaxLength="100" Style="text-transform: uppercase"/></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="LblBarrio" runat="server" Text="Barrio:" /></td>
                        <td><asp:TextBox ID="TxtBarrio" runat="server" MaxLength="45" onkeypress="return ValidaSoloLetras(event)" Style="text-transform: uppercase"/></td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td><asp:Label ID="Lbltelefono" runat="server" Text="Telefono:" /></td>
                        <td><asp:TextBox ID="TxtTelefono" runat="server" MaxLength="10"/>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                 Enabled="True" TargetControlID="TxtTelefono" FilterType="Numbers" />
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="LblCelular" runat="server" Text="Celular:" /></td>
                        <td><asp:TextBox ID="TxtCelular" runat="server" MaxLength="10"/>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                 Enabled="True" TargetControlID="TxtCelular" FilterType="Numbers" />
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td><asp:Label ID="LblEps" runat="server" Text="EPS:" /></td>
                        <td><asp:TextBox ID="TxtEps" runat="server" MaxLength="50" Style="text-transform: uppercase" /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="LblAfp" runat="server" Text="AFP:" /></td>
                        <td><asp:TextBox ID="TxtAfp" runat="server" MaxLength="50" Style="text-transform: uppercase"/></td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td><asp:Label ID="LblCesantias" runat="server" Text="Fondo Cesantias:" /></td>
                        <td><asp:TextBox ID="TxtCesantias" runat="server" MaxLength="50" Style="text-transform: uppercase"/></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="LblCorreo" runat="server" Text="Correo:" /></td>
                        <td><asp:TextBox ID="TxtCorreo" runat="server" MaxLength="100" /><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtCorreo" ErrorMessage="*Ingrese un correo válido" ForeColor="#CC3300" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td><asp:Label ID="LblPass" runat="server" Text="Contraseña:" Visible="false"/></td>
                        <td><asp:TextBox ID="TxtPass" runat="server" MaxLength="15" Visible="false" TextMode="Password" /><asp:RequiredFieldValidator ID="RequiredContrasena" ValidationGroup="form" ControlToValidate="TxtPass" runat="server" ErrorMessage="Debe Ingresar una contraseña." ForeColor="#CC3300" Visible="false" Display="Dynamic"/><asp:CustomValidator ID="ValidatorPass" ValidationGroup="form" runat="server" ForeColor="#CC3300" ErrorMessage="Debe ingresar más de 4 carácteres" Visible="false" OnServerValidate="ValidatorPass_ServerValidate" Display="Dynamic"/>  
                        </td>                
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="BotonTablaDatos"><asp:Button ID="BtnEditar" runat="server" Text="Almacenar Información" ValidationGroup="form" OnClick="BtnEditar_Click" /></td>
                        <td class="BotonTablaDatos"><asp:Button ID="BtnPass" runat="server" Text="Cambiar Contraseña" OnClick="BtnContrasena_Click" /></td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnEditar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="BtnPass" EventName="Click" />
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