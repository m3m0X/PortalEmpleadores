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
    public partial class PeriodoExtra : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql"].ConnectionString.ToString();
        string Cn3 = ConfigurationManager.ConnectionStrings["CadenaConexioMySql3"].ConnectionString.ToString();
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

                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'PeriodoExtra.aspx' AND idEmpresa = '" + Session["idEmpresa"] + "'", MySqlCn);
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

        #region Eventos

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
        /// Limpia los controles creados
        /// </summary>
        public void LimpiarControles() 
        {
            txtFechaInicio.Text = string.Empty;
            txtFechaFin.Text = string.Empty;
            gvJefeEmpleado.DataSource = null;
            gvJefeEmpleado.DataBind();
        }

        #endregion

        #region Controles

        /// <summary>
        /// Busca los usuarios en JefeEmpleado
        /// </summary>
        /// <param name="sender">Objeto sender</param>
        /// <param name="e">Evento e</param>
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            LblMsj.Text = string.Empty;
            LblMsj.Visible = false;
            UpdatePanel3.Update();

            try
            {
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;

                MySqlCn = new MySqlConnection(Cn);
                MySqlCommand scSqlCommand;
                string consulta = "SELECT jefeempleado.idJefeEmpleado, " +
                                  "jefeempleado.idTercero, " +
                                  "jefeempleado.idCompania, " +
                                  "jefeempleado.Cedula_Empleado, " +
                                  "empleados.Nombres_Completos_Empleado " +
                                  "FROM pru_modobjetivos.jefeempleado " +
                                  "INNER JOIN pru_trabajadores.empleados ON jefeempleado.Cedula_Empleado = empleados.Id_Empleado " +
                                  "where idCompania = '" + Session["proyecto"] +
                                  "' AND Cedula_Jefe = " + txtJefe.Text +
                                  " AND Cedula_Empleado = " + txtUser.Text +
                                  " AND Ano = '" + ddlAnio.SelectedValue + "';";

                scSqlCommand = new MySqlCommand(consulta, MySqlCn);
                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    gvJefeEmpleado.DataSource = dtDataTable;
                }
                else
                {
                    gvJefeEmpleado.DataSource = null;
                    MensajeError("No se encuentra el registro.");
                }

                gvJefeEmpleado.DataBind();
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
        /// Eventos de la grilla Jefe Empleado
        /// </summary>
        /// <param name="sender">Objeto sender</param>
        /// <param name="e">Evento grilla e</param>
        protected void gvJefeEmpleado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LblMsj.Visible = false;
            UpdatePanel3.Update();
            
            try
            {
                if (e.CommandName == "Periodos")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:CargarCalendario(); ", true);
                    Container_UpdatePanel1.Visible = false;
                    Container_UpdatePanel2.Visible = true;
                    UpdatePanel1.Update();
                    Session.Add("idJefeEmpleado", e.CommandArgument);
                }
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// Guarda el formulario de fechas
        /// </summary>
        /// <param name="sender">Objeto sender</param>
        /// <param name="e">Evento e</param>
        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            CnMysql Conexion = new CnMysql(Cn3);
            int res = 0;

            try
            {
                Conexion.AbrirCnMysql();

                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;

                MySqlCn = new MySqlConnection(Cn);
                MySqlCommand scSqlCommand;
                string consulta = "SELECT * FROM pru_modobjetivos.fechasetapas" + 
                                  " where ('" + txtFechaInicio.Text + "' > fechasetapas.Fecha_Limite" +
                                  " or '" + txtFechaFin.Text + "' > fechasetapas.Fecha_Limite)" +
                                  " and fechasetapas.Etapas_idEtapas = " + ddlEtapas.SelectedValue + 
                                  " and Ano = " + DateTime.Now.Year + ";";

                scSqlCommand = new MySqlCommand(consulta, MySqlCn);
                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    MensajeError("No puede agregar el periodo ya que sobrepasa la fecha limite para la etapa seleccionada");
                }
                else 
                {
                    MySqlCommand cmd;

                    cmd = new MySqlCommand("sp_CrearPeriodoExtra", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Etapas_idEtapas", ddlEtapas.SelectedValue);
                    cmd.Parameters.AddWithValue("@JefeEmpleado_idJefeEmpleado", Session["idJefeEmpleado"]);
                    cmd.Parameters.AddWithValue("@Fecha_Inicio", DateTime.Parse(txtFechaInicio.Text));
                    cmd.Parameters.AddWithValue("@Fecha_Fin", DateTime.Parse(txtFechaFin.Text));

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
                        MensajeError("Parametros creados correctamente");
                    }
                }
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            finally
            {
                this.LimpiarControles();
                Container_UpdatePanel1.Visible = true;
                Container_UpdatePanel2.Visible = false;
                UpdatePanel1.Update();
                Conexion.CerrarCnMysql();
            }
        }

        /// <summary>
        /// Cancela el formulario actual
        /// </summary>
        /// <param name="sender">Objeto sender</param>
        /// <param name="e">Evento e</param>
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            this.LimpiarControles();
            Container_UpdatePanel1.Visible = true;
            Container_UpdatePanel2.Visible = false;
            UpdatePanel1.Update();
        }

        #endregion
    }
}