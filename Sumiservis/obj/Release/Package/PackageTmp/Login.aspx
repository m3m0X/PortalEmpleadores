<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PortalTrabajadores.Portal.login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <title>Servicio en línea empleadores</title>
    <link href="CSS/login.css" rel="stylesheet" type="text/css" />
    <!-- Js De Los campos de Textos -->
    <script src="Js/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="Js/funciones.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
        <header id="loginHeader">
			<h1>Servicio en línea empleadores</h1>
		</header>
        <div class="Capa1">
            <div id="titulologin">Sitio para empresas</div>
            <div id="texto">
                <p>Por favor digite su usuario y contraseña para iniciar al sistema. </p>
            </div>
            <div id="Capa2">
                <asp:TextBox ID="txtuser" runat="server" MaxLength="20" CssClass="MarcaAgua"
                     ToolTip="Usuario" placeholder="Número de Identificación" onkeypress="return ValidaSoloNumeros(event)"></asp:TextBox>
                <br />
                <br />
                <div id="usuarioAuto" style="display:none;">
                    <asp:TextBox ID="txtUserTer" runat="server" MaxLength="20" CssClass="MarcaAgua"
                     ToolTip="Usuario" placeholder="Usuario"></asp:TextBox>
                    <br />
                    <br />
                </div>
                <asp:TextBox ID="txtPass" runat="server" MaxLength="20" CssClass="MarcaAgua" onkeypress="hideOnKeyPress(); return true;"
                    placeholder="Introduzca Contraseña" TextMode="Password" ToolTip="Contraseña"></asp:TextBox>
                <br />
                <br />
                <div class="ckBox">
                    <asp:CheckBox ID="cboxUsuario" runat="server" />
                    <asp:Label ID="lbluser" runat="server" Text="Usuario Autorizado"></asp:Label>
                </div>                
                <br />
                <br />
                <div id="BotonPantalla">
                    <asp:Button ID="btnlogin" CssClass="botonimagen" runat="server" Text="Autenticar" 
                        OnClick="btnlogin_Click" ValidationGroup="Submit"></asp:Button>
                </div>
                <br />
                <div class="MenuVinculos">
                    <asp:LinkButton ID="btnPass" CssClass="botonimagen" runat="server" Text="Recuperar Contraseña" 
                    OnClick="btnPass_Click"></asp:LinkButton>
                </div>
                <div class="MensajesPantalla">                        
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                        ErrorMessage="*Digite Usuario" ControlToValidate="txtuser"
                        SetFocusOnError="true" ValidationGroup="Submit"
                        Display="Dynamic"></asp:RequiredFieldValidator>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                        ErrorMessage="*Digite Clave" ControlToValidate="txtPass" ValidationGroup="Submit"
                        SetFocusOnError="true"></asp:RequiredFieldValidator>
                    <br />
                    <asp:Label ID="LblMsj" runat="server" Visible="False"></asp:Label>
                </div>
                <br />
                <figure id="logo"></figure>
            </div>
        </div>
    </form>
</body>
</html>
