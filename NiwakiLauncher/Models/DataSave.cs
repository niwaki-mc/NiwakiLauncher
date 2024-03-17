using Newtonsoft.Json;

namespace NiwakiLauncher.Models;

public class DataSave
{
    [JsonProperty("ram")]
    public int SelectedRam { get; set; }
}