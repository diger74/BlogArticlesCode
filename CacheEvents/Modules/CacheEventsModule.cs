using CMS;
using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using DancingGoat.Infrastructure;
using DancingGoat.Modules;

[assembly: RegisterModule(typeof(CacheEventsModule))]
namespace DancingGoat.Modules
{
    /// <summary>
    /// Module for cache events.
    /// </summary>
    public class CacheEventsModule : Module
    {
        public CacheEventsModule()
            : base("CacheEventsModule")
        {
        }

        protected override void OnInit()
        {
            CacheEvents.CacheItemRemoved.Execute += CacheItemRemovedOnExecute;
        }

        private void CacheItemRemovedOnExecute(object sender, CMSEventArgs<CacheItemRemovedEventArgs> e)
        {
            if (e.Parameter.Reason == CMSCacheItemRemovedReason.Expired)
            {
                CacheExpirationCallbacks.Call(e.Parameter.Key);
            }
        }
    }
}