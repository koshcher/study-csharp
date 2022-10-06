using System.ComponentModel.DataAnnotations.Schema;

namespace StoreDbLibrary.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }

        public virtual ICollection<AppUser> AppUsers { get; set; }
    }
}