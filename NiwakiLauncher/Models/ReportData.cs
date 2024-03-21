using Newtonsoft.Json;

namespace NiwakiLauncher.Models;

public class ReportData
{
    [JsonProperty("minecraft_user_name")]
    public string? MinecraftUserName { get; set; }
    
    [JsonProperty("computer_user_name")]
    public string? ComputerUserName { get; set; }
    
    [JsonProperty("error_message")]
    public string? ErrorMessage { get; set; }
    
    [JsonProperty("detailed_error")]
    public string? DetailedError { get; set; }
}