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
    public partial class Nomina : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql"].ConnectionString.ToString();
        MySqlConnection MySqlCn;

        #region Definicion de los Metodos de la Clase

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
                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'Nomina.aspx' AND idEmpresa = 'AE'", MySqlCn);
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

        #region Metodo GridView1_RowCommand
        /* ****************************************************************************/
        /* Evento que se produce al dar clic sobre el control IMG del GridView
        /* ****************************************************************************/
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Excel")
                {
                    string Cn = ConfigurationManager.ConnectionStrings["trabajadoresConnectionString"].ConnectionString.ToString();
                    CnMysql Conexion = new CnMysql(Cn);
                    MySqlCommand cmd = new MySqlCommand();
                    
                    Conexion.AbrirCnMysql();
                    cmd = new MySqlCommand("sp_ExcelNomina", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DocNomina", e.CommandArgument.ToString());
                    cmd.Parameters.AddWithValue("@TipoEmpresa", "AE");
                    cmd.Parameters.AddWithValue("@Proyecto", Session["proyecto"].ToString());
                    cmd.CommandTimeout = 500;

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable datos = new DataTable();

                    da.Fill(datos);

                    if (datos.Rows.Count > 0)
                    {
                        foreach (DataRow row in datos.Rows)
                        {
                            for (int i = 0; i < datos.Columns.Count; i++)
                            {
                                if (datos.Columns[i].DataType == typeof(string))
                                    row[i] = ReplaceHexadecimalSymbols((string)row[i]);
                            }
                        }

                        // Create the workbook
                        XLWorkbook workbook = new XLWorkbook();
                        workbook.Worksheets.Add(datos, "Nomina");

                        // Prepare the response
                        HttpResponse httpResponse = Response;
                        httpResponse.Clear();
                        httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        httpResponse.AddHeader("content-disposition", "attachment;filename=\"Nomina.xlsx\"");

                        // Flush the workbook to the Response.OutputStream
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            workbook.SaveAs(memoryStream);
                            memoryStream.WriteTo(httpResponse.OutputStream);
                            memoryStream.Close();
                        }

                        httpResponse.End();
                    }               
                }
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            string Cn = ConfigurationManager.ConnectionStrings["trabajadoresConnectionString"].ConnectionString.ToString();
            CnMysql Conexion = new CnMysql(Cn);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                Conexion.AbrirCnMysql();
                cmd = new MySqlCommand("sp_nominaEmpleados", Conexion.ObtenerCnMysql());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NumNit", Session["usuario"].ToString());
                cmd.Parameters.AddWithValue("@IdEmpresa", "AE");
                cmd.Parameters.AddWithValue("@mes", ddlMes.SelectedValue);
                cmd.Parameters.AddWithValue("@anio", txtAnio.Text);
                cmd.Parameters.AddWithValue("@Proyecto", Session["proyecto"].ToString());                        

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable datos = new DataTable();

                da.Fill(datos);

                if (datos.Rows.Count > 0)
                {
                    GridView1.DataSource = datos;//SelectTopDataRow(datos, 0, 10);
                    GridView1.DataBind();
                    Session["NominaEmpleados"] = datos;
                }
                else
                {
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    MensajeError("No hay datos para esta fecha");
                }
            }
            catch (Exception ex)
            {
                MensajeError(ex.Message);
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable datos = (DataTable)Session["NominaEmpleados"];
            GridView gv = (GridView)sender;
            gv.PageIndex = e.NewPageIndex;

            if (datos != null)
            {
                gv.DataSource = datos;
                gv.DataBind();
            }
        }

        private static string QuoteValue(string value)
        {
            return String.Concat("\"", value.Replace("\"", "\"\""), "\"");
        }

        public static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }

        #endregion
    }
}