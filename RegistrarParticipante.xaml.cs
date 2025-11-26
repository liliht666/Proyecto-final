using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace WpfAppProyectoFinalP2C
{
    /// <summary>
    /// Lógica de interacción para RegistrarParticipante.xaml
    /// </summary>
    public partial class RegistrarParticipante : Window
    {
        private readonly string rutaArchLogin = "c:\\signup\\registroUsuarios.txt";
        private readonly string rutaArchEquipos = "c:\\signup\\registroEquipos.txt";
        string datos;
        string codigo;
        string correo;
        string contraseña;
        //Expresión regular para solo letras
        string patronNombre = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$";
        //Expresión regular solo números
        string patronNumeros = @"^\d+$";
        //Expresión regular para números (solo Bolivia)
        string patronCelular = @"^\d{8}$";
        //Expresión regular formato fecha
        string patronFecha = @"^\d{2}\/\d{2}\/\d{4}$";
        //Expresion regular para el curso (Acepta: Letras, números, espacios, guiones, puntos y el símbolo º)
        string patronCurso = @"^[a-zA-Z0-9\s\-\.º]+$";
        //Expresion regular para codigo (Acepta letras y números sin espacios)
        string patronCodigo = @"^[a-zA-Z0-9]{4}-[a-zA-Z0-9]{4}$";
        // Regex para Nombre de Equipo e Institución
        string patronNombreEquipo = @"^[a-zA-Z0-9\s\.\-]+$";
        //Regex para Tipo de Institución
        string patronTipoIns = @"^(?i)(colegio|universidad)$";
        public RegistrarParticipante()
        {
            InitializeComponent();
        }

        private void btnIrARegistrarEquipo_Click(object sender, RoutedEventArgs e)
        {
            if (GridRegistroEquipo.Width == 0)
            {
                // ABRIENDO EL PANEL:

                // Paso A: Agrandamos la ventana completa (500 del original + 500 del nuevo)
                this.Width = 1000;

                // Paso B: Le damos tamaño al panel derecho para que se vea
                GridRegistroEquipo.Width = 500;

                //Ocultamos el boton y el label de registrar equipo
                btnIrARegistrarEquipo.Visibility = Visibility.Hidden;
                lblMensajeRegistrarEquipo.Visibility = Visibility.Hidden;

                //ocultamos el boton "REGISTRARSE" de participante ya que esta funcionalidad esta en el boton CREAR EQUIPO Y REGISTRARME
                btnRegistrarParticipante.Visibility = Visibility.Hidden;

                //Movemos el logo del ICPC hacia la derecha
                imgLogoIPCP.RenderTransform = new TranslateTransform(250, 0);

                //Se bloquea el txtRPCodigo ya que se generará solo
                txtRPCodigo.IsEnabled = false;
                txtRPCodigo.Text = "SE GENERARÁ AUTOMÁTICAMENTE";
            }
            else
            {
            }
        }

        private void btnVolverRegistroPersonal_Click(object sender, RoutedEventArgs e)
        {
            // Paso A: Regresamos la ventana a su tamaño normal
            this.Width = 500;
            // Paso B: Ocultamos el panel derecho
            GridRegistroEquipo.Width = 0;
            //Mostramos el boton y el label de registrar equipo
            btnIrARegistrarEquipo.Visibility = Visibility.Visible;
            lblMensajeRegistrarEquipo.Visibility = Visibility.Visible;
            btnRegistrarParticipante.Visibility = Visibility.Visible;
            //movemos el logo del ICPC hacia la izquierda
            //Movemos el logo del ICPC hacia la derecha
            imgLogoIPCP.RenderTransform = new TranslateTransform(0, 0);
            //Se desbloquea el txtRPCodigo
            txtRPCodigo.IsEnabled = true;
            txtRPCodigo.Text = "";
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            MainWindow ventanaPrincipal = new MainWindow();
            this.Close();
            ventanaPrincipal.Show();
        }

        private void btnRegistrarParticipante_Click(object sender, RoutedEventArgs e)
        {
            if (validarDatosPersonales() == true)
            {
                if (!Regex.IsMatch(txtRPCodigo.Text, patronCodigo))
                {
                    mostrarAdvertencia();
                    lblRPAdvertencia.Content = "El código de equipo debe tener el formato: XXXX-XXXX \n Si no tiene equipo registré uno";
                    return;
                }
                else if (!File.Exists(rutaArchEquipos))
                {
                    mostrarAdvertencia();
                    lblRPAdvertencia.Content = "No existe ningún equipo registrado";
                    return;
                }

                string[] lineasEquipos = File.ReadAllLines(rutaArchEquipos);
                bool verificarCodigo = false;
                foreach (string linea in lineasEquipos)
                {
                    string[] partes = linea.Split(',');

                    if (partes[3].Trim().Equals(txtRPCodigo.Text.Trim())) //hacer con equals
                    {
                        verificarCodigo = true;

                        break;
                    }
                    else
                    {

                    }
                }
                if (verificarCodigo == true)
                {
                    codigo = txtRPCodigo.Text.Trim();
                    guardarDatosPersonales();
                    MessageBox.Show("Su usuario es: " + correo + "\n" +
                                    "Su contraseña es: " + contraseña);
                    borrarDatos();
                }
                else
                {
                    mostrarAdvertencia();
                    lblRPAdvertencia.Content = "El código de equipo no existe.";
                }
            }
        }
        private void btnRegistrarEquipo_Click(object sender, RoutedEventArgs e)
        {
            //Validar equipo
            if (txtNombreEquipo.Text == "" || txtInstitución.Text == "" || txtTipoIns.Text == "")
            {
                mostrarAdvertenciaEquipo();
                lblEquipoAdvertencia.Content = "Todos los campos del equipo son obligatorios";
                return;
            }

            if (!Regex.IsMatch(txtNombreEquipo.Text, patronNombreEquipo))
            {
                mostrarAdvertenciaEquipo();
                lblEquipoAdvertencia.Content = "Nombre de equipo inválido (solo letras/números)";
                return;
            }

            if (!Regex.IsMatch(txtInstitución.Text, patronNombreEquipo))
            {
                mostrarAdvertenciaEquipo();
                lblEquipoAdvertencia.Content = "Nombre de institución inválido (solo letras/números)";
                return;
            }
            if (!Regex.IsMatch(txtTipoIns.Text.Trim(), patronTipoIns))
            {
                mostrarAdvertenciaEquipo();
                lblEquipoAdvertencia.Content = "El tipo solo puede ser: Colegio o Universidad";
                return;
            }
            if (validarDatosPersonales() == false)
            {
                return;
            }
            else
            {
                codigo = generarCodigoEquipo();
                guardarDatosPersonales();

                //Guardar datos del equipo
                guardarDatosEquipo();
                MessageBox.Show("Su usuario es: " + correo + "\n" +
                                "Su contraseña es: " + contraseña + "\n" +
                                "El código de su equipo es: " + codigo);
                borrarDatos();
            }
        }


        private void borrarDatos()
        {
            txtRPNombre.Text = "";
            txtRPApellido.Text = "";
            txtRPMatricula.Text = "";
            txtRPFechaNac.Text = "";
            txtRPCurso.Text = "";
            txtRPCelular.Text = "";
            txtRPCodigo.Text = "";
            txtInstitución.Text = "";
            txtNombreEquipo.Text = "";
            txtTipoIns.Text = "";
        }
        private void mostrarAdvertencia()
        {
            lblRPAdvertencia.Visibility = Visibility.Visible;
            lblRPAdvertencia.Foreground = Brushes.Red;
        }
        private void mostrarAdvertenciaEquipo()
        {
            lblEquipoAdvertencia.Visibility = Visibility.Visible;
            lblEquipoAdvertencia.Foreground = Brushes.Red;
        }
        private string crearCorreo()
        {
            //Obtener textos limpios
            string letraNom = txtRPNombre.Text.Trim().PadRight(2, 'x').Substring(0, 2);
            string letraApe = txtRPApellido.Text.Trim().PadRight(2, 'x').Substring(0, 2);
            string numCel = txtRPCelular.Text.Trim().PadRight(2, '0').Substring(0, 2);
            string numMat = txtRPMatricula.Text.Trim().PadRight(2, '0').Substring(0, 2);

            //Generacion de correo
            return $"{letraNom}{letraApe}{numCel}{numMat}@icpc.com".ToLower();
        }
        private string crearContrasena()
        {
            //Caracteres posibles de la contraseña
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            //Generar contraseña
            Random random = new Random();
            char[] password = new char[6]; // Contraseña de 6 dígitos

            //Llenamos los 6 espacios eligiendo al azar
            for (int i = 0; i < 6; i++)
            {
                password[i] = caracteres[random.Next(caracteres.Length)];
            }

            //Devolver contraseña como texto
            return new string(password);
        }

        private string generarCodigoEquipo()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();

            // Generamos la primera parte (4 caracteres)
            char[] parte1 = new char[4];
            for (int i = 0; i < 4; i++)
            {
                parte1[i] = chars[random.Next(chars.Length)];
            }

            // Generamos la segunda parte (4 caracteres)
            char[] parte2 = new char[4];
            for (int i = 0; i < 4; i++)
            {
                parte2[i] = chars[random.Next(chars.Length)];
            }

            // Unimos las partes con el guion
            return new string(parte1) + "-" + new string(parte2);
        }

        private bool validarDatosPersonales()
        {
            if (txtRPNombre.Text == "" || txtRPApellido.Text == "" || txtRPMatricula.Text == "" || txtRPFechaNac.Text == "" || txtRPCurso.Text == "" ||
                txtRPCelular.Text == "" || txtRPCodigo.Text == "")
            {
                mostrarAdvertencia();
                lblRPAdvertencia.Content = "Todos los espacios personales deben completarse";
                return false;
            }
            else
            {
                if (!Regex.IsMatch(txtRPNombre.Text, patronNombre) || !Regex.IsMatch(txtRPApellido.Text, patronNombre))
                {
                    mostrarAdvertencia();
                    lblRPAdvertencia.Content = "Los nombres solo pueden contener letras";
                    return false;
                }
                else if (!Regex.IsMatch(txtRPMatricula.Text, patronNumeros))
                {
                    mostrarAdvertencia();
                    lblRPAdvertencia.Content = "La matrícula solo debe tener números";
                    return false;
                }
                else if (!Regex.IsMatch(txtRPFechaNac.Text, patronFecha))
                {
                    mostrarAdvertencia();
                    lblRPAdvertencia.Content = "La fecha debe ser dd/mm/aaaa (Ej: 25/05/2000)";
                    return false;
                }
                else if (!Regex.IsMatch(txtRPCurso.Text, patronCurso))
                {
                    mostrarAdvertencia();
                    lblRPAdvertencia.Content = "El curso contiene caracteres no válidos";
                    return false;
                }

                else if (!Regex.IsMatch(txtRPCelular.Text, patronCelular))
                {
                    mostrarAdvertencia();
                    lblRPAdvertencia.Content = "El celular solo puede contener 8 números";
                    return false;
                }
            }
            return true;
        }
        private void guardarDatosPersonales()
        {
            correo = crearCorreo();
            contraseña = crearContrasena();
            datos = txtRPNombre.Text.ToString() + txtRPApellido.Text.ToString() + "," + txtRPMatricula.Text.ToString() + ","
                     + txtRPFechaNac.Text.ToString() + "," + txtRPCurso.Text.ToString() + "," + txtRPCelular.Text.ToString() + ","
                     + codigo + "," + correo + "," + contraseña + "\n";
            File.AppendAllText(rutaArchLogin, datos, Encoding.UTF8);

            lblRPAdvertencia.Visibility = Visibility.Visible;
            lblRPAdvertencia.Foreground = Brushes.Yellow;
            lblRPAdvertencia.Content = "Registro exitoso";
        }
        private void guardarDatosEquipo()
        {
            string datosEquipo = txtNombreEquipo.Text.ToString() + "," + txtInstitución.Text.ToString() + "," +
                                 txtTipoIns.Text.ToString() + "," + codigo + "\n";
            File.AppendAllText(rutaArchEquipos, datosEquipo, Encoding.UTF8);
        }
    }
}
