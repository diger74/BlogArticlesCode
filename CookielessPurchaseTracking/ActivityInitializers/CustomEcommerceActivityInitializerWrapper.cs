using CMS.Activities;

namespace ActivityInitializers
{
    public class CustomEcommerceActivityInitializerWrapper : ActivityInitializerWrapperBase
    {
        private readonly int _contactId;
        private readonly int _siteId;

        /// <summary>
        /// Create the initializer, with the original initializer and the
        /// ContactID and SiteID values.
        /// </summary>
        /// <param name="originalInitializer"></param>
        /// <param name="contactId"></param>
        /// <param name="siteId"></param>
        public CustomEcommerceActivityInitializerWrapper(
            IActivityInitializer originalInitializer,
            int contactId,
            int siteId)
            : base(originalInitializer)
        {
            _contactId = contactId;
            _siteId = siteId;
        }

        /// <summary>
        /// Call the original initializer and then set the ActivityContactID
        /// and ActivitySiteID properties.
        /// </summary>
        /// <param name="activity"></param>
        public override void Initialize(IActivityInfo activity)
        {
            OriginalInitializer.Initialize(activity);
            activity.ActivityContactID = _contactId;
            activity.ActivitySiteID = _siteId;
        }
    }
}