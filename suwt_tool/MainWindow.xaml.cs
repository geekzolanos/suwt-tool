using System.Windows;

namespace suwt
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowVm vm;

        public MainWindow()
        {            
            InitializeComponent();
            vm = new MainWindowVm();
            DataContext = vm;
        }

        private void Enable_Click(object sender, RoutedEventArgs e)
        {            
            vm.StartService();
        }

        private void Disable_Click(object sender, RoutedEventArgs e)
        {
            vm.StopService();
        }
    }
}
