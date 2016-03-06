<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Retenciones.aspx.cs" Inherits="PortalTrabajadores.Portal.Retenciones" Culture="en-GB"  %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="650px" Width="900px" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" OnLoad="ReportViewer1_Load">
            <LocalReport ReportPath="Portal\CertificadoRetencion.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ODSRetencion" Name="DataSet2" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ODSRetencion" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="PortalTrabajadores.DataSet2TableAdapters.formularioingresosretencionesTableAdapter"></asp:ObjectDataSource>
    </form>
</body>
</html>
