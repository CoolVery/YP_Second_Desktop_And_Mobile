using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP_desktop.Models
{
    [Table("themes_faculty")]
    public class ThemeFaculty : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("id_theme")]
        public int ThemeId { get; set; }

        [Column("id_faculty")]
        public int FacultyId { get; set; }
    }
}
