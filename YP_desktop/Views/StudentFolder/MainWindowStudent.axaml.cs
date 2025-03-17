using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using YP_desktop.Models;
using YP_desktop.ViewModels.StudentFolder;

namespace YP_desktop.Views.StudentFolder;

public partial class MainWindowStudent : UserControl
{
    public MainWindowStudent()
    {
        InitializeComponent();
    }
    public MainWindowStudent(User user)
    {
        InitializeComponent();
        DataContext = new MainWindowStudentViewModel(user);
    }
}