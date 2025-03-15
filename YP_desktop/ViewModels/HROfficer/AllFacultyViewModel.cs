using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using YP_desktop.Models;

namespace YP_desktop.ViewModels.HROfficer
{
	public class AllFacultyViewModel : ReactiveObject
	{
		List<Faculty> _allFaculties = new List<Faculty>();
		public AllFacultyViewModel()
		{
            InitializeAsync().ConfigureAwait(false);

        }

        public List<Faculty> AllFaculties { get => _allFaculties; set => this.RaiseAndSetIfChanged(ref _allFaculties, value); }
        private async Task InitializeAsync()
        {
            try
            {
                var faculties = await GetAllFaculty().ConfigureAwait(false);
                AllFaculties = faculties;
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<Faculty>?> GetAllFaculty()
        {
            try {
                var response = await MainWindowViewModel.LinkMWViewModel.SupabaseService.Supabase.From<Faculty>()

                        .Select("id, " +
                        "name, " +
                        "number_of_hours, " +
                        "id_type_subject:types_of_subjects(id, name), " +
                        "id_dean_faculty:deans(id, id_login_user, full_name, address, phone)")
                        .Get();
                return response.Models?.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}