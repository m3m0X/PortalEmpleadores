using ClosedXML.Excel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PortalTrabajadores.Portal
{
    public partial class RepEmpleadosSinJefe : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql"].ConnectionString.ToString();
        string bd2 = ConfigurationManager.AppSettings["BD2"].ToString();
        string bd3 = ConfigurationManager.AppSettings["BD3"].ToString();
        MySqlConnection MySqlCn;

        #region Metodo Page Load
        /* ****************************************************************************/
        /* Metodo que se ejecuta al momento de la carga de la Pagina
        /* ****************************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                //Redirecciona a la pagina de login en caso de que el usuario no se halla autenticado
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    try
                    {
                        MySqlCn = new MySqlConnection(Cn);
                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'RepEmpleadosSinJefe.aspx'", MySqlCn);
                        MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                        DataSet dsDataSet = new DataSet();
                        DataTable dtDataTable = null;
                        MySqlCn.Open();
                        sdaSqlDataAdapter.Fill(dsDataSet);
                        dtDataTable = dsDataSet.Tables[0];
                        if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                        {
                            this.lblTitulo.Text = dtDataTable.Rows[0].ItemArray[0].ToString();
                        }

                        this.CargarAnio();
                    }
                    catch (Exception ex)
                    {
                        MensajeError("Ha ocurrido el siguiente error: " + ex.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                    }
                    finally
                    {
                        MySqlCn.Close();
                    }                    
                }
            }
        }
        #endregion

        #region Metodo MensajeError
        /* ****************************************************************************/
        /* Metodo que habilita el label de mensaje de error
        /* ****************************************************************************/
        public void MensajeError(string Msj)
        {
            ContentPlaceHolder cPlaceHolder;
            cPlaceHolder = (ContentPlaceHolder)Master.FindControl("Errores");
            if (cPlaceHolder != null)
            {
                Label lblMessage = (Label)cPlaceHolder.FindControl("LblMsj") as Label;
                if (lblMessage != null)
                {
                    lblMessage.Text = Msj;
                    lblMessage.Visible = true;
                }
            }
        }
        #endregion

        #region funciones

        /// <summary>
        /// Reemplaza los simbolos hexa
        /// </summary>
        /// <param name="txt">texto enviado</param>
        /// <returns>texto organizado</returns>
        public static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }

        /// <summary>
        /// Carga el año actual y el pasado
        /// </summary>
        public void CargarAnio()
        {
            try
            {
                ConsultasGenerales consultas = new ConsultasGenerales();
                DataTable datos = consultas.ConsultarAnos(Session["proyecto"].ToString(),
                                                          Session["idEmpresa"].ToString());

                if (datos != null)
                {
                    ddlAnio.DataTextField = "Ano";
                    ddlAnio.DataValueField = "Ano";
                    ddlAnio.DataSource = datos;
                    ddlAnio.DataBind();
                }
                else
                {
                    DateTime fechaAnioActual = DateTime.Now;
                    ddlAnio.Items.Add(new ListItem(fechaAnioActual.Year.ToString(), fechaAnioActual.Year.ToString()));
                }
            }
            catch (Exception ex)
            {
                MensajeError(ex.Message);
            }
        }

        #endregion

        #region eventos

        /// <summary>
        /// Consultar Reporte
        /// </summary>
        /// <param name="sender">Objeto Sender</param>
        /// <param name="e">Evento e</param>
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            lblCambio.Text = string.Empty;

            try
            {
                string Cn3 = ConfigurationManager.ConnectionStrings["CadenaConexioMySql3"].ConnectionString.ToString();

                MySqlCn = new MySqlConnection(Cn3);
                MySqlCommand scSqlCommand = new MySqlCommand(bd3 + ".sp_EmpleadosSinJefe", MySqlCn);
                scSqlCommand.CommandType = CommandType.StoredProcedure;
                scSqlCommand.Parameters.AddWithValue("@idCompania", Session["proyecto"]);
                scSqlCommand.Parameters.AddWithValue("@idEmpresa", Session["idEmpresa"]);
                scSqlCommand.Parameters.AddWithValue("@ano", ddlAnio.SelectedValue);

                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                DataTable dtDataTable = new DataTable();
                MySqlCn.Open();
                sdaSqlDataAdapter.Fill(dtDataTable);

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dtDataTable.Rows)
                    {
                        for (int i = 0; i < dtDataTable.Columns.Count; i++)
                        {
                            if (dtDataTable.Columns[i].DataType == typeof(string))
                                row[i] = ReplaceHexadecimalSymbols((string)row[i]);
                        }
                    }

                    // Create the workbook
                    XLWorkbook workbook = new XLWorkbook();
                    workbook.Worksheets.Add(dtDataTable, "Empleados");

                    // Prepare the response
                    HttpResponse httpResponse = Response;
                    httpResponse.Clear();
                    httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    httpResponse.AddHeader("content-disposition", "attachment;filename=\"EstadoJefeEmpleado.xlsx\"");

                    // Flush the workbook to the Response.OutputStream
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        workbook.SaveAs(memoryStream);
                        memoryStream.WriteTo(httpResponse.OutputStream);
                        memoryStream.Close();
                    }

                    httpResponse.End();
                    lblCambio.Text = "Archivo Generado";                    
                }
                else 
                {
                    lblCambio.Text = "No hay empleados activos";
                }
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        #endregion
    }
}