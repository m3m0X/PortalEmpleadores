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
    public partial class AsignarCompetencias : System.Web.UI.Page
    {
        string CnBasica = ConfigurationManager.ConnectionStrings["CadenaConexioMySql"].ConnectionString.ToString();
        string CnTrabajadores = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();
        string CnCompetencias = ConfigurationManager.ConnectionStrings["CadenaConexioMySql4"].ConnectionString.ToString();
        string bdTrabajadores = ConfigurationManager.AppSettings["BD2"].ToString();
        string bdModCompetencias = ConfigurationManager.AppSettings["BD4"].ToString();
        MySqlConnection MySqlCn;
        ConsultasGenerales consultas;

        #region Metodo Page Load

        /// <summary>
        /// Page load de la pagina
        /// </summary>
        /// <param name="sender">Objeto Sender</param>
        /// <param name="e">Evento e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            consultas = new ConsultasGenerales();

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
                        MySqlCn = new MySqlConnection(CnBasica);

                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'AsignarCompetencias.aspx'", MySqlCn);
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

            try
            {
                Session.Add("anoVigente", ano);

                DataTable dtDataTable = consultas.ConsultarCargos(Session["usuario"].ToString(),
                                                                  Session["idEmpresa"].ToString(),
                                                                  Session["anoVigente"].ToString());

                Session.Add("DataCargos", dtDataTable);

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    gvCargosCreados.DataSource = dtDataTable;
                }
                else
                {
                    gvCargosCreados.DataSource = null;
                }

                gvCargosCreados.DataBind();
                UpdatePanel1.Update();
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Sin RED");
            }
        }

        /// <summary>
        /// Carga la grilla actualizada
        /// </summary>
        private void CargarCompetenciasGrid()
        {
            this.LimpiarMensajes();

            try
            {
                DataTable dtDataTable = consultas.ConsultarCompetenciasXCargo(Session["proyecto"].ToString(),
                                                                              Session["idEmpresa"].ToString(),
                                                                              Convert.ToInt32(Session["idCargos"].ToString()),
                                                                              Session["anoVigente"].ToString());

                Session.Add("DataCompetencias", dtDataTable);

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    gvCompetenciasCreadas.DataSource = dtDataTable;
                }
                else
                {
                    gvCompetenciasCreadas.DataSource = null;
                }

                gvCompetenciasCreadas.DataBind();
                UpdatePanel1.Update();
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Sin RED");
            }
        }

        /// <summary>
        /// Devuelve el resultado de nivel competencias
        /// </summary>
        /// <param name="anio">ddl nivel competencias</param>
        private void CargarCompetenciasDll()
        {
            try
            {
                DataTable dtCompetencias = consultas.ConsultarCompetencias(Session["usuario"].ToString(),
                                                                           Session["proyecto"].ToString(),
                                                                           Session["idEmpresa"].ToString(),
                                                                           Session["anoVigente"].ToString());

                if (dtCompetencias == null)
                {
                    this.MensajeError("No se han establecido competencias, definalos primero antes de continuar");
                    ddlCompetencias.DataSource = null;
                }
                else
                {
                    ddlCompetencias.DataTextField = "competencia";
                    ddlCompetencias.DataValueField = "idCompetencia";
                    ddlCompetencias.DataSource = dtCompetencias;
                }

                ddlCompetencias.DataBind();
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Devuelve el resultado de nivel competencias
        /// </summary>
        private void CargarNivelCompetencias()
        {
            try
            {
                DataTable dtCompetencias = consultas.ConsultarNivelCompetencias(Session["usuario"].ToString(),
                                                                                Session["proyecto"].ToString(),
                                                                                Session["idEmpresa"].ToString(),
                                                                                Session["anoVigente"].ToString());

                if (dtCompetencias == null)
                {
                    this.MensajeError("No se han establecido niveles de competencias, definalos primero antes de continuar");
                    ddlNivelCompetencia.DataSource = null;
                }
                else
                {
                    ddlNivelCompetencia.DataTextField = "nombre";
                    ddlNivelCompetencia.DataValueField = "idNivelCompetencias";
                    ddlNivelCompetencia.DataSource = dtCompetencias;
                }

                ddlNivelCompetencia.DataBind();
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga las grillas de conductas disponibles
        /// </summary>
        /// <param name="idCargo">id Cargo.</param>
        /// <param name="idCompetencia">id competencia.</param>
        /// <param name="idCompania">id compañia.</param>
        /// <param name="idEmpresa">id empresa.</param>
        private void CargarGrillasConductas(string idCargo, string idCompetencia, string idCompania, string idEmpresa)
        {
            try
            {
                DataTable dtCargoConductas = consultas.ConsultarCargosConductas(idCargo,
                                                                                idCompetencia,
                                                                                idCompania,
                                                                                idEmpresa,
                                                                                Session["anoVigente"].ToString());

                Session.Add("dtCargoConductas", dtCargoConductas);

                DataTable dtConductasDisponibles = consultas.ConsultaConductasActivas(dtCargoConductas,
                                                                                      Session["usuario"].ToString(),
                                                                                      idCompania,
                                                                                      idEmpresa,
                                                                                      Session["anoVigente"].ToString());

                Session.Add("dtConductasDisponibles", dtConductasDisponibles);

                if (dtCargoConductas == null)
                {
                    gvConductas.DataSource = null;
                }
                else
                {
                    gvConductas.DataSource = dtCargoConductas;
                }

                gvConductas.DataBind();

                if (dtConductasDisponibles == null)
                {
                    gvAgregarConducta.DataSource = null;
                }
                else
                {
                    gvAgregarConducta.DataSource = dtConductasDisponibles;
                }

                gvAgregarConducta.DataBind();

                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                throw ex;
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
                Container_UpdatePanel2.Visible = false;
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
        /// Guarda un area creada
        /// </summary>
        /// <param name="sender">Objeto Sender</param>
        /// <param name="e">Evento e</param>
        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            this.LimpiarMensajes();
            CnMysql Conexion = new CnMysql(CnCompetencias);
            int res = 0;

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd;

                cmd = new MySqlCommand("sp_AsignarCompetencias", Conexion.ObtenerCnMysql());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCargo", Session["idCargos"]);
                cmd.Parameters.AddWithValue("@idCompetencia", ddlCompetencias.SelectedValue);
                cmd.Parameters.AddWithValue("@idNivelCompetencias", ddlNivelCompetencia.SelectedValue);
                cmd.Parameters.AddWithValue("@idTercero", Session["usuario"]);
                cmd.Parameters.AddWithValue("@idCompania", Session["proyecto"]);
                cmd.Parameters.AddWithValue("@idEmpresa", Session["idEmpresa"]);
                cmd.Parameters.AddWithValue("@estado", true);
                cmd.Parameters.AddWithValue("@ano", Session["anoVigente"]);

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
                    this.CargarCompetenciasGrid();
                    this.CargarGrid(Session["anoVigente"].ToString());
                    this.MensajeError("La competencia fue guardada correctamente.");
                }
                if (res == 2)
                {
                    this.CargarCompetenciasGrid();
                    this.CargarGrid(Session["anoVigente"].ToString());
                    this.MensajeError("La competencia fue actualizada.");
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
        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.LimpiarMensajes();
            Container_UpdatePanel2.Visible = false;
            this.CargarGrid(Session["anoVigente"].ToString());
        }

        /// <summary>
        /// Boton regresar
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento e</param>
        protected void BtnRegresarCargo_Click(object sender, EventArgs e)
        {
            this.lblTitulo.Text = "Asignar Competencias a Cargos";
            this.CargarCompetenciasDll();
            this.CargarNivelCompetencias();
            this.CargarCompetenciasGrid();

            Container_UpdatePanel1.Visible = true;
            Container_UpdatePanel2.Visible = true;
            Container_UpdatePanel3.Visible = false;
            UpdatePanel1.Update();
        }

        /// <summary>
        /// Acciones que ejecuta la grilla
        /// </summary>
        /// <param name="sender">Objeto Sender</param>
        /// <param name="e">Evento e</param>
        protected void gvCargosCreados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            this.LimpiarMensajes();

            CnMysql Conexion = new CnMysql(CnTrabajadores);
            Conexion.AbrirCnMysql();

            try
            {
                if (e.CommandName == "Crear")
                {
                    GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int RowIndex = gvr.RowIndex;

                    Session.Add("idCargos", e.CommandArgument);
                    this.CargarCompetenciasDll();
                    this.CargarNivelCompetencias();
                    this.CargarCompetenciasGrid();
                    Container_UpdatePanel2.Visible = true;
                    UpdatePanel1.Update();
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
        protected void gvCargosCreados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            this.LimpiarMensajes();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgbtnON = (ImageButton)e.Row.FindControl("btnON");
                ImageButton imgbtnOFF = (ImageButton)e.Row.FindControl("btnOFF");

                string nCompetencias = DataBinder.Eval(e.Row.DataItem, "NCompetencias").ToString();

                if (nCompetencias == "0")
                {
                    imgbtnON.Visible = false;
                }
                else
                {
                    imgbtnON.Visible = true;
                }
            }
        }

        /// <summary>
        /// Index de la pagina
        /// </summary>
        /// <param name="sender">Sender objeto</param>
        /// <param name="e">evento e</param>
        protected void gvCargosCreados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.LimpiarMensajes();
            DataTable datos = (DataTable)Session["DataCargos"];
            GridView gv = (GridView)sender;
            gv.PageIndex = e.NewPageIndex;

            if (datos != null)
            {
                gv.DataSource = datos;
                gv.DataBind();
            }
        }

        /// <summary>
        /// Acciones que ejecuta la grilla
        /// </summary>
        /// <param name="sender">Objeto Sender</param>
        /// <param name="e">Evento e</param>
        protected void gvCompetenciasCreadas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            this.LimpiarMensajes();
            CnMysql Conexion = new CnMysql(CnCompetencias);
            int res = 0;

            try
            {
                Session.Add("idCompetencia", e.CommandArgument.ToString());

                Conexion.AbrirCnMysql();
                MySqlCommand cmd;

                if (e.CommandName == "On")
                {
                    cmd = new MySqlCommand("sp_EstadoCargoCompetencia", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCargo", Session["idCargos"]);
                    cmd.Parameters.AddWithValue("@idCompetencia", e.CommandArgument);
                    cmd.Parameters.AddWithValue("@idCompania", Session["proyecto"]);
                    cmd.Parameters.AddWithValue("@idEmpresa", Session["idEmpresa"]);
                    cmd.Parameters.AddWithValue("@estado", false);

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
                        this.CargarCompetenciasGrid();
                        this.CargarGrid(Session["anoVigente"].ToString());
                    }
                }
                else if (e.CommandName == "Off")
                {
                    cmd = new MySqlCommand("sp_EstadoCargoCompetencia", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCargo", Session["idCargos"]);
                    cmd.Parameters.AddWithValue("@idCompetencia", e.CommandArgument);
                    cmd.Parameters.AddWithValue("@idCompania", Session["proyecto"]);
                    cmd.Parameters.AddWithValue("@idEmpresa", Session["idEmpresa"]);
                    cmd.Parameters.AddWithValue("@estado", true);

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
                        this.CargarCompetenciasGrid();
                        this.CargarGrid(Session["anoVigente"].ToString());
                    }
                }
                else if (e.CommandName == "Conducta")
                {
                    this.lblTitulo.Text = "Asignar Conductas";

                    Container_UpdatePanel1.Visible = false;
                    Container_UpdatePanel2.Visible = false;
                    Container_UpdatePanel3.Visible = true;
                    UpdatePanel1.Update();

                    this.CargarGrillasConductas(Session["idCargos"].ToString(),
                                                Session["idCompetencia"].ToString(),
                                                Session["proyecto"].ToString(),
                                                Session["idEmpresa"].ToString());
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
        /// Al cargar la grilla modifica el valor de las imagenes
        /// </summary>
        /// <param name="sender">Sender objeto</param>
        /// <param name="e">evento e</param>
        protected void gvCompetenciasCreadas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgbtnON = (ImageButton)e.Row.FindControl("btnON");
                ImageButton imgbtnOFF = (ImageButton)e.Row.FindControl("btnOFF");

                string estado = DataBinder.Eval(e.Row.DataItem, "estado").ToString();

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
        protected void gvCompetenciasCreadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.LimpiarMensajes();
            DataTable datos = (DataTable)Session["DataCompetencias"];
            GridView gv = (GridView)sender;
            gv.PageIndex = e.NewPageIndex;

            if (datos != null)
            {
                gv.DataSource = datos;
                gv.DataBind();
            }
        }

        /// <summary>
        /// Comandos de la grilla conducta
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento e</param>
        protected void gvConductas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            this.LimpiarMensajes();
            CnMysql Conexion = new CnMysql(CnCompetencias);
            int res = 0;

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd;

                if (e.CommandName == "Off")
                {
                    cmd = new MySqlCommand("sp_ActualizarEstadoCargoConducta", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCarConCom", e.CommandArgument);
                    cmd.Parameters.AddWithValue("@estado", false);

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
                        this.CargarGrillasConductas(Session["idCargos"].ToString(),
                                                Session["idCompetencia"].ToString(),
                                                Session["proyecto"].ToString(),
                                                Session["idEmpresa"].ToString());
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
        /// Paginacion de las conductas
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento e</param>
        protected void gvConductas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.LimpiarMensajes();
            DataTable datos = (DataTable)Session["dtCargoConductas"];
            GridView gv = (GridView)sender;
            gv.PageIndex = e.NewPageIndex;

            if (datos != null)
            {
                gv.DataSource = datos;
                gv.DataBind();
            }
        }

        /// <summary>
        /// Comandos de la grilla agregar conducta
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento e</param>
        protected void gvAgregarConducta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            this.LimpiarMensajes();
            CnMysql Conexion = new CnMysql(CnCompetencias);
            int res = 0;

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd;

                if (e.CommandName == "On")
                {
                    bool existe = consultas.ExisteCargoConducta(e.CommandArgument.ToString(),
                                                                Session["proyecto"].ToString(),
                                                                Session["idEmpresa"].ToString(),
                                                                Session["idCargos"].ToString(),
                                                                Session["idCompetencia"].ToString(),
                                                                Session["anoVigente"].ToString());

                    if (!existe)
                    {
                        cmd = new MySqlCommand("sp_CrearCargoConducta", Conexion.ObtenerCnMysql());
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idCargo", Session["idCargos"]);
                        cmd.Parameters.AddWithValue("@idCompetencia", Session["idCompetencia"]);
                        cmd.Parameters.AddWithValue("@idCompania", Session["proyecto"]);
                        cmd.Parameters.AddWithValue("@idEmpresa", Session["idEmpresa"]);
                        cmd.Parameters.AddWithValue("@idConducta", e.CommandArgument);
                        cmd.Parameters.AddWithValue("@ano", Session["anoVigente"]);

                        // Crea un parametro de salida para el SP
                        MySqlParameter outputIdParam = new MySqlParameter("@respuesta", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        cmd.Parameters.Add(outputIdParam);
                        cmd.ExecuteNonQuery();

                        //Almacena la respuesta de la variable de retorno del SP
                        res = int.Parse(outputIdParam.Value.ToString());
                    }
                    else
                    {
                        cmd = new MySqlCommand("sp_ActualizarEstadoCargoConducta", Conexion.ObtenerCnMysql());
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idCarConCom", e.CommandArgument);
                        cmd.Parameters.AddWithValue("@estado", true);

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
                            this.CargarGrillasConductas(Session["idCargos"].ToString(),
                                                    Session["idCompetencia"].ToString(),
                                                    Session["proyecto"].ToString(),
                                                    Session["idEmpresa"].ToString());
                        }
                    }

                    if (res == 1)
                    {
                        this.CargarGrillasConductas(Session["idCargos"].ToString(),
                                                    Session["idCompetencia"].ToString(),
                                                    Session["proyecto"].ToString(),
                                                    Session["idEmpresa"].ToString());
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
        /// Paginacion agregar conducta
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento e</param>
        protected void gvAgregarConducta_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.LimpiarMensajes();
            DataTable datos = (DataTable)Session["dtConductasDisponibles"];
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