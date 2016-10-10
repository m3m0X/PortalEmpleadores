using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace PortalTrabajadores.Portal
{
    #region Definicion de la Clase Pagina Maestra

    public partial class PaginaMaestra : System.Web.UI.MasterPage
    {
        string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql"].ConnectionString.ToString();
        string bd1 = ConfigurationManager.AppSettings["BD1"].ToString();
        MySqlConnection MySqlCn;

        #region Definicion de los Metodos de la Clase

        #region Metodo Page Load
        /* ****************************************************************************/
        /* Metodo que se ejecuta al momento de la carga de la Pagina Maestra
        /* ****************************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usuario"] == null)
                {
                    //Redirecciona a la pagina de login en caso de que el usuario no se halla autenticado
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    //Valida que la pagina ya fue enviada al servidor para que no se cargue otra vez el control menu
                    if (!Page.IsPostBack)
                    {
                        lbldate.Text = DateTime.Now.ToLongDateString();
                        lbluserlogon.Text = Session["nombre"].ToString();

                        if (Session["nombreProyecto"] != null)
                        {
                            lblProyLogon.Text = Session["nombreProyecto"].ToString();
                            lblProyLogon.Visible = true;
                        }

                        if (Session["Menu"] == null)
                        {
                            bindMenuControl2();
                        }
                        else
                        {
                            bindMenuControl(false);
                        }
                    }
                    else
                    {
                        lbldate.Text = DateTime.Now.ToLongDateString();
                        lbluserlogon.Text = Session["nombre"].ToString();

                        if (Session["nombreProyecto"] != null)
                        {
                            lblProyLogon.Text = Session["nombreProyecto"].ToString();
                            lblProyLogon.Visible = true;
                        }

                        //Va a la base de datos para cargar el Menú
                        //Realice el cargue del Menu para todas las páginas
                        if (Session["Menu"] == null)
                        {
                            //Va a la base de datos
                            bindMenuControl(true);
                        }
                        else
                        {
                            //Lo trae de la Variable session
                            bindMenuControl(false);
                        }

                    }
                }
            }
            catch
            {
                //Mostrar pagina que el sistema no se encuentra disponible
                MensajeError("El Sistema no se encuentra disponible, intente más tarde.");
            }
        }
        #endregion

        #region Metodo Navigation_MenuItemClick
        /* ****************************************************************************/
        /* Metodo que Cierra la sesion de la pagina al dar clic en el Menu
        /* ****************************************************************************/
        protected void Navigation_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (e.Item.Text)
            {
                case "Salir":
                    Session.RemoveAll();
                    Response.Redirect("~/login.aspx");
                    return;
                case "Atras":
                    Session["Menu"] = null;
                    bindMenuControl2();
                    Response.Redirect("/SumitempEmpleadores/Portal/index.aspx");
                    return;
            }
        }
        #endregion

        #region Metodo bindMenuControl
        /* ****************************************************************************/
        /* Metodo que Carga Los Items del Control Menu
        /* ****************************************************************************/
        protected void bindMenuControl(Boolean valor)
        {
            ConsultasGenerales consultaGeneral = new ConsultasGenerales();

            if (valor)
            {
                CnMysql Conexion = new CnMysql(Cn);
                MySqlCommand cmd = new MySqlCommand();
                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter();
                DataTable dtDataTable = null;
                DataSet dsDataSet = new DataSet();

                try
                {
                    Conexion.AbrirCnMysql();
                    sdaSqlDataAdapter = new MySqlDataAdapter(bd1 + ".CargueMenuEmp", Conexion.ObtenerCnMysql());
                    sdaSqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sdaSqlDataAdapter.SelectCommand.Parameters.AddWithValue("@Id_Empresa", "ST");
                    sdaSqlDataAdapter.SelectCommand.Parameters.AddWithValue("@TipoPortal", "E");
                    sdaSqlDataAdapter.SelectCommand.Parameters.AddWithValue("@IdRol", Session["rol"].ToString());
                    sdaSqlDataAdapter.Fill(dsDataSet);

                    dtDataTable = dsDataSet.Tables[0];
                    if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow drDataRow in dtDataTable.Rows)
                        {
                            if (Convert.ToInt32(drDataRow[0]) == Convert.ToInt32(drDataRow[2]))
                            {
                                MySqlCommand rolCommand = new MySqlCommand("SELECT Id_Menu FROM " + bd1 + ".roles_menu WHERE Id_Rol = " + this.Session["rol"].ToString() + " AND Id_Menu = " + drDataRow[2], Conexion.ObtenerCnMysql());
                                MySqlDataAdapter rolDataAdapter = new MySqlDataAdapter(rolCommand);
                                DataSet rolDataSet = new DataSet();
                                DataTable rolDataTable = null;

                                rolDataAdapter.Fill(rolDataSet);
                                rolDataTable = rolDataSet.Tables[0];

                                if (rolDataTable != null && rolDataTable.Rows.Count > 0)
                                {
                                    MenuItem miMenuItem = new MenuItem(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[3]));
                                    this.MenuPrincipal.Items.Add(miMenuItem);
                                    AddChildItem(ref miMenuItem, dtDataTable);
                                }
                            }
                        }

                        Session["Menu"] = dsDataSet;
                    }
                }
                catch (Exception E)
                {
                    MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                finally
                {
                    if (Conexion.EstadoConexion() == ConnectionState.Open)
                    {
                        Conexion.CerrarCnMysql();
                        dtDataTable.Dispose();
                        dsDataSet.Dispose();
                        sdaSqlDataAdapter.Dispose();
                    }
                }
            }
            else
            {
                bool objetivos = consultaGeneral.ComprobarModuloObjetivos(Session["proyecto"].ToString(), Session["idEmpresa"].ToString());
                bool competencias = consultaGeneral.ComprobarModuloCompetencias(Session["proyecto"].ToString(), Session["idEmpresa"].ToString());

                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;
                try
                {
                    dsDataSet = (DataSet)Session["Menu"];
                    dtDataTable = dsDataSet.Tables[0];
                    this.MenuPrincipal.Items.Clear();
                    if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow drDataRow in dtDataTable.Rows)
                        {
                            if (Convert.ToInt32(drDataRow[0]) == Convert.ToInt32(drDataRow[2]))
                            {
                                MySqlConnection MySqlCn = new MySqlConnection(Cn);
                                MySqlCommand rolCommand = new MySqlCommand("SELECT Id_Menu FROM roles_menu WHERE Id_Rol = " + this.Session["rol"].ToString() + " AND Id_Menu = " + drDataRow[2], MySqlCn);
                                MySqlDataAdapter rolDataAdapter = new MySqlDataAdapter(rolCommand);
                                DataSet rolDataSet = new DataSet();
                                DataTable rolDataTable = null;

                                rolDataAdapter.Fill(rolDataSet);
                                rolDataTable = rolDataSet.Tables[0];

                                if (rolDataTable != null && rolDataTable.Rows.Count > 0)
                                {
                                    if (drDataRow[1].ToString().Contains("Parametros Objetivos") ||
                                        drDataRow[1].ToString().Contains("Reportes") ||
                                        drDataRow[1].ToString().Contains("Parametros Generales"))
                                    {
                                        if (objetivos)
                                        {
                                            /*Se eliminan todos los items del menu porque se esstan sumando */
                                            MenuItem miMenuItem = new MenuItem(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[3]));
                                            this.MenuPrincipal.Items.Add(miMenuItem);
                                            AddChildItem(ref miMenuItem, dtDataTable);
                                        }
                                        else if (competencias && drDataRow[1].ToString().Contains("Parametros Generales"))
                                        {
                                            /*Se eliminan todos los items del menu porque se esstan sumando */
                                            MenuItem miMenuItem = new MenuItem(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[3]));
                                            this.MenuPrincipal.Items.Add(miMenuItem);
                                            AddChildItem(ref miMenuItem, dtDataTable);
                                        }
                                    }
                                    else if (drDataRow[1].ToString().Contains("Parametros Competencias"))
                                    {
                                        if (competencias)
                                        {
                                            /*Se eliminan todos los items del menu porque se esstan sumando */
                                            MenuItem miMenuItem = new MenuItem(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[3]));
                                            this.MenuPrincipal.Items.Add(miMenuItem);
                                            AddChildItem(ref miMenuItem, dtDataTable);
                                        }
                                    }
                                    else if (drDataRow[1].ToString().Contains("Crear Empleados"))
                                    {
                                        if (objetivos || competencias)
                                        {
                                            /*Se eliminan todos los items del menu porque se esstan sumando */
                                            MenuItem miMenuItem = new MenuItem(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[3]));
                                            this.MenuPrincipal.Items.Add(miMenuItem);
                                            AddChildItem(ref miMenuItem, dtDataTable);
                                        }
                                    }   
                                    else
                                    {
                                        /*Se eliminan todos los items del menu porque se esstan sumando */
                                        MenuItem miMenuItem = new MenuItem(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[3]));
                                        this.MenuPrincipal.Items.Add(miMenuItem);
                                        AddChildItem(ref miMenuItem, dtDataTable);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception E)
                {
                    MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                finally
                {
                    dsDataSet.Dispose();
                    dtDataTable.Dispose();
                }
            }
        }
        #endregion

        #region Metodo bindMenuControl
        /* ****************************************************************************/
        /* Metodo que Carga Los Items del Control Menu Salir
        /* ****************************************************************************/
        protected void bindMenuControl2()
        {
            MySqlCn = new MySqlConnection(Cn);
            MySqlCommand scSqlCommand = new MySqlCommand("SELECT idOption_Menu, descripcion, idparent_option_Menu, url FROM " + bd1 + ".Options_Menu WHERE TipoPortal = 'E' and (idOption_Menu = 72 OR idOption_Menu = 29)", MySqlCn);
            MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(scSqlCommand);
            DataSet dsDataSet = new DataSet();
            DataTable dtDataTable = null;

            try
            {
                MySqlCn.Open();
                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    foreach (DataRow drDataRow in dtDataTable.Rows)
                    {
                        if (Convert.ToInt32(drDataRow[0]) == Convert.ToInt32(drDataRow[2]))
                        {
                            MySqlCommand rolCommand = new MySqlCommand("SELECT Id_Menu FROM " + bd1 + ".roles_menu WHERE Id_Rol = " + this.Session["rol"].ToString() + " AND Id_Menu = " + drDataRow[2], MySqlCn);
                            MySqlDataAdapter rolDataAdapter = new MySqlDataAdapter(rolCommand);
                            DataSet rolDataSet = new DataSet();
                            DataTable rolDataTable = null;

                            rolDataAdapter.Fill(rolDataSet);
                            rolDataTable = rolDataSet.Tables[0];

                            if (rolDataTable != null && rolDataTable.Rows.Count > 0)
                            {
                                MenuItem miMenuItem = new MenuItem(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[3]));
                                this.MenuPrincipal.Items.Add(miMenuItem);
                                AddChildItem(ref miMenuItem, dtDataTable);
                            }
                        }
                    }
                }

                MySqlCn.Close();
            }
            catch (Exception E)
            {
                MensajeError("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            finally
            {
                if (MySqlCn.State == ConnectionState.Open)
                {
                    MySqlCn.Close();
                    dtDataTable.Dispose();
                    dsDataSet.Dispose();
                    sdaSqlDataAdapter.Dispose();
                }
            }
        }
        #endregion

        #region Metodo AddChildItem
        /* ****************************************************************************/
        /* Metodo que agrega Los valores a los MenuItems
        /* ****************************************************************************/
        protected void AddChildItem(ref MenuItem miMenuItem, DataTable dtDataTable)
        {
            foreach (DataRow drDataRow in dtDataTable.Rows)
            {
                if (Convert.ToInt32(drDataRow[2]) == Convert.ToInt32(miMenuItem.Value) && Convert.ToInt32(drDataRow[0]) != Convert.ToInt32(drDataRow[2]))
                {
                    MenuItem miMenuItemChild = new MenuItem(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[3]));
                    miMenuItem.ChildItems.Add(miMenuItemChild);
                    AddChildItem(ref miMenuItemChild, dtDataTable);
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
    #endregion
}