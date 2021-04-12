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
            this.InitializeComponent();
            this.DataContext = App.Container.Resolve<SimulatorSettingsEditViewModel>();
            (this.DataContext as SimulatorSettingsEditViewModel).Dispatcher = this.Dispatcher;
        }
    }
}
