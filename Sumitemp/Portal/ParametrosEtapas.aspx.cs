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
    public partial class ParametrosEtapas : System.Web.UI.Page
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

                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'ParametrosEtapas.aspx' AND idEmpresa = '" + Session["idEmpresa"] + "'", MySqlCn);
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
                        this.CargarEtapas(DateTime.Now.Year);
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

        #region funciones

        /// <summary>
        /// Carga el año actual y el pasado
        /// </summary>
        public void CargarAnio()
        {
            try
            {
                ConsultasGenerales consultas = new ConsultasGenerales();
                DataTable datos = consultas.ConsultarAnos(Session["proyecto"].ToString(),
                                                          Session["idEmpresa"].ToString());

                if (datos != null)
                {
                    ddlAnio.DataTextField = "Ano";
                    ddlAnio.DataValueField = "Ano";
                    ddlAnio.DataSource = datos;
                    ddlAnio.DataBind();
                }
                else
                {
                    DateTime fechaAnioActual = DateTime.Now;
                    ddlAnio.Items.Add(new ListItem(fechaAnioActual.Year.ToString(), fechaAnioActual.Year.ToString()));
                }
            }
            catch (Exception ex)
            {
                MensajeError(ex.Message);
            }
        }

        /// <summary>
        /// Devuelve el resultado de las etapas dependiendo del año
        /// </summary>
        /// <param name="anio">Año Seleccionado</param>
        public void CargarEtapas(int anio)
        {
            try
            {
                ConsultasGenerales consultas = new ConsultasGenerales();
                Session.Add("sesionPeriodo", consultas.ConsultarPeriodoSeguimiento(Session["proyecto"].ToString(),
                                                                                   Session["idEmpresa"].ToString(),
                                                                                   anio));

                if (Session["sesionPeriodo"].ToString() == "0")
                {
                    this.MensajeError("No se han establecido los parametros generales para el año actual. Por favor definalos antes de continuar");
                    ddlEtapas.DataSource = null;
                    Container_UpdatePanel1.Visible = false;
                    UpdatePanel1.Update();
                }
                else
                {
                    ddlEtapas.DataTextField = "Etapa";
                    ddlEtapas.DataValueField = "idEtapas";
                    ddlEtapas.DataSource = consultas.ConsultarEtapasActivas(Session["sesionPeriodo"].ToString());
                }

                ddlEtapas.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region eventos

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
                string consulta = "SELECT * FROM " + bd3 + ".fechasetapas where Ano = '" + ddlAnio.SelectedValue
                                + "' AND Etapas_idEtapas = " + ddlEtapas.SelectedValue + ";";

                scSqlCommand = new MySqlCommand(consulta, MySqlCn);

                MySqlCn.Open();
                MySqlDataReader rd = scSqlCommand.ExecuteReader();

                lblEtapa.Text = ddlEtapas.SelectedItem.ToString();

                if (rd.HasRows)
                {
                    if (rd.Read())
                    {
                        txtFechaInicio.Text = DateTime.Parse(rd["Fecha_Inicio"].ToString()).ToString("yyyy/MM/dd");
                        txtFechaFin.Text = DateTime.Parse(rd["Fecha_Fin"].ToString()).ToString("yyyy/MM/dd");
                        txtCorteInicio.Text = DateTime.Parse(rd["Corte_Inicio"].ToString()).ToString("yyyy/MM/dd");
                        txtCorteFin.Text = DateTime.Parse(rd["Corte_Fin"].ToString()).ToString("yyyy/MM/dd");
                        txtLimite.Text = DateTime.Parse(rd["Fecha_Limite"].ToString()).ToString("yyyy/MM/dd");
                    }

                    BtnEditar.Text = "Actualizar";
                }
                else
                {
                    txtFechaInicio.Text = string.Empty;
                    txtFechaFin.Text = string.Empty;
                    txtCorteInicio.Text = string.Empty;
                    txtCorteFin.Text = string.Empty;
                    txtLimite.Text = string.Empty;
                    BtnEditar.Text = "Guardar";
                }

                ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:CargarCalendario(); ", true);
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
                    cmd = new MySqlCommand("sp_CrearFechaEtapas", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd = new MySqlCommand("sp_ActualizarFechaEtapas", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                }

                cmd.Parameters.AddWithValue("@Etapas_idEtapas", ddlEtapas.SelectedValue);
                cmd.Parameters.AddWithValue("@Fecha_Inicio", DateTime.Parse(txtFechaInicio.Text));
                cmd.Parameters.AddWithValue("@Fecha_Fin", DateTime.Parse(txtFechaFin.Text));
                cmd.Parameters.AddWithValue("@Corte_Inicio", DateTime.Parse(txtCorteInicio.Text));
                cmd.Parameters.AddWithValue("@Corte_Fin", DateTime.Parse(txtCorteFin.Text));
                cmd.Parameters.AddWithValue("@Fecha_Limite", DateTime.Parse(txtLimite.Text));
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
            txtFechaInicio.Text = string.Empty;
            txtFechaFin.Text = string.Empty;
            txtCorteInicio.Text = string.Empty;
            txtCorteFin.Text = string.Empty;
            txtLimite.Text = string.Empty;

            Container_UpdatePanel2.Visible = false;
            UpdatePanel1.Update();
        }

        /// <summary>
        /// Evento cuando se modifica el ddl
        /// </summary>
        /// <param name="sender">Objeto sender</param>
        /// <param name="e">evento e</param>
        protected void ddlAnio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.CargarEtapas(Convert.ToInt32(ddlAnio.SelectedValue));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}