<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="Areas.aspx.cs" Inherits="PortalTrabajadores.Portal.Areas" %>

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
                <asp:GridView ID="gvAreasCreadas" runat="server" OnRowDataBound="gvAreasCreadas_RowDataBound" OnRowCommand="gvAreasCreadas_RowCommand" OnPageIndexChanging="gvAreasCreadas_PageIndexChanging" AutoGenerateColumns="False" AllowPaging="true" PageSize="10">
                    <AlternatingRowStyle CssClass="ColorOscuro" />
                    <Columns>
                        <asp:BoundField DataField="Area" HeaderText="Áreas" SortExpression="Area" />
                        <asp:BoundField DataField="SubAreas" HeaderText="Sub-Áreas" SortExpression="SubAreas" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" Visible="false" />
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnUpdate" runat="server" ImageUrl="~/Img/edit.gif" CommandArgument='<%#Eval("IdAreas")%>' CommandName="Editar" />
                                <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/Img/add.gif" CommandArgument='<%#Eval("IdAreas")%>' CommandName="Agregar" />
                                <asp:ImageButton ID="btnON" runat="server" ImageUrl="~/Img/on.png" CommandArgument='<%#Eval("IdAreas")%>' CommandName="On" />
                                <asp:ImageButton ID="btnOFF" runat="server" ImageUrl="~/Img/off.png" CommandArgument='<%#Eval("IdAreas")%>' CommandName="Off" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <asp:Button ID="BtnArea" runat="server" Text="Crear Area" OnClick="BtnArea_Click" />
            </div>
            <div id="Container_UpdatePanel2" runat="server" visible="false">
                <table id="TablaDatos">
                    <tr>
                        <th colspan="2">Datos de Area</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="LblArea" runat="server" Text="Área:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="TxtArea" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvArea" ControlToValidate="TxtArea" CssClass="MensajeError" Display="Dynamic" ValidationGroup="areaForm" runat="server" ErrorMessage="Digite área"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnGuardar" runat="server" Text="Guardar" ValidationGroup="areaForm" OnClick="BtnGuardar_Click" /></td>
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" /></td>
                    </tr>
                </table>
            </div>
            <div id="Container_UpdatePanel3" runat="server" visible="false">
                <asp:GridView ID="gvSubAreasCreadas" runat="server" OnRowCommand="gvSubAreasCreadas_RowCommand" AutoGenerateColumns="False">
                    <AlternatingRowStyle CssClass="ColorOscuro" />
                    <Columns>
                        <asp:BoundField DataField="SubArea" HeaderText="Sub-Areas" SortExpression="SubArea" />
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnUpdate" runat="server" ImageUrl="~/Img/delete.gif" CommandArgument='<%#Eval("IdSubarea")%>' CommandName="Eliminar" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <table id="TablaDatos2">
                    <tr>
                        <th colspan="2">Datos de SubArea</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblSubarea" runat="server" Text="Sub-Área::" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtSubarea" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvSubarea" ControlToValidate="txtSubarea" CssClass="MensajeError" Display="Dynamic" ValidationGroup="subareaForm" runat="server" ErrorMessage="Digite sub-área"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnGuardarSA" runat="server" Text="Guardar" ValidationGroup="subareaForm" OnClick="BtnGuardarSA_Click" /></td>
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnCancelarSA" runat="server" Text="Cancelar" OnClick="BtnCancelarSA_Click" /></td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnArea" />
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
