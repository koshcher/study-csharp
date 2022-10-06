namespace StoreDbLibrary.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AppLanguage> AppLanguages { get; set; }
    }
}