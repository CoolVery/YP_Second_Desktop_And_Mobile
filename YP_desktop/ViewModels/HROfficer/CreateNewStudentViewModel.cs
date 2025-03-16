using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using ReactiveUI;
using YP_desktop.Models;
using YP_desktop.Models.Faculties;
using YP_desktop.Models.Students;

namespace YP_desktop.ViewModels.HROfficer
{
    public class CreateNewStudentViewModel : ReactiveObject
	{
        List<Group> _allGroups = new List<Group>();
        Group _selectedGroup;
        ObservableCollection<FacultyInsert> _allFaculty = new ObservableCollection<FacultyInsert>();
        FacultyInsert _SelectedFaculty = new FacultyInsert();
        ObservableCollection<FacultyInsert> _allFacultyNewUser = new ObservableCollection<FacultyInsert>();
        FacultyInsert _SelectedFacultyNewUser = new FacultyInsert();
        StudentInsert _newStudent = new StudentInsert();
        bool _isCorrectCreateNewStudent = false;

        public List<Group> AllGroups { 
            get => _allGroups; 
            set => this.RaiseAndSetIfChanged(ref _allGroups, value); 
        }
        public Group SelectedGroup { 
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                NewStudent.GroupId = _selectedGroup.Id;
            } 
        }
        public StudentInsert NewStudent { 
            get => _newStudent; 
            set => this.RaiseAndSetIfChanged(ref _newStudent, value); 
        }
        public bool IsCorrectCreateNewStudent { 
            get => _isCorrectCreateNewStudent; 
            set => this.RaiseAndSetIfChanged(ref _isCorrectCreateNewStudent, value); 
        }
        public ObservableCollection<FacultyInsert> AllFaculty { 
            get => _allFaculty; 
            set => this.RaiseAndSetIfChanged(ref _allFaculty, value); 
        }
        public FacultyInsert SelectedFaculty { 
            get => _SelectedFaculty; 
            set =>  _SelectedFaculty = value; 
        }
        public ObservableCollection<FacultyInsert> AllFacultyNewUser { 
            get => _allFacultyNewUser; 
            set => this.RaiseAndSetIfChanged(ref _allFacultyNewUser, value); 
        }
        public FacultyInsert SelectedFacultyNewUser { 
            get => _SelectedFacultyNewUser; 
            set => _SelectedFacultyNewUser = value; 
        }

        public CreateNewStudentViewModel()
        {
            InitializeAsync(1).ConfigureAwait(false);
        }
        private async Task InitializeAsync(int paramResponsw)
        {
            try
            {
                switch (paramResponsw)
                {
                    case 1:
                        var groups = await GetAllGroups().ConfigureAwait(false);
                        var faculty = await GetAllFaculty().ConfigureAwait(false);
                        AllGroups = groups;
                        AllFaculty = faculty;
                        break;
                    case 2:
                        //var isCorrectInsert = await InsertNewFaculty().ConfigureAwait(false);
                        //switch (isCorrectInsert)
                        //{
                        //    case true:
                        //        IsCorrectCreateNewFaculty = true;
                        //        await Task.Delay(3000);
                        //        await Dispatcher.UIThread.InvokeAsync(() =>
                        //        {
                        //            ToAllFaculties();
                        //        });
                        //        break;
                        //}
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void AddFacultyInNewStudent()
        {
            if (SelectedFaculty != null)
            {
                AllFacultyNewUser.Add(SelectedFaculty);
                AllFaculty.Remove(SelectedFaculty);
            }
        }
        public void RemoveFacultyInNewStudent()
        {
            if (SelectedFacultyNewUser != null)
            {
                AllFaculty.Add(SelectedFacultyNewUser);
                AllFacultyNewUser.Remove(SelectedFacultyNewUser);
            }
        }
        private async Task<List<Group>?> GetAllGroups()
        {
            var response = await MainWindowViewModel.LinkMWViewModel.SupabaseService.Supabase.From<Group>().Get();
            return response.Models.ToList();
        }
        private async Task<ObservableCollection<FacultyInsert>?> GetAllFaculty()
        {
            var response = await MainWindowViewModel.LinkMWViewModel.SupabaseService.Supabase.From<FacultyInsert>().Get();
            return new ObservableCollection<FacultyInsert>(response.Models);
        }
    }
}