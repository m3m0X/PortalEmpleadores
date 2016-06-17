using MySql.Data.MySqlClient;
using PortalTrabajadores.Portal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace PortalTrabajadores.Portal
{
    public class ConsultasGenerales
    {
        string CnObjetivos = ConfigurationManager.ConnectionStrings["CadenaConexioMySql3"].ConnectionString.ToString();
        string CnTrabajadores = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();
        string CnBasica = ConfigurationManager.ConnectionStrings["CadenaConexioMySql"].ConnectionString.ToString();        
        string bdBasica = ConfigurationManager.AppSettings["BD1"].ToString();
        string bdTrabajadores = ConfigurationManager.AppSettings["BD2"].ToString();
        string bdModobjetivos = ConfigurationManager.AppSettings["BD3"].ToString();

        public ConsultasGenerales()
        {            
        }

        /// <summary>
        /// Comprueba si la compania tiene el modulo de objetivos activos
        /// </summary>
        /// <returns>True si esta activo</returns>
        public bool ComprobarModuloObjetivos(string idCompania, string idEmpresa)
        {
            CnMysql Conexion = new CnMysql(CnTrabajadores);

            try
            {
                MySqlCommand rolCommand = new MySqlCommand("SELECT * FROM " +
                                                            bdBasica + ".matriz_modulostercero where idModulo = 1 and idCompania = '" + 
                                                            idCompania + "' and idEmpresa = '" + 
                                                            idEmpresa + "'", Conexion.ObtenerCnMysql());

                MySqlDataAdapter rolDataAdapter = new MySqlDataAdapter(rolCommand);
                DataSet rolDataSet = new DataSet();
                DataTable rolDataTable = null;

                rolDataAdapter.Fill(rolDataSet);
                rolDataTable = rolDataSet.Tables[0];

                if (rolDataTable != null && rolDataTable.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (Conexion.EstadoConexion() == ConnectionState.Open)
                {
                    Conexion.CerrarCnMysql();
                }
            }
        }

        /// <summary>
        /// Comprueba si la compania tiene el modulo de competencias activos
        /// </summary>
        /// <returns>True si esta activo</returns>
        public bool ComprobarModuloCompetencias(string idCompania, string idEmpresa)
        {
            CnMysql Conexion = new CnMysql(CnTrabajadores);

            try
            {
                MySqlCommand rolCommand = new MySqlCommand("SELECT * FROM " +
                                                            bdBasica + ".matriz_modulostercero where idModulo = 2 and idCompania = '" +
                                                            idCompania + "' and idEmpresa = '" +
                                                            idEmpresa + "'", Conexion.ObtenerCnMysql());

                MySqlDataAdapter rolDataAdapter = new MySqlDataAdapter(rolCommand);
                DataSet rolDataSet = new DataSet();
                DataTable rolDataTable = null;

                rolDataAdapter.Fill(rolDataSet);
                rolDataTable = rolDataSet.Tables[0];

                if (rolDataTable != null && rolDataTable.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (Conexion.EstadoConexion() == ConnectionState.Open)
                {
                    Conexion.CerrarCnMysql();
                }
            }
        }

        /// <summary>
        /// Consulta el periodo seleccionado (semestre trimestre)
        /// </summary>
        /// <param name="idCompania">Id Compañia</param>
        /// <param name="idEmpresa">Id Empresa</param>
        /// <returns>Valor del periodo</returns>
        public string ConsultarPeriodoSeguimiento(string idCompania, string idEmpresa, int ano)
        {
            CnMysql Conexion = new CnMysql(CnTrabajadores);

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd = new MySqlCommand("SELECT Periodo_Seguimiento FROM " + bdModobjetivos +
                                                    ".parametrosgenerales where idCompania = '" + idCompania +
                                                    "' AND Empresas_idEmpresa = '" + idEmpresa + 
                                                    "' AND Ano = '" + ano + "'", Conexion.ObtenerCnMysql());
                MySqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    return rd["Periodo_Seguimiento"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                Conexion.CerrarCnMysql();
            }
        }

        /// <summary>
        /// Comprueba que el peso de los objetivos sea igual a 100
        /// </summary>
        /// <param name="idJefeEmpleado">Id jefe empleado</param>
        /// <returns>Devuelve true si cumple los 100</returns>
        public string ComprobarPesoObjetivos(string idJefeEmpleado) 
        {
            CnMysql Conexion = new CnMysql(CnTrabajadores);

            try
            {
                Conexion.AbrirCnMysql();
                MySqlCommand cmd = new MySqlCommand("SELECT sum(Peso) as Peso FROM " + bdModobjetivos +
                                                    ".objetivos where JefeEmpleado_idJefeEmpleado = " + idJefeEmpleado, Conexion.ObtenerCnMysql());
                MySqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    return rd["Peso"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            {
                Conexion.CerrarCnMysql();
            }
        }

        /// <summary>
        /// Dependiendo del periodo devuelve las etapas a mostrar
        /// </summary>
        /// <param name="periodo">Periodo activo</param>
        /// <returns>Datatable con los valores</returns>
        public DataTable ConsultarEtapasActivas(string periodo)
        {
            CnMysql Conexion = new CnMysql(CnObjetivos);

            try
            {
                Conexion.AbrirCnMysql();
                string consulta;

                if (periodo == "1") 
                {
                    consulta = "SELECT idEtapas, Etapa FROM " + bdModobjetivos + ".etapas WHERE idEtapas = 1 or idEtapas = 4";
                }
                else if (periodo == "2") 
                {
                    consulta = "SELECT idEtapas, Etapa FROM " + bdModobjetivos + ".etapas WHERE idEtapas != 3";
                }
                else 
                {
                    consulta = "SELECT idEtapas, Etapa FROM " + bdModobjetivos + ".etapas";
                }
                
                MySqlCommand cmd = new MySqlCommand(consulta, Conexion.ObtenerCnMysql());
                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(cmd);
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;
                
                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    return dtDataTable;   
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conexion.CerrarCnMysql();
            }
        }

        /// <summary>
        /// Devuelve los años registrados en el sistema
        /// </summary>
        /// <returns></returns>
        public DataTable ConsultarAnos(string idCompania, string idEmpresa)
        {
            CnMysql Conexion = new CnMysql(CnObjetivos);

            try
            {
                Conexion.AbrirCnMysql();
                string consulta;

                consulta = "select Ano FROM " + bdModobjetivos + ".parametrosgenerales where idCompania = '" +
                           idCompania + "' and Empresas_idEmpresa = '" + idEmpresa + "' order by ano desc;";

                MySqlCommand cmd = new MySqlCommand(consulta, Conexion.ObtenerCnMysql());
                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(cmd);
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;

                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    return dtDataTable;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conexion.CerrarCnMysql();
            }
        } 
    }
}