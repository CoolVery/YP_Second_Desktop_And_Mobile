using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using YP_desktop.ViewModels.HROfficer;

namespace YP_desktop.Views.HROfficer;

public partial class AllFaculty : UserControl
{
    public AllFaculty()
    {
        InitializeComponent();
        DataContext = new AllFacultyViewModel();
        
    }
}