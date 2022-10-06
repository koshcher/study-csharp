using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogDbLibrary.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        [DisplayName("Display Text")]
        public string Text { get; set; }

        public string Publisher { get; set; }
    }
}