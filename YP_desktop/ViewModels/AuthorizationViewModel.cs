using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using ReactiveUI;
using YP_desktop.Models.Students;
using YP_desktop.Models.Supabase;
using YP_desktop.Views.HROfficer;
using YP_desktop.Views.StudentFolder;
using YP_desktop.Views.Teacher;

namespace YP_desktop.ViewModels
{
	public class AuthorizationViewModel : ReactiveObject
	{
		string _login = default!;
		string _password = default!;
        char _passwordChar = '*';
        public string Login { get => _login; set => this.RaiseAndSetIfChanged(ref _login, value); }
        public string Password { get => _password; set => this.RaiseAndSetIfChanged(ref _password, value); }
        public char PasswordChar { get => _passwordChar; set => this.RaiseAndSetIfChanged(ref _passwordChar, value); }

        public async void SignIn()
		{
            AuthService authService = new AuthService();
            var userSupabase = await authService.SignUpAsync(Login, Password);
            if (userSupabase != null)
            {
                switch (userSupabase.RoleId)
                {
                    case 1:
                        break;
                    case 2:
                        MainWindowViewModel.LinkMWViewModel.Uc = new MainWindowStudent(userSupabase);
                        break;
                    case 3:
                        MainWindowViewModel.LinkMWViewModel.Uc = new MainWindowTeacher();
                        break;
                    case 4:
                        MainWindowViewModel.LinkMWViewModel.Uc = new AllFaculty();
                        break;
                }
            }
        }
        public void WatchPassword()
        {
            switch (PasswordChar)
            {
                case '*':
                    PasswordChar = '\0';
                    break;
                case '\0':
                    PasswordChar = '*';
                    break;
            }
        }
	}
}