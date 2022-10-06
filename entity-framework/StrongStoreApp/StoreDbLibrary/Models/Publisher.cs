namespace StoreDbLibrary.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<App> Apps { get; set; }
    }
}