using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfAppProyectoFinalP2C
{
    /// <summary>
    /// Lógica de interacción para RegistrarStaff.xaml
    /// </summary>
    public partial class RegistrarStaff : Window
    {
        private readonly string rutaArchStaff = "c:\\signup\\registroStaff.txt";

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
      

        string correo;
        string contraseña;
        string datos;
        public RegistrarStaff()
        {
            InitializeComponent();
        }

        private void btnVolverLogIn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow ventanaPrincipal = new MainWindow();
            this.Close();
            ventanaPrincipal.Show();
        }

        private void btnRegistrarStaff_Click(object sender, RoutedEventArgs e)
        {
            if (txtRStaffNombre.Text == "" || txtRStaffApellido.Text == "" || txtRStaffCelular.Text == "" || txtRStaffFechaNac.Text == "" || txtRStaffCiudad.Text == "")
            {
                mostrarAdvertencia();
                lblRPAdvertencia.Content = "Todos los espacios personales deben completarse";
            }
            else
            {
                if (!Regex.IsMatch(txtRStaffNombre.Text, patronNombre) || !Regex.IsMatch(txtRStaffApellido.Text, patronNombre))
                {
                    mostrarAdvertencia();
                    lblRPAdvertencia.Content = "Nombre y Apellido solo pueden contener letras";
                    return;
                }
                else if (!Regex.IsMatch(txtRStaffCelular.Text, patronCelular))
                {
                    mostrarAdvertencia();
                    lblRPAdvertencia.Content = "El celular solo puede contener 8 números";
                    return;
                }

                else if (!Regex.IsMatch(txtRStaffFechaNac.Text, patronFecha))
                {
                    mostrarAdvertencia();
                    lblRPAdvertencia.Content = "La fecha de nacimiento debe tener el formato dd/mm/aaaa";
                    return;
                }
                else if (!Regex.IsMatch(txtRStaffCiudad.Text, patronNombre))
                {
                    mostrarAdvertencia();
                    lblRPAdvertencia.Content = "Ciudad solo puede contener letras";
                    return;
                }
                else
                {
                    guardarDatosStaff();   
                }
            }
        }
        private void mostrarAdvertencia()
        {
            lblRPAdvertencia.Visibility = Visibility.Visible;
            lblRPAdvertencia.Foreground = Brushes.Red;
        }
        private void guardarDatosStaff()
        {
            correo = crearCorreo();
            contraseña = crearContrasena();
            datos = txtRStaffNombre.Text.ToString() + txtRStaffApellido.Text.ToString() + "," + txtRStaffCelular.Text.ToString() + ","
                     + txtRStaffFechaNac.Text.ToString() + "," + txtRStaffCiudad.Text.ToString() + "," +
                     correo + "," + contraseña + "\n";
            File.AppendAllText(rutaArchStaff, datos, Encoding.UTF8);

            MessageBox.Show("Su usuario es: " + correo + "\n" +
                            "Su contraseña es: " + contraseña);
            borrarDatos();
            lblRPAdvertencia.Visibility = Visibility.Visible;
            lblRPAdvertencia.Foreground = Brushes.Yellow;
            lblRPAdvertencia.Content = "Registro exitoso";
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

        private string crearCorreo()
        {
            //Obtener textos limpios
            string letraNom = txtRStaffNombre.Text.Trim().PadRight(2, 'x').Substring(0, 2);
            string letraApe = txtRStaffApellido.Text.Trim().PadRight(2, 'x').Substring(0, 2);
            string numCel = txtRStaffCelular.Text.Trim().PadRight(2, '0').Substring(0, 2);
            string numMat = txtRStaffCiudad.Text.Trim().PadRight(2, '0').Substring(0, 2);
            //Generacion de correo
            return $"staff{letraNom}{letraApe}{numCel}{numMat}@icpc.com".ToLower();

        }
        private void borrarDatos()
        {
            txtRStaffNombre.Text = "";
            txtRStaffApellido.Text = "";
            txtRStaffCelular.Text = "";
            txtRStaffFechaNac.Text = "";
            txtRStaffCiudad.Text = "";
        }
    }
}
