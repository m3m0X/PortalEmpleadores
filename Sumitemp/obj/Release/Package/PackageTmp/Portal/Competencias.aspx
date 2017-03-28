<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="Competencias.aspx.cs" Inherits="PortalTrabajadores.Portal.Competencias" %>
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
            <div id="Container_UpdatePanelAnio" runat="server">
                <table id="TablaDatos">
                    <tr>
                        <th colspan="2">Seleccione el año</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblAnio" runat="server" Text="Año:" />
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:DropDownList ID="ddlAnio" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="BotonTablaDatos">
                            <asp:Button ID="BtnBuscar" runat="server" Text="Buscar" OnClick="BtnBuscar_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="Container_UpdatePanel1" runat="server" visible="false">
                <asp:GridView ID="gvCompetenciasCreadas" runat="server" 
                    OnRowDataBound="gvCompetenciasCreadas_RowDataBound"
                    OnRowCommand="gvCompetenciasCreadas_RowCommand" 
                    OnPageIndexChanging="gvCompetenciasCreadas_PageIndexChanging" 
                    AutoGenerateColumns="False" AllowPaging="true" PageSize="10" >
                    <AlternatingRowStyle CssClass="ColorOscuro" />
                    <Columns>
                        <asp:BoundField DataField="competencia" HeaderText="Competencia" />
                        <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
                        <asp:BoundField DataField="activo" HeaderText="activo" SortExpression="Estado" Visible="false" />
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnUpdate" runat="server" ImageUrl="~/Img/edit.gif" CommandArgument='<%#Eval("idCompetencia")%>' CommandName="Editar" />
                                <asp:ImageButton ID="btnON" runat="server" ImageUrl="~/Img/on.png" CommandArgument='<%#Eval("idCompetencia")%>' CommandName="On" />
                                <asp:ImageButton ID="btnOFF" runat="server" ImageUrl="~/Img/off.png" CommandArgument='<%#Eval("idCompetencia")%>' CommandName="Off" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <asp:Button ID="BtnCompetencias" runat="server" Text="Crear Competencia" OnClick="BtnCompetencias_Click"  />
                <asp:Button ID="btnRegresar" runat="server" Text="Regresar" OnClick="btnRegresar_Click" />
            </div>
            <div id="Container_UpdatePanel2" runat="server" visible="false">
                <br />
                <table id="TablaDatos">
                    <tr>
                        <th colspan="2">Digite la competencia</th>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblCompetencia" runat="server" Text="Competencia:" />
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:TextBox ID="txtCompetencia" runat="server"></asp:TextBox>                            
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td colspan="2">
                            <asp:RequiredFieldValidator ID="rfvCompetencia"
                                runat="server"
                                ErrorMessage="Debe digitar una competencia"
                                CssClass="MensajeError"
                                Display="Dynamic"
                                ControlToValidate="txtCompetencia"
                                ValidationGroup="objForm"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblDescripcion" runat="server" Text="Descripción:" />
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:TextBox ID="txtDescripcion" runat="server" 
                                TextMode="MultiLine" MaxLength="500" Height="60px" Width="180px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:RequiredFieldValidator ID="rfvObservacion"
                                runat="server"
                                ErrorMessage="Debe digitar una Descripcion"
                                CssClass="MensajeError"
                                Display="Dynamic"
                                ControlToValidate="txtDescripcion"
                                ValidationGroup="objForm"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnGuardar" runat="server" Text="Guardar" ValidationGroup="objForm" OnClick="BtnGuardar_Click" /></td>
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnCancelar" runat="server" Text="Regresar" OnClick="BtnCancelar_Click" /></td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnCompetencias" />
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
