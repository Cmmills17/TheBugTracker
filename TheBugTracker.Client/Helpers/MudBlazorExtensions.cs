using MudBlazor;
using TheBugTracker.Client.Models.Enums;

namespace TheBugTracker.Client.Helpers
{
    public static class MudBlazorExtensions
    {
        public static MudBlazor.Color GetColor(this ProjectPriority priority)
        {

            Color color = priority switch
            {
                ProjectPriority.Low => Color.Success,
                ProjectPriority.Medium => Color.Secondary,
                ProjectPriority.High => Color.Error,
                ProjectPriority.Urgent => Color.Dark,
                _ => Color.Default,

            };
            return color;
        }
    }
}
