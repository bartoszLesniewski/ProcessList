using ProcessList.ViewModel;
using System.Windows;

namespace ProcessList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ProcessViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ProcessViewModel();
            DataContext = _viewModel;
        }
    }
}
