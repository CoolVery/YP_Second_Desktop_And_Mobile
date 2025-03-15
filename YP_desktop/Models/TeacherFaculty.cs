using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP_desktop.Models
{
    [Table("teachers_faculty")]
    public class TeacherFaculty : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("id_teacher")]
        public int TeacherId { get; set; }

        [Column("id_faculty")]
        public int FacultyId { get; set; }
    }
}
