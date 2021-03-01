using System.Windows;
using Hurace.RaceControl.ViewModels;
using Unity;

namespace Hurace.RaceControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.Container.Resolve<MainViewModel>();
        }
    }
}
