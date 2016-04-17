using MySql.Data.MySqlClient;
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
    public partial class CrearEmpleado : System.Web.UI.Page
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

                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'CrearEmpleado.aspx' AND idEmpresa = '" + Session["idEmpresa"] + "'", MySqlCn);
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

                    this.CargarAreasCargos();
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
            Session["cedula"] = txtUser.Text;

            try
            {
                this.LimpiarActivarCampos();

                MySqlCn = new MySqlConnection(CnMysql);
                MySqlCommand scSqlCommand;
                string consulta = "SELECT * FROM " + bd2 + ".empleados where Id_Empleado = " + Session["cedula"] + ";";

                scSqlCommand = new MySqlCommand(consulta, MySqlCn);

                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;
                MySqlCn.Open();
                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    if (dtDataTable.Rows[0]["Externo"] != null)
                    {
                        if (dtDataTable.Rows[0]["Externo"].ToString() == "1")
                        {
                            this.CargarInfoCliente(Session["cedula"].ToString(), true);
                        }
                        else
                        {
                            this.CargarInfoCliente(Session["cedula"].ToString(), false);
                        }
                    }
                    else
                    {
                        this.CargarInfoCliente(Session["cedula"].ToString(), false);
                    }
                }
                else
                {
                    BtnEditar.Text = "Guardar Información";
                    txtUser2.Text = Session["cedula"].ToString();                    
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
                    cmd = new MySqlCommand("sp_CrearEmpleadoExt", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TipoId_empleado", ddlTipoDocumento.SelectedValue);
                    cmd.Parameters.AddWithValue("@Id_Empleado", txtUser2.Text);
                    cmd.Parameters.AddWithValue("@idContrato", txtUser2.Text + "_EXT");
                    cmd.Parameters.AddWithValue("@Companias_idEmpresa", Session["idEmpresa"]);
                    cmd.Parameters.AddWithValue("@Companias_idCompania", Session["proyecto"]);
                    cmd.Parameters.AddWithValue("@Lugar_expediccion_IdEmpleado", txtExpedicion.Text);
                    cmd.Parameters.AddWithValue("@Nombres_Empleado", txtNombres.Text);
                    cmd.Parameters.AddWithValue("@Primer_Apellido_empleado", txtPrimerApellido.Text);
                    cmd.Parameters.AddWithValue("@Segundo_Apellido_Empleado", txtSegundoApellido.Text);
                    cmd.Parameters.AddWithValue("@Nombres_Completos_Empleado", txtNombres.Text + " " + txtPrimerApellido.Text + " " + txtSegundoApellido.Text);
                    cmd.Parameters.AddWithValue("@Sexo_Empleado", ddlSexo.SelectedValue);
                    cmd.Parameters.AddWithValue("@Nombre_Cargo_Empleado", ddlCargo.SelectedItem);
                    cmd.Parameters.AddWithValue("@Correo_Empleado", txtCorreo.Text);
                    cmd.Parameters.AddWithValue("@Direccion_Empleado", txtDireccion.Text);
                    cmd.Parameters.AddWithValue("@Barrio_Empleado", txtBarrio.Text);
                    cmd.Parameters.AddWithValue("@Celular_Empleado", txtCelular.Text);
                    cmd.Parameters.AddWithValue("@Telefono_Empleado", txtTelefono.Text);
                    cmd.Parameters.AddWithValue("@EstadoCivil_Empleado", ddlEstadosCiviles.SelectedValue);
                    cmd.Parameters.AddWithValue("@Fecha_Ingreso_Empleado", DateTime.Now.ToString("yyyyMMdd"));
                    cmd.Parameters.AddWithValue("@EPS_Empleado", txtEPS.Text);
                    cmd.Parameters.AddWithValue("@AFP_Empleado", txtAFP.Text);
                    cmd.Parameters.AddWithValue("@Cesantias_Empleado", txtCesantias.Text);
                    cmd.Parameters.AddWithValue("@Activo_Empleado", "A");
                    cmd.Parameters.AddWithValue("@Fecha_nacimiento_Empleado", txtFechaNacimiento.Text.Replace("/", string.Empty));
                    cmd.Parameters.AddWithValue("@Contrasena_Empleado", txtUser2.Text);
                    cmd.Parameters.AddWithValue("@Contrasena_Activo", 1);
                    cmd.Parameters.AddWithValue("@Externo", true);
                    cmd.Parameters.AddWithValue("@IdAreas", ddlArea.SelectedValue);
                    cmd.Parameters.AddWithValue("@IdCargos", ddlCargo.SelectedValue);                    
                }
                else
                {
                    cmd = new MySqlCommand("sp_ActualizarEmpleadoExt", Conexion.ObtenerCnMysql());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TipoId_empleado", ddlTipoDocumento.SelectedValue);
                    cmd.Parameters.AddWithValue("@Id_Empleado", txtUser2.Text);
                    cmd.Parameters.AddWithValue("@Companias_idEmpresa", Session["idEmpresa"]);
                    cmd.Parameters.AddWithValue("@Companias_idCompania", Session["proyecto"]);
                    cmd.Parameters.AddWithValue("@Nombres_Empleado", txtNombres.Text);
                    cmd.Parameters.AddWithValue("@Primer_Apellido_empleado", txtPrimerApellido.Text);
                    cmd.Parameters.AddWithValue("@Segundo_Apellido_Empleado", txtSegundoApellido.Text);
                    cmd.Parameters.AddWithValue("@Nombres_Completos_Empleado", txtNombres.Text + " " + txtPrimerApellido.Text + " " + txtSegundoApellido.Text);
                    cmd.Parameters.AddWithValue("@Sexo_Empleado", ddlSexo.SelectedValue);
                    cmd.Parameters.AddWithValue("@Nombre_Cargo_Empleado", ddlCargo.SelectedItem);
                    cmd.Parameters.AddWithValue("@Correo_Empleado", txtCorreo.Text);
                    cmd.Parameters.AddWithValue("@Direccion_Empleado", txtDireccion.Text);
                    cmd.Parameters.AddWithValue("@Barrio_Empleado", txtBarrio.Text);
                    cmd.Parameters.AddWithValue("@Celular_Empleado", txtCelular.Text);
                    cmd.Parameters.AddWithValue("@Telefono_Empleado", txtTelefono.Text);
                    cmd.Parameters.AddWithValue("@EstadoCivil_Empleado", ddlEstadosCiviles.SelectedValue);
                    cmd.Parameters.AddWithValue("@EPS_Empleado", txtEPS.Text);
                    cmd.Parameters.AddWithValue("@AFP_Empleado", txtAFP.Text);
                    cmd.Parameters.AddWithValue("@Cesantias_Empleado", txtCesantias.Text);
                    cmd.Parameters.AddWithValue("@Fecha_nacimiento_Empleado", txtFechaNacimiento.Text.Replace("/", string.Empty));                    
                    cmd.Parameters.AddWithValue("@IdAreas", ddlArea.SelectedValue);
                    cmd.Parameters.AddWithValue("@IdCargos", ddlCargo.SelectedValue);
                }

                if (cbJefe.Checked)
                {
                    cmd.Parameters.AddWithValue("@Id_Rol", "6");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Id_Rol", "2");
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
                
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Cargar informacion del usuario
        /// </summary>
        /// <param name="idUsuario">Id del usuario</param>
        /// <param name="externo">Indica si es un usuario externo</param>
        public void CargarInfoCliente(string idUsuario, bool externo)
        {
            this.LimpiarMensajes();
            CnMysql Conexion = new CnMysql(Cn);

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd = new MySqlCommand(bd2 + ".sp_ConsultaEmpleadosExt", Conexion.ObtenerCnMysql());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_Empleado", idUsuario);
                cmd.Parameters.AddWithValue("@Companias_idCompania", Session["proyecto"]);
                cmd.Parameters.AddWithValue("@empresa", Session["idEmpresa"]);
                MySqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    ddlTipoDocumento.SelectedValue = rd["TipoId_empleado"].ToString();
                    txtUser2.Text = Session["cedula"].ToString();
                    txtExpedicion.Text = rd["Lugar_expediccion_IdEmpleado"].ToString();
                    txtNombres.Text = rd["Nombres_Empleado"].ToString();
                    txtPrimerApellido.Text = rd["Primer_Apellido_empleado"].ToString();
                    txtSegundoApellido.Text = rd["Segundo_Apellido_Empleado"].ToString();
                    ddlSexo.SelectedValue = rd["Sexo_Empleado"].ToString();
                    txtCorreo.Text = rd["Correo_Empleado"].ToString();
                    txtDireccion.Text = rd["Direccion_Empleado"].ToString();
                    txtBarrio.Text = rd["Barrio_Empleado"].ToString();
                    txtCelular.Text = rd["Celular_Empleado"].ToString();
                    txtTelefono.Text = rd["Telefono_Empleado"].ToString();
                    ddlEstadosCiviles.SelectedValue = rd["EstadoCivil_Empleado"].ToString();
                    txtEPS.Text = rd["EPS_Empleado"].ToString();
                    txtAFP.Text = rd["AFP_Empleado"].ToString();
                    txtCesantias.Text = rd["Cesantias_Empleado"].ToString();

                    DateTime d = DateTime.ParseExact(rd["Fecha_nacimiento_Empleado"].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                    txtFechaNacimiento.Text = d.ToString("yyyy/MM/dd");

                    if (rd["Id_Rol"].ToString() != "")
                    {
                        cbJefe.Checked = rd["Id_Rol"].ToString().Equals("6");
                    }

                    if (rd["IdAreas"].ToString() != "")
                    {
                        ddlArea.SelectedValue = rd["IdAreas"].ToString();
                    }

                    if (rd["IdCargos"].ToString() != "")
                    {
                        ddlCargo.SelectedValue = rd["IdCargos"].ToString();
                    }

                    BtnEditar.Text = "Editar Información";
                }
                else
                {
                    MensajeError("Error al cargar la información");
                }

                rd.Close();

                if (externo)
                {
                    ddlTipoDocumento.Enabled = false;
                    txtExpedicion.Enabled = false;
                }
                else
                {
                    ddlTipoDocumento.Enabled = false;
                    txtExpedicion.Enabled = false;
                    txtNombres.Enabled = false;
                    txtPrimerApellido.Enabled = false;
                    txtSegundoApellido.Enabled = false;
                    ddlSexo.Enabled = false;
                    txtCorreo.Enabled = false;
                    txtDireccion.Enabled = false;
                    txtBarrio.Enabled = false;
                    txtCelular.Enabled = false;
                    txtTelefono.Enabled = false;
                    ddlEstadosCiviles.Enabled = false;
                    txtEPS.Enabled = false;
                    txtAFP.Enabled = false;
                    txtCesantias.Enabled = false;
                    txtFechaNacimiento.Enabled = false;
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
        /// Carga las areas y los cargos al ddl
        /// </summary>
        public void CargarAreasCargos()
        {
            this.LimpiarMensajes();
            CnMysql Conexion = new CnMysql(CnMysql);

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd = new MySqlCommand(bd2 + ".sp_ConsultaAreas", Conexion.ObtenerCnMysql());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nitTercero", Session["usuario"]);
                cmd.Parameters.AddWithValue("@idEmpresa", "ST");
                cmd.Parameters.AddWithValue("@id_Compania", Session["proyecto"]);
                cmd.Parameters.AddWithValue("@estado", true);

                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(cmd);
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;
                MySqlCn.Open();
                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    ddlArea.DataSource = dtDataTable;
                    ddlArea.DataTextField = "Area";
                    ddlArea.DataValueField = "IdAreas";
                    ddlArea.DataBind();

                    ddlArea.Items.Insert(0, new ListItem("---Seleccione---", "0", true));
                }

                cmd = new MySqlCommand(bd2 + ".sp_ConsultaCargos", Conexion.ObtenerCnMysql());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nitTercero", Session["usuario"]);
                cmd.Parameters.AddWithValue("@idEmpresa", "ST");
                cmd.Parameters.AddWithValue("@id_Compania", Session["proyecto"]);
                cmd.Parameters.AddWithValue("@estado", true);

                sdaSqlDataAdapter = new MySqlDataAdapter(cmd);
                DataSet dsDataCargos = new DataSet();
                DataTable dtDataTableCargos = null;
                sdaSqlDataAdapter.Fill(dsDataCargos);
                dtDataTableCargos = dsDataCargos.Tables[0];

                if (dtDataTableCargos != null && dtDataTableCargos.Rows.Count > 0)
                {
                    ddlCargo.DataSource = dtDataTableCargos;
                    ddlCargo.DataTextField = "Cargo";
                    ddlCargo.DataValueField = "IdCargos";
                    ddlCargo.DataBind();

                    ddlCargo.Items.Insert(0, new ListItem("---Seleccione---", "0", true));
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
        /// Activa los campos
        /// </summary>
        public void LimpiarActivarCampos() 
        {
            this.LimpiarMensajes();

            ddlTipoDocumento.Enabled = true;
            txtExpedicion.Enabled = true;
            txtNombres.Enabled = true;
            txtPrimerApellido.Enabled = true;
            txtSegundoApellido.Enabled = true;
            ddlSexo.Enabled = true;
            txtCorreo.Enabled = true;
            txtDireccion.Enabled = true;
            txtBarrio.Enabled = true;
            txtCelular.Enabled = true;
            txtTelefono.Enabled = true;
            ddlEstadosCiviles.Enabled = true;
            txtEPS.Enabled = true;
            txtAFP.Enabled = true;
            txtCesantias.Enabled = true;
            txtFechaNacimiento.Enabled = true;

            txtUser.Text = string.Empty;
            txtUser2.Text = string.Empty;
            txtExpedicion.Text = string.Empty;
            txtNombres.Text = string.Empty;
            txtPrimerApellido.Text = string.Empty;
            txtSegundoApellido.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtBarrio.Text = string.Empty;
            txtCelular.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtEPS.Text = string.Empty;
            txtAFP.Text = string.Empty;
            txtCesantias.Text = string.Empty;
            txtFechaNacimiento.Text = string.Empty;
            cbJefe.Checked = false;
            ddlArea.SelectedValue = "0";
            ddlCargo.SelectedValue = "0";

            Container_UpdatePanel2.Visible = false;
            UpdatePanel1.Update();
        }
    }
}