using Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SpectralAnalysis.View
{
    /// <summary>
    /// Логика взаимодействия для PicWindow.xaml
    /// </summary>
    public partial class PicWindow : Window
    {
        public PicWindow()
        {
            InitializeComponent();
            PicViewModel viewModel = new();
            DataContext = viewModel;
        }
        private void PicWindow_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.MainWindow.Show();
        }
    }
}
