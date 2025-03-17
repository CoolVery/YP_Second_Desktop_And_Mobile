using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP_desktop.Models.Supabase
{
    public class SupabaseContext
    {
        private readonly Client _supabase;
        public SupabaseContext()
        {
            // Получение переменных окружения
            var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
            var key = Environment.GetEnvironmentVariable("SUPABASE_KEY");

            // Настройка опций
            var options = new SupabaseOptions
            {
                AutoConnectRealtime = true,
                AutoRefreshToken = true
            };

            _supabase = new Client(url, key, options);
        }

        public Client Supabase => _supabase;

        public async Task InitializeAsync()
        {
            try
            {
                await _supabase.InitializeAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Supabase initialization failed: {ex.Message}");
                throw;
            }
        }
    }
}
