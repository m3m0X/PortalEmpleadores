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
    public partial class index1 : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();
        MySqlConnection MySqlCn;

        #region Definicion de los Metodos de la Clase

        #region Metodo Page Load
        /* ****************************************************************************/
        /* Metodo que se ejecuta al momento de la carga de la Pagina
        /* ****************************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            MySqlCn = new MySqlConnection(Cn);

            if (Session["usuario"] == null)
            {
                //Redirecciona a la pagina de login en caso de que el usuario no se haya autenticado
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    MySqlCommand scSqlCommand = new MySqlCommand("SELECT Contrasena_Activo FROM trabajadores.terceros where nit_tercero = '" + this.Session["usuario"].ToString() + "'", MySqlCn);
                    MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                    DataSet dsDataSet = new DataSet();
                    DataTable dtDataTable = null;

                    try
                    {
                        MySqlCn.Open();
                        sdaSqlDataAdapter.Fill(dsDataSet);
                        dtDataTable = dsDataSet.Tables[0];

                        if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                        {
                            if (dtDataTable.Rows[0].ItemArray[0].ToString() == "1")
                            {
                                Response.Redirect("PrimeraContrasena.aspx");
                            }
                            else
                            {
                                LlenadoDropBox utilLlenar = new LlenadoDropBox();
                                string command = "SELECT idCompania, Descripcion_compania FROM trabajadores.companias where Empresas_idEmpresa = 'AE' and activo_compania = 1 and Terceros_Nit_Tercero =" + Session["usuario"];
                                DropListProyecto.Items.Clear();
                                DropListProyecto.DataSource = utilLlenar.LoadTipoID(command);
                                DropListProyecto.DataTextField = "Descripcion_compania";
                                DropListProyecto.DataValueField = "idCompania";
                                DropListProyecto.DataBind();
                            }
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

        protected void BtnProyectos_Click(object sender, EventArgs e)
        {
            //Redirecciona a la pagina Principal
            Session["proyecto"] = DropListProyecto.SelectedItem.Value;
            Session["nombreProyecto"] = "Proyecto: " + DropListProyecto.SelectedItem.Text;

            Response.Redirect("~/Portal/Principal.aspx");
        }

        #endregion


    }
}