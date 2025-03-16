using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace YP_desktop.Models.Students
{
    [Table("students")]
    public class StudentInsert : BaseModel
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
        public int GroupId { get; set; }

    }
}
