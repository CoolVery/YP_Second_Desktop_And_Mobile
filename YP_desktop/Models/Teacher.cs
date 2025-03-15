using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP_desktop.Models
{
    [Table("teachers")]
    public class Teacher : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("experience")]
        public int Experience { get; set; }

        [Column("id_user_login")]
        public int UserId { get; set; }
    }
}
