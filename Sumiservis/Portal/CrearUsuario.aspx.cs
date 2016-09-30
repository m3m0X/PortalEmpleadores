using MySql.Data.MySqlClient;
using PortalTrabajadores.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PortalTrabajadores.Portal
{
    public partial class CrearUsuario : System.Web.UI.Page
    {
        string CnMysql = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();
        string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql"].ConnectionString.ToString();
        string bd2 = ConfigurationManager.AppSettings["BD2"].ToString();
        MySqlConnection MySqlCn;

        #region Metodo Page_Load

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

                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'CrearUsuario.aspx'", MySqlCn);
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
        }

        #endregion

        #region Metodo MensajeError

        /// <summary>
        /// Mensaje de error
        /// </summary>
        /// <param name="Msj">Mensaje de error</param>
        public void MensajeError(string Msj)
        {
            LblMsj.Text = Msj;
            LblMsj.Visible = true;
            UpdatePanel3.Update();
        }

        /// <summary>
        /// Limpia los mensajes
        /// </summary>
        private void LimpiarMensajes()
        {
            LblMsj.Visible = false;
            UpdatePanel3.Update();
        }

        #endregion

        /// <summary>
        /// Busca los datos del usuario
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento e</param>
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            this.LimpiarMensajes();
            Session["txtUser"] = txtUser.Text;

            try
            {
                this.LimpiarActivarCampos();

                MySqlCn = new MySqlConnection(CnMysql);
                MySqlCommand scSqlCommand;
                string consulta = "SELECT * FROM " + bd2 + ".usuariotercero where" +
                                  " Terceros_Nit_Tercero = " + Session["usuario"] +
                                  " AND nombreUsuario = '" + Session["txtUser"] + "';";

                scSqlCommand = new MySqlCommand(consulta, MySqlCn);

                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;
                MySqlCn.Open();
                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    BtnEditar.Text = "Editar Información";

                    txtUser2.Text = Session["txtUser"].ToString();
                    ddlRol.SelectedValue = dtDataTable.Rows[0]["id_Rol"].ToString();
                    cbActivo.Checked = dtDataTable.Rows[0]["activo"].ToString() == "1" ? true : false;
                }
                else
                {
                    BtnEditar.Text = "Guardar Información";
                    txtUser2.Text = Session["txtUser"].ToString();
                }

                Container_UpdatePanel2.Visible = true;
                UpdatePanel1.Update();
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

        /// <summary>
        /// Edita los datos del usuario
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento e</param>
        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            this.LimpiarMensajes();
            CnMysql Conexion = new CnMysql(CnMysql);
            int res = 0;

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd;

                if (BtnEditar.Text == "Guardar Información")
                {
                    cmd = new MySqlCommand("sp_CrearUsuarioTer", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Pass", CifrarContrasena.HashSHA1(txtUser2.Text));
                }
                else
                {
                    cmd = new MySqlCommand("sp_ActualizaUsuarioTer", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                }

                cmd.Parameters.AddWithValue("@NitTercero", Session["usuario"]);
                cmd.Parameters.AddWithValue("@Usuario", txtUser2.Text);
                cmd.Parameters.AddWithValue("@activo", 1);
                cmd.Parameters.AddWithValue("@rol", ddlRol.SelectedValue);

                // Crea un parametro de salida para el SP
                MySqlParameter outputIdParam = new MySqlParameter("@respuesta", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(outputIdParam);
                cmd.ExecuteNonQuery();

                //Almacena la respuesta de la variable de retorno del SP
                res = int.Parse(outputIdParam.Value.ToString());

                if (res == 1)
                {
                    Container_UpdatePanel2.Visible = false;
                    UpdatePanel1.Update();

                    if (BtnEditar.Text == "Guardar Información")
                    {
                        MensajeError("Usuario creado correctamente");
                    }
                    else
                    {
                        MensajeError("Se realizo la actualización correctamente");
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

        /// <summary>
        /// Cancela el formulario creado
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento e</param>
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                txtUser.Text = string.Empty;
                Container_UpdatePanel2.Visible = false;
                UpdatePanel1.Update();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Activa los campos
        /// </summary>
        public void LimpiarActivarCampos()
        {
            this.LimpiarMensajes();

            txtUser.Text = string.Empty;
            txtUser2.Text = string.Empty;
            ddlRol.SelectedValue = "3";

            Container_UpdatePanel2.Visible = false;
            UpdatePanel1.Update();
        }

        /// <summary>
        /// Restablece la contraseña
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnPass_Click(object sender, EventArgs e)
        {
            this.LimpiarMensajes();
            CnMysql Conexion = new CnMysql(CnMysql);
            int res = 0;

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd;

                cmd = new MySqlCommand("sp_ActualizaPassUsuarioTer", Conexion.ObtenerCnMysql());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NitTercero", Session["usuario"]);
                cmd.Parameters.AddWithValue("@Usuario", txtUser2.Text);
                cmd.Parameters.AddWithValue("@Pass", CifrarContrasena.HashSHA1(txtUser2.Text));

                // Crea un parametro de salida para el SP
                MySqlParameter outputIdParam = new MySqlParameter("@respuesta", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(outputIdParam);
                cmd.ExecuteNonQuery();

                //Almacena la respuesta de la variable de retorno del SP
                res = int.Parse(outputIdParam.Value.ToString());

                if (res == 1)
                {
                    Container_UpdatePanel2.Visible = false;
                    UpdatePanel1.Update();
                    MensajeError("Se restablecio la contraseña");
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
}