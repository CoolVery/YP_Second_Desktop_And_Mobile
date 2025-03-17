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
using YP_desktop.Models.StudentsFacilties;
using YP_desktop.Views.HROfficer;

namespace YP_desktop.ViewModels.HROfficer
{
    public class CreateNewStudentViewModel : ReactiveObject
    {
        List<Group> _allGroups = new List<Group>();
        Group _selectedGroup;
        ObservableCollection<FacultyInsert> _allFaculty = new ObservableCollection<FacultyInsert>();
        FacultyInsert _SelectedFaculty = new FacultyInsert();
        ObservableCollection<StudentFacultyJoinTable> _allFacultyNewUser = new ObservableCollection<StudentFacultyJoinTable>();
        FacultyInsert _SelectedFacultyNewUser = new FacultyInsert();
        StudentInsert _newStudent = new StudentInsert();
        User _newUser = new User();
        bool _isCorrectCreateNewStudent = false;

        public List<Group> AllGroups
        {
            get => _allGroups;
            set => this.RaiseAndSetIfChanged(ref _allGroups, value);
        }
        public Group SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                NewStudent.GroupId = _selectedGroup.Id;
            }
        }
        public StudentInsert NewStudent
        {
            get => _newStudent;
            set => this.RaiseAndSetIfChanged(ref _newStudent, value);
        }
        public bool IsCorrectCreateNewStudent
        {
            get => _isCorrectCreateNewStudent;
            set => this.RaiseAndSetIfChanged(ref _isCorrectCreateNewStudent, value);
        }
        public ObservableCollection<FacultyInsert> AllFaculty
        {
            get => _allFaculty;
            set => this.RaiseAndSetIfChanged(ref _allFaculty, value);
        }
        public FacultyInsert SelectedFaculty
        {
            get => _SelectedFaculty;
            set => _SelectedFaculty = value;
        }
        public ObservableCollection<FacultyInsert> AllFacultyNewUser
        {
            get => _allFacultyNewUser;
            set => this.RaiseAndSetIfChanged(ref _allFacultyNewUser, value);
        }
        public FacultyInsert SelectedFacultyNewUser
        {
            get => _SelectedFacultyNewUser;
            set => _SelectedFacultyNewUser = value;
        }
        public User NewUser 
        { 
            get => _newUser; 
            set => this.RaiseAndSetIfChanged(ref _newUser, value); 
        }

        public CreateNewStudentViewModel()
        {
            InitializeAsync(1).ConfigureAwait(false);
        }
        public void CreateNewStudentWithFaculties()
        {
            InitializeAsync(2).ConfigureAwait(false);
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
                        var isCorrectInsert = await InsertNewStudentWithFaculties().ConfigureAwait(false);
                        switch (isCorrectInsert)
                        {
                            case true:
                                IsCorrectCreateNewStudent = true;
                                await Task.Delay(3000);
                                await Dispatcher.UIThread.InvokeAsync(() =>
                                {
                                    ToAllStudents();
                                });
                                break;
                        }
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
        private async Task<bool> InsertNewStudentWithFaculties()
        {
            try
            {
                NewUser.RoleId = 2;
                var responseUser = await MainWindowViewModel.LinkMWViewModel.SupabaseService.Supabase.From<User>().Insert(NewUser);
                var newUser = await MainWindowViewModel.LinkMWViewModel.SupabaseService.Supabase.From<User>()
                        .Select("id")
                        .Where(x => x.Login == NewUser.Login && x.Password == NewUser.Password)
                        .Get();
                NewStudent.UserId = newUser.Model.Id;
                var responseStudent = await MainWindowViewModel.LinkMWViewModel.SupabaseService.Supabase.From<StudentInsert>().Insert(NewStudent);
                if (responseStudent.ResponseMessage.IsSuccessStatusCode)
                {
                    var newStudent = await MainWindowViewModel.LinkMWViewModel.SupabaseService.Supabase.From<StudentInsert>()
                        .Select("id")
                        .Where(x => x.FullName == NewStudent.FullName && x.Address == NewStudent.Address)
                        .Get();
                    List<StudentFaculty> studentFaculties = new List<StudentFaculty>();
                    foreach (var item in AllFacultyNewUser)
                    {
                        StudentFaculty studentFaculty = new StudentFaculty();
                        studentFaculty.StudentId = newStudent.Model.Id;
                        studentFaculty.FacultyId = item.Id;
                        studentFaculty.Evaluation = item.;
                        studentFaculties.Add(studentFaculty);
                    }
                    var responseStudentFaculty = await MainWindowViewModel.LinkMWViewModel.SupabaseService.Supabase.From<StudentFaculty>().Insert(studentFaculties);
                    return responseStudentFaculty.ResponseMessage.IsSuccessStatusCode;
                }

                return false;
            }
            catch (Exception e)
            {
                return false;

            }
        }

        public void ToAllStudents()
        {
            MainWindowViewModel.LinkMWViewModel.Uc = new AllStudents();
        }
    }
}