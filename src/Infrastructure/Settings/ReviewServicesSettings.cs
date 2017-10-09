namespace Infrastructure.Settings
{
    public class ReviewServicesSettings : MultiElementSettingsBase<ReviewService>
    {
        public ReviewService Filmweb { get; set; }
    }
}