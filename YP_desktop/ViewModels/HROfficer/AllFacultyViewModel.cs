using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using YP_desktop.Models.Faculties;
using YP_desktop.Views;
using YP_desktop.Views.HROfficer;

namespace YP_desktop.ViewModels.HROfficer
{
    public class AllFacultyViewModel : ReactiveObject
	{
		List<Faculty> _allFaculties = new List<Faculty>();
        List<Faculty> _currentFaculties = new List<Faculty>();

        string _searchByName = "";
		public AllFacultyViewModel()
		{
            InitializeAsync().ConfigureAwait(false);
        }
        public List<Faculty> CurrentFaculties { 
            get => _currentFaculties; 
            set => this.RaiseAndSetIfChanged(ref _currentFaculties, value); }
        public string SearchByName { 
            get => _searchByName;
            set
            {
                this.RaiseAndSetIfChanged(ref _searchByName, value);
                AllFilters();
            }  
        }
        private async Task InitializeAsync()
        {
            try
            {
                var faculties = await GetAllFaculty().ConfigureAwait(false);
                CurrentFaculties = faculties;
                _allFaculties = new List<Faculty>(faculties);
            }
            catch (Exception ex)
            {

            }
        }
        private async Task<List<Faculty>?> GetAllFaculty()
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
        public void ToCreateNewFaculty()
        {
            MainWindowViewModel.LinkMWViewModel.Uc = new CreateNewFaculty();
        }
        public void ToAllStudent()
        {
            MainWindowViewModel.LinkMWViewModel.Uc = new AllStudents();

        }
        public void AllFilters()
        {
            SearchByNameFaculty();
        }

        private void SearchByNameFaculty()
        {
            switch(string.IsNullOrEmpty(SearchByName))
            {
                case false:
                    CurrentFaculties = _allFaculties.Where(x => x.Name.Contains(SearchByName)).ToList();
                    break;
                case true:
                    CurrentFaculties = _allFaculties;
                    break;
            }
        }
        public void ToAuth()
        {
            MainWindowViewModel.LinkMWViewModel.Uc = new Authorization();
        }
    }
}