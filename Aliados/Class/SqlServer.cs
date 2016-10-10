using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/* Importamos las Librerias Necesarias para Trabajar */
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;


namespace PortalTrabajadores.Portal
{
    public class SqlServer
    {
        // Objetos de la Conexion
        private SqlConnection Sqlcon = new SqlConnection();
        private DataTable tb = new DataTable();

        public SqlServer(string conexion)
        {
            // Contendra los Datos de la Conexion.
            Sqlcon.ConnectionString = conexion;
        }

        public SqlConnection Obtener_SqlCon()
        {
            return Sqlcon;
        }

        public void Abrir() // Metodo para Abrir la Conexion
        {
            Sqlcon.Open();
        }

        public void Cerrar() // Metodo para Cerrar la Conexion y Liberar Recursos
        {
            Sqlcon.Close();
        }

        public DataTable mostrar(string Comando) // Metodo para Ejecutar Comandos y retorno de consulta
        {
            SqlDataAdapter cmd = new SqlDataAdapter(Comando, Sqlcon); // Creamos un DataAdapter con el Comando y la Conexion

            //DataSet DS = new DataSet(); // Creamos el DataSet que Devolvera el Metodo

            cmd.Fill(tb); // Ejecutamos el Comando en la Tabla
            
            return tb; // Regresamos el DataSet  
        }

        public void Ejecutar(string Comando) // Metodo para Ejecutar Comandos sin retorno
        {
            Sqlcon.Open();

            SqlCommand cmd2 = new SqlCommand(Comando, Sqlcon); // Creamos un Command con el Comando y la Conexion

            cmd2.ExecuteNonQuery(); // Ejecutamos el Comando en la Tabla sin retorno de datos

            Sqlcon.Close();

        }



        public int contar(string Comando, string usuario) // Metodo para Ejecutar Comandos
        {
            Sqlcon.Open();

            SqlCommand cmd2 = new SqlCommand(Comando, Sqlcon); // Creamos un Command con el Comando y la Conexion

            int a = Convert.ToInt32(cmd2.ExecuteScalar()); // Ejecutamos el Comando en la Tabla

            Sqlcon.Close();

            return a;

        }
    }
}
