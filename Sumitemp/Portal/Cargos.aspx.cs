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
    public partial class Cargos : System.Web.UI.Page
    {
        string CnMysql = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();
        string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql"].ConnectionString.ToString();
        string bd2 = ConfigurationManager.AppSettings["BD2"].ToString();
        MySqlConnection MySqlCn;

        #region Definicion de los Metodos de la Clase

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
                    this.CargarGrid();

                    try
                    {
                        MySqlCn = new MySqlConnection(Cn);

                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'Cargos.aspx' AND idEmpresa = '" + Session["idEmpresa"].ToString() + "'", MySqlCn);
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
        /// Habilita el menu de crear un cargo
        /// </summary>
        /// <param name="sender">Objeto Sender</param>
        /// <param name="e">Evento e</param>
        protected void BtnCargo_Click(object sender, EventArgs e)
        {
            try
            {
                this.LimpiarMensajes();
                TxtCargo.Text = string.Empty;
                BtnGuardar.Text = "Guardar";
                Container_UpdatePanel2.Visible = true;
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
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
            CnMysql Conexion = new CnMysql(CnMysql);
            int res = 0;

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd;

                if (BtnGuardar.Text == "Guardar")
                {
                    cmd = new MySqlCommand("sp_CrearCargo", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nitTercero", Session["usuario"]);
                    cmd.Parameters.AddWithValue("@idEmpresa", Session["idEmpresa"]);
                    cmd.Parameters.AddWithValue("@id_Compania", Session["proyecto"]);
                    cmd.Parameters.AddWithValue("@cargo", TxtCargo.Text);
                    cmd.Parameters.AddWithValue("@estado", true);
                }
                else
                {
                    cmd = new MySqlCommand("sp_ActualizarCargo", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCargos", Convert.ToInt32(Session["idCargos"]));
                    cmd.Parameters.AddWithValue("@cargo", TxtCargo.Text);
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

                if (res == 1)
                {
                    this.CargarGrid();
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
            TxtCargo.Text = string.Empty;
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
            int res = 0;

            CnMysql Conexion = new CnMysql(CnMysql);
            Conexion.AbrirCnMysql();

            try
            {
                if (e.CommandName == "Editar")
                {
                    GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int RowIndex = gvr.RowIndex;

                    TxtCargo.Text = gvCargosCreados.Rows[RowIndex].Cells[0].Text;

                    BtnGuardar.Text = "Editar";
                    Container_UpdatePanel2.Visible = true;
                    UpdatePanel1.Update();
                    Session.Add("idCargos", e.CommandArgument);
                }
                else if (e.CommandName == "On" || e.CommandName == "Off")
                {
                    MySqlCommand cmd = new MySqlCommand("sp_EstadoCargo", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCargos", Convert.ToInt32(e.CommandArgument));

                    if (e.CommandName == "On")
                    {
                        cmd.Parameters.AddWithValue("@estado", false);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@estado", true);
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

                    if (res == 1)
                    {
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

                string estado = DataBinder.Eval(e.Row.DataItem, "Estado").ToString();

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
        /// Carga la grilla actualizada
        /// </summary>
        private void CargarGrid()
        {
            this.LimpiarMensajes();
            CnMysql MysqlCn = new CnMysql(CnMysql);

            try
            {
                DataTable dtDataTable = null;
                MysqlCn.AbrirCnMysql();
                dtDataTable = MysqlCn.ConsultarRegistros("SELECT IdCargos, Cargo, Estado FROM " + bd2 + ".cargos"
                                                         + " where nittercero = " + Session["usuario"]
                                                         + " and idCompania = '" + Session["proyecto"]
                                                         + "' and Empresas_idEmpresa = '" + Session["idEmpresa"] + "';");

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
            finally
            {
                MysqlCn.CerrarCnMysql();
            }
        }

        #endregion
    }
}