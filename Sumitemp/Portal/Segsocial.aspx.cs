using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Net;
using Ionic.Zip;
using System.Text;

namespace PortalTrabajadores.Portal
{
    public partial class Segsocial : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql"].ConnectionString.ToString();
        string ruta = ConfigurationManager.AppSettings["RepositorioPDF"].ToString();
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
                if (!IsPostBack)
                {
                    try
                    {
                        MySqlCn = new MySqlConnection(Cn);
                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'Segsocial.aspx'", MySqlCn);
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

        #region Metodo GridView1_RowCommand
        /* ****************************************************************************/
        /* Evento que se produce al dar clic sobre el control IMG del GridView
        /* ****************************************************************************/
        //Tener en cuenta el trigguer que se utiliza para el postback en la pagina
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            SqlDataSource1.SelectCommand = "SELECT idParafiscal, Anio_Parafiscal, Mes_Parafiscal, Ruta_archivo_Parafiscal as ruta, " +
                                        "Nombre_archivo_parafiscal as nombre " +
                                        "FROM " + bd2 + ".parafiscales, " + bd2 + ".empleados " +
                                        "where empleados_Id_Empleado =" + Session["cedula"].ToString() + " and " +
                                        bd2 + ".empleados.Companias_idCompania = '" + Session["proyecto"].ToString() + "' and " +
                                        bd2 + ".parafiscales.Empleados_Id_Empleado = " + bd2 + ".empleados.Id_Empleado and " +
                                        "Empleados_idEmpresa = 'ST' LIMIT 1;";
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
                        if (drvSql["idParafiscal"].ToString() == index.ToString())
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
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.Clear();
                        //Response.ContentType = "application/octet-stream";
                        //Response.AddHeader("Content-Disposition", "attachment; filename=" + filename2);
                        //Response.WriteFile(filename);
                        Response.Buffer = true;
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename2);
                        Response.WriteFile(filename);
                        Response.Flush();
                        Response.End();
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
                MySqlCommand scSqlCommand;
                string consulta = "SELECT idParafiscal, Anio_Parafiscal, Mes_Parafiscal, Ruta_archivo_Parafiscal as ruta, " +
                                                    "Nombre_archivo_parafiscal as nombre " +
                                                    "FROM " + bd2 + ".parafiscales, " + bd2 + ".empleados " +
                                                    "where empleados_Id_Empleado =" + Session["cedula"].ToString() + " and " +
                                                    bd2 + ".empleados.Companias_idCompania = '" + Session["proyecto"].ToString() + "' and " +
                                                    bd2 + ".parafiscales.Empleados_Id_Empleado = " + bd2 + ".empleados.Id_Empleado and " +
                                                    "Empleados_idEmpresa = 'ST' LIMIT 1";

                scSqlCommand = new MySqlCommand(consulta, MySqlCn);

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

        protected void BtnTodos_Click(object sender, EventArgs e)
        {
            LblMsj.Text = string.Empty;
            LblMsj.Visible = false;
            UpdatePanel3.Update();

            string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();

            try
            {
                MySqlCn = new MySqlConnection(Cn);
                MySqlCommand scSqlCommand;
                string consulta = "SELECT distinct idParafiscal, Anio_Parafiscal, Mes_Parafiscal, Ruta_archivo_Parafiscal as ruta, " +
                                                    "Nombre_archivo_parafiscal as nombre " +
                                                    "FROM " + bd2 + ".parafiscales, " + bd2 + ".empleados " +
                                                    "where " + bd2 + ".empleados.Companias_idCompania = '" + Session["proyecto"].ToString() + "' and " +
                                                    bd2 + ".parafiscales.Empleados_Id_Empleado = " + bd2 + ".empleados.Id_Empleado and " +
                                                    "Empleados_idEmpresa = 'ST'";

                scSqlCommand = new MySqlCommand(consulta, MySqlCn);

                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;
                MySqlCn.Open();
                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    string archivoZip = Session["usuario"].ToString() + ".zip";
                    string rutaArchivo = dtDataTable.Rows[0].ItemArray[3].ToString();
                    string rutazip = System.IO.Path.Combine(ruta, rutaArchivo, Session["usuario"].ToString());
                    string zipArchivo = System.IO.Path.Combine(ruta, rutaArchivo, archivoZip);

                    if (System.IO.File.Exists(zipArchivo))
                    {
                        System.IO.File.Delete(zipArchivo);
                    }

                    Response.Clear();
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "filename=" + archivoZip);

                    // Zip the contents of the selected files
                    using (ZipFile zip = new ZipFile())
                    {
                        for (int x = 0; x < dtDataTable.Rows.Count; x++)
                        {
                            string archivo = dtDataTable.Rows[x].ItemArray[4].ToString();
                            string filename = System.IO.Path.Combine(ruta, rutaArchivo, archivo);

                            if (System.IO.File.Exists(filename))
                            {
                                zip.AddFile(filename, "SegSocial");
                            }                            
                        }

                        //zip.Save(Response.OutputStream);
                        zip.Save(zipArchivo);
                    }

                    //Response.Close();

                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + archivoZip);
                    Response.WriteFile(zipArchivo);
                    Response.Flush();
                    Response.End();
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

        #endregion
    }
}