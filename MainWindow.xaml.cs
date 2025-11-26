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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppProyectoFinalP2C
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string rutaArchLogin = "c:\\signup\\registroUsuarios.txt";
        private readonly string rutaArchStaff = "c:\\signup\\registroStaff.txt";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnInicioIngresar_Click(object sender, RoutedEventArgs e)
        {
            if (txtInicioUsuario.Text == "" || PwbInicioContraseña.Password == "")
            {
                mostrarAdvertencia();
                lblInicioAdvertencia.Content = "!Debe llenar todos los campos!";
                return;
            }
            else if (!File.Exists(rutaArchLogin))
            {
                mostrarAdvertencia();
                lblInicioAdvertencia.Content = "No existen ningún usuario registrado.";
                return;
            }
            //buscar coincidencias
            bool accesoParticipante = false;
            //leer el archivo
            string[] lineas = File.ReadAllLines(rutaArchLogin);
            foreach (string linea in lineas)
            {
                // Separamos la línea por comas
                // Estructura: NombreApe(0), Matricula(1), Fecha(2), Curso(3), Celular(4), Codigo(5), Usuario(6), Contraseña(7)
                string[] datosUsuario = linea.Split(',');

                // Usamos Trim() para limpiar espacios invisibles que puedan quedar
                string usuarioGuardado = datosUsuario[6].Trim();
                string contraGuardado = datosUsuario[7].Trim();
                // COMPARACIÓN
                if (usuarioGuardado == txtInicioUsuario.Text && contraGuardado == PwbInicioContraseña.Password)
                {
                    accesoParticipante = true;
                    break; // Rompemos el ciclo porque ya lo encontramos
                }
            }
            bool accesoStaff = false;
            string[] lineas2 = File.ReadAllLines(rutaArchStaff);
            foreach (string linea in lineas2)
            {
                string[] datosStaff = linea.Split(',');

                string staffGuardado = datosStaff[4].Trim();
                string contraStaffGuardada = datosStaff[5].Trim();
                if (staffGuardado == txtInicioUsuario.Text && contraStaffGuardada == PwbInicioContraseña.Password)
                {
                    accesoStaff = true;
                    break; // Rompemos el ciclo porque ya lo encontramos
                }
                else
                {
                    // Login Fallido
                    lblInicioAdvertencia.Visibility = Visibility.Visible;
                    lblInicioAdvertencia.Foreground = Brushes.Red;
                    lblInicioAdvertencia.Content = "Usuario o contraseña incorrectos";
                }
            }
            if (accesoParticipante == true)
            {
                WpfBienvenido ventanaBienvenidoParticipante = new WpfBienvenido();
                ventanaBienvenidoParticipante.Show();
                this.Close();
            }
            else if (accesoStaff == true)
            {
                WpfBienvenidoStaff ventanaBienvenidoStaff = new WpfBienvenidoStaff();
                ventanaBienvenidoStaff.Show();
                this.Close();
            }
        }
        private void btnInicioRegristrarMiembro_Click(object sender, RoutedEventArgs e)
        {
            RegistrarParticipante ventanaRegistrarP = new RegistrarParticipante();
            this.Close();
            ventanaRegistrarP.Show();
        }

        private void btnRegistrarStaff_Click(object sender, RoutedEventArgs e)
        {
            RegistrarStaff registrarStaff = new RegistrarStaff();
            this.Close();
            registrarStaff.Show();
        }
        private void mostrarAdvertencia()
        {
            lblInicioAdvertencia.Visibility = Visibility.Visible;
            lblInicioAdvertencia.Foreground = Brushes.Red;
        }
    }
}
