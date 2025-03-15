using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using YP_desktop.ViewModels;

namespace YP_desktop.Views;

public partial class Authorization : UserControl
{
    public Authorization()
    {
        InitializeComponent();
        DataContext = new AuthorizationViewModel();
    }
}