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
    public partial class login : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();
        string bd2 = ConfigurationManager.AppSettings["BD2"].ToString();
        MySqlConnection MySqlCn;

        #region Definicion de los Metodos de la Clase

        #region Metodo Page Load
        /* ****************************************************************************/
        /* Metodo que se ejecuta al momento de la carga de la Pagina
        /* ****************************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            MySqlCn = new MySqlConnection(Cn);
            txtuser.Focus();
        }
        #endregion

        #region Metodo btnlogin_Click
        /* ****************************************************************************/
        /* Evento del Boton para realizar la autenticacion al Portal de trabajadores
        /* ****************************************************************************/
        protected void btnlogin_Click(object sender, EventArgs e)
        {
            Page.Validate();
            try
            {
                MySqlCommand scSqlCommand = new MySqlCommand("SELECT Nit_Tercero, Id_Rol, Razon_social FROM " + bd2 + ".terceros JOIN " + bd2 + ".companias as a ON Nit_Tercero = a.Terceros_Nit_Tercero where Nit_Tercero = '" + this.txtuser.Text + "' and Contrasena_tercero = '" + this.txtPass.Text + "' and activo_tercero = '1' and Activo_Compania = 1 and Empresas_idEmpresa = 'AE'", MySqlCn);
                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;
                //Asegura que los controles de la pagina hayan sido validados
                if (Page.IsValid)
                {
                    MySqlCn.Open();
                    sdaSqlDataAdapter.Fill(dsDataSet);
                    dtDataTable = dsDataSet.Tables[0];

                    if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                    {
                        //Creo las Variables de Sesion de la Pagina
                        Session.Add("usuario", txtuser.Text);
                        Session.Add("rol", dtDataTable.Rows[0].ItemArray[1].ToString());
                        Session.Add("nombre", dtDataTable.Rows[0].ItemArray[2].ToString());

                        //redirecciona al usuario a la pagina principal del Portal
                        Response.Redirect("~/Portal/index.aspx");
                    }
                    else
                    {
                        MensajeError("Usuario y/o contraseña equivocados.");
                    }
                }
            }
            catch
            {
                MensajeError("El sistema no se encuentra disponible en este momento. Intente más tarde.");
            }
            finally
            {
                MySqlCn.Close();
            }
        }
        #endregion

        #region Metodo btnPass_Click
        /* ****************************************************************************/
        /* Evento del boton que redirecciona a la pagina de recuperar contraseña
        /* ****************************************************************************/
        protected void btnPass_Click(object sender, EventArgs e)
        {
            //redirecciona al usuario a la pagina principal de la encuesta si no la ha contestado
            Response.Redirect("~/RecuperarContrasena.aspx");
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
        }
        #endregion

        #endregion
    }
}