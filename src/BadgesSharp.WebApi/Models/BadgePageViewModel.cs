namespace BadgesSharp.WebApi.Models
{
    /// <summary>
    /// The badge page view model.
    /// </summary>
    public class BadgePageViewModel : Badge
    {
        public string Title { get; set; }

        public string Badge { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string Report { get; set; }

        public string RunCommand { get; set; }
    }
}