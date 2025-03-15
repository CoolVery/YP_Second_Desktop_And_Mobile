using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP_desktop.Models
{
    [Table("deans")]
    public class Dean : BaseModel
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
    }
}
