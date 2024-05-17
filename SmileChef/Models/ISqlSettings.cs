namespace ChefApp.Models
{
    public interface ISqlSettings
    {
        string? ConnectionString { get; set; }
        bool InMemory { get; set; }
    }
}