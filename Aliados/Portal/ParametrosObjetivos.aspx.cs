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
        ConsultasGenerales consultas;

        #region Metodo Page_Load

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
                        MySqlCn = new MySqlConnection(Cn);

                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'ParametrosObjetivos.aspx'", MySqlCn);
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
        /// Busca los datos del usuario
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento e</param>
        protected void BtnCrear_Click(object sender, EventArgs e)
        {
            BtnEditar.Text = "Guardar";

            txtAno.Enabled = true;
            ddlMin.Enabled = true;
            ddlMax.Enabled = true;
            ddlSeguimiento.Enabled = true;   
            Container_UpdatePanel2.Visible = true;
            UpdatePanel1.Update();
        }

        /// <summary>
        /// Busca los datos del usuario
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento e</param>
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            ddlMin.Enabled = true;
            ddlMax.Enabled = true;
            ddlSeguimiento.Enabled = true;            

            LblMsj.Text = string.Empty;
            LblMsj.Visible = false;
            UpdatePanel3.Update();
            
            try
            {
                MySqlCn = new MySqlConnection(CnMysql);
                MySqlCommand scSqlCommand;
                string consulta = "SELECT * FROM " + bd3 + 
                                   ".parametrosgenerales where ano = " + ddlAnio.SelectedValue + 
                                   " AND idTercero = '" + Session["usuario"] + 
                                   "' AND idCompania = '" + Session["proyecto"] + "';";

                scSqlCommand = new MySqlCommand(consulta, MySqlCn);

                MySqlCn.Open();
                MySqlDataReader rd = scSqlCommand.ExecuteReader();

                if (rd.HasRows)
                {
                    if (rd.Read())
                    {
                        txtAno.Text = ddlAnio.SelectedValue;
                        ddlMin.SelectedValue = rd["Min_Objetivos"].ToString();
                        ddlMax.SelectedValue = rd["Max_Objetivos"].ToString();
                        ddlSeguimiento.SelectedValue = rd["Periodo_Seguimiento"].ToString();
                        cbActivo.Checked = rd["Activo"].ToString().Equals("1");

                        txtAno.Enabled = false;
                        ddlMin.Enabled = false;
                        ddlMax.Enabled = false;
                        ddlSeguimiento.Enabled = false;

                        BtnEditar.Text = "Editar";
                    }                    
                }
                else 
                {
                    ddlMin.SelectedValue = "1";
                    ddlMax.SelectedValue = "1";
                    ddlSeguimiento.SelectedValue = "0";
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
        /// Edita o guarda la informacion del formulario
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento e</param>
        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            CnMysql Conexion = new CnMysql(CnMysql);
            int res = 0;
            string anoActivo = string.Empty;
            bool ejecutar = true;

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd;

                if (BtnEditar.Text == "Guardar")
                {
                    bool valido = consultas.ConsultarAnoValido(Session["proyecto"].ToString(),
                                                                 Session["idEmpresa"].ToString(),
                                                                 Convert.ToInt32(txtAno.Text));

                    if (valido)
                    {
                        ejecutar = false;
                    }
                    
                    cmd = new MySqlCommand("sp_CrearParametrosGenerales", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;                    
                }
                else
                {
                    cmd = new MySqlCommand("sp_ActualizarParametrosGenerales", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                }

                if (ejecutar)
                {
                    cmd.Parameters.AddWithValue("@idTercero", Session["usuario"]);
                    cmd.Parameters.AddWithValue("@idCompania", Session["proyecto"]);
                    cmd.Parameters.AddWithValue("@Empresas_idEmpresa", Session["idEmpresa"]);
                    cmd.Parameters.AddWithValue("@Max_Objetivos", ddlMax.SelectedValue);
                    cmd.Parameters.AddWithValue("@Min_Objetivos", ddlMin.SelectedValue);
                    cmd.Parameters.AddWithValue("@Periodo_Seguimiento", ddlSeguimiento.SelectedValue);                    
                    cmd.Parameters.AddWithValue("@Ano", txtAno.Text);

                    if (cbActivo.Checked)
                    {
                        string valido = consultas.ConsultarAnoActivo(Session["proyecto"].ToString(),
                                                                     Session["idEmpresa"].ToString());

                        if (valido == "0")
                        {
                            cmd.Parameters.AddWithValue("@Activo", cbActivo.Checked);
                        }
                        else 
                        {
                            cmd.Parameters.AddWithValue("@Activo", false);
                            anoActivo = "No se activo ya que el año " + valido + " está activo. Desactivelo primero";
                        }
                    }
                    else 
                    {
                        cmd.Parameters.AddWithValue("@Activo", cbActivo.Checked);
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
                        Container_UpdatePanel2.Visible = false;
                        UpdatePanel1.Update();

                        if (BtnEditar.Text == "Guardar")
                        {
                            MensajeError("Parametros creados correctamente. " + anoActivo);
                        }
                        else
                        {
                            MensajeError("Parametros actualizados correctamente. " + anoActivo);
                        }

                        this.CargarAnio();
                    }
                }
                else 
                {
                    MensajeError("El año digitado ya existe");
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
            ddlMin.SelectedValue = "1";
            ddlMax.SelectedValue = "1";
            ddlSeguimiento.SelectedValue = "0";
            ddlMin.Enabled = true;
            ddlMax.Enabled = true;
            ddlSeguimiento.Enabled = true;

            Container_UpdatePanel2.Visible = false;
            UpdatePanel1.Update();
        }
    }
}