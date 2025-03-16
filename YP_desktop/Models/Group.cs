using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace YP_desktop.Models
{
    [Table("groups")]
    public class Group : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}
