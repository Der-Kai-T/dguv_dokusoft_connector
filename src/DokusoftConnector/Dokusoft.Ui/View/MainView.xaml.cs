using Dokusoft.Ui.ViewModel;

namespace Dokusoft.Ui.View;

public partial class MainView
{
    public MainView()
    {
        InitializeComponent();
        DataContext = new MainViewViewModel();
    }
}