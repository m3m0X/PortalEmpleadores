using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PortalTrabajadores.Portal
{
    public partial class RecuperarContrasena : System.Web.UI.Page
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

        #region Metodo btnSendPass_Click
        /* ***********************************************************************************************/
        /* Metodo que ejecuta el envío de correo y desactiva la contraseña para que la coloque de nuevo
        /* ***********************************************************************************************/
        protected void btnSendPass_Click(object sender, EventArgs e)
        {
            bool resultado = false;
            try
            {
                MySqlCommand scSqlCommand = new MySqlCommand("SELECT Nit_Tercero, Contrasena_tercero FROM " + bd2 + ".terceros where Correo_Tercero = '" + this.txtMail.Text + "' and Nit_Tercero = '" + this.txtIdentificacion.Text + "'", MySqlCn);
                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;
                MySqlCn.Open();
                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];
                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    string Id_Empleado = dtDataTable.Rows[0].ItemArray[0].ToString();
                    string Contrasena = dtDataTable.Rows[0].ItemArray[1].ToString();

                    resultado = this.SendCorreo(Id_Empleado, Contrasena, this.txtMail.Text);
                }

                if (resultado)
                {
                    lblMsg.Text = "Se ha enviado un mensaje a su correo. Si no recibe este correo, comuniquese con el administrador.";

                    MySqlCommand sqlCommand = new MySqlCommand("UPDATE " + bd2 + ".terceros SET Contrasena_activo = 1 WHERE Nit_Tercero = '" + this.txtIdentificacion.Text + "'", MySqlCn);
                    sqlCommand.ExecuteNonQuery();
                }
                else
                {
                    lblMsg.Text = "La información suministrada no es correcta, por favor comuniquese con el administrador.";
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception("Error presentado.", ex);
            }
            finally
            {
                MySqlCn.Close();
            }
        }
        #endregion

        #region Metodo SendCorreo
        /* ****************************************************************************/
        /* Metodo para envíar el correo con el usuario y contraseña
        /* ****************************************************************************/
        protected bool SendCorreo(string Id_Empleado, string Contrasena, string Mail)
        {
            string ServidorSMTP = ConfigurationManager.AppSettings["ServidorSMTP"].ToString();
            string UsuarioCorreo = ConfigurationManager.AppSettings["UsuarioCorreo"].ToString();
            string ClaveCorreo = ConfigurationManager.AppSettings["ClaveCorreo"].ToString();
            int port = Convert.ToInt16(ConfigurationManager.AppSettings["port"]);

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.To.Add(Mail);
            msg.From = new MailAddress(UsuarioCorreo, "Recuperar Contraseña", System.Text.Encoding.UTF8);
            msg.Subject = "Recuperar Contraseña";
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = "Su usuario es:" + Id_Empleado + " y su contraseña es:" + Contrasena;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = false;

            //Cliente que se conecta al servidor SMTP
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(UsuarioCorreo, ClaveCorreo);
            client.Port = port;
            client.Host = ServidorSMTP;
            client.EnableSsl = false; //Esto es para que vaya a través de SSL
            try
            {
                client.Send(msg);
                return true;
            }
            catch (System.Net.Mail.SmtpException E)
            {
                throw new System.Exception("Error presentado.", E);
            }
            finally
            {
                client.Dispose();
            }
        }
        #endregion

        #region Metodo btnAtras_Click
        /* ****************************************************************************/
        /* Metodo que devuelve a la págia de login
        /* ****************************************************************************/
        protected void btnAtras_Click(object sender, EventArgs e)
        {
            //redirecciona al usuario a la pagina principal de Login
            Response.Redirect("~/Login.aspx");
        }
        #endregion

        #endregion
    }
}