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
    public partial class ParametrosObjetivos : System.Web.UI.Page
    {
        string CnMysql = ConfigurationManager.ConnectionStrings["CadenaConexioMySql3"].ConnectionString.ToString();
        string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql"].ConnectionString.ToString();
        string bd3 = ConfigurationManager.AppSettings["BD3"].ToString();
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

                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'ParametrosObjetivos.aspx' AND idEmpresa = '" + Session["idEmpresa"] + "'", MySqlCn);
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

        #endregion

        /// <summary>
        /// Busca los datos del usuario
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento e</param>
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            LblMsj.Text = string.Empty;
            LblMsj.Visible = false;
            UpdatePanel3.Update();
            
            try
            {
                MySqlCn = new MySqlConnection(CnMysql);
                MySqlCommand scSqlCommand;
                string consulta = "SELECT * FROM " + bd3 + ".parametrosgenerales where ano = " + ddlAnio.SelectedValue
                                                                               + " AND idTercero = '" + Session["usuario"]
                                                                               + "' AND idCompania = '" + Session["proyecto"] + "';";

                scSqlCommand = new MySqlCommand(consulta, MySqlCn);

                MySqlCn.Open();
                MySqlDataReader rd = scSqlCommand.ExecuteReader();

                if (rd.HasRows)
                {
                    if (rd.Read())
                    {
                        txtMin.Text = rd["Min_Objetivos"].ToString();
                        txtMax.Text = rd["Max_Objetivos"].ToString();
                    }

                    BtnEditar.Text = "Actualizar";
                }
                else 
                {
                    txtMin.Text = string.Empty;
                    txtMax.Text = string.Empty;
                    BtnEditar.Text = "Guardar";
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
        /// Carga el año actual y el pasado
        /// </summary>
        public void CargarAnio() 
        {
            try 
            {
                DateTime fechaAnioActual = DateTime.Now;
                int fechaAnioPasado = fechaAnioActual.Year - 1;

                ddlAnio.Items.Add(new ListItem(fechaAnioPasado.ToString(), fechaAnioPasado.ToString()));
                ddlAnio.Items.Add(new ListItem(fechaAnioActual.Year.ToString(), fechaAnioActual.Year.ToString()));
            }
            catch(Exception ex)
            {
            }
        }

        /// <summary>
        /// Edita o guarda la informacion del formulario
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento e</param>
        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            CnMysql Conexion = new CnMysql(CnMysql);
            int res = 0;

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd;

                if (BtnEditar.Text == "Guardar")
                {
                    cmd = new MySqlCommand("sp_CrearParametrosGenerales", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;                    
                }
                else
                {
                    cmd = new MySqlCommand("sp_ActualizarParametrosGenerales", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                }

                cmd.Parameters.AddWithValue("@idTercero", Session["usuario"]);
                cmd.Parameters.AddWithValue("@idCompania", Session["proyecto"]);
                cmd.Parameters.AddWithValue("@Max_Objetivos", txtMax.Text);
                cmd.Parameters.AddWithValue("@Min_Objetivos", txtMin.Text);
                cmd.Parameters.AddWithValue("@Ano", ddlAnio.SelectedValue);

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

                    if (BtnEditar.Text == "Guardar")
                    {
                        MensajeError("Parametros creados correctamente");
                    }
                    else
                    {
                        MensajeError("Parametros actualizados correctamente");
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
        /// Cancela el formulario actual
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento e</param>
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            txtMin.Text = string.Empty;
            txtMax.Text = string.Empty;

            Container_UpdatePanel2.Visible = false;
            UpdatePanel1.Update();
        }
    }
}