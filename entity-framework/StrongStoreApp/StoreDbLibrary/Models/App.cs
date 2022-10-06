using System.ComponentModel.DataAnnotations.Schema;

namespace StoreDbLibrary.Models
{
    public class App
    {
        public virtual ICollection<AppCategory> AppCategories { get; set; }
        public virtual ICollection<AppLanguage> AppLanguages { get; set; }
        public virtual ICollection<AppUser> AppUsers { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; }

        public int Id { get; set; }
        public bool IsGame { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public virtual Publisher Publisher { get; set; }
        public int PublisherId { get; set; }
        public int Rating { get; set; }

        [Column(TypeName = "date")]
        public DateTime ReleaseDate { get; set; }
    }
}