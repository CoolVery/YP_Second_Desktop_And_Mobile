using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReactiveUI;
using YP_desktop.Models.Supabase;
using YP_desktop.Views.HROfficer;

namespace YP_desktop.ViewModels
{
	public class AuthorizationViewModel : ReactiveObject
	{
		string _login = default!;
		string _password = default!;

        public string Login { get => _login; set => this.RaiseAndSetIfChanged( ref _login, value); }
        public string Password { get => _password; set => this.RaiseAndSetIfChanged( ref _password, value); }
		public async void SignIn()
		{
            AuthService authService = new AuthService();
            var userSupabase = await authService.SignUpAsync(Login, Password);
            if (userSupabase != null)
            {
                MainWindowViewModel.LinkMWViewModel.Uc = new AllFaculty();
            }




        }
	}
}