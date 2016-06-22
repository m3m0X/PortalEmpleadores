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
                    this.CargarGrid();

                    try
                    {
                        MySqlCn = new MySqlConnection(CnBasica);

                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'AsignarCompetencias.aspx' AND idEmpresa = '" + Session["idEmpresa"].ToString() + "'", MySqlCn);
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

                        this.CargarGrid();
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
        /// Carga la grilla actualizada
        /// </summary>
        private void CargarGrid()
        {
            this.LimpiarMensajes();

            try
            {
                DataTable dtDataTable = consultas.ConsultarCargos(Session["usuario"].ToString(),
                                                                  Session["proyecto"].ToString(),
                                                                  Session["idEmpresa"].ToString());

                Session.Add("DataCargos", dtDataTable);

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    gvCargosCreados.DataSource = dtDataTable;
                    gvCargosCreados.DataBind();
                    Container_UpdatePanel2.Visible = false;
                    UpdatePanel1.Update();
                }
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
                                                                              Convert.ToInt32(Session["idCargos"].ToString()));

                Session.Add("DataCompetencias", dtDataTable);

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    gvCompetenciasCreadas.DataSource = dtDataTable;
                    gvCompetenciasCreadas.DataBind();
                    Container_UpdatePanel2.Visible = false;
                    UpdatePanel1.Update();
                }
                else 
                {
                    gvCompetenciasCreadas.DataSource = null;
                    gvCompetenciasCreadas.DataBind();
                    Container_UpdatePanel2.Visible = false;
                    UpdatePanel1.Update();
                }
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
                                                                           Session["idEmpresa"].ToString());

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
                bool valido = consultas.ConsultarCargoCompetencia(Session["proyecto"].ToString(),
                                                                  Session["idEmpresa"].ToString(),
                                                                  Convert.ToInt32(Session["idCargos"].ToString()),
                                                                  Convert.ToInt32(ddlCompetencias.SelectedValue));

                if (!valido)
                {
                    Conexion.AbrirCnMysql();
                    MySqlCommand cmd;

                    cmd = new MySqlCommand("sp_AsignarCompetencias", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCargo", Session["idCargos"]);
                    cmd.Parameters.AddWithValue("@idCompetencia", ddlCompetencias.SelectedValue);
                    cmd.Parameters.AddWithValue("@idTercero", Session["usuario"]);
                    cmd.Parameters.AddWithValue("@idCompania", Session["proyecto"]);
                    cmd.Parameters.AddWithValue("@idEmpresa", Session["idEmpresa"]);

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
                        this.CargarGrid();
                    }
                }
                else 
                {
                    this.CargarGrid();
                    this.MensajeError("La competencia ya ha sido asignada.");
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
            this.CargarGrid();
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
                Conexion.AbrirCnMysql();
                MySqlCommand cmd;

                if (e.CommandName == "Eliminar")
                {
                    cmd = new MySqlCommand("sp_EliminarCargoCompetencia", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCargo", Session["idCargos"]);
                    cmd.Parameters.AddWithValue("@idCompetencia", e.CommandArgument);
                    cmd.Parameters.AddWithValue("@idCompania", Session["proyecto"]);
                    cmd.Parameters.AddWithValue("@idEmpresa", Session["idEmpresa"]);

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
                        this.CargarGrid();
                    }
                    else if (res == 2)
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:CargarMensaje('No se puede desactivar porque esta asociado a uno o más empleados.'); ", true);
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
    }
}