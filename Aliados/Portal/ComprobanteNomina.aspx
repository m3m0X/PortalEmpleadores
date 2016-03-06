<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="ComprobanteNomina.aspx.cs" Inherits="PortalTrabajadores.Portal.ComprobanteNomina" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContainerTitulo" runat="server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTitulo" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Container" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="Container_UpdatePanel1">
                <table id="TablaDatos">
                    <tr>
                        <th colspan="2">Busqueda de Usuario</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="LblCambio" runat="server" Text="Digite Identificación:" />
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:TextBox ID="txtuser" runat="server" MaxLength="20" CssClass="MarcaAgua"
                                ToolTip="Usuario" placeholder="Identificación"  onkeypress="return ValidaSoloNumeros(event)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="BotonTablaDatos">
                                <asp:Button ID="BtnBuscar" runat="server" Text="Buscar" OnClick="BtnBuscar_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand">
                <AlternatingRowStyle CssClass="ColorOscuro" />
                <Columns>
                    <asp:BoundField DataField="temporal" HeaderText="Empresa" SortExpression="temporal" />
                    <asp:BoundField DataField="DescripcionCargo" HeaderText="Cargo" SortExpression="DescripcionCargo" />
                    <asp:BoundField DataField="Numero_Nomina" HeaderText="Número de Nómina" SortExpression="Numero_Nomina" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Tipo de pago" SortExpression="Descripcion" />
                    <asp:BoundField DataField="Fecha_Generacion_Nomina" HeaderText="Fecha Nómina" SortExpression="Fecha_Generacion_Nomina" />
                    <asp:BoundField DataField="Empleados_idContrato" HeaderText="ID Contrato" SortExpression="Empleados_idContrato" Visible="false" />
                    <asp:TemplateField HeaderText="Generar" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="Imgpdf" runat="server" ImageUrl="~/Img/printButton.png" CommandArgument='<%#Eval("Fecha_Generacion_Nomina") +";"+ Eval("Numero_Nomina") +";"+ Eval("Empleados_idContrato") %>' CommandName="Comprobante" />
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
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Errores" runat="server">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <asp:Label ID="LblMsj" runat="server" Text="" Visible="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>