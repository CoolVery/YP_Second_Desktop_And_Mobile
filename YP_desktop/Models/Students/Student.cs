using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP_desktop.Models.Students
{
    [Table("students")]
    public class Student : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("id_login_user")]
        public int UserId { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("id_group")]
        public Group GroupId { get; set; }
    }
}
