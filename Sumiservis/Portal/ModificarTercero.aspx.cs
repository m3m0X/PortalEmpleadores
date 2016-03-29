using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace PortalTrabajadores.Portal
{
    public partial class ModificarTercero : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["trabajadoresConnectionString"].ConnectionString.ToString();
        string bd1 = ConfigurationManager.AppSettings["BD1"].ToString();
        string bd2 = ConfigurationManager.AppSettings["BD2"].ToString();
        int bandera = 1;

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
                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM " + bd1 + ".Options_Menu WHERE url = 'ModificarTercero.aspx' and idEmpresa = 'SS' and Tipoportal = 'E'", Conexion.ObtenerCnMysql());
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

                        CargarInfoCliente(Session["usuario"].ToString());
                    }
                    catch (Exception E)
                    {
                        MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                    }
                    finally
                    {
                        Conexion.CerrarCnMysql();
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
            LblMsj.Text = Msj;
            LblMsj.Visible = true;
            UpdatePanel3.Update();
        }
        #endregion

        #region Metodo CargarInfoCliente
        /* ****************************************************************************/
        /* Metodo que carga la informacion del tercero en el formulario
        /* ****************************************************************************/
        public void CargarInfoCliente(string Nit_tercero)
        {
            CnMysql Conexion = new CnMysql(Cn);

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd = new MySqlCommand(bd2 + ".sp_ConsultaTerceros", Conexion.ObtenerCnMysql());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NitTercero", Nit_tercero);
                MySqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    Container_UpdatePanel2.Visible = true;
                    TxtDoc.Text = rd["Nit_tercero"].ToString();
                    TxtNombres.Text = rd["Razon_Social"].ToString();
                    TxtCelular.Text = rd["Celular_tercero"].ToString();
                    TxtDir.Text = rd["Direccion_tercero"].ToString();
                    TxtCorreo.Text = rd["Correo_tercero"].ToString();
                    Session.Add("Contrasena", rd["Contrasena_Tercero"].ToString());
                    TxtPass.Text = "";
                    BtnEditar.Text = "Guardar Información";
                }
                else
                {
                    MensajeError("No se encontro el NIT digitado, Para agregar un nuevo Cliente haga click en el Botón: ");
                }
                rd.Close();
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            finally
            {
                Conexion.CerrarCnMysql();
            }
        }
        #endregion

        #region Metodo BtnContrasena_Click
        /* ***************************************************************************************/
        /* Evento que Habilita los controles en el formulario para la actualizacion de Contraseña
        /* ***************************************************************************************/
        protected void BtnContrasena_Click(object sender, EventArgs e)
        {
            LblPass.Visible = true;
            TxtPass.Visible = true;
            BtnPass.Visible = false;
            RequiredContrasena.Visible = true;
            ValidatorPass.Visible = true;

            Session["cambiaContrasena"] = true;
            TxtPass.Focus();
        }
        #endregion

        #region Metodo ValidatorPass_ServerValidate
        /* ****************************************************************************/
        /* Metodo que valida el password digitado en el servidor
        /* ****************************************************************************/
        protected void ValidatorPass_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (TxtPass.Text.Count() < 5)
            {
                args.IsValid = false;
                bandera = 0;
            }
            else
            {
                CnMysql Conexion = new CnMysql(Cn);
                try
                {
                    MySqlCommand scSqlCommand = new MySqlCommand("SELECT Contrasena_tercero FROM " + bd2 + ".terceros where Nit_tercero =" + Session["usuario"].ToString(), Conexion.ObtenerCnMysql());
                    MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                    DataSet dsDataSet = new DataSet();
                    DataTable dtDataTable = null;

                    Conexion.AbrirCnMysql();
                    sdaSqlDataAdapter.Fill(dsDataSet);
                    dtDataTable = dsDataSet.Tables[0];
                    if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                    {
                        string Contrasena = dtDataTable.Rows[0].ItemArray[0].ToString();
                        if (Contrasena != TxtPass.Text)
                        {
                            args.IsValid = true;
                        }
                        else
                        {
                            bandera = 0;
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
                    Conexion.CerrarCnMysql();
                }
            }
        }
        #endregion

        #region Metodo BtnEditar_Click
        /* ********************************************************************************************************/
        /* Evento que se produce al dar clic sobre el boton BtnEditar para almacenar la informacion del tercero
        /* ********************************************************************************************************/
        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            if (bandera == 1)
            {
                Editar_Click();
            }
            if (BtnEditar.Text == "Insertar Cliente")
            {
                TxtCelular.Enabled = false;
                TxtCelular.BackColor = System.Drawing.Color.LightGray;
                TxtDir.Enabled = false;
                TxtDir.BackColor = System.Drawing.Color.LightGray;
                TxtNombres.Enabled = false;
                TxtNombres.BackColor = System.Drawing.Color.LightGray;
                TxtPass.Enabled = false;
                TxtPass.BackColor = System.Drawing.Color.LightGray;
                BtnEditar.Enabled = false;
                BtnEditar.BackColor = System.Drawing.Color.LightGray;
                BtnPass.Enabled = false;
                BtnPass.BackColor = System.Drawing.Color.LightGray;

                RequiredContrasena.Visible = false;
                ValidatorPass.Visible = false;

                MensajeError("La información se ha Almacenado correctamente");
            }
        }
        #endregion

        #region Metodo Editar_Click
        /* ****************************************************************************/
        /* Metodo que Bloquea los campos y actualiza el tercero al Dar Clic en el Boton Almacenar Informacion
        /* ****************************************************************************/
        protected void Editar_Click()
        {
            int R = ActualizaInfoTercero(Session["usuario"].ToString());
            if (R == 1)
            {
                MensajeError("La información se ha actualizado correctamente");

                TxtCelular.Enabled = false;
                TxtCelular.BackColor = System.Drawing.Color.LightGray;
                TxtDir.Enabled = false;
                TxtDir.BackColor = System.Drawing.Color.LightGray;
                TxtCorreo.Enabled = false;
                TxtCorreo.BackColor = System.Drawing.Color.LightGray;
                TxtNombres.Enabled = false;
                TxtNombres.BackColor = System.Drawing.Color.LightGray;
                TxtPass.Enabled = false;
                TxtPass.BackColor = System.Drawing.Color.LightGray;
                BtnEditar.Enabled = false;
                BtnEditar.BackColor = System.Drawing.Color.LightGray;
                BtnPass.Enabled = false;
                BtnPass.BackColor = System.Drawing.Color.LightGray;

                RequiredContrasena.Visible = false;
                ValidatorPass.Visible = false;
            }
            else
            {
                MensajeError("La información no se ha actualizado correctamente. Pongase en contacto con el Administrador.");
            }
        }
        #endregion

        #region Metodo ActualizaInfoTercero
        /* ****************************************************************************/
        /* Metodo que actualiza la informacion del tercero en la Base de datos
        /* ****************************************************************************/
        public int ActualizaInfoTercero(string Nit_tercero)
        {
            CnMysql Conexion = new CnMysql(Cn);
            int res = 0;

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd = new MySqlCommand(bd2 +".sp_ActualizaTercero", Conexion.ObtenerCnMysql());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NitTercero", TxtDoc.Text);
                cmd.Parameters.AddWithValue("@nombreTercero", TxtNombres.Text);
                cmd.Parameters.AddWithValue("@Celular", TxtCelular.Text);
                cmd.Parameters.AddWithValue("@direccion", TxtDir.Text);
                cmd.Parameters.AddWithValue("@correo", TxtCorreo.Text);
                cmd.Parameters.AddWithValue("@Activo", SqlDbType.Bit).Value = true;

                if (Session["cambiaContrasena"] != null)
                {
                    if (Session["cambiaContrasena"].ToString() == "True")
                    {
                        cmd.Parameters.AddWithValue("@contrasena", TxtPass.Text);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@contrasena", Session["Contrasena"].ToString());
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@contrasena", Session["Contrasena"].ToString());
                }

                // Crea un parametro de salida para el SP
                MySqlParameter outputIdParam = new MySqlParameter("@respuesta", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(outputIdParam);
                cmd.ExecuteNonQuery();

                //Almacena la respuesta de la variable de retorno del SP
                res = int.Parse(outputIdParam.Value.ToString());
                return res;
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                return res;
            }
            finally
            {
                Conexion.CerrarCnMysql();
                Session.Remove("Contrasena");
                Session.Remove("cambiaContrasena");
            }
        }
        #endregion

        #endregion
    }
}