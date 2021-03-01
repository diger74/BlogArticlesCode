using CMS.Activities;
using CMS.Ecommerce;
using CMS.Helpers;

namespace ActivityInitializers
{
    public class CustomPurchasedProductActivityInitializer : IActivityInitializer
    {
        private readonly ActivityTitleBuilder mTitleBuilder = new ActivityTitleBuilder();
        private readonly int mSkuID;
        private readonly int mSkuUnits;
        private readonly int mVariantID;
        private readonly string mSkuName;

        public CustomPurchasedProductActivityInitializer(SKUInfo sku, int skuUnits)
        {
            this.mSkuID = GetSKUID(sku);
            this.mVariantID = GetVariantID(sku);
            this.mSkuUnits = skuUnits;
            this.mSkuName = GetSKUName(sku);
        }

        public void Initialize(IActivityInfo activity)
        {
            activity.ActivityItemID = this.mSkuID;
            activity.ActivityItemDetailID = this.mVariantID;
            activity.ActivityValue = this.mSkuUnits.ToString();
            activity.ActivityTitle = this.mTitleBuilder.CreateTitle(this.ActivityType, this.mSkuName);
        }

        public string ActivityType => "purchasedproduct";

        public string SettingsKeyName => "CMSCMPurchasedProduct";

        private int GetSKUID(SKUInfo sku) => sku.IsProductVariant ? sku.SKUParentSKUID : sku.SKUID;

        private int GetVariantID(SKUInfo sku) => sku.IsProductVariant ? sku.SKUID : 0;

        private string GetSKUName(SKUInfo sku) => sku.IsProductVariant ? ResHelper.LocalizeString(sku.Parent.Generalized.ObjectDisplayName) : ResHelper.LocalizeString(sku.SKUName);
    }
}