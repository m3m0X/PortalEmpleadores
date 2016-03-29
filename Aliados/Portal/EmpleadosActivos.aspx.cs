using ClosedXML.Excel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PortalTrabajadores.Portal
{
    public partial class EmpleadosActivos : System.Web.UI.Page
    {
        string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql"].ConnectionString.ToString();
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
                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'EmpleadosActivos.aspx' AND idEmpresa = 'AE'", MySqlCn);
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

                try
                {
                    string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();

                    MySqlCn = new MySqlConnection(Cn);
                    MySqlCommand scSqlCommand = new MySqlCommand("SELECT " + bd2 + ".empleados.Id_Empleado AS Cedula, " +
                                                                    bd2 + ".empleados.Nombres_Completos_Empleado As Nombre, " +
                                                                    bd2 + ".empleados.Sexo_Empleado As Sexo, " +
                                                                    bd2 + ".empleados.Nombre_Cargo_Empleado As Cargo, " +
                                                                    bd2 + ".empleados.Fecha_nacimiento_Empleado AS 'Fecha de Nacimiento', " +
                                                                    bd2 + ".empleados.Correo_Empleado As Correo, " +
                                                                    bd2 + ".empleados.Fecha_Ingreso_Empleado As Ingreso, " +
                                                                    bd2 + ".empleados.Fecha_terminacion_Empleado As Terminacion, " +
                                                                    bd2 + ".empleados.Outsourcing As 'Centro de costos' " +
                                                                    "FROM " + bd2 + ".empleados " +
                                                                    "JOIN " + bd2 + ".companias ON " +
                                                                    bd2 + ".empleados.companias_idcompania = " + bd2 + ".companias.idCompania AND " +
                                                                    bd2 + ".empleados.Companias_idEmpresa = " + bd2 + ".companias.Empresas_idEmpresa " +
                                                                    "where Companias_idEmpresa = 'AE' AND  " +
                                                                    bd2 + ".companias.Terceros_Nit_Tercero = " + Session["usuario"].ToString() + " AND " +
                                                                    bd2 + ".companias.idCompania = '" + Session["proyecto"].ToString() + "' AND " +
                                                                    "(DATE_FORMAT(fecha_terminacion_Empleado,'%Y%m%d') > DATE_FORMAT('" + DateTime.Now.ToShortDateString() +
                                                                    "','%Y%m%d') OR Estado_Contrato_Empleado = 'A')", MySqlCn);

                    MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                    DataTable dtDataTable = new DataTable();
                    MySqlCn.Open();
                    sdaSqlDataAdapter.Fill(dtDataTable);

                    if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                    {
                        // Create the workbook
                        XLWorkbook workbook = new XLWorkbook();
                        workbook.Worksheets.Add(dtDataTable, "EmpleadosActivos");

                        // Prepare the response
                        HttpResponse httpResponse = Response;
                        httpResponse.Clear();
                        httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        httpResponse.AddHeader("content-disposition", "attachment;filename=\"EmpleadosActivos.xlsx\"");

                        // Flush the workbook to the Response.OutputStream
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            workbook.SaveAs(memoryStream);
                            memoryStream.WriteTo(httpResponse.OutputStream);
                            memoryStream.Close();
                        }

                        httpResponse.End();
                    }
                }
                catch (Exception E)
                {
                    MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
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
            ContentPlaceHolder cPlaceHolder;
            cPlaceHolder = (ContentPlaceHolder)Master.FindControl("Errores");
            if (cPlaceHolder != null)
            {
                Label lblMessage = (Label)cPlaceHolder.FindControl("LblMsj") as Label;
                if (lblMessage != null)
                {
                    lblMessage.Text = Msj;
                    lblMessage.Visible = true;
                }
            }
        }
        #endregion
                
        #endregion
    }
}