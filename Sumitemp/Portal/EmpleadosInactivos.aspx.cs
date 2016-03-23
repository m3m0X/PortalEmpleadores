﻿using ClosedXML.Excel;
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
    public partial class EmpleadosInactivos : System.Web.UI.Page
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
                        MySqlCommand scSqlCommand = new MySqlCommand("SELECT descripcion FROM Options_Menu WHERE url = 'EmpleadosInactivos.aspx' AND idEmpresa = 'ST'", MySqlCn);
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
        /* ****************************************************************************/
        /* Evento que se produce al dar clic sobre el control IMG del GridView
        /* ****************************************************************************/
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Excel")
                {
                    string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();

                    MySqlCn = new MySqlConnection(Cn);
                    MySqlCommand scSqlCommand = new MySqlCommand(Session["ConsultaInactivos"].ToString(), MySqlCn);

                    MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                    DataTable dtDataTable = new DataTable();
                    MySqlCn.Open();
                    sdaSqlDataAdapter.Fill(dtDataTable);
                    if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                    {
                        string filename = "";

                        if (ddlEstado.SelectedValue == "1")
                        {
                            filename = "Ingresos.xlsx";
                        }
                        else if (ddlEstado.SelectedValue == "2")
                        {
                            filename = "Retiros.xlsx";
                        }
                        else if (ddlEstado.SelectedValue == "3")
                        {
                            filename = "IngresosyRetiros.xlsx";
                        }
                        
                        // Create the workbook
                        XLWorkbook workbook = new XLWorkbook();
                        workbook.Worksheets.Add(dtDataTable, "Sheet 1");

                        // Prepare the response
                        HttpResponse httpResponse = Response;
                        httpResponse.Clear();
                        httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        httpResponse.AddHeader("content-disposition", "attachment;filename=" + filename + "");

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
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();
            try
            {
                MySqlCn = new MySqlConnection(Cn);
                if (ddlEstado.SelectedValue == "1") 
                { 
                    /*Ingresos*/
                    Session["ConsultaInactivos"] = "SELECT trabajadores.empleados.Id_Empleado AS Cedula, " +
                                                   "trabajadores.empleados.Nombres_Completos_Empleado As Nombre, " +
                                                   "trabajadores.empleados.Sexo_Empleado As Sexo, " +
                                                   "trabajadores.empleados.Nombre_Cargo_Empleado As Cargo, " +
                                                   "trabajadores.empleados.Correo_Empleado As Correo, " +
                                                   "trabajadores.empleados.Fecha_Ingreso_Empleado As Ingreso, " +
                                                   "trabajadores.empleados.Fecha_terminacion_Empleado As Terminacion, " +
                                                   "trabajadores.empleados.Outsourcing As 'Centro de costos' " +
                                                   "FROM trabajadores.empleados " + 
                                                   "JOIN trabajadores.companias ON " +
                                                   "trabajadores.empleados.companias_idcompania = trabajadores.companias.idCompania AND " +
                                                   "trabajadores.empleados.Companias_idEmpresa = trabajadores.companias.Empresas_idEmpresa " +
                                                   "where Companias_idEmpresa = '" + Session["idEmpresa"].ToString() + "' AND  " +
                                                   "trabajadores.companias.Terceros_Nit_Tercero = " + Session["usuario"].ToString() + " AND " +
                                                   "trabajadores.companias.idCompania = '" + Session["proyecto"].ToString() + "' AND " + 
                                                   "year(Fecha_Ingreso_Empleado) = " + txtAnio.Text + " and " +
                                                   "month(Fecha_Ingreso_Empleado) = " + ddlMes.SelectedValue;
                }
                else if (ddlEstado.SelectedValue == "2")
                {
                    /*Retiros*/
                    Session["ConsultaInactivos"] = "SELECT trabajadores.empleados.Id_Empleado AS Cedula, " +
                                                   "trabajadores.empleados.Nombres_Completos_Empleado As Nombre, " +
                                                   "trabajadores.empleados.Sexo_Empleado As Sexo, " +
                                                   "trabajadores.empleados.Nombre_Cargo_Empleado As Cargo, " +
                                                   "trabajadores.empleados.Correo_Empleado As Correo, " +
                                                   "trabajadores.empleados.Fecha_Ingreso_Empleado As Ingreso, " +
                                                   "trabajadores.empleados.Fecha_terminacion_Empleado As Terminacion, " +
                                                   "trabajadores.empleados.Outsourcing As 'Centro de costos' " +
                                                   "FROM trabajadores.empleados " + 
                                                   "JOIN trabajadores.companias ON " +
                                                   "trabajadores.empleados.companias_idcompania = trabajadores.companias.idCompania AND " +
                                                   "trabajadores.empleados.Companias_idEmpresa = trabajadores.companias.Empresas_idEmpresa " +
                                                   "where Companias_idEmpresa = '" + Session["idEmpresa"].ToString() + "' AND  " +
                                                   "trabajadores.companias.Terceros_Nit_Tercero = " + Session["usuario"].ToString() + " AND " +
                                                   "trabajadores.companias.idCompania = '" + Session["proyecto"].ToString() + "' AND " + 
                                                   "year(Fecha_terminacion_Empleado) = " + txtAnio.Text + " and " +
                                                   "month(Fecha_terminacion_Empleado) = " + ddlMes.SelectedValue;
                }
                else if (ddlEstado.SelectedValue == "3")
                {
                    /*Ingresos y Retiros*/
                    Session["ConsultaInactivos"] = "SELECT trabajadores.empleados.Id_Empleado AS Cedula, " +
                                                   "trabajadores.empleados.Nombres_Completos_Empleado As Nombre, " +
                                                   "trabajadores.empleados.Sexo_Empleado As Sexo, " +
                                                   "trabajadores.empleados.Nombre_Cargo_Empleado As Cargo, " +
                                                   "trabajadores.empleados.Correo_Empleado As Correo, " +
                                                   "trabajadores.empleados.Fecha_Ingreso_Empleado As Ingreso, " +
                                                   "trabajadores.empleados.Fecha_terminacion_Empleado As Terminacion, " +
                                                   "trabajadores.empleados.Outsourcing As 'Centro de costos' " +
                                                   "FROM trabajadores.empleados " + 
                                                   "JOIN trabajadores.companias ON " +
                                                   "trabajadores.empleados.companias_idcompania = trabajadores.companias.idCompania AND " +
                                                   "trabajadores.empleados.Companias_idEmpresa = trabajadores.companias.Empresas_idEmpresa " +
                                                   "where trabajadores.empleados.Companias_idEmpresa = '" + Session["idEmpresa"].ToString() + "' AND  " +
                                                   "trabajadores.companias.Terceros_Nit_Tercero = " + Session["usuario"].ToString() + " AND " +
                                                   "trabajadores.companias.idCompania = '" + Session["proyecto"].ToString() + "' AND " + 
                                                   "(year(Fecha_Ingreso_Empleado) = " + txtAnio.Text + " OR " +
                                                   "year(Fecha_terminacion_Empleado) = " + txtAnio.Text + " ) AND" +
                                                   "(month(Fecha_Ingreso_Empleado) = " + ddlMes.SelectedValue + " OR " +
                                                   "month(Fecha_terminacion_Empleado) = " + ddlMes.SelectedValue + ")";
                }

                MySqlCommand scSqlCommand = new MySqlCommand(Session["ConsultaInactivos"].ToString(), MySqlCn);
                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;
                MySqlCn.Open();
                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];
                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    DataTable datos = new DataTable();
                    datos.Columns.Add("Mes");
                    DataRow _ravi = datos.NewRow();
                    _ravi["Mes"] = ddlMes.SelectedItem.Text;
                    datos.Rows.Add(_ravi);

                    GridView1.DataSource = datos;
                    GridView1.DataBind();
                }
                else
                {
                    //Muestra un Mensaje en la aplicacion indicando al usuario que se abrio una nueva ventana
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    MensajeError("No existen datos en este mes.");
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

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable datos = (DataTable)Session["NominaEmpleados"];
            GridView gv = (GridView)sender;
            gv.PageIndex = e.NewPageIndex;

            if (datos != null)
            {
                gv.DataSource = datos;
                gv.DataBind();
            }
        }

        #endregion
    }
}