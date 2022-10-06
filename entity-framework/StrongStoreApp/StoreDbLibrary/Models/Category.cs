namespace StoreDbLibrary.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsForGame { get; set; }

        public virtual ICollection<AppCategory> AppCategories { get; set; }
    }
}