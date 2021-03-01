using CMS.Ecommerce;

namespace Interfaces
{
    public interface ICookielessPurchaseService
    {
        void EnsurePurchaseActivitiesAreLogged(ShoppingCartInfo cart);
    }
}