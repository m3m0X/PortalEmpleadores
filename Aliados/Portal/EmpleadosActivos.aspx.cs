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
                    MySqlCommand scSqlCommand = new MySqlCommand("SELECT trabajadores.empleados.Id_Empleado AS Cedula, " +
                                                                    "trabajadores.empleados.Nombres_Completos_Empleado As Nombre, " +
                                                                    "trabajadores.empleados.Sexo_Empleado As Sexo, " +
                                                                    "trabajadores.empleados.Nombre_Cargo_Empleado As Cargo, " +
                                                                    "trabajadores.empleados.Fecha_nacimiento_Empleado AS 'Fecha de Nacimiento', " +
                                                                    "trabajadores.empleados.Correo_Empleado As Correo, " +
                                                                    "trabajadores.empleados.Fecha_Ingreso_Empleado As Ingreso, " +
                                                                    "trabajadores.empleados.Fecha_terminacion_Empleado As Terminacion, " +
                                                                    "trabajadores.empleados.Outsourcing As 'Centro de costos' " +
                                                                    "FROM trabajadores.empleados " +
                                                                    "JOIN trabajadores.companias ON " +
                                                                    "trabajadores.empleados.companias_idcompania = trabajadores.companias.idCompania AND " +
                                                                    "trabajadores.empleados.Companias_idEmpresa = trabajadores.companias.Empresas_idEmpresa " +
                                                                    "where Companias_idEmpresa = 'AE' AND  " +
                                                                    "trabajadores.companias.Terceros_Nit_Tercero = " + Session["usuario"].ToString() + " AND " +
                                                                    "trabajadores.companias.idCompania = '" + Session["proyecto"].ToString() + "' AND " +
                                                                    "(DATE_FORMAT(fecha_terminacion_Empleado,'%Y%m%d') > DATE_FORMAT('" + DateTime.Now.ToShortDateString() +
                                                                    "','%Y%m%d') OR Estado_Contrato_Empleado = 'A')", MySqlCn);

                    MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                    DataTable dtDataTable = new DataTable();
                    MySqlCn.Open();
                    sdaSqlDataAdapter.Fill(dtDataTable);
                    if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                    {
                        string filename = "EmpleadosActivos.xls";
                        System.IO.StringWriter tw = new System.IO.StringWriter();
                        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                        DataGrid dgGrid = new DataGrid();
                        dgGrid.DataSource = dtDataTable;
                        dgGrid.DataBind();

                        //Get the HTML for the control.
                        dgGrid.RenderControl(hw);
                        //Write the HTML back to the browser.
                        //Response.ContentType = application/vnd.ms-excel;
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                        this.EnableViewState = false;
                        Response.Write(tw.ToString());
                        Response.Flush();
                        Response.Close();
                    }
                }
                catch (Exception E)
                {
                    MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                finally
                {
                    Response.ClearContent();
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