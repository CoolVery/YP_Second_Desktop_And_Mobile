using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using ReactiveUI;
using YP_desktop.Models;
using YP_desktop.Models.Faculties;
using YP_desktop.Views.HROfficer;

namespace YP_desktop.ViewModels.HROfficer
{
    public class CreateNewFacultyViewModel : ReactiveObject
    {
        List<Dean> _allDeans = new List<Dean>();
        Dean _selectedDean;
        List<TypeOfSubject> _allTypeOfSubjects = new List<TypeOfSubject>();
        TypeOfSubject _selectedTypeOfSubject;
        FacultyInsert _newFaculty = new FacultyInsert();
        bool _isCorrectCreateNewFaculty = false;
        public List<Dean> AllDeans
        {
            get => _allDeans;
            set => this.RaiseAndSetIfChanged(ref _allDeans, value);
        }
        public List<TypeOfSubject> AllTypeOfSubjects
        {
            get => _allTypeOfSubjects;
            set => this.RaiseAndSetIfChanged(ref _allTypeOfSubjects, value);
        }
        public FacultyInsert NewFaculty
        {
            get => _newFaculty;
            set => this.RaiseAndSetIfChanged(ref _newFaculty, value);
        }
        public Dean SelectedDean
        {
            get => _selectedDean;
            set
            {
                _selectedDean = value;
                NewFaculty.DeanId = _selectedDean.Id;
            }
        }
        public TypeOfSubject SelectedTypeOfSubject
        {
            get => _selectedTypeOfSubject;
            set
            {
                _selectedTypeOfSubject = value;
                NewFaculty.SubjectTypeId = _selectedTypeOfSubject.Id;
            }

        }
        public bool IsCorrectCreateNewFaculty { 
            get => _isCorrectCreateNewFaculty; 
            set => this.RaiseAndSetIfChanged( ref _isCorrectCreateNewFaculty, value); 
        }

        public CreateNewFacultyViewModel()
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
                        var deans = await GetAllDeans().ConfigureAwait(false);
                        var typeOfSubjects = await GetAllTypeOfSubject().ConfigureAwait(false);
                        AllDeans = deans;
                        AllTypeOfSubjects = typeOfSubjects;
                        break;
                    case 2:
                        var isCorrectInsert = await InsertNewFaculty().ConfigureAwait(false);
                        switch (isCorrectInsert)
                        {
                            case true:
                                IsCorrectCreateNewFaculty = true;
                                await Task.Delay(3000);
                                await Dispatcher.UIThread.InvokeAsync(() =>
                                {
                                    ToAllFaculties();
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
        private async Task<List<Dean>?> GetAllDeans()
        {
            var response = await MainWindowViewModel.LinkMWViewModel.SupabaseService.Supabase.From<Dean>().Get();
            return response.Models?.ToList();
        }
        private async Task<List<TypeOfSubject>?> GetAllTypeOfSubject()
        {
            var response = await MainWindowViewModel.LinkMWViewModel.SupabaseService.Supabase.From<TypeOfSubject>().Get();
            return response.Models?.ToList();
        }
        private async Task<bool> InsertNewFaculty()
        {
            try
            {
                var response = await MainWindowViewModel.LinkMWViewModel.SupabaseService.Supabase.From<FacultyInsert>().Insert(NewFaculty);
                return response.ResponseMessage!.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        public void ToAllFaculties()
        {
            MainWindowViewModel.LinkMWViewModel.Uc = new AllFaculty();
        }
        public void StartAsyncInsertNewFaculty()
        {
            InitializeAsync(2).ConfigureAwait(false);
        }
    }
}