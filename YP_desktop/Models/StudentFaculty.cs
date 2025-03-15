using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP_desktop.Models
{
    [Table("studenrs_faculties")]
    public class StudentFaculty : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("id_student")]
        public int StudentId { get; set; }

        [Column("id_faculty")]
        public int FacultyId { get; set; }

        [Column("evaluation")]
        public int Evaluation { get; set; }
    }
}
