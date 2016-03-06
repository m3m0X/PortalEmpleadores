<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecuperarContrasena.aspx.cs" Inherits="PortalTrabajadores.Portal.RecuperarContrasena" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <title>Servicio en línea empleadores</title>
    <link href="CSS/login.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
        <header id="loginHeader">
			<h1>Recuperar la contraseña</h1>
		</header>
        <div class="Capa1">
            <div id="titulologin">Recuperar contraseña</div>
            <div id="texto">
                <p>Por favor digite su cedula y correo registrado</p>
            </div>
            <div id="Capa2">
                <asp:TextBox ID="txtIdentificacion" runat="server" MaxLength="20" CssClass="MarcaAgua" onkeypress="return ValidaSoloNumeros(event)"
                     ToolTip="Nit" placeholder="Nit"></asp:TextBox>
                <br />
                <br />
                <asp:TextBox ID="txtMail" runat="server" MaxLength="50" CssClass="MarcaAgua" onkeypress="hideOnKeyPress(); return true;"
                    placeholder="Introduzca Correo" ToolTip="Correo"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtMail" ErrorMessage="*Ingrese un correo válido" ForeColor="#CC3300" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
                <br />
                <br />
                <div id="BotonPantalla">
                    <asp:Button ID="btnSendPass" CssClass="botonimagen" runat="server" Text="Recuperar" 
                        OnClick="btnSendPass_Click" ValidationGroup="Submit" Font-Bold="True"></asp:Button>
                </div>
                <br />
                <div class="MenuVinculos">
                    <asp:LinkButton ID="btnAtras" CssClass="botonimagen" runat="server" Text="Atras" 
                        OnClick="btnAtras_Click" ValidationGroup="Submit" />
                </div>
                <div class="MensajesPantalla">
                    <asp:Label ID="lblMsg" runat="server" />
                </div>
                <br />
                <figure id="logo"></figure>
            </div>
        </div>
    </form>
</body>
</html>
