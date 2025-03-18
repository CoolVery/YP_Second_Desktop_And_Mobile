using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using ReactiveUI;
using YP_desktop.Models;
using YP_desktop.Models.Faculties;
using YP_desktop.Views;
using YP_desktop.Views.HROfficer;

namespace YP_desktop.ViewModels.HROfficer
{
    public class AllFacultyViewModel : ReactiveObject
	{
		List<Faculty> _allFaculties = new List<Faculty>();
        List<Faculty> _currentFaculties = new List<Faculty>();
        List<Dean> _allDean = new List<Dean>();
        bool _isFileCreated = false;

        Dean _selectedDean = new Dean();
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

        public List<Dean> AllDean 
        { 
            get => _allDean;
            set
            {
                this.RaiseAndSetIfChanged(ref _allDean, value);
            }
        }
        public Dean SelectedDean 
        { 
            get => _selectedDean;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedDean, value);
                AllFilters();
            }
        }
        public bool IsFileCreated 
        { 
            get => _isFileCreated; 
            set => this.RaiseAndSetIfChanged(ref _isFileCreated, value); 
        }

        private async Task InitializeAsync()
        {
            try
            {
                var faculties = await GetAllFaculty().ConfigureAwait(false);
                var deans = await GetAllDean().ConfigureAwait(false);
                CurrentFaculties = faculties;
                AllDean = deans;
                AllDean.Add(new Dean()
                {
                    FullName = "Без фильтров"
                });
                _allFaculties = new List<Faculty>(faculties);
            }
            catch (Exception ex)
            {

            }
        }
        private async Task<List<Dean>> GetAllDean()
        {
            try
            {
                var response = await MainWindowViewModel.LinkMWViewModel.SupabaseService.Supabase.From<Dean>().Get();
                return response.Models.ToList();
            }
            catch (Exception e)
            {
                return null;
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
            GroupByDean();
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
        private void GroupByDean()
        {
            switch (SelectedDean.FullName == "Без фильтров")
            {
                case false:
                    CurrentFaculties = CurrentFaculties.Where(x => x.DeanId.Id == SelectedDean.Id).ToList();
                    break;
                
            }
        }
        public async void LoadCurrentListFaculties()
        {
            try
            {
                string nameFile = "Список факультетов";
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Отчет оценок");
                    int countValues = 2;
                    worksheet.Cells["A1:D1"].Style.Font.Bold = true;
                    worksheet.Cells["A1"].Value = "Название факультета";
                    worksheet.Cells["B1"].Value = "Количество часов";
                    worksheet.Cells["C1"].Value = "Тип предмета";
                    worksheet.Cells["D1"].Value = "Декан";

                    foreach (var item in CurrentFaculties)
                    {
                        worksheet.Cells[$"A{countValues}"].Value = item.Name;
                        worksheet.Cells[$"B{countValues}"].Value = item.Hours;
                        worksheet.Cells[$"C{countValues}"].Value = item.SubjectTypeId.Name;
                        worksheet.Cells[$"D{countValues}"].Value = item.DeanId.FullName;

                        countValues++;
                    }                    
                    worksheet.Columns.AutoFit();

                    var fileInfo = new FileInfo(nameFile);
                    package.SaveAs(fileInfo);
                }
                IsFileCreated = true;
                Process.Start(new ProcessStartInfo(nameFile)
                {
                    UseShellExecute = true
                });
                await Task.Delay(3000);
                IsFileCreated = false;

            }
            catch (Exception e)
            {

            }
        }
        public void ToAuth()
        {
            MainWindowViewModel.LinkMWViewModel.Uc = new Authorization();
        }
    }
}