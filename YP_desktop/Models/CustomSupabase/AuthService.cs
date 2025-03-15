using Supabase;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YP_desktop.ViewModels;

namespace YP_desktop.Models.Supabase
{
    public class AuthService
    {
        // Регистрация нового пользователя
        public async Task<User?> SignUpAsync(string email, string password)
        {
            try
            {
                var response = await MainWindowViewModel.LinkMWViewModel.SupabaseService.Supabase.From<User>()
                    .Select(x => new object[] { x.Login, x.Password, x.Id, x.RoleId})
                    .Where(x => x.Login == email && x.Password == password)
                    .Get();
                return response.Model;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка регистрации: {ex.Message}");
                return null;
            }
        }


    }
}
