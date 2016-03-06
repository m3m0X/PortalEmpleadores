<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="EstadoFondo.aspx.cs" Inherits="PortalTrabajadores.Portal.EstadoFondo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContainerTitulo" runat="server">
    <asp:Label ID="lblTitulo" runat="server"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Container" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" GridLines="Vertical" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand">
                <AlternatingRowStyle CssClass="ColorOscuro" />
                <Columns>
                    <asp:BoundField DataField="anio_fondo" HeaderText="Año" >
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="mes_fondo" HeaderText="Mes" >
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Descargar" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="Imgpdf" runat="server" ImageUrl="~/Img/pdf.gif" CommandArgument='<%#Eval("idFondo") %>' CommandName="PDF" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CadenaConexioMySql2 %>"
                ProviderName="<%$ ConnectionStrings:CadenaConexioMySql.ProviderName %>"></asp:SqlDataSource>                       
        </ContentTemplate>
         <Triggers>
            <asp:PostBackTrigger  ControlID="GridView1"/>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
