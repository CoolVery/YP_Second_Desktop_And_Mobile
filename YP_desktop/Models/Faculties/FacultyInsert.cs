using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP_desktop.Models.Faculties
{
    [Table("faculties")]
    public class FacultyInsert : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("number_of_hours")]
        public int Hours { get; set; }

        [Column("id_type_subject")]
        public int SubjectTypeId { get; set; }

        [Column("id_dean_faculty")]
        public int DeanId { get; set; }
    }
}
