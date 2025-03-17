using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YP_desktop.Models.Faculties;

namespace YP_desktop.Models.StudentsFacilties
{
    [Table("students_faculties")]

    public class StudentFacultyJoinTable : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("id_student")]
        public int StudentId { get; set; }

        [Column("id_faculty")]
        public Faculty FacultyId { get; set; }

        [Column("evaluation")]
        public int Evaluation { get; set; }
    }
}
