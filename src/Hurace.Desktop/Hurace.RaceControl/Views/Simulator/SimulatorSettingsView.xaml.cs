using System.Windows;
using Hurace.RaceControl.ViewModels.Simulator;
using Unity;

namespace Hurace.RaceControl.Views.Simulator
{
    /// <summary>
    /// Interaction logic for SimulatorSettingsView.xaml
    /// </summary>
    public partial class SimulatorSettingsView : Window
    {
        public SimulatorSettingsView()
        {
            InitializeComponent();
            DataContext = App.Container.Resolve<SimulatorSettingsEditViewModel>();
            (DataContext as SimulatorSettingsEditViewModel).Dispatcher = Dispatcher;
        }
    }
}
