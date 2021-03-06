﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="NivelCompetencias.aspx.cs" Inherits="PortalTrabajadores.Portal.NivelCompetencias" %>
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
                <asp:GridView ID="gvNivelesCreados" runat="server" OnRowCommand="gvNivelesCreados_RowCommand" 
                    OnRowDataBound="gvNivelesCreados_RowDataBound"
                    OnPageIndexChanging="gvNivelesCreados_PageIndexChanging" 
                    AutoGenerateColumns="False" 
                    AllowPaging="true" PageSize="10" >
                    <AlternatingRowStyle CssClass="ColorOscuro" />
                    <Columns>
                        <asp:BoundField DataField="nombre" HeaderText="Nivel" />
                        <asp:BoundField DataField="rangoMin" HeaderText="Minimo" />
                        <asp:BoundField DataField="rangoMax" HeaderText="Máximo" />
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnUpdate" runat="server" ImageUrl="~/Img/edit.gif" CommandArgument='<%#Eval("idNivelCompetencias")%>' CommandName="Editar" />
                                <asp:ImageButton ID="btnON" runat="server" ImageUrl="~/Img/on.png" CommandArgument='<%#Eval("idNivelCompetencias")%>' CommandName="On" />
                                <asp:ImageButton ID="btnOFF" runat="server" ImageUrl="~/Img/off.png" CommandArgument='<%#Eval("idNivelCompetencias")%>' CommandName="Off" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <asp:Button ID="btnNivel" runat="server" Text="Crear Nivel" OnClick="btnNivel_Click"  />
                <asp:Button ID="btnRegresar" runat="server" Text="Regresar" OnClick="btnRegresar_Click" />
            </div>
            <div id="Container_UpdatePanel2" runat="server" visible="false">
                <br />
                <table id="TablaDatos2">
                    <tr>
                        <th colspan="2">Nivel de Competencia</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblNombre" runat="server" Text="Nombre" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
                        </td>
                    </tr>    
                    <tr>
                        <td colspan="2">
                            <asp:RequiredFieldValidator ID="rfvNombre"
                                runat="server"
                                ErrorMessage="Debe digitar un nombre"
                                CssClass="MensajeError"
                                Display="Dynamic"
                                ControlToValidate="txtNombre"
                                ValidationGroup="objForm"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblMin" runat="server" Text="Rango Min" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtCien" runat="server" Text="100" Style="display: none" />
                            <asp:TextBox ID="txtMin" runat="server"
                                MaxLength="3" onkeypress="return ValidaSoloNumeros(event)"></asp:TextBox>                            
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td colspan="2">
                            <asp:CompareValidator ID="cValidator"
                                runat="server"
                                ErrorMessage="CompareValidator"
                                ControlToValidate="txtMin"
                                ControlToCompare="txtCien"
                                CssClass="MensajeError"
                                Display="Dynamic"
                                Operator="LessThanEqual"
                                Type="Integer"
                                Text="Error: El rango no puede ser mayor a 100"
                                ValidationGroup="objForm">
                            </asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="rfvMeta"
                                runat="server"
                                ErrorMessage="Debe digitar valor"
                                CssClass="MensajeError"
                                Display="Dynamic"
                                ControlToValidate="txtMin"
                                ValidationGroup="objForm"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblMax" runat="server" Text="Rango Max" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtMax" runat="server" 
                                MaxLength="3" onkeypress="return ValidaSoloNumeros(event)"></asp:TextBox>                            
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos" colspan="2">
                            <asp:CompareValidator ID="CompareValidator2"
                                runat="server"
                                ErrorMessage="CompareValidator"
                                ControlToValidate="txtMax"
                                ControlToCompare="txtCien"
                                CssClass="MensajeError"
                                Display="Dynamic"
                                Operator="LessThanEqual"
                                Type="Integer"
                                Text="Error: El rango no puede ser mayor a 100"
                                ValidationGroup="objForm">
                            </asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                runat="server"
                                ErrorMessage="Debe digitar valor"
                                CssClass="MensajeError"
                                Display="Dynamic"
                                ControlToValidate="txtMax"
                                ValidationGroup="objForm"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator1" 
                                runat="server" 
                                ErrorMessage="CompareValidator"
                                ControlToValidate="txtMax"
                                ControlToCompare="txtMin"
                                CssClass="MensajeError" 
                                Display="Dynamic"
                                Operator="GreaterThanEqual"
                                Type="Integer"
                                Text="Error: No puede ser menor a Rango Min"
                                ValidationGroup="objForm">
                            </asp:CompareValidator>
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnEditar" runat="server" Text="Guardar" ValidationGroup="objForm" OnClick="BtnEditar_Click" /></td>
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnCancel" runat="server" Text="Regresar" OnClick="BtnCancel_Click" /></td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnNivel" />
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
