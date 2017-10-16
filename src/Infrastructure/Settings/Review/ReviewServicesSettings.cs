using Infrastructure.Settings.Common;

namespace Infrastructure.Settings.Review
{
    public class ReviewServicesSettings : MultiElementSettingsBase<ReviewService>
    {
        public ReviewService Filmweb { get; set; }
    }
}