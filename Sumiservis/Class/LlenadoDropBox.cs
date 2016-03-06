using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;

namespace PortalTrabajadores.Portal
{
    public class LlenadoDropBox
    {
        public DataTable LoadTipoID(string command)
        {
            try
            {
                CnMysql loadCN = new CnMysql(ConfigurationManager.ConnectionStrings["CadenaConexioMySql"].ConnectionString);                
                loadCN.AbrirCnMysql();
                DataTable dbtable = loadCN.ConsultarRegistros(command);
                return dbtable;
            }
            catch (Exception ex)
            {
                throw new System.Exception("Error presentado.", ex);
            }
        }

    }
}