<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="CargueArchivos.aspx.cs" Inherits="PortalTrabajadores.Portal.CargueArchivos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContainerTitulo" runat="server">
    <asp:Label ID="lblTitulo" runat="server"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Container" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <p class="TextoFiltros">
                <asp:Label ID="LblTipoCargue" runat="server" Text="Tipo de Cargue:"></asp:Label>
                <asp:DropDownList ID="DropListTipoCargue" runat="server" OnSelectedIndexChanged="DropListTipoCargue_SelectedIndexChanged" AutoPostBack="True" Height="22px">
                    <asp:ListItem Text="Seleccione Tipo Cargue" Value="0" Selected="true" />
                    <asp:ListItem Text="Cargue Cesantias" Value="1" />
                    <asp:ListItem Text="Cargue Fondo" Value="2" />
                    <asp:ListItem Text="Cargue SeguridadSocial" Value="3" />
                    <asp:ListItem Text="Cargue ReteFuente" Value="4" />
                </asp:DropDownList>

                <asp:Label ID="LblAñoCargue" runat="server" Text="Año Cargue:"/>
                <asp:DropDownList ID="DropListAño" runat="server" AutoPostBack="True" Height="22px"/>
                
                <asp:Label ID="LblMesCargue" runat="server" Text="Mes Cargue:"></asp:Label>
                <asp:DropDownList ID="DropListMes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropListMes_SelectedIndexChanged" Height="22px">
                    <asp:ListItem Text="Seleccione un Mes" Value="0" Selected="true" />
                    <asp:ListItem Text="Enero" Value="1" />
                    <asp:ListItem Text="Febrero" Value="2" />
                    <asp:ListItem Text="Marzo" Value="3" />
                    <asp:ListItem Text="Abril" Value="4" />
                    <asp:ListItem Text="Mayo" Value="5" />
                    <asp:ListItem Text="Junio" Value="6" />
                    <asp:ListItem Text="Julio" Value="7" />
                    <asp:ListItem Text="Agosto" Value="8" />
                    <asp:ListItem Text="Septiembre" Value="9" />
                    <asp:ListItem Text="Octubre" Value="10" />
                    <asp:ListItem Text="Noviembre" Value="11" />
                    <asp:ListItem Text="Diciembre" Value="12" />
                </asp:DropDownList>

                <asp:Button ID="BtnCargue" runat="server" Text="Cargar Archivos" OnClick="BtnCargue_Click" Enabled="False" />
            </p>
            <asp:ListBox ID="ListArchivos" runat="server" Height="143px" SelectionMode="Multiple" Visible="False" Width="166px"></asp:ListBox>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnCargue"/>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

