<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="Nomina.aspx.cs" Inherits="PortalTrabajadores.Portal.Nomina" %>

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
                        <th colspan="2">Busqueda de Nomina</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblAnio" runat="server" Text="Digite el año:" />
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:TextBox ID="txtAnio" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="LblCambio" runat="server" Text="Seleccione el Mes:" />
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:DropDownList ID="ddlMes" runat="server">
                                <asp:ListItem Value="1">Enero</asp:ListItem>
                                <asp:ListItem Value="2">Febrero</asp:ListItem>
                                <asp:ListItem Value="3">Marzo</asp:ListItem>
                                <asp:ListItem Value="4">Abril</asp:ListItem>
                                <asp:ListItem Value="5">Mayo</asp:ListItem>
                                <asp:ListItem Value="6">Junio</asp:ListItem>
                                <asp:ListItem Value="7">Julio</asp:ListItem>
                                <asp:ListItem Value="8">Agosto</asp:ListItem>
                                <asp:ListItem Value="9">Septiembre</asp:ListItem>
                                <asp:ListItem Value="10">Octubre</asp:ListItem>
                                <asp:ListItem Value="11">Noviembre</asp:ListItem>
                                <asp:ListItem Value="12">Diciembre</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="LblTipo" runat="server" Text="Seleccione el tipo de reporte:" /></td>
                        <td class="BotonTablaDatos">
                            <asp:DropDownList ID="ddlTipo" runat="server">
                                <asp:ListItem Value="1">Nomina</asp:ListItem>
                                <asp:ListItem Value="2">Nomina Consolidado</asp:ListItem>
                                <asp:ListItem Value="3">Centro de Costo</asp:ListItem>
                                <asp:ListItem Value="4">Concepto</asp:ListItem>
                            </asp:DropDownList>
                        </td>   
                    </tr>
                    <tr>
                        <td colspan="2" class="BotonTablaDatos">
                            <asp:Button ID="BtnBuscar" runat="server" Text="Buscar" OnClick="BtnBuscar_Click"  />
                            <asp:Button ID="BtnLimpiar" runat="server" Text="Limpiar" OnClick="BtnLimpiar_Click"  />
                        </td>
                    </tr>
                </table>         
            </div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" OnRowCommand="GridView1_RowCommand" AllowPaging="true" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging">
                <AlternatingRowStyle CssClass="ColorOscuro" />
                <Columns>
                    <%--<asp:BoundField DataField="Nomina" HeaderText="No Nomina" SortExpression="Nomina" />
                    <asp:BoundField DataField="Fecha Generación" HeaderText="Fecha" SortExpression="Fecha Generación" />
                    <asp:BoundField DataField="Doc Nomina" HeaderText="" SortExpression="Doc Nomina" Visible="false"/>--%>
                    <asp:TemplateField HeaderText="Generar" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="Imgpdf" runat="server" ImageUrl="~/Img/excel.gif" CommandName="Excel" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="GridView1"/>
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
