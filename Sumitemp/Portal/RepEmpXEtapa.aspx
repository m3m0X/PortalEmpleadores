<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="RepEmpXEtapa.aspx.cs" Inherits="PortalTrabajadores.Portal.RepEmpXEtapa" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContainerTitulo" runat="server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTitulo" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Container" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div id="Container_UpdatePanel1">
                <table id="TablaDatos">
                    <tr>
                        <th colspan="2">Reporte Estado Empleados por etapas</th>
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
                            <asp:Button ID="btnConsultar" runat="server" Text="Consultar" OnClick="btnConsultar_Click" />
                        </td>
                    </tr>
                </table>           
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnConsultar"/>
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="overlay" />
            <div class="overlayContent">
                <h2>Loading...</h2>
                <img src="../Img/loader.gif" alt="Loading" border="1" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
