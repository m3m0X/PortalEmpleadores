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
    public partial class Comprobantes : System.Web.UI.Page
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

                DataSet1.sp_GenComprobanteDataTable dt = new DataSet1.sp_GenComprobanteDataTable();
                DataSet1TableAdapters.sp_GenComprobanteTableAdapter da = new DataSet1TableAdapters.sp_GenComprobanteTableAdapter();

                da.FillBy(dt, Convert.ToInt32(Session["cedula"]), Session["FechaGeneracion"].ToString(), Session["NumNomina"].ToString(), Session["NumContrato"].ToString());

                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    if (dt[x].Marcacion_Descripcion == 1)
                    {
                        dt[x].Descripcion_compania = dt[x].Descripcion_Compania2;
                    }
                }

                ReportDataSource RD = new ReportDataSource();
                RD.Value = dt;
                RD.Name = "DataSet1";

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(RD);
                ReportViewer1.LocalReport.ReportEmbeddedResource = "Comprobante.rdlc";
                ReportViewer1.LocalReport.ReportPath = @"Portal/Comprobante.rdlc";
                ReportViewer1.LocalReport.Refresh();
                foreach (RenderingExtension elemento in ReportViewer1.LocalReport.ListRenderingExtensions())
                {
                    //ponemos la condición para entrar a poner falso la exportación a Excel y a Word
                    if (elemento.Name == "Excel" | elemento.Name == "WORD" | elemento.Name == "WORDOPENXML" | elemento.Name == "EXCELOPENXML" | elemento.Name == "EXCEL")
                    {
                        //traemos la información del campo con sus respectivos flags
                        FieldInfo infCampo = elemento.GetType().GetField("m_isVisible", BindingFlags.Instance | BindingFlags.NonPublic);
                        //colocamos el valor de false a la extension 
                        infCampo.SetValue(elemento, false);
                    }
                }
            }
        }
        #endregion

        #endregion
    }
}