using MySql.Data.MySqlClient;
using PortalTrabajadores.Class;
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
    public partial class PrimeraContrasena : System.Web.UI.Page
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
        }
        #endregion

        #region Metodo btnlogin_Click
        /* ****************************************************************************/
        /* Evento al dar clic en el boton para cambio de contraseña
        /* ****************************************************************************/
        protected void btnlogin_Click(object sender, EventArgs e)
        {
            MySqlCommand scSqlCommand;

            if (Session["contrasenaActiva"] == null)
            {
                scSqlCommand = new MySqlCommand("SELECT Contrasena_tercero FROM " + bd2 + ".terceros where Nit_Tercero = '" +
                                                Session["usuario"].ToString() + "'", MySqlCn);
            }
            else
            {
                scSqlCommand = new MySqlCommand("SELECT contrasena FROM " + bd2 + ".usuariotercero where Terceros_Nit_Tercero = '" +
                                                Session["usuario"].ToString() + "' AND nombreUsuario = '" +
                                                Session["nombre"].ToString() + "'", MySqlCn);
            }

            MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
            DataSet dsDataSet = new DataSet();
            DataTable dtDataTable = null;
            MySqlCn.Open();
            sdaSqlDataAdapter.Fill(dsDataSet);
            dtDataTable = dsDataSet.Tables[0];

            if (dtDataTable != null && dtDataTable.Rows.Count > 0)
            {
                string Contrasena = dtDataTable.Rows[0].ItemArray[0].ToString();
                try
                {
                    if (Session["contrasenaActiva"] == null)
                    {
                        if (Contrasena != this.txtPass1.Text)
                        {
                            MySqlCommand sqlCommand2 = new MySqlCommand("UPDATE " + bd2 + ".terceros SET Contrasena_Activo = 0, Contrasena_tercero = '" + this.txtPass1.Text + "' WHERE Nit_Tercero = '" + Session["usuario"].ToString() + "'", MySqlCn);
                            sqlCommand2.ExecuteNonQuery();

                            Response.Redirect("~/Portal/index.aspx");
                        }
                        else
                        {
                            MensajeError("La contraseña no puede ser igual a la anterior.");
                        }
                    }
                    else
                    {
                        string cifrada = CifrarContrasena.HashSHA1(this.txtPass1.Text);

                        if (Contrasena != cifrada)
                        {
                            MySqlCommand sqlCommand2 = new MySqlCommand("UPDATE " + bd2 + ".usuariotercero SET contrasenaActiva = 1, contrasena = '" +
                                                                        cifrada + "' where Terceros_Nit_Tercero = '" +
                                                                        Session["usuario"].ToString() + "' AND nombreUsuario = '" +
                                                                        Session["nombre"].ToString() + "'", MySqlCn);
                            sqlCommand2.ExecuteNonQuery();

                            Session.Remove("contrasenaActiva");

                            Response.Redirect("~/Portal/index.aspx");
                        }
                        else
                        {
                            MensajeError("La contraseña no puede ser igual a la anterior.");
                        }
                    }
                }
                catch (Exception E)
                {
                    MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                finally
                {
                    MySqlCn.Close();
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
            if (LblMsj != null)
            {
                LblMsj.Text = Msj;
                LblMsj.Visible = true;
            }
        }
        #endregion

        #endregion
    }
}