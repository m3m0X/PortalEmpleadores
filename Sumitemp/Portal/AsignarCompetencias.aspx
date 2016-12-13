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
                                <asp:ImageButton ID="btnUpdate" runat="server" ImageUrl="~/Img/edit.gif" 
                                    CommandArgument='<%#Eval("IdCargos")%>' CommandName="Crear" />
                                <asp:ImageButton ID="btnON" runat="server" ImageUrl="~/Img/ok.gif" Enabled="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <asp:Button ID="btnRegresar" runat="server" Text="Regresar" OnClick="btnRegresar_Click" />
            </div>
            <div id="Container_UpdatePanel2" runat="server" visible="false">
                <br />
                <asp:GridView ID="gvCompetenciasCreadas" runat="server" 
                    OnRowCommand="gvCompetenciasCreadas_RowCommand" 
                    OnRowDataBound="gvCompetenciasCreadas_RowDataBound"
                    OnPageIndexChanging="gvCompetenciasCreadas_PageIndexChanging" 
                    AutoGenerateColumns="False" AllowPaging="true" PageSize="10" >
                    <AlternatingRowStyle CssClass="ColorOscuro" />
                    <Columns>
                        <asp:BoundField DataField="competencia" HeaderText="Competencia" />
                        <asp:BoundField DataField="nivelCompetencia" HeaderText="Nivel" />
                        <asp:BoundField DataField="conductas" HeaderText="Conductas" />
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnON" runat="server" ImageUrl="~/Img/on.png" ToolTip="Activar" 
                                    CommandArgument='<%#Eval("idCompetencia")%>' CommandName="On" />
                                <asp:ImageButton ID="btnOFF" runat="server" ImageUrl="~/Img/off.png" ToolTip="Desactivar" 
                                    CommandArgument='<%#Eval("idCompetencia")%>' CommandName="Off" />
                                <asp:ImageButton ID="btnCond" runat="server" ImageUrl="~/Img/add.gif" ToolTip="Conductas" 
                                    CommandArgument='<%#Eval("idCompetencia")%>' CommandName="Conducta" />
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
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            Seleccione la competencia
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:DropDownList ID="ddlCompetencias" runat="server"></asp:DropDownList>                            
                        </td>
                    </tr>    
                    <tr>
                        <td class="CeldaTablaDatos">
                            Seleccione el nivel de la competencia
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:DropDownList ID="ddlNivelCompetencia" runat="server"></asp:DropDownList>                            
                        </td>
                    </tr>                    
                    <tr class="ColorOscuro">
                        <td class="BotonTablaDatos"><asp:Button ID="BtnGuardar" runat="server" Text="Guardar" OnClick="BtnGuardar_Click" /></td>
                        <td class="BotonTablaDatos"><asp:Button ID="BtnCancelar" runat="server" Text="Cerrar" OnClick="BtnCancelar_Click" /></td>
                    </tr>
                </table>
            </div>
            <div id="Container_UpdatePanel3" runat="server" visible="false">
                <asp:GridView ID="gvConductas" runat="server" 
                    OnRowCommand="gvConductas_RowCommand" 
                    OnPageIndexChanging="gvConductas_PageIndexChanging" 
                    AutoGenerateColumns="False" AllowPaging="true" PageSize="10" 
                    Caption='<table border="1" width="100%"><tr><th>Conductas asignadas.</th></tr></table>'
                    EmptyDataText="Sin conductas asignadas">
                    <AlternatingRowStyle CssClass="ColorOscuro" />
                    <Columns>
                        <asp:BoundField DataField="conducta" HeaderText="Conductas" />
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnOFF" runat="server" ImageUrl="~/Img/off.png" ToolTip="Desactivar" 
                                    CommandArgument='<%#Eval("idCarConCom")%>' CommandName="Off" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>            
                <br />  
                <asp:GridView ID="gvAgregarConducta" runat="server" 
                    OnRowCommand="gvAgregarConducta_RowCommand" 
                    OnPageIndexChanging="gvAgregarConducta_PageIndexChanging" 
                    AutoGenerateColumns="False" AllowPaging="true" PageSize="10" 
                    Caption='<table border="1" width="100%"><tr><th>Conductas disponibles.</th></tr></table>'
                    EmptyDataText="No existen conductas creadas">
                    <AlternatingRowStyle CssClass="ColorOscuro" />
                    <Columns>
                        <asp:BoundField DataField="conducta" HeaderText="Conductas" />
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnON" runat="server" ImageUrl="~/Img/on.png" ToolTip="Activar" 
                                    CommandArgument='<%#Eval("idConducta")%>' CommandName="On" />                                
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>            
                <br>
                <asp:Button ID="BtnRegresarCargo" runat="server" Text="Regresar" OnClick="BtnRegresarCargo_Click" />
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

