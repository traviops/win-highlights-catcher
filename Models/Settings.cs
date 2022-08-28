using Newtonsoft.Json;

namespace win_highlights_catcher.Models
{
    public class Settings
    {
        [JsonProperty("AUTO_START")]
        public int AutoStart { get; set; }

        public bool IsAutoStartEnabled
        {
            get
            {
                return AutoStart > 0;
            }
        }
    }
}
