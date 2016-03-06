using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Reflection;

namespace PortalTrabajadores.Portal
{
    public partial class Retenciones : System.Web.UI.Page
    {
        #region Definicion de los Metodos de la Clase

        #region Metodo ReportView Load
        /* ****************************************************************************/
        /* Metodo que se ejecuta al momento de la carga de ReportViewer
        /* ****************************************************************************/

        protected void ReportViewer1_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                DataSet2.ingretencionesDataTable dt = new DataSet2.ingretencionesDataTable();
                DataSet2TableAdapters.ingretencionesTableAdapter da = new DataSet2TableAdapters.ingretencionesTableAdapter();

                da.FillBy(dt,Session["cedula"].ToString(), "SS");

                ReportDataSource RD = new ReportDataSource();
                RD.Value = dt;
                RD.Name = "DataSet2";

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(RD);
                ReportViewer1.LocalReport.ReportEmbeddedResource = "CertificadoRetencion.rdlc";
                ReportViewer1.LocalReport.ReportPath = @"Portal/CertificadoRetencion.rdlc";

                // Codigo Agregado para que el archivo se descargue sin mostrar el reportviewer

                ReportViewer1.LocalReport.DataSources.Add(RD);

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename=CertificadoRetencion." + extension);
                Response.BinaryWrite(bytes); // create the file
                Response.Flush(); // send it to the client to download

                //ReportViewer1.LocalReport.Refresh();
                //foreach (RenderingExtension elemento in ReportViewer1.LocalReport.ListRenderingExtensions())
                //{
                //    //ponemos la condición para entrar a poner falso la exportación a Excel y a Word
                //    if (elemento.Name == "Excel" | elemento.Name == "WORD" | elemento.Name == "WORDOPENXML" | elemento.Name == "EXCELOPENXML" | elemento.Name == "EXCEL")
                //    {
                //        //traemos la información del campo con sus respectivos flags
                //        FieldInfo infCampo = elemento.GetType().GetField("m_isVisible", BindingFlags.Instance | BindingFlags.NonPublic);
                //        //colocamos el valor de false a la extension 
                //        infCampo.SetValue(elemento, false);
                //    }
                //}
            }
        }
        #endregion

        #endregion
    }
}