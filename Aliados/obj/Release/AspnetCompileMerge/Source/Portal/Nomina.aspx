<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="Nomina.aspx.cs" Inherits="PortalTrabajadores.Portal.Nomina" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContainerTitulo" runat="server">
    <asp:Label ID="lblTitulo" runat="server"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Container" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" OnRowCommand="GridView1_RowCommand">
                <AlternatingRowStyle CssClass="ColorOscuro" />
                <Columns>
                    <asp:BoundField DataField="temporal" HeaderText="Empresa" SortExpression="temporal" />
                    <asp:BoundField DataField="DescripcionCargo" HeaderText="Cargo" SortExpression="DescripcionCargo" />
                    <asp:BoundField DataField="Fecha_Generacion_Nomina" HeaderText="Fecha Nómina" SortExpression="Fecha_Generacion_Nomina" />
                    <asp:TemplateField HeaderText="Generar" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="Imgpdf" runat="server" ImageUrl="~/Img/printButton.png" CommandArgument='<%#Eval("Fecha_Generacion_Nomina") %>' CommandName="Comprobante" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:trabajadoresConnectionString %>" ProviderName="<%$ ConnectionStrings:trabajadoresConnectionString.ProviderName %>" SelectCommand="sp_numcomprobantes" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="idEmpleado" SessionField="usuario" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="GridView1"/>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

