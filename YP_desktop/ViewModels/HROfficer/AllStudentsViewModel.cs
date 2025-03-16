using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using YP_desktop.Models.Faculties;
using YP_desktop.Views.HROfficer;

namespace YP_desktop.ViewModels.HROfficer
{
    public class AllStudentsViewModel : ReactiveObject
	{
        List<Models.Students.Student> _allStudents = new List<Models.Students.Student>();
        List<Models.Students.Student> _currentStudents = new List<Models.Students.Student>();

        string _searchByName = "";
        public AllStudentsViewModel()
        {
            InitializeAsync().ConfigureAwait(false);
        }
        public List<Models.Students.Student> CurrentStudents
        {
            get => _currentStudents;
            set => this.RaiseAndSetIfChanged(ref _currentStudents, value);
        }
        public string SearchByName
        {
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
                CurrentStudents = faculties;
                _allStudents = new List<Models.Students.Student>(faculties);
            }
            catch (Exception ex)
            {

            }
        }
        private async Task<List<Models.Students.Student>?> GetAllFaculty()
        {
            try
            {
                var response = await MainWindowViewModel.LinkMWViewModel.SupabaseService.Supabase.From<Models.Students.Student>()
                        .Select("id, " +
                        "id_login_user, " +
                        "full_name, " +
                        "address, " +
                        "phone," +
                        "id_group:groups(id, name)")
                        .Get();
                return response.Models?.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public void ToCreateNewStudent()
        {
            MainWindowViewModel.LinkMWViewModel.Uc = new CreateNewStudent();
        }
        public void ToAllTeacher()
        {
            //MainWindowViewModel.LinkMWViewModel.Uc = new AllStudents();
        }
        public void ToAllFaculty()
        {
            MainWindowViewModel.LinkMWViewModel.Uc = new AllFaculty();
        }
        private void AllFilters()
        {
            SearchByFullNameStudent();
        }
        private void SearchByFullNameStudent()
        {
            switch (string.IsNullOrEmpty(SearchByName))
            {
                case false:
                    CurrentStudents = _allStudents.Where(x => x.FullName.Contains(SearchByName)).ToList();
                    break;
                case true:
                    CurrentStudents = _allStudents;
                    break;
            }
        }
    }
}