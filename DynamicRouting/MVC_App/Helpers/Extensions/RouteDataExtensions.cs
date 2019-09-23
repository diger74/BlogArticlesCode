using System;
using System.Web.Routing;
using CMS.Helpers;

namespace Custom.Extensions
{
    public static class RouteDataExtensions
    {
        public static string GetValueSafe(this RouteData routeData, string key)
        {
            return routeData.Values.ContainsKey(key)
                ? ValidationHelper.GetString(routeData.Values[key], String.Empty)
                : String.Empty;
        }
    }
}