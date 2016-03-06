<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="Contactenos.aspx.cs" Inherits="PortalTrabajadores.Portal.Contactenos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--Hoja de estilo de la ventana modal-->
    <link href="../CSS/VentanaModal.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContainerTitulo" runat="server">
    <asp:Label ID="lblTitulo" runat="server"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Container" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <p class="TextoFiltros">
                <asp:Label ID="LblTipoSol" runat="server" Text="Tipo de Solicitud:"></asp:Label>
                <asp:DropDownList ID="DropListTipoSol" runat="server" Height="22px" />
            </p>
            <p class="TextoFiltros">
                <asp:TextBox ID="TxtMensaje" runat="server" Height="102px" MaxLength="255" TextMode="MultiLine" Width="370px" />
            </p>
            <p>
                <asp:Label ID="LblTextoCampo" runat="server" Text="Si desea recibir respuesta, por favor llene el campo correo." />
            </p>
            <p class="TextoFiltros">
                <asp:Label ID="LblCorreo" runat="server" Text="Correo: " />
                <asp:TextBox ID="TxtMail" runat="server" MaxLength="100" />
                <asp:Button ID="BtnEnviar" runat="server" Text="Enviar" OnClick="BtnEnviar_Click" />
            </p>
            <div class="MensajeError">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtMail" ErrorMessage="*Ingrese un correo válido" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </div>
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenTargetShow" BackgroundCssClass="modalbackground" OkControlID="btnok" PopupControlID="pnl"/>
            <asp:Panel ID="pnl" runat="server" CssClass="modalpopup">
                <asp:Label ID="lblMdlPop" runat="server">This is a modal popup dialog</asp:Label>
                <br />
                <br />
                <asp:Button ID="btnok" runat="server" Text="Aceptar"/>
            </asp:Panel>
            <asp:HiddenField ID="HiddenTargetShow" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnEnviar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
