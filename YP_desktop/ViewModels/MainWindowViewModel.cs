using Avalonia.Controls;
using ReactiveUI;
using System.ComponentModel;
using System.Threading.Tasks;
using YP_desktop.Models.Supabase;
using YP_desktop.Views;

namespace YP_desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public static  MainWindowViewModel LinkMWViewModel = default!;
        UserControl _uc = new Authorization();
        readonly SupabaseContext _supabaseService;

        public MainWindowViewModel()
        {
            LinkMWViewModel = this;
            _supabaseService = new SupabaseContext();
            InitializeAsync().ConfigureAwait(false); 
        }
        private async Task InitializeAsync()
        {
            await _supabaseService.InitializeAsync();
        }
        public UserControl Uc { get => _uc; set => this.RaiseAndSetIfChanged(ref _uc, value); }

        public SupabaseContext SupabaseService => _supabaseService;
    }
}
