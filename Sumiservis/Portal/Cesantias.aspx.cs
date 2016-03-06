using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace PortalTrabajadores.Portal
{
    public partial class Estadocesantias : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql"].ConnectionString.ToString();
        string ruta = ConfigurationManager.AppSettings["RepositorioPDF"].ToString();
        MySqlConnection MySqlCn;

        #region Definicion de los Metodos de la Clase

        #region Metodo Page Load
        /* ****************************************************************************/
        /* Metodo que se ejecuta al momento de la carga de la Pagina Maestra
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
                if (!IsPostBack)
                {
                    try
                    {
                        MySqlCn = new MySqlConnection(Cn);
                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'Cesantias.aspx' AND idEmpresa = 'SS'", MySqlCn);
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
                    catch (Exception ex)
                    {
                        MensajeError("Ha ocurrido el siguiente error: " + ex.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
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

        #region Metodo GridView1_RowCommand
        /* ***********************************************************************************/
        /* Evento que se ejecuta al momento de dar clic sobre los elementos IMG del GridView
        /* ***********************************************************************************/
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            SqlDataSource1.SelectCommand =  "SELECT idCesantia, anio_cesantia, Mes_Cesantia, ruta_archivo_cesantia as ruta, " +
                                            "nombre_archivo_cesantia as nombre " +
                                            "FROM trabajadores.cesantias, " +
                                            "trabajadores.empleados, " +
                                            "trabajadores.companias " +
                                            "where Empleados_Id_Empleado = " + Session["cedula"].ToString() + " and " +
                                            "trabajadores.companias.idcompania = '" + Session["proyecto"].ToString() + "' and " +
                                            "Empleados_idEmpresa = 'SS' and " +
                                            "trabajadores.companias.Terceros_Nit_Tercero = " + Session["usuario"].ToString() + " and " +
                                            "trabajadores.cesantias.empleados_Id_Empleado = trabajadores.empleados.id_Empleado and " +
                                            "trabajadores.companias.idcompania = trabajadores.empleados.companias_idcompania and " +
                                            "trabajadores.companias.Empresas_idEmpresa = trabajadores.cesantias.Empleados_idEmpresa LIMIT 1"; 

            try
            {
                if (e.CommandName == "PDF")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    string filepath = "";
                    string filename2 = "";
                    DataView dvSql = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);

                    //Recorre el SqlDataSource
                    foreach (DataRowView drvSql in dvSql)
                    {
                        if (drvSql["idCesantia"].ToString() == index.ToString())
                        {
                            filepath = drvSql["ruta"].ToString();
                            filename2 = drvSql["nombre"].ToString();
                            break;
                        }
                    }
                    //Valida la Existencia de la ruta Origen para poder descargar el PDF los Archivos
                    string filename = System.IO.Path.Combine(ruta, filepath, filename2);
                    if (System.IO.File.Exists(filename))
                    {
                        //Response.Write(string.Format("<script>window.open('{0}','_blank');</script>", System.IO.Path.Combine(filepath, filename2)));
                        Response.Clear();
                        //Response.ContentType = "application/octet-stream";
                        //Response.AddHeader("Content-Disposition", "attachment; filename=" + filename2);
                        //Response.WriteFile(filename);
                        Response.Buffer = true;
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename2);
                        Response.WriteFile(filename);
                        Response.Flush();
                        Response.Close();
                    }
                    else
                    {
                        MensajeError("Aun no se encuentra disponible el certificado.");
                    }
                }
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion

        #endregion

        /* ****************************************************************************/
        /* Metodo que se ejecuta con el boton buscar
        /* ****************************************************************************/
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            LblMsj.Text = string.Empty;
            LblMsj.Visible = false;
            UpdatePanel3.Update();

            Session["cedula"] = txtuser.Text;

            string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();

            try
            {
                MySqlCn = new MySqlConnection(Cn);
                string consulta = "SELECT idCesantia, anio_cesantia, Mes_Cesantia, ruta_archivo_cesantia as ruta, " +
                                                            "nombre_archivo_cesantia as nombre " +
                                                            "FROM trabajadores.cesantias, " +
                                                            "trabajadores.empleados, " +
                                                            "trabajadores.companias " +
                                                            "where Empleados_Id_Empleado = " + Session["cedula"].ToString() + " and " +
                                                            "trabajadores.companias.idcompania = '" + Session["proyecto"].ToString() + "' and " +
                                                            "Empleados_idEmpresa = 'SS' and " +
                                                            "trabajadores.companias.Terceros_Nit_Tercero = " + Session["usuario"].ToString() + " and " +
                                                            "trabajadores.cesantias.empleados_Id_Empleado = trabajadores.empleados.id_Empleado and " +
                                                            "trabajadores.companias.idcompania = trabajadores.empleados.companias_idcompania and " +
                                                            "trabajadores.companias.Empresas_idEmpresa = trabajadores.cesantias.Empleados_idEmpresa LIMIT 1;";

                MySqlCommand scSqlCommand = new MySqlCommand(consulta, MySqlCn);

                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;
                MySqlCn.Open();
                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];
                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    SqlDataSource1.SelectCommand = consulta; 
                }
                else
                {
                    //Muestra un Mensaje en la aplicacion indicando al usuario que se abrio una nueva ventana
                    MensajeError("El usuario no existe.");
                }
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
    }
}