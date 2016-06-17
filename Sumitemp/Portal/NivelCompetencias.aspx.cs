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
                    this.CargarGrid();

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
            CnMysql MysqlCn = new CnMysql(Cncompetencias);

            try
            {
                DataTable dtDataTable = null;
                MysqlCn.AbrirCnMysql();
                dtDataTable = MysqlCn.ConsultarRegistros("SELECT * FROM " + bdCompetencias + ".nivelcompetencias"
                                                         + " where idTercero = " + Session["usuario"]
                                                         + " and idCompania = '" + Session["proyecto"]
                                                         + "' and idEmpresa = '" + Session["idEmpresa"] + "';");

                Session.Add("DataNivel", dtDataTable);

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    gvNivelesCreados.DataSource = dtDataTable;
                    gvNivelesCreados.DataBind();
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
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            this.LimpiarMensajes();
            txtNombre.Text = string.Empty;
            txtMax.Text = string.Empty;
            txtMin.Text = string.Empty;
            this.CargarGrid();
        }

        /// <summary>
        /// Acciones que ejecuta la grilla
        /// </summary>
        /// <param name="sender">Objeto Sender</param>
        /// <param name="e">Evento e</param>
        protected void gvNivelesCreados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            this.LimpiarMensajes();

            try
            {
                if (e.CommandName == "Editar")
                {
                    GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int RowIndex = gvr.RowIndex;

                    txtNombre.Text = gvNivelesCreados.Rows[RowIndex].Cells[0].Text;
                    txtMin.Text = gvNivelesCreados.Rows[RowIndex].Cells[1].Text;
                    txtMax.Text = gvNivelesCreados.Rows[RowIndex].Cells[2].Text;

                    BtnEditar.Text = "Editar";
                    Container_UpdatePanel2.Visible = true;
                    UpdatePanel1.Update();
                    Session.Add("idNivelCompetencias", e.CommandArgument);
                }
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
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
    }
}