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
    public partial class NivelCompetencias : System.Web.UI.Page
    {
        string Cnbasica = ConfigurationManager.ConnectionStrings["CadenaConexioMySql"].ConnectionString.ToString();
        string Cntrabajadores = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();
        string Cncompetencias = ConfigurationManager.ConnectionStrings["CadenaConexioMySql4"].ConnectionString.ToString();
        string bdTrabajadores = ConfigurationManager.AppSettings["BD2"].ToString();
        string bdCompetencias = ConfigurationManager.AppSettings["BD4"].ToString();
        MySqlConnection MySqlCn;

        #region Metodo Page Load

        /// <summary>
        /// Page load de la pagina
        /// </summary>
        /// <param name="sender">Objeto Sender</param>
        /// <param name="e">Evento e</param>
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
                        MySqlCn = new MySqlConnection(Cnbasica);

                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'NivelCompetencias.aspx' AND idEmpresa = '" + Session["idEmpresa"].ToString() + "'", MySqlCn);
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

        /// <summary>
        /// Limpia los mensajes
        /// </summary>
        private void LimpiarMensajes()
        {
            LblMsj.Visible = false;
            UpdatePanel3.Update();
        }

        #endregion

        #region Funciones

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
        /// Carga la grilla actualizada
        /// </summary>
        private void CargarGrid(string ano)
        {
            this.LimpiarMensajes();
            CnMysql MysqlCn = new CnMysql(Cncompetencias);

            try
            {
                Session.Add("anoVigente", ano);

                DataTable dtDataTable = null;
                MysqlCn.AbrirCnMysql();
                dtDataTable = MysqlCn.ConsultarRegistros("SELECT * FROM " + bdCompetencias + ".nivelcompetencias"
                                                         + " where idTercero = " + Session["usuario"]
                                                         + " and idCompania = '" + Session["proyecto"]
                                                         + "' and ano = '" + Session["anoVigente"]
                                                         + "' and idEmpresa = '" + Session["idEmpresa"] + "';");

                Session.Add("DataNivel", dtDataTable);

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    gvNivelesCreados.DataSource = dtDataTable;                    
                }
                else 
                {
                    gvNivelesCreados.DataSource = null;
                }

                gvNivelesCreados.DataBind();
                Container_UpdatePanel2.Visible = false;
                UpdatePanel1.Update();
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Sin RED");
            }
            finally
            {
                MysqlCn.CerrarCnMysql();
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Busca los datos del usuario
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento e</param>
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            LimpiarMensajes();
            Container_UpdatePanelAnio.Visible = false;
            Container_UpdatePanel1.Visible = true;
            UpdatePanel1.Update();

            try
            {
                CargarGrid(ddlAnio.SelectedValue);
            }
            catch (Exception ex)
            {
                MensajeError("Ha ocurrido el siguiente error: " + ex.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// Regresa a la seleccion de año
        /// </summary>
        /// <param name="sender">Objeto Sender</param>
        /// <param name="e">Evento e</param>
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            LimpiarMensajes();
            Container_UpdatePanelAnio.Visible = true;
            Container_UpdatePanel1.Visible = false;
            Container_UpdatePanel2.Visible = false;
            UpdatePanel1.Update();
        }

        /// <summary>
        /// Habilita el menu de crear un cargo
        /// </summary>
        /// <param name="sender">Objeto Sender</param>
        /// <param name="e">Evento e</param>
        protected void btnNivel_Click(object sender, EventArgs e)
        {
            try
            {
                this.LimpiarMensajes();
                txtNombre.Text = string.Empty;
                txtMin.Text = string.Empty;
                txtMax.Text = string.Empty;
                Container_UpdatePanel2.Visible = true;
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                MensajeError(ex.Message);
            }
        }

        /// <summary>
        /// Guarda un area creada
        /// </summary>
        /// <param name="sender">Objeto Sender</param>
        /// <param name="e">Evento e</param>
        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            this.LimpiarMensajes();
            CnMysql Conexion = new CnMysql(Cncompetencias);
            int res = 0;

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd;

                if (BtnEditar.Text == "Guardar")
                {
                    cmd = new MySqlCommand("sp_CrearNivelCompetencias", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idTercero", Session["usuario"]);
                    cmd.Parameters.AddWithValue("@idCompania", Session["proyecto"]);
                    cmd.Parameters.AddWithValue("@idEmpresa", Session["idEmpresa"]);
                    cmd.Parameters.AddWithValue("@estadoNivel", true);
                    cmd.Parameters.AddWithValue("@ano", Session["anoVigente"]);
                }
                else
                {
                    cmd = new MySqlCommand("sp_ActualizarNivelCompetencias", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idNivelCompetencias", Convert.ToInt32(Session["idNivelCompetencias"]));
                }

                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@rangoMin", txtMin.Text);
                cmd.Parameters.AddWithValue("@rangoMax", txtMax.Text);
                cmd.Parameters.AddWithValue("@planDesarrollo", cbPlan.Checked);

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
                    this.CargarGrid(Session["anoVigente"].ToString());
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
        /// Oculta y restablece el formulario de area
        /// </summary>
        /// <param name="sender">Objeto Sender</param>
        /// <param name="e">Evento e</param>
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            this.LimpiarMensajes();
            txtNombre.Text = string.Empty;
            txtMax.Text = string.Empty;
            txtMin.Text = string.Empty;
            this.CargarGrid(Session["anoVigente"].ToString());
        }

        /// <summary>
        /// Acciones que ejecuta la grilla
        /// </summary>
        /// <param name="sender">Objeto Sender</param>
        /// <param name="e">Evento e</param>
        protected void gvNivelesCreados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            this.LimpiarMensajes();
            CnMysql Conexion = new CnMysql(Cncompetencias);
            int res = 0;

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd;

                if (e.CommandName == "Editar")
                {
                    GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int RowIndex = gvr.RowIndex;

                    txtNombre.Text = gvNivelesCreados.Rows[RowIndex].Cells[0].Text;
                    txtMin.Text = gvNivelesCreados.Rows[RowIndex].Cells[1].Text;
                    txtMax.Text = gvNivelesCreados.Rows[RowIndex].Cells[2].Text;
                    cbPlan.Checked = (gvNivelesCreados.Rows[RowIndex].Cells[3].Text == "1") ? true : false;

                    BtnEditar.Text = "Editar";
                    Container_UpdatePanel2.Visible = true;
                    UpdatePanel1.Update();
                    Session.Add("idNivelCompetencias", e.CommandArgument);
                }
                else if (e.CommandName == "On")
                {
                    cmd = new MySqlCommand("sp_EstadoNivelCompetencia", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idNivelCompetencia", Convert.ToInt32(e.CommandArgument));
                    cmd.Parameters.AddWithValue("@activo", false);

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
                        this.CargarGrid(Session["anoVigente"].ToString());
                    }
                    else if (res == 2)
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:CargarMensaje('No se puede desactivar porque esta asociado a uno o más empleados.'); ", true);
                    }
                }
                else if (e.CommandName == "Off")
                {
                    cmd = new MySqlCommand("sp_EstadoNivelCompetencia", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idNivelCompetencia", Convert.ToInt32(e.CommandArgument));
                    cmd.Parameters.AddWithValue("@activo", true);

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
                        this.CargarGrid(Session["anoVigente"].ToString());
                    }
                }
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// Al cargar la grilla modifica el valor de las imagenes
        /// </summary>
        /// <param name="sender">Sender objeto</param>
        /// <param name="e">evento e</param>
        protected void gvNivelesCreados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            this.LimpiarMensajes();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgbtnON = (ImageButton)e.Row.FindControl("btnON");
                ImageButton imgbtnOFF = (ImageButton)e.Row.FindControl("btnOFF");

                string estado = DataBinder.Eval(e.Row.DataItem, "estadoNivel").ToString();

                if (estado == "1")
                {
                    imgbtnON.Visible = true;
                    imgbtnOFF.Visible = false;
                }
                else
                {
                    imgbtnON.Visible = false;
                    imgbtnOFF.Visible = true;
                }
            }
        }

        /// <summary>
        /// Index de la pagina
        /// </summary>
        /// <param name="sender">Sender objeto</param>
        /// <param name="e">evento e</param>
        protected void gvNivelesCreados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.LimpiarMensajes();
            DataTable datos = (DataTable)Session["DataNivel"];
            GridView gv = (GridView)sender;
            gv.PageIndex = e.NewPageIndex;

            if (datos != null)
            {
                gv.DataSource = datos;
                gv.DataBind();
            }
        }

        #endregion        
    }
}