using System;
using System.Collections.Generic;
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

namespace WpfAppProyectoFinalP2C
{
    /// <summary>
    /// Lógica de interacción para WpfBienvenidoStaff.xaml
    /// </summary>
    public partial class WpfBienvenidoStaff : Window
    {
        public WpfBienvenidoStaff()
        {
            InitializeComponent();
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            MainWindow ventanaPrincipal = new MainWindow();
            this.Close();
            ventanaPrincipal.Show();
        }
    }
}
