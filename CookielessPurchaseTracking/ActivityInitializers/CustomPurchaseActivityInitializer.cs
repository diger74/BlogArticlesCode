using System;
using CMS.Activities;
using CMS.Helpers;

namespace ActivityInitializers
{
    public class CustomPurchaseActivityInitializer : IActivityInitializer
    {
        private readonly int mOrderId;
        private readonly Decimal mTotalPrice;
        private readonly string mTotalPriceInCorrectCurrency;
        private readonly ActivityTitleBuilder mTitleBuilder = new ActivityTitleBuilder();

        public CustomPurchaseActivityInitializer(
            int orderId,
            Decimal totalPrice,
            string totalPriceInCorrectCurrency)
        {
            this.mOrderId = orderId;
            this.mTotalPrice = totalPrice;
            this.mTotalPriceInCorrectCurrency = totalPriceInCorrectCurrency;
        }

        public void Initialize(IActivityInfo activity)
        {
            activity.ActivityItemID = this.mOrderId;
            activity.ActivityTitle = this.mTitleBuilder.CreateTitle(this.ActivityType, this.mTotalPriceInCorrectCurrency);
            activity.ActivityValue = this.mTotalPrice.ToString((IFormatProvider)CultureHelper.EnglishCulture);
        }

        public string ActivityType => "purchase";

        public string SettingsKeyName => "CMSCMPurchase";
    }
}