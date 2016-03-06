using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.html;
using iTextSharp.text;
using System.IO;
using System.Net;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;
using System.Drawing;

namespace PortalTrabajadores.Portal
{
    public partial class Visual : System.Web.UI.Page
    {
        #region Definicion de los Metodos de la Clase

        #region Metodo Page Load
        /* ****************************************************************************/
        /* Metodo que se ejecuta al momento de la carga de la Pagina Maestra
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
                //generarCertificado(Session["usuario"].ToString());
                generarCertificado(Session["cedula"].ToString(), Session["usuario"].ToString());
            }
        }
        #endregion

        #region Metodo MesLetras
        /* ****************************************************************************/
        /* Metodo que devuelve el valor en letras del Mes consultado en Numeros
        /* ****************************************************************************/
        public string mesLetras(int m)
        {
            string mes = "MMMM";
            String[] Mes = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            mes = Mes[m - 1].ToString();
            return mes;
        }
        #endregion

        #region Metodo generarCertificado
        /* ****************************************************************************/
        /* Metodo que genera el Certificado Laboral en una ventana Externa
        /* ****************************************************************************/
        public void generarCertificado(string id_Empleado, string id_NitEmpresa)
        {
            string Cn = ConfigurationManager.ConnectionStrings["CadenaConexioMySql2"].ConnectionString.ToString();
            CnMysql Conexion = new CnMysql(Cn);
            MySqlDataReader reader = null;
            MySqlCommand cmd = new MySqlCommand();
            Document document = new Document(PageSize.LETTER);
            try
            {
                Conexion.AbrirCnMysql();
                cmd = new MySqlCommand("sp_GenCertificadoEmpleados", Conexion.ObtenerCnMysql());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NumCed", id_Empleado);
                cmd.Parameters.AddWithValue("@NitCompania", id_NitEmpresa);
                cmd.Parameters.AddWithValue("@IdEmpresa", "AE");
                cmd.Parameters.AddWithValue("@ProyectoCompania", Session["proyecto"].ToString());

                reader = cmd.ExecuteReader();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "inline;filename=CertificadoLaboral.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);               

                // Read in the contents of the Receipt.htm file...
                string contents = File.ReadAllText(Server.MapPath("~/Html/Aliados.html"));

                PdfWriter.GetInstance(document, Response.OutputStream);

                //open Document
                document.Open();

                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Img/Aliados.jpg"));
                logo.ScaleAbsolute(205, 44);
                logo.SetAbsolutePosition(205, 685);
                //logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                document.Add(logo);

                reader.Read();

                string FechaInicial = reader["Fecha_Ingreso_Empleado"].ToString();
                DateTime dt = new DateTime(Convert.ToInt16(FechaInicial.Substring(0, 4)), Convert.ToInt16(FechaInicial.Substring(4, 2)), Convert.ToInt16(FechaInicial.Substring(6, 2)));
                string fechaGeneracion = reader["Gen_Nomina"].ToString();
                DateTime dt2 = new DateTime(Convert.ToInt16(fechaGeneracion.Substring(0, 4)), Convert.ToInt16(fechaGeneracion.Substring(4, 2)), Convert.ToInt16(fechaGeneracion.Substring(6, 2)));

                // Replace the placeholders with the user-specified text
                contents = contents.Replace("[NOMBRE_EMPLEADO]", reader["Nombres_Completos_Empleado"].ToString());
                contents = contents.Replace("[TIPOID_EMPLEADO]", reader["Nombre_TipoID"].ToString());
                contents = contents.Replace("[ID_EMPLEADO]", reader["Id_Empleado"].ToString());
                contents = contents.Replace("[LUGAR_EXPEDICION_DOC]", reader["Lugar_expediccion_IdEmpleado"].ToString());

                if (reader["Marcacion_Descripcion"].ToString() == "1")
                {
                    contents = contents.Replace("[NOMBRE_COMPANIA]", reader["Descripcion_Compania2"].ToString());
                }
                else
                {
                    contents = contents.Replace("[NOMBRE_COMPANIA]", reader["Descripcion_compania"].ToString());
                }

                contents = contents.Replace("[CARGO_EMPLEADO]", reader["Nombre_Cargo_Empleado"].ToString());

                if (reader["Tipo_Contrato"].ToString() == "99999999")
                {
                    contents = contents.Replace("[TIPO_CONTRATO]", "Indefinido");
                }
                else
                {
                    contents = contents.Replace("[TIPO_CONTRATO]", reader["Tipo_Contrato"].ToString());
                }  

                contents = contents.Replace("[OUTSOURCING]", reader["outsourcing"].ToString()); 
                contents = contents.Replace("[FECHA_INI_CONTRATO]", dt.ToString("MMMM dd/yyyy"));
                contents = contents.Replace("[VALOR_CONTRATO_LETRAS]", enLetras(reader["Salario_Empleado"].ToString()) + " PESOS MCTE");
                contents = contents.Replace("[VALOR_CONTRATO_NUM]", String.Format(CultureInfo.CreateSpecificCulture("es-CO"), "{0:0,0.00}", reader["Salario_Empleado"]).ToString());

                contents = contents.Replace("[DIA_ACTUAL]", DateTime.Now.Day.ToString());
                contents = contents.Replace("[MES_ACTUAL]", mesLetras(DateTime.Now.Month));
                contents = contents.Replace("[ANIO_ACTUAL]", DateTime.Now.Year.ToString());
                contents = contents.Replace("[FECHA_ACTUALIZACION]", dt2.ToString("MMMM dd/yyyy"));

                if (reader["Fecha_terminacion_empleado"].ToString() == "" || reader["Fecha_terminacion_empleado"].ToString() == " ")
                {
                    contents = contents.Replace("[ESTADO_VINCULACION]", "está actualmente vinculado(a)");
                    contents = contents.Replace("[FECHA_FIN_CONTRATO]", ",");
                }
                else
                {
                    contents = contents.Replace("[ESTADO_VINCULACION]", "estuvo vinculado(a)");
                    string FechaFinal = reader["fecha_terminacion_empleado"].ToString();
                    dt = new DateTime(Convert.ToInt16(FechaFinal.Substring(0, 4)), Convert.ToInt16(FechaFinal.Substring(4, 2)), Convert.ToInt16(FechaFinal.Substring(6, 2)));
                    contents = contents.Replace("[FECHA_FIN_CONTRATO]", " hasta " + dt.ToString("MMMM dd/yyyy")+",");
                }

                iTextSharp.text.Image firma = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Img/Firma.png"));
                firma.ScaleAbsolute(180, 53);
                firma.SetAbsolutePosition(38, 250);
                document.Add(firma);

                contents = contents.Replace("[NOMBRE_DIRECTOR]", reader["Nombre_Firma"].ToString());
                contents = contents.Replace("[CARGO_DIRECTOR]", reader["Cargo_Firma"].ToString());
                
                // Step 4: Parse the HTML string into a collection of elements...
                var parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(contents), null);

                // Enumerate the elements, adding each one to the Document...
                foreach (var htmlElement in parsedHtmlElements)
                    document.Add(htmlElement as IElement);

                //Agrego el Footer
                //iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Img/footer.png"));
                //footer.ScaleAbsolute(530, 121);
                //footer.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                //document.Add(footer);
            }
            catch (Exception E)
            {
                Paragraph paragraph = new Paragraph();
                paragraph.Add("Ha ocurrido el siguiente error: " + E.Message + " _Metodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                document.Add(paragraph);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                cmd.Dispose();
                Conexion.CerrarCnMysql();
                //Finish Write PDF
                document.Close();
                Response.Write(document);
                Response.End();
            }
        }
        #endregion

        #region Metodo enLetras
        /* ****************************************************************************/
        /* Metodo que recibe una cadena de texto para convertirla en Letras
        /* ****************************************************************************/
        public string enLetras(string num)
        {

            string res, dec = "";

            Int64 entero;

            int decimales;

            double nro;

            try
            {
                nro = Convert.ToDouble(num);
            }
            catch
            {
                return "";
            }

            entero = Convert.ToInt64(Math.Truncate(nro));
            decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));

            if (decimales > 0)
            {
                dec = " CON " + decimales.ToString() + "/100";
            }
            res = toText(Convert.ToDouble(entero)) + dec;

            return res;
        }
        #endregion

        #region Metodo toText
        /* ****************************************************************************/
        /* Metodo que retorna el numero en letras para un numero dado
        /* ****************************************************************************/
        private string toText(double value)
        {

            string Num2Text = "";

            value = Math.Truncate(value);

            if (value == 0) Num2Text = "CERO";

            else if (value == 1) Num2Text = "UNO";

            else if (value == 2) Num2Text = "DOS";

            else if (value == 3) Num2Text = "TRES";

            else if (value == 4) Num2Text = "CUATRO";

            else if (value == 5) Num2Text = "CINCO";

            else if (value == 6) Num2Text = "SEIS";

            else if (value == 7) Num2Text = "SIETE";

            else if (value == 8) Num2Text = "OCHO";

            else if (value == 9) Num2Text = "NUEVE";

            else if (value == 10) Num2Text = "DIEZ";

            else if (value == 11) Num2Text = "ONCE";

            else if (value == 12) Num2Text = "DOCE";

            else if (value == 13) Num2Text = "TRECE";

            else if (value == 14) Num2Text = "CATORCE";

            else if (value == 15) Num2Text = "QUINCE";

            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);

            else if (value == 20) Num2Text = "VEINTE";

            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);

            else if (value == 30) Num2Text = "TREINTA";

            else if (value == 40) Num2Text = "CUARENTA";

            else if (value == 50) Num2Text = "CINCUENTA";

            else if (value == 60) Num2Text = "SESENTA";

            else if (value == 70) Num2Text = "SETENTA";

            else if (value == 80) Num2Text = "OCHENTA";

            else if (value == 90) Num2Text = "NOVENTA";

            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);

            else if (value == 100) Num2Text = "CIEN";

            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);

            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";

            else if (value == 500) Num2Text = "QUINIENTOS";

            else if (value == 700) Num2Text = "SETECIENTOS";

            else if (value == 900) Num2Text = "NOVECIENTOS";

            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);

            else if (value == 1000) Num2Text = "MIL";

            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);

            else if (value < 1000000)
            {

                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";

                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);

            }

            else if (value == 1000000) Num2Text = "UN MILLON";

            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);

            else if (value < 1000000000000)
            {

                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";

                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);

            }
            else if (value == 1000000000000) Num2Text = "UN BILLON";

            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {
                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";

                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            return Num2Text;
        }
        #endregion

        #endregion
    }
}