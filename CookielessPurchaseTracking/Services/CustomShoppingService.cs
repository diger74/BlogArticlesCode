using CMS;
using CMS.Ecommerce;
using CMS.Globalization;
using CMS.Helpers;
using Interfaces;
using Services;

[assembly: RegisterImplementation(typeof(IShoppingService), typeof(CustomShoppingService))]
namespace Services
{
    public class CustomShoppingService : ShoppingService
    {
        private readonly ICurrentCookieLevelProvider currentCookieLevelProvider;
        private readonly ICookielessPurchaseService cookielessPurchaseService;
        private bool IsCookieLevelAtLeastVisitor => currentCookieLevelProvider.GetCurrentCookieLevel() >= 200;

        public CustomShoppingService(
            IEcommerceActivityLogger ecommerceActivityLogger,
            ICartItemChecker cartItemChecker,
            ICurrentShoppingCartService currentShoppingCartService,
            ICustomerShoppingService customerShoppingService,
            IShoppingCartAdapterService shoppingCartAdapterService,
            IShippingPriceService shippingPriceService,
            IAddressInfoProvider addressInfoProvider,
            ICustomerInfoProvider customerInfoProvider,
            IPaymentOptionInfoProvider paymentOptionInfoProvider,
            IShippingOptionInfoProvider shippingOptionInfoProvider,
            IShoppingCartInfoProvider shoppingCartInfoProvider,
            IShoppingCartItemInfoProvider shoppingCartItemInfoProvider,
            ISKUInfoProvider skuInfoProvider,
            ICountryInfoProvider countryInfoProvider,
            IStateInfoProvider stateInfoProvider,
            ICurrentCookieLevelProvider currentCookieLevelProvider,
            ICookielessPurchaseService cookielessPurchaseService)
            : base(
                ecommerceActivityLogger,
                cartItemChecker,
                currentShoppingCartService,
                customerShoppingService,
                shoppingCartAdapterService,
                shippingPriceService,
                addressInfoProvider,
                customerInfoProvider,
                paymentOptionInfoProvider,
                shippingOptionInfoProvider,
                shoppingCartInfoProvider,
                shoppingCartItemInfoProvider,
                skuInfoProvider,
                countryInfoProvider,
                stateInfoProvider)
        {
            this.currentCookieLevelProvider = currentCookieLevelProvider;
            this.cookielessPurchaseService = cookielessPurchaseService;
        }

        protected override void LogPurchaseActivities(ShoppingCartInfo cart)
        {
            if (IsCookieLevelAtLeastVisitor)
            {
                base.LogPurchaseActivities(cart);
            }
            else
            {
                cookielessPurchaseService.EnsurePurchaseActivitiesAreLogged(cart);
            }
        }
    }
}