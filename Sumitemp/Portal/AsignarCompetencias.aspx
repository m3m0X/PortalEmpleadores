<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="AsignarCompetencias.aspx.cs" Inherits="PortalTrabajadores.Portal.AsignarCompetencias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Css para la fecha -->
    <link href="../CSS/CSSCallapsePanel.css" rel="stylesheet" type="text/css" />
    <!-- Js De Los campos de Textos -->
    <script src="../Js/funciones.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContainerTitulo" runat="server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTitulo" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Container" runat="server">
    <asp:UpdateProgress ID="upProgress" DynamicLayout="true" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="loader">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="Container_UpdatePanel1">
                <asp:GridView ID="gvCargosCreados" runat="server" 
                    OnRowDataBound="gvCargosCreados_RowDataBound" 
                    OnRowCommand="gvCargosCreados_RowCommand" 
                    OnPageIndexChanging="gvCargosCreados_PageIndexChanging" 
                    AutoGenerateColumns="False" AllowPaging="true" PageSize="10" >
                    <AlternatingRowStyle CssClass="ColorOscuro" />
                    <Columns>
                        <asp:BoundField DataField="Cargo" HeaderText="Cargos" />
                        <asp:BoundField DataField="NCompetencias" HeaderText="# Competecias" />
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnUpdate" runat="server" ImageUrl="~/Img/edit.gif" CommandArgument='<%#Eval("IdCargos")%>' CommandName="Crear" />
                                <asp:ImageButton ID="btnON" runat="server" ImageUrl="~/Img/ok.gif" Enabled="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="Container_UpdatePanel2" runat="server" visible="false">
                <asp:GridView ID="gvCompetenciasCreadas" runat="server" 
                    OnRowCommand="gvCompetenciasCreadas_RowCommand" 
                    OnPageIndexChanging="gvCompetenciasCreadas_PageIndexChanging" 
                    AutoGenerateColumns="False" AllowPaging="true" PageSize="10" >
                    <AlternatingRowStyle CssClass="ColorOscuro" />
                    <Columns>
                        <asp:BoundField DataField="competencia" HeaderText="Competencia" />
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnOFF" runat="server" ImageUrl="~/Img/delete.gif" CommandArgument='<%#Eval("idCompetencia")%>' CommandName="Eliminar" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>            
                <br />    
                <table id="TablaDatos">
                    <tr>
                        <th colspan="2">Asignar Competencia al cargo <asp:Label ID="lblCargoSelected" runat="server"></asp:Label></th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            Seleccione la competencia
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:DropDownList ID="ddlCompetencias" runat="server"></asp:DropDownList>                            
                        </td>
                    </tr>                    
                    <tr class="ColorOscuro">
                        <td class="BotonTablaDatos"><asp:Button ID="BtnGuardar" runat="server" Text="Guardar" OnClick="BtnGuardar_Click" /></td>
                        <td class="BotonTablaDatos"><asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" /></td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvCargosCreados" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Errores" runat="server">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <asp:Label ID="LblMsj" runat="server" Text="LabelMsjError" Visible="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

