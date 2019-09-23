using System.Web.Mvc;
using CMS.DocumentEngine;
using Custom.Domain;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;
using IRequestContext = Custom.DynamicRouting.Contexts.IRequestContext;

namespace Custom.DynamicRouting.Controllers
{
    public class BaseController : Controller
    {
        protected IRequestContext RequestContext { get; set; }

        public BaseController(IRequestContext requestContext)
        {
            RequestContext = requestContext;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!this.RequestContext.ContextResolved)
                this.ResolveContext();

            base.OnActionExecuting(filterContext);
        }

        protected virtual T GetContextItem<T>() where T: TreeNode, new()
        {
            return this.RequestContext.GetContextItem<T>();
        }

        private void ResolveContext()
        {
            this.RequestContext.ContextItemId = this.HttpContext.Items[Constants.DynamicRouting.ContextItemDocumentId] as int?;
            this.RequestContext.IsPreview = this.HttpContext.Kentico().Preview().Enabled;
            this.RequestContext.ContextResolved = true;
        }
    }
}