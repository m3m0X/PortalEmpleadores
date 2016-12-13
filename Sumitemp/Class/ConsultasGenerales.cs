using MySql.Data.MySqlClient;
using PortalTrabajadores.Class;
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
        string CnBasica = ConfigurationManager.ConnectionStrings["CadenaConexioMySql"].ConnectionString.ToString();
        string CnTrabajadores = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();
        string CnObjetivos = ConfigurationManager.ConnectionStrings["CadenaConexioMySql3"].ConnectionString.ToString();
        string CnCompetencias = ConfigurationManager.ConnectionStrings["CadenaConexioMySql4"].ConnectionString.ToString();
        string bdBasica = ConfigurationManager.AppSettings["BD1"].ToString();
        string bdTrabajadores = ConfigurationManager.AppSettings["BD2"].ToString();
        string bdModobjetivos = ConfigurationManager.AppSettings["BD3"].ToString();
        string bdModCompetencias = ConfigurationManager.AppSettings["BD4"].ToString();

        /// <summary>
        /// Devuelve el inicio de sesion
        /// </summary>
        /// "(Activo_Empleado = 'A' OR Activo_Empleado = '1') and " +
        public DataTable InicioSesion(string usuario, string pass, string userter, bool tercero)
        {
            CnMysql Conexion = new CnMysql(CnObjetivos);

            try
            {
                Conexion.AbrirCnMysql();
                string consulta;

                if (tercero)
                {
                    string cifradaPass = CifrarContrasena.HashSHA1(pass);

                    consulta = "SELECT Ter.Nit_Tercero, usTer.id_Rol, usTer.nombreUsuario, comp.Empresas_idempresa, usTer.contrasenaActiva FROM " +
                               bdTrabajadores + ".usuariotercero AS usTer JOIN " +
                               bdTrabajadores + ".terceros AS ter ON usTer.Terceros_Nit_Tercero = ter.Nit_Tercero JOIN " +
                               bdTrabajadores + ".companias AS comp ON Ter.Nit_Tercero = comp.Terceros_Nit_Tercero " +
                               "WHERE nombreUsuario = '" + userter + "' AND usTer.Terceros_Nit_Tercero = '" + usuario + "'" +
                               " AND contrasena = '" + cifradaPass + "' AND activo = 1 " +
                               "and comp.Activo_Compania = '1' and (comp.Empresas_idEmpresa = 'ST' OR comp.Empresas_idEmpresa = 'SB') ;";
                }
                else
                {
                    consulta = "SELECT Nit_Tercero, Id_Rol, Razon_social, Empresas_idempresa FROM " +
                            bdTrabajadores + ".terceros JOIN " + bdTrabajadores + ".companias ON " +
                            bdTrabajadores + ".terceros.Nit_Tercero = " +
                            bdTrabajadores + ".companias.Terceros_Nit_Tercero where Nit_Tercero = '" + usuario +
                            "' and Contrasena_tercero = '" + pass +
                            "' and activo_tercero = '1' and (Empresas_idEmpresa = 'ST' OR Empresas_idEmpresa = 'SB') " +
                            " and Activo_Compania = 1";
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
        /// Comprueba si la compania tiene el modulo de objetivos activos
        /// </summary>
        /// <returns>True si esta activo</returns>
        public bool ComprobarModuloObjetivos(string idTercero, string idEmpresa)
        {
            CnMysql Conexion = new CnMysql(CnTrabajadores);

            try
            {
                MySqlCommand rolCommand = new MySqlCommand("SELECT * FROM " +
                                                            bdBasica + ".matriz_modulostercero where idModulo = 1 and idTercero = '" +
                                                            idTercero + "' and idEmpresa = '" +
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
        public bool ComprobarModuloCompetencias(string idTercero, string idEmpresa)
        {
            CnMysql Conexion = new CnMysql(CnTrabajadores);

            try
            {
                MySqlCommand rolCommand = new MySqlCommand("SELECT * FROM " +
                                                            bdBasica + ".matriz_modulostercero where idModulo = 2 and idTercero = '" +
                                                            idTercero + "' and idEmpresa = '" +
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

        /// <summary>
        /// Devuelve si el año es valido para ingresarlo
        /// </summary>
        /// <returns>true o false</returns>
        public bool ConsultarAnoValido(string idCompania, string idEmpresa, int ano)
        {
            CnMysql Conexion = new CnMysql(CnObjetivos);

            try
            {
                Conexion.AbrirCnMysql();
                string consulta;

                consulta = "select Ano FROM " + bdModobjetivos + ".parametrosgenerales where idCompania = '" +
                           idCompania + "' and Empresas_idEmpresa = '" + idEmpresa + "' AND ano = " +
                           ano + " order by ano desc;";

                MySqlCommand cmd = new MySqlCommand(consulta, Conexion.ObtenerCnMysql());
                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(cmd);
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;

                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
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
        /// Revisa si hay un año activo y devuelve su valor
        /// </summary>
        /// <returns>Año activo</returns>
        public string ConsultarAnoActivo(string idCompania, string idEmpresa)
        {
            CnMysql Conexion = new CnMysql(CnObjetivos);

            try
            {
                Conexion.AbrirCnMysql();
                string consulta;

                consulta = "select Ano FROM " + bdModobjetivos + ".parametrosgenerales where idCompania = '" +
                           idCompania + "' and Empresas_idEmpresa = '" + idEmpresa + "' AND Activo = " +
                           1 + " order by ano desc;";

                MySqlCommand cmd = new MySqlCommand(consulta, Conexion.ObtenerCnMysql());
                MySqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    return rd["Ano"].ToString();
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

        #region Areas

        public DataTable ConsultarAreas(string nit, string idEmpresa)
        {
            CnMysql Conexion = new CnMysql(CnTrabajadores);

            try
            {
                Conexion.AbrirCnMysql();
                string consulta = "SELECT areas.IdAreas, areas.IdAreas AS IdArea, " +
                    "IF((SELECT COUNT(subareas.Subarea) FROM subareas WHERE subareas.IdAreas = IdArea), " +
                    "(SELECT COUNT(subareas.Subarea) FROM subareas WHERE subareas.IdAreas = IdArea), 0) AS SubAreas, " +
                    "areas.Area, areas.Estado FROM " + bdTrabajadores + ".areas " +
                    "WHERE nittercero = " + nit + " AND Empresas_idEmpresa = '" + idEmpresa + "';";

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

        #endregion

        #region ModCompetencias

        #region Cargos

        /// <summary>
        /// Consulta los cargos en el sistema
        /// </summary>
        /// <returns>Datos de competencias</returns>
        public DataTable ConsultarCargos(string idTercero, string idEmpresa, string ano)
        {
            CnMysql Conexion = new CnMysql(CnCompetencias);

            try
            {
                Conexion.AbrirCnMysql();
                string consulta;

                consulta = "SELECT cargos.IdCargos, cargos.Cargo, " +
                           "(Select Count(*) from " + bdModCompetencias + ".cargocompetencias " +
                           "where cargocompetencias.idCargo = cargos.IdCargos and cargocompetencias.ano = '" +
                           ano + "') as NCompetencias " +
                           "FROM " + bdTrabajadores + ".cargos " +
                           "where nittercero = " + idTercero +
                           " and Empresas_idEmpresa = '" + idEmpresa + "'" +
                           " and Estado = 1;";

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

        #endregion

        #region Competencias

        /// <summary>
        /// Consulta el nivel de competencias
        /// </summary>
        /// <returns>Datos de competencias</returns>
        public DataTable ConsultarCompetencias(string idTercero, string idCompania, string idEmpresa, string ano)
        {
            CnMysql Conexion = new CnMysql(CnCompetencias);

            try
            {
                Conexion.AbrirCnMysql();
                string consulta;

                consulta = "SELECT idCompetencia, competencia, descripcion," +
                           " activo FROM " + bdModCompetencias +
                           ".competencias where idTercero = " + idTercero + " and idCompania = '" +
                           idCompania + "' and idEmpresa = '" + idEmpresa + "' and ano = '" + ano + "';";

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
        /// Consulta el nivel de competencias
        /// </summary>
        /// <returns>Datos de competencias</returns>
        public DataTable ConsultarCompetenciasXCargo(string idCompania, string idEmpresa, int idCargo, string ano)
        {
            CnMysql Conexion = new CnMysql(CnCompetencias);

            try
            {
                Conexion.AbrirCnMysql();
                string consulta;

                consulta = "SELECT cargocompetencias.idCompetencia," +
                           "(Select nombre from " + bdModCompetencias + ".nivelcompetencias as nc " +
                           "where nc.idNivelCompetencias = cargocompetencias.idNivelCompetencias) as nivelCompetencia," +
                           "competencias.competencia, cargocompetencias.estado, " +
                           "(SELECT Count(*) FROM " + bdModCompetencias + ".cargocompconductas AS ccc " +
                           "WHERE ccc.idCompetencia = cargocompetencias.idCompetencia " +
                           "AND ccc.idCargo = " + idCargo + " AND ccc.estado = 1) AS conductas " +
                           "FROM " + bdModCompetencias + ".cargocompetencias " +
                           "INNER JOIN " + bdModCompetencias + ".competencias ON " +
                           "cargocompetencias.idCompetencia = competencias.idCompetencia " +
                           "WHERE cargocompetencias.idCargo = " + idCargo +
                           " AND cargocompetencias.idCompania = '" + idCompania + "'" +
                           " AND cargocompetencias.idEmpresa = '" + idEmpresa + "'" +
                           " AND cargocompetencias.ano = '" + ano + "';";

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
        /// Devuelve el cargo ya existe
        /// </summary>
        /// <returns>true o false</returns>
        public bool ConsultarCargoCompetencia(string idCompania, string idEmpresa, int idCargo, int idCompetencia, string ano)
        {
            CnMysql Conexion = new CnMysql(CnObjetivos);

            try
            {
                Conexion.AbrirCnMysql();
                string consulta;

                consulta = "SELECT * FROM " + bdModCompetencias + ".cargocompetencias " +
                           "WHERE idCargo = " + idCargo +
                           " AND idCompetencia = " + idCompetencia +
                           " AND idCompania = '" + idCompania + "'" +
                           " AND idEmpresa = '" + idEmpresa + "'" +
                           " AND ano = '" + ano + "';";

                MySqlCommand cmd = new MySqlCommand(consulta, Conexion.ObtenerCnMysql());
                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(cmd);
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;

                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
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

        #endregion

        #region Conductas

        /// <summary>
        /// Consulta las conductas.
        /// </summary>
        /// <returns>Datos de conductas.</returns>
        public DataTable ConsultarConductas(string idTercero, string idCompania, string idEmpresa, string ano)
        {
            CnMysql Conexion = new CnMysql(CnCompetencias);

            try
            {
                Conexion.AbrirCnMysql();
                string consulta;

                consulta = "SELECT idConducta, conducta, " +
                           " activo FROM " + bdModCompetencias +
                           ".conductas where idTercero = " + idTercero + " and idCompania = '" +
                           idCompania + "' and idEmpresa = '" + idEmpresa + "' and ano = '" + ano + "';";

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
        /// Consulta los cargos con conductas
        /// </summary>
        /// <param name="idCargo">id cargos</param>
        /// <param name="idCompetencia">id competencia</param>
        /// <param name="idCompania">id compañia</param>
        /// <param name="idEmpresa">id empresa</param>
        /// <param name="ano">ano consulta</param>
        /// <returns>Conductas con cargos</returns>
        public DataTable ConsultarCargosConductas(string idCargo, string idCompetencia, string idCompania, string idEmpresa, string ano)
        {
            CnMysql Conexion = new CnMysql(CnCompetencias);

            try
            {
                Conexion.AbrirCnMysql();
                string consulta;

                consulta = "SELECT ccc.idCarConCom, ccc.idConducta, " +
                           "cond.conducta FROM " + bdModCompetencias +
                           ".cargocompconductas ccc INNER JOIN " + bdModCompetencias +
                           ".conductas cond ON ccc.idConducta = cond.idConducta WHERE " +
                           "ccc.idCargo = " + idCargo + " AND ccc.idCompetencia = " + idCompetencia +
                           " AND ccc.idCompania = '" + idCompania + "' AND ccc.idEmpresa = '" + idEmpresa + "'" +
                           " AND ccc.ano = " + ano + " AND estado = 1;";

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
        /// Consulta las conductas disponibles
        /// </summary>
        /// <param name="dtCargoConductas">conductas cargadas en el cargo</param>
        /// <param name="idTercero">id tercero</param>
        /// <param name="idCompania">id compañia</param>
        /// <param name="idEmpresa">id empresa</param>
        /// <param name="ano">año seleccionado</param>
        /// <returns>Conductas disponibles</returns>
        public DataTable ConsultaConductasActivas(DataTable dtCargoConductas, string idTercero, string idCompania, string idEmpresa, string ano)
        {
            CnMysql Conexion = new CnMysql(CnCompetencias);

            try
            {
                Conexion.AbrirCnMysql();
                string consulta;

                consulta = "SELECT idConducta, conducta FROM " + bdModCompetencias +
                           ".conductas WHERE idTercero = " + idTercero + " AND idCompania = '" + idCompania + "' " +
                           "AND idEmpresa = '" + idEmpresa + "' AND ano = " + ano + " AND activo = 1;";
        
                MySqlCommand cmd = new MySqlCommand(consulta, Conexion.ObtenerCnMysql());
                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(cmd);
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;

                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    if (dtCargoConductas != null)
                    {
                        foreach (DataRow row in dtCargoConductas.Rows)
                        {
                            string idConducta = row["idConducta"].ToString();

                            for (int i = 0; i < dtDataTable.Rows.Count; i++)
                            {
                                DataRow dato = dtDataTable.Rows[i];

                                if (dato["idConducta"].ToString() == idConducta)
                                {
                                    dtDataTable.Rows.Remove(dato);
                                }
                            }
                        }
                    }

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


        public bool ExisteCargoConducta(string idConducta, string idCompania, string idEmpresa, string idCargo, string idCompetencia, string ano)
        {
            CnMysql Conexion = new CnMysql(CnObjetivos);

            try
            {
                Conexion.AbrirCnMysql();
                string consulta;

                consulta = "SELECT idCarConCom, idConducta " +
                           "FROM " + bdModCompetencias + ".cargocompconductas WHERE " +
                           "idCargo = " + idCargo + " AND idCompetencia = " + idCompetencia +
                           " AND idCompania = '" + idCompania + "' AND idEmpresa = '" + idEmpresa + "'" +
                           " AND ano = " + ano + " AND idConducta = " + idConducta + ";";

                MySqlCommand cmd = new MySqlCommand(consulta, Conexion.ObtenerCnMysql());
                MySqlDataAdapter sdaSqlDataAdapter = new MySqlDataAdapter(cmd);
                DataSet dsDataSet = new DataSet();
                DataTable dtDataTable = null;

                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];

                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
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

        #endregion

        #region NivelCompetencias

        /// <summary>
        /// Consulta el nivel de competencias
        /// </summary>
        /// <returns>Datos de competencias</returns>
        public DataTable ConsultarNivelCompetencias(string idTercero, string idCompania, string idEmpresa, string ano)
        {
            CnMysql Conexion = new CnMysql(CnCompetencias);

            try
            {
                Conexion.AbrirCnMysql();
                string consulta;

                consulta = "SELECT * FROM " + bdModCompetencias + ".nivelcompetencias"
                            + " where idTercero = " + idTercero
                            + " and idCompania = '" + idCompania
                            + "' and idEmpresa = '" + idEmpresa 
                            + "' and ano = '" + ano + "';";

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

        #endregion

        #endregion
    }
}