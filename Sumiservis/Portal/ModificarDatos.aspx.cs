using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace PortalTrabajadores.Portal
{
    public partial class ModificarDatos : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql"].ConnectionString.ToString();
        string bd1 = ConfigurationManager.AppSettings["BD1"].ToString();
        string bd2 = ConfigurationManager.AppSettings["BD2"].ToString();
        MySqlConnection MySqlCn;

        #region Definicion de los Metodos de la Clase

        #region Metodo Page Load
        /* ****************************************************************************/
        /* Metodo que se ejecuta al momento de la carga de la Pagina
        /* ****************************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                //Redirecciona a la pagina de login en caso de que el usuario no se halla autenticado
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                Session.Add("cambiaContrasena", false);
                if (!IsPostBack)
                {
                    try
                    {
                        MySqlCn = new MySqlConnection(Cn);
                        txtuser.Focus();

                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'ModificarDatos.aspx'", MySqlCn);
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
        /* ****************************************************************************/
        /* Metodo que habilita el label de mensaje de error
        /* ****************************************************************************/
        public void MensajeError(string Msj)
        {
            LblMsj.Text = Msj;
            LblMsj.Visible = true;
            UpdatePanel3.Update();
        }
        #endregion

        #region Metodo BtnBuscar_Click
        /* ****************************************************************************/
        /* Metodo que consulta la información del empleado a actualizar
        /* ****************************************************************************/
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (BtnBuscar.Text == "Nueva Búsqueda")
            {
                txtuser.Text = "";
                txtuser.Enabled = true;
                BtnBuscar.Text = "Buscar Usuario";
                MensajeError("");

                Container_UpdatePanel2.Visible = false;
                TxtDoc.Text = "";
                TxtNombres.Text = "";
                TxtPrimerApellido.Text = "";
                TxtSegundoApellido.Text = "";
                TxtExp.Text = "";
                TxtCorreo.Text = "";
                TxtBarrio.Text = "";
                TxtCelular.Text = "";
                TxtEps.Text = "";
                TxtAfp.Text = "";
                TxtTelefono.Text = "";
                TxtCesantias.Text = "";
                TxtDir.Text = "";
                TxtFechNac.Text = "";
                Session.Remove("Contrasena");
                txtuser.Focus();

                TxtAfp.Enabled = true;
                TxtAfp.BackColor = System.Drawing.Color.White;
                TxtExp.Enabled = true;
                TxtExp.BackColor = System.Drawing.Color.White;
                TxtBarrio.Enabled = true;
                TxtBarrio.BackColor = System.Drawing.Color.White;
                TxtCelular.Enabled = true;
                TxtCelular.BackColor = System.Drawing.Color.White;
                TxtCesantias.Enabled = true;
                TxtCesantias.BackColor = System.Drawing.Color.White;
                TxtCorreo.Enabled = true;
                TxtCorreo.BackColor = System.Drawing.Color.White;
                TxtDir.Enabled = true;
                TxtDir.BackColor = System.Drawing.Color.White;
                TxtEps.Enabled = true;
                TxtEps.BackColor = System.Drawing.Color.White;
                TxtEstCivil.Enabled = true;
                TxtEstCivil.BackColor = System.Drawing.Color.White;
                TxtFechNac.Enabled = true;
                TxtFechNac.BackColor = System.Drawing.Color.White;
                ddlCiudades.Enabled = true;
                ddlCiudades.BackColor = System.Drawing.Color.White;
                TxtNombres.Enabled = true;
                TxtNombres.BackColor = System.Drawing.Color.White;
                TxtPrimerApellido.Enabled = true;
                TxtPrimerApellido.BackColor = System.Drawing.Color.White;
                TxtSexoEmp.Enabled = true;
                TxtSexoEmp.BackColor = System.Drawing.Color.White;
                TxtSegundoApellido.Enabled = true;
                TxtSegundoApellido.BackColor = System.Drawing.Color.White;
                TxtTelefono.Enabled = true;
                TxtTelefono.BackColor = System.Drawing.Color.White;
                TxtPass.Enabled = true;
                TxtPass.BackColor = System.Drawing.Color.White;
                BtnEditar.Enabled = true;
                BtnEditar.BackColor = System.Drawing.Color.White;
            }
            else
            {
                BtnBuscar.Text = "Nueva Búsqueda";
                txtuser.Enabled = false;
                ddlCiudades_Load();
                CargarInfoEmpleado(txtuser.Text);
            }
        }
        #endregion

        #region ddlCiudades_Load
        /* ****************************************************************************/
        /* Metodo que carga el DropList de Ciudades del Formulario de la Pagina
        /* ****************************************************************************/
        protected void ddlCiudades_Load()
        {
            try
            {
                MySqlCn = new MySqlConnection(Cn);
                MySqlCommand comando = new MySqlCommand("SELECT idCiudades, Descripcion_Ciudades FROM " + bd1 + ".ciudades ORDER BY Descripcion_Ciudades", MySqlCn);
                MySqlDataAdapter da = new MySqlDataAdapter(comando);
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;

                MySqlCn.Open();
                da.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];
                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    ddlCiudades.DataSource = dtDataTable;
                    ddlCiudades.DataTextField = "Descripcion_Ciudades";
                    ddlCiudades.DataValueField = "idCiudades";
                    ddlCiudades.DataBind();
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
        #endregion

        #region Metodo CargarInfoEmpleado
        /* ****************************************************************************/
        /* Metodo que carga la informacion del empleado en el formulario
        /* ****************************************************************************/
        public void CargarInfoEmpleado(string id_Empleado)
        {
            string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();
            CnMysql Conexion = new CnMysql(Cn);

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd = new MySqlCommand("sp_ConsultaEmpleados", Conexion.ObtenerCnMysql());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idEmpleado", id_Empleado);
                cmd.Parameters.AddWithValue("@empresa", "SS");
                MySqlDataReader rd = cmd.ExecuteReader();

                    if (rd.Read())
                    {
                        Container_UpdatePanel2.Visible = true;
                        TxtDoc.Text = rd["Id_Empleado"].ToString();
                        TxtNombres.Text = rd["Nombres_Empleado"].ToString();
                        TxtPrimerApellido.Text = rd["Primer_Apellido_empleado"].ToString();
                        TxtSegundoApellido.Text = rd["Segundo_Apellido_Empleado"].ToString();
                        if (rd["Sexo_Empleado"].ToString() != null)
                        {
                            TxtSexoEmp.SelectedValue = rd["Sexo_Empleado"].ToString();
                        }
                        TxtExp.Text = rd["Lugar_expediccion_IdEmpleado"].ToString();
                        TxtCorreo.Text = rd["Correo_Empleado"].ToString();
                        TxtBarrio.Text = rd["Barrio_Empleado"].ToString();
                        TxtCelular.Text = rd["Celular_Empleado"].ToString();
                        TxtEps.Text = rd["EPS_Empleado"].ToString();
                        TxtAfp.Text = rd["AFP_Empleado"].ToString();
                        TxtTelefono.Text = rd["Telefono_Empleado"].ToString();
                        TxtCesantias.Text = rd["Cesantias_Empleado"].ToString();
                        if (rd["EstadoCivil_Empleado"].ToString() != null)
                        {
                            TxtEstCivil.SelectedValue = rd["EstadoCivil_Empleado"].ToString();
                        }
                        TxtDir.Text = rd["Direccion_Empleado"].ToString();
                        TxtFechNac.Text = rd["Fecha_nacimiento_Empleado"].ToString();
                        if (ddlCiudades.Items.FindByValue(rd["Lugar_Nacimiento"].ToString()) != null)
                        {
                            ddlCiudades.SelectedValue = rd["Lugar_Nacimiento"].ToString();
                        }
                        Session.Add("Contrasena", rd["Contrasena_Empleado"].ToString());
                        Container_UpdatePanel2.Visible = true;
                    }
                    else
                    {
                        MensajeError("No se encontro el número de identificación digitado. Por favor verifique.");
                    }
                rd.Close();
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
        #endregion

        #region Metodo ValidatorPass_ServerValidate
        /* ****************************************************************************/
        /* Metodo que valida el password digitado en el servidor
        /* ****************************************************************************/
        protected void ValidatorPass_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (TxtPass.Text.Count() < 5)
            {
                args.IsValid = false;
            }
            else
            {
                try
                {
                    MySqlCn = new MySqlConnection(Cn);
                    MySqlCommand scSqlCommand = new MySqlCommand("SELECT Contrasena_Empleado FROM " + bd2 + ".empleados where Id_Empleado = '" + Session["usuario"].ToString() + "'", MySqlCn);
                    MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                    DataSet dsDataSet = new DataSet();
                    DataTable dtDataTable = null;
                    MySqlCn.Open();
                    sdaSqlDataAdapter.Fill(dsDataSet);
                    dtDataTable = dsDataSet.Tables[0];
                    if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                    {
                        string Contrasena = dtDataTable.Rows[0].ItemArray[0].ToString();
                        if (Contrasena != TxtPass.Text)
                        {
                            args.IsValid = true;
                            Editar_Click();
                        }
                        else
                        {
                            MensajeError("La contraseña no puede ser igual a la anterior.");
                            MySqlCn.Close();
                        }
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
        #endregion

        #region Metodo BtnEditar_Click
        /* *****************************************************************************************/
        /* Evento que se produce al dar clic sobre el boton BtnEditar para el cambio de Contraseña
        /* *****************************************************************************************/
        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            if (Session["cambiaContrasena"] != null)
            {
                if (Session["cambiaContrasena"].ToString() != "True")
                {
                    Editar_Click();
                }
            }
        }
        #endregion

        #region Metodo Editar_Click
        /* ****************************************************************************/
        /* Metodo que Bloquea los campos al Dar Clic en el Boton Almacenar Informacion
        /* ****************************************************************************/
        protected void Editar_Click()
        {
            int R = GuardarInfoEmpleado(this.Session["usuario"].ToString());
            if (R == 1)
            {
                MensajeError("La información se ha actualizado correctamente");

                TxtAfp.Enabled = false;
                TxtAfp.BackColor = System.Drawing.Color.LightGray;
                TxtExp.Enabled = false;
                TxtExp.BackColor = System.Drawing.Color.LightGray;
                TxtBarrio.Enabled = false;
                TxtBarrio.BackColor = System.Drawing.Color.LightGray;
                TxtCelular.Enabled = false;
                TxtCelular.BackColor = System.Drawing.Color.LightGray;
                TxtCesantias.Enabled = false;
                TxtCesantias.BackColor = System.Drawing.Color.LightGray;
                TxtCorreo.Enabled = false;
                TxtCorreo.BackColor = System.Drawing.Color.LightGray;
                TxtDir.Enabled = false;
                TxtDir.BackColor = System.Drawing.Color.LightGray;
                TxtEps.Enabled = false;
                TxtEps.BackColor = System.Drawing.Color.LightGray;
                TxtEstCivil.Enabled = false;
                TxtEstCivil.BackColor = System.Drawing.Color.LightGray;
                TxtFechNac.Enabled = false;
                TxtFechNac.BackColor = System.Drawing.Color.LightGray;
                ddlCiudades.Enabled = false;
                ddlCiudades.BackColor = System.Drawing.Color.LightGray;
                TxtNombres.Enabled = false;
                TxtNombres.BackColor = System.Drawing.Color.LightGray;
                TxtPrimerApellido.Enabled = false;
                TxtPrimerApellido.BackColor = System.Drawing.Color.LightGray;
                TxtSexoEmp.Enabled = false;
                TxtSexoEmp.BackColor = System.Drawing.Color.LightGray;
                TxtSegundoApellido.Enabled = false;
                TxtSegundoApellido.BackColor = System.Drawing.Color.LightGray;
                TxtTelefono.Enabled = false;
                TxtTelefono.BackColor = System.Drawing.Color.LightGray;
                TxtPass.Enabled = false;
                TxtPass.BackColor = System.Drawing.Color.LightGray;
                BtnEditar.Enabled = false;
                BtnEditar.BackColor = System.Drawing.Color.LightGray;
            }
            else
            {
                MensajeError("La información no se ha actualizado correctamente. Pongase en contacto con el Administrador.");
            }
        }
        #endregion

        #region Metodo GuardarInfoEmpleado
        /* ****************************************************************************/
        /* Metodo que actualiza la informacion del empleado en la Base de datos
        /* ****************************************************************************/
        public int GuardarInfoEmpleado(string id_Empleado)
        {
            string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();
            CnMysql Conexion = new CnMysql(Cn);
            int res = 0;

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd = new MySqlCommand("sp_ActualizaEmpleado", Conexion.ObtenerCnMysql());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idEmpleado", TxtDoc.Text);
                cmd.Parameters.AddWithValue("@nombresEmpleado", TxtNombres.Text);
                cmd.Parameters.AddWithValue("@PApellidoEmpleado", TxtPrimerApellido.Text);
                cmd.Parameters.AddWithValue("@SApellidoEmpleado", TxtSegundoApellido.Text);
                string NombreCompleto = TxtPrimerApellido.Text + " " + TxtSegundoApellido.Text + " " + TxtNombres.Text;
                cmd.Parameters.AddWithValue("@nombreCompletoEmpleado", NombreCompleto);
                cmd.Parameters.AddWithValue("@SexoEmpleado", TxtSexoEmp.SelectedValue);
                cmd.Parameters.AddWithValue("@expDocumento", TxtExp.Text);
                cmd.Parameters.AddWithValue("@correo", TxtCorreo.Text);
                cmd.Parameters.AddWithValue("@barrio", TxtBarrio.Text);
                cmd.Parameters.AddWithValue("@celular", TxtCelular.Text);
                cmd.Parameters.AddWithValue("@EPS", TxtEps.Text);
                cmd.Parameters.AddWithValue("@AFP", TxtAfp.Text);
                cmd.Parameters.AddWithValue("@telefono", TxtTelefono.Text);
                cmd.Parameters.AddWithValue("@cesantias", TxtCesantias.Text);
                cmd.Parameters.AddWithValue("@estadoCivil", TxtEstCivil.SelectedValue);
                cmd.Parameters.AddWithValue("@direccion", TxtDir.Text);
                string FechaNacimiento = TxtFechNac.Text.Replace("-", "").Replace("/", "");
                cmd.Parameters.AddWithValue("@fechaNacimiento", FechaNacimiento);
                cmd.Parameters.AddWithValue("@lugarNacimiento", ddlCiudades.SelectedValue);

                if (Session["cambiaContrasena"].ToString() == "True")
                {
                    cmd.Parameters.AddWithValue("@contrasena", TxtPass.Text);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@contrasena", Session["Contrasena"].ToString());
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
                Session.Remove("Contrasena");
                Session.Remove("cambiaContrasena");
                return res;
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Session.Remove("Contrasena");
                Session.Remove("cambiaContrasena");
                return res;
            }
            finally
            {
                Conexion.CerrarCnMysql();
            }
        }
        #endregion

        #endregion
    }
}