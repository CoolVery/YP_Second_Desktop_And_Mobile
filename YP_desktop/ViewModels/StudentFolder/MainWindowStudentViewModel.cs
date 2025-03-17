using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using ReactiveUI;
using YP_desktop.Models;
using YP_desktop.Models.Students;
using YP_desktop.Models.StudentsFacilties;
using YP_desktop.Views;

namespace YP_desktop.ViewModels.StudentFolder
{
    public class MainWindowStudentViewModel : ReactiveObject
    {
        Student _currentStudent = new Student();
        List<StudentFacultyJoinTable> _currentstudentFaculty = new List<StudentFacultyJoinTable>();
        User _currentUser = new User();
        bool _isCreatedFile = false;
        public Student CurrentStudent { get => _currentStudent; set => this.RaiseAndSetIfChanged(ref _currentStudent, value); }
        public List<StudentFacultyJoinTable> CurrentStudentFaculty { get => _currentstudentFaculty; set => this.RaiseAndSetIfChanged(ref _currentstudentFaculty, value); }
        public User CurrentUser { get => _currentUser; set => this.RaiseAndSetIfChanged(ref _currentUser, value); }
        public bool IsCreatedFile { get => _isCreatedFile; set => this.RaiseAndSetIfChanged(ref _isCreatedFile, value); }

        public MainWindowStudentViewModel(User user)
        {
            CurrentUser = user;
            InitializeAsync().ConfigureAwait(false);
        }
        private async Task InitializeAsync()
        {
            try
            {
                CurrentStudent = await GetCurrentStudent().ConfigureAwait(false);
                CurrentStudentFaculty =  await GetCurrentUserFaculty().ConfigureAwait(false);
            }
            catch (Exception e)
            {

            }
        }
        public void ToAuthorization()
        {
            MainWindowViewModel.LinkMWViewModel.Uc = new Authorization();
        }
        private async Task<Student> GetCurrentStudent()
        {
            var response = await MainWindowViewModel.LinkMWViewModel.SupabaseService.Supabase.From<Student>()
                .Select("id, " +
                        "id_login_user, " +
                        "full_name, " +
                        "address, " +
                        "phone," +
                        "id_group:groups(id, name)")
                .Where(x => x.UserId == _currentUser.Id)
                .Get();
            return response.Model;
        }
        private async Task<List<StudentFacultyJoinTable>> GetCurrentUserFaculty()
        {
            var response = await MainWindowViewModel.LinkMWViewModel.SupabaseService.Supabase.From<StudentFacultyJoinTable>()
                .Select("id," +
                        "id_student," +
                        "id_faculty:faculties(name)," +
                        "evaluation")
                .Where(x => x.StudentId == CurrentStudent.Id)
                .Get();
            return response.Models.ToList();
        }
        public async void CreateExcelFileWithEvaluation()
        {
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Отчет оценок");
                    int countValues = 1;
                    int summEvaluation = 0;
                    foreach (var item in _currentstudentFaculty)
                    {
                        worksheet.Cells[$"A{countValues}"].Value = item.FacultyId.Name;
                        worksheet.Cells[$"B{countValues}"].Value = item.Evaluation;
                        countValues++;
                        summEvaluation += Convert.ToInt32(item.Evaluation);
                    }
                    double averageValue = summEvaluation / --countValues;
                    worksheet.Cells[$"A{countValues + 1}"].Value = $"{CurrentStudent.FullName}";
                    worksheet.Cells[$"B{countValues + 1}"].Value = $"Среднее значение оценок - {averageValue}";
                    worksheet.Columns.AutoFit();

                    var fileInfo = new FileInfo("Среднее значение оценок.xlsx");
                    package.SaveAs(fileInfo);
                }
                IsCreatedFile = true;
                await Task.Delay(3000);
                IsCreatedFile = false;
            }
            catch (Exception e)
            {

            }
        }
    }
}