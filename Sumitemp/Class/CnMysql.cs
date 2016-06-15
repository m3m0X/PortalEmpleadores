using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace PortalTrabajadores.Portal
{
    class CnMysql
    {
        // Objetos de la Conexion
        private MySqlConnection conexion = new MySqlConnection();
        private DataTable tb = new DataTable();        

        // Contendra los Datos de la Conexion.
        public CnMysql(string cadenaConexion)
        {
            conexion.ConnectionString = cadenaConexion;
        }

        // Metodo para Abrir la Conexion
        public void AbrirCnMysql()
        {
            conexion.Open();
        }

        // Metodo para Obtener el estado de la Conexion
        public ConnectionState EstadoConexion()
        {
            return conexion.State;
        }

        //Obtiene la cadena de conexion actual
        public MySqlConnection ObtenerCnMysql()
        {
            return conexion;
        }

        // Metodo para Cerrar la Conexion y Liberar Recursos
        public void CerrarCnMysql()
        {
            conexion.Close();
        }

        // Metodo para Ejecutar Comandos y retorno de consulta
        public DataTable ConsultarRegistros(string Comando)
        {
            // Creamos un DataAdapter con el Comando y la Conexion
            MySqlDataAdapter cmd = new MySqlDataAdapter(Comando, conexion);
            //DataSet DS = new DataSet(); // Creamos el DataSet que Devolvera el Metodo
            cmd.Fill(tb); // Ejecutamos el Comando en la Tabla

            return tb; // Regresamos el DataSet  
        }

        // Metodo para Ejecutar Comandos sin retorno
        public void EjecutarComando(string Comando)
        {
            int registrosafectados = 0;
            MySqlCommand cmd2 = new MySqlCommand(Comando, conexion); // Creamos un Command con el Comando y la Conexion
            registrosafectados = cmd2.ExecuteNonQuery(); // Ejecutamos el Comando en la Tabla sin retorno de datos
        }

        // Metodo para Ejecutar Comandos con retorno
        public int EjecutarComandoCon(string Comando)
        {
            int registrosafectados = 0;
            MySqlCommand cmd2 = new MySqlCommand(Comando, conexion); // Creamos un Command con el Comando y la Conexion
            registrosafectados = cmd2.ExecuteNonQuery(); // Ejecutamos el Comando en la Tabla sin retorno de datos
            return registrosafectados;
        }

        // Metodo para Ejecutar Comandos
        public int ContarRegistros(string Comando, string usuario) 
        {
            MySqlCommand cmd2 = new MySqlCommand(Comando, conexion); // Creamos un Command con el Comando y la Conexion
            int a = Convert.ToInt32(cmd2.ExecuteScalar()); // Ejecutamos el Comando en la Tabla
            return a;
        }

        //Este metodo contiene un DataReader que lee todos los registros
        //y carga la información a la tabla con la ayuda del evento ExecuteReader del comando.
        public static DataTable EjecutarComandoSelect(MySqlCommand comando)
        {
            DataTable _tabla;
            try
            {
                comando.Connection.Open();
                MySqlDataReader _lector = comando.ExecuteReader();
                _tabla = new DataTable();
                _tabla.Load(_lector);
                _lector.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                comando.Connection.Close();
            }
            return _tabla;
        }

        //Este método es reutilizable para poder crear comandos.
        public static MySqlCommand CrearComando()
        {
            string _cadenaConexion = " server = localhost; user id = root; database = myCompany";
            MySqlConnection _conexion = new MySqlConnection();
            _conexion.ConnectionString = _cadenaConexion;
            MySqlCommand _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.Text;
            return _comando;
        }


    }
}

