using System.Windows;
using Hurace.RaceControl.ViewModels;

namespace Hurace.RaceControl
{
    /// <summary>
    /// Interaction logic for MonitorWindow.xaml
    /// </summary>
    public partial class MonitorWindow : Window
    {
        public MonitorWindow(MonitorViewModel monitorViewModel)
        {
            this.InitializeComponent();
            this.DataContext = monitorViewModel;
        }
    }
}
