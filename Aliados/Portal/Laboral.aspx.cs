using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace PortalTrabajadores.Portal
{
    public partial class Laboral : System.Web.UI.Page
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
                //Valida que la pagina ya fue enviada al servidor para que no se cargue otra vez el control menu
                if (!Page.IsPostBack)
                {
                    try
                    {
                        MySqlCn = new MySqlConnection(Cn);
                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'Laboral.aspx' AND idEmpresa = 'AE'", MySqlCn);
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
        //IniciaMetodo
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

        /* ****************************************************************************/
        /* Metodo que se ejecuta con el boton buscar
        /* ****************************************************************************/
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            ContentPlaceHolder cPlaceHolder;
            cPlaceHolder = (ContentPlaceHolder)Master.FindControl("Errores");
            Label lblMessage = (Label)cPlaceHolder.FindControl("LblMsj") as Label;
            lblMessage.Visible = false;

            Session["cedula"] = txtuser.Text;

            string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();
            CnMysql Conexion = new CnMysql(Cn);
            MySqlDataReader reader = null;
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                Conexion.AbrirCnMysql();
                cmd = new MySqlCommand("sp_GenCertificadoEmpleados", Conexion.ObtenerCnMysql());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NumCed", Session["cedula"].ToString());
                cmd.Parameters.AddWithValue("@NitCompania", Session["usuario"].ToString());
                cmd.Parameters.AddWithValue("@IdEmpresa", "AE");
                cmd.Parameters.AddWithValue("@ProyectoCompania", Session["proyecto"].ToString());

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    //Abre una nueva ventana por fuera del Browser
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('Certificado.aspx', 'Graph', height=400, width=500);", true);
                    //Muestra un Mensaje en la aplicacion indicando al usuario que se abrio una nueva ventana
                    MensajeError("Su certificado se ha generado en una ventana externa.");
                }
                else 
                {
                    //Muestra un Mensaje en la aplicacion indicando al usuario que se abrio una nueva ventana
                    MensajeError("El usuario no existe.");
                }
            }
            catch (Exception ex) 
            {
                MensajeError(ex.Message);
            }
        }

        #endregion
    }
}