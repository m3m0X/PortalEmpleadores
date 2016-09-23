using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PortalTrabajadores.Portal
{
    public partial class BusquedaRetencion : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["trabajadoresConnectionString"].ConnectionString.ToString();
        string bd1 = ConfigurationManager.AppSettings["BD1"].ToString();

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
                    CnMysql Conexion = new CnMysql(Cn);
                    try
                    {
                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM " + bd1 + ".Options_Menu WHERE url = 'BusquedaRetencion.aspx'", Conexion.ObtenerCnMysql());
                        MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                        DataSet dsDataSet = new DataSet();
                        DataTable dtDataTable = null;

                        Conexion.AbrirCnMysql();
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
                        Conexion.CerrarCnMysql();
                    }
                }
            }
        }
        #endregion

        #region Metodo Desplegar el certificado
        /* ****************************************************************************/
        /* Evento que se produce al dar clic sobre el control IMG del GridView
        /* ****************************************************************************/

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Comprobante")
                {
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('retenciones.aspx', 'Graph', height=400, width=500);", true);
                }
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            finally
            {
                Response.ClearContent();
            }
        }
        #endregion

        #region Botón Buscar

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            LblMsj.Text = string.Empty;
            LblMsj.Visible = false;
            UpdatePanel3.Update();

            if (txtuser.Text != string.Empty)
            {
                Session["cedula"] = txtuser.Text;

                string Cn = ConfigurationManager.ConnectionStrings["trabajadoresConnectionString"].ConnectionString.ToString();
                CnMysql Conexion = new CnMysql(Cn);
                MySqlCommand cmd = new MySqlCommand();
                try
                {
                    Conexion.AbrirCnMysql();
                    cmd = new MySqlCommand("sp_retencionUsuario", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cedula", Session["cedula"].ToString());
                    cmd.Parameters.AddWithValue("@id_Empresa", "ST");
                    cmd.Parameters.AddWithValue("@proyecto", Session["Proyecto"].ToString());
                    cmd.Parameters.AddWithValue("@tercero", Session["usuario"].ToString());

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable datos = new DataTable();

                    da.Fill(datos);

                    if (datos.Rows.Count > 0)
                    {
                        GridView1.DataSource = datos;//SelectTopDataRow(datos, 0, 10);
                        GridView1.DataBind();
                        Session["RetencionUsuario"] = datos;
                    }
                    else
                    {
                        MensajeError("No se encontró información de este empleado");
                    }
                }
                catch (Exception E)
                {
                    MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
            }
            else 
            {
                MensajeError("Debe digitar una cedula");
            }
        }
        #endregion

        #region Metodo MensajeError
        /* ****************************************************************************/
        /* Metodo que habilita el label de mensaje de error
        /* ****************************************************************************/
        public void MensajeError(string Msj)
        {
            LblMsj.Text = Msj;
            LblMsj.Visible = true;
            UpdatePanel3.Update();
        }
        #endregion

        #endregion
    }
}