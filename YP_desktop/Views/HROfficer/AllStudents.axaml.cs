using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using YP_desktop.ViewModels.HROfficer;

namespace YP_desktop.Views.HROfficer;

public partial class AllStudents : UserControl
{
    public AllStudents()
    {
        InitializeComponent();
        DataContext = new AllStudentsViewModel();
    }
}