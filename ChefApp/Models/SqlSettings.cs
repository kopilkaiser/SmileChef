namespace ChefApp.Models
{
    public class SqlSettings : ISqlSettings
    {
        public string? ConnectionString { get; set; }
        public bool InMemory { get; set; }
    }
}
