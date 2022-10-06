namespace StoreDbLibrary.Models
{
    public class AppLanguage
    {
        public int Id { get; set; }

        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }

        public int AppId { get; set; }
        public virtual App App { get; set; }
    }
}