<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="PrimeraContrasena.aspx.cs" Inherits="PortalTrabajadores.Portal.PrimeraContrasena" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <title>Cambio de contraseña</title>
    <link href="../CSS/login.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
        <header id="loginHeader">
			<h1>Cambio de contraseña</h1>
		</header>
        <div class="Capa1">
            <div id="titulologin">Nueva Contraseña</div>
            <div id="texto">
                <p>Por favor antes de continuar, cambie su contraseña por un nueva.</p>
            </div>
            <div id="Capa2">
                <asp:TextBox ID="txtPass1" runat="server" MaxLength="20" CssClass="MarcaAgua"
                    ToolTip="Contraseña Nueva" placeholder="Contraseña Nueva" TextMode="Password"></asp:TextBox>
                <br />
                <br />
                <asp:TextBox ID="txtPass2" runat="server" MaxLength="20" CssClass="MarcaAgua"
                    placeholder="Repita su contraseña" TextMode="Password" ToolTip="Repita su contraseña"></asp:TextBox>
                <br />
                <br />
                <div id="BotonPantalla">
                    <asp:Button ID="btnlogin" CssClass="botonimagen" runat="server" Text="Aceptar"
                        ValidationGroup="Submit" Font-Bold="True" OnClick="btnlogin_Click"></asp:Button>
                </div>
                <br />
                <br />
                <div class="MensajesPantalla">        
                    <asp:Label ID="LblMsj" runat="server" Text="LabelMsjError" Visible="False"></asp:Label>
                    <br />                
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                        ErrorMessage="*Digite Contraseña" ControlToValidate="txtPass1"
                        SetFocusOnError="true" ValidationGroup="Submit" Display="Dynamic"></asp:RequiredFieldValidator>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                        ErrorMessage="*Repita su contraseña" ControlToValidate="txtPass2" ValidationGroup="Submit"
                        SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                    <br />
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*Las claves no concuerdan"
                        ValidationGroup="Submit" ControlToValidate="txtPass1" ControlToCompare="txtPass2" Display="Dynamic"></asp:CompareValidator>
                </div>
                <br />
                <figure id="logo"></figure>
            </div>
        </div>
    </form>
</body>
</html>