using System;
using System.Windows.Controls;
using Unity;

namespace Hurace.RaceControl.ViewModels.Controls
{
    public class NavigationItemViewModel<TParentViewModel>
    {
        public string Title { get; private set; }
        public UserControl View { get; private set; }
        public TabViewModel<TParentViewModel> ViewModel { get; private set; }

        public NavigationItemViewModel(
            string title, 
            UserControl view, 
            TabViewModel<TParentViewModel> viewModel)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            View = view ?? throw new ArgumentNullException(nameof(view));
            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            View.DataContext = ViewModel;
        }

        // ToString needs to be implemented so the dragablz:TabablzControl is showing the
        // right header because there is currently no way to define a custom header template
        public override string ToString() => Title;

        public static NavigationItemViewModel<TParentViewModel> Of<TView, TViewModel>(
            string title, 
            TParentViewModel parentViewModel,
            object routeData = null)
            where TView : UserControl, new()
            where TViewModel : TabViewModel<TParentViewModel>
        {
            var view = new TView();

            var viewModel = App.Container.Resolve<TViewModel>();
            viewModel.Parent = parentViewModel;
            viewModel.Dispatcher = view.Dispatcher;
            viewModel.RouteData = routeData;

            return new NavigationItemViewModel<TParentViewModel>(title, view, viewModel);
        }                        
    }
}
