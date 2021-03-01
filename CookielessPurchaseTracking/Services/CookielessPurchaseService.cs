using ActivityInitializers;
using CMS.Activities;
using CMS.ContactManagement;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.SiteProvider;
using Interfaces;

namespace Services
{
    public class CookielessPurchaseService : ICookielessPurchaseService
    {
        private readonly IContactInfoProvider contactInfoProvider;
        private readonly ICustomerInfoProvider customerInfoProvider;
        private readonly IContactMembershipInfoProvider contactMembershipInfoProvider;
        private readonly IActivityLogService activityLogService;

        public CookielessPurchaseService(
            IContactInfoProvider contactInfoProvider,
            ICustomerInfoProvider customerInfoProvider,
            IContactMembershipInfoProvider contactMembershipInfoProvider,
            IActivityLogService activityLogService)
        {
            this.contactInfoProvider = contactInfoProvider;
            this.customerInfoProvider = customerInfoProvider;
            this.contactMembershipInfoProvider = contactMembershipInfoProvider;
            this.activityLogService = activityLogService;
        }

        public void EnsurePurchaseActivitiesAreLogged(ShoppingCartInfo cart)
        {
            var order = cart.Order;

            var customer = customerInfoProvider.Get(order.OrderCustomerID);

            var contact = GetRelevantContact(customer, out var membershipUpdateRequired);
            ContactInfoProvider.UpdateContactFromExternalData(customer, DataClassInfoProviderBase<DataClassInfoProvider>.GetDataClassInfo(CustomerInfo.TYPEINFO.ObjectClassName).ClassContactOverwriteEnabled, contact);

            if (membershipUpdateRequired)
            {
                contactMembershipInfoProvider.Set(new ContactMembershipInfo
                {
                    ContactID = contact.ContactID,
                    RelatedID = customer.CustomerID,
                    MemberType = MemberTypeEnum.EcommerceCustomer
                });
            }

            foreach (var cartItem in cart.CartItems)
            {
                var itemInitializer =
                    new CustomPurchasedProductActivityInitializer(cartItem.SKU, cartItem.CartItemUnits);
                var itemWrapper = new CustomEcommerceActivityInitializerWrapper(itemInitializer, contact.ContactID,
                    SiteContext.CurrentSiteID);
                activityLogService.LogWithoutModifiersAndFilters(itemWrapper);
            }

            var totalPriceInCorrectCurrency = string.Format(CurrencyInfoProvider.GetMainCurrency(SiteContext.CurrentSiteID).CurrencyFormatString, order.OrderTotalPriceInMainCurrency);
            var initializer =
                new CustomPurchaseActivityInitializer(order.OrderID, order.OrderTotalPrice,
                    totalPriceInCorrectCurrency);
            var wrapper =
                new CustomEcommerceActivityInitializerWrapper(initializer, contact.ContactID,
                    SiteContext.CurrentSiteID);
            activityLogService.LogWithoutModifiersAndFilters(wrapper);
        }

        private ContactInfo GetRelevantContact(CustomerInfo customer, out bool membershipUpdateRequired)
        {
            var existingMemberContactId =
                ContactMembershipInfoProvider.GetContactIDByMembership(customer.CustomerID, MemberTypeEnum.EcommerceCustomer);
            if (existingMemberContactId != 0)
            {
                membershipUpdateRequired = false;
                return contactInfoProvider.Get(existingMemberContactId);
            }

            membershipUpdateRequired = true;
            return ContactInfoProvider.GetContactInfo(customer.CustomerEmail) ??
                   new ContactInfo {ContactMonitored = true};
        }
    }
}