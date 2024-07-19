using System;
using System.Web;
using System.Web.UI;

using SOS.Core;

namespace SOS.Web
{
    public class SOSPage : Page
    {

#region Members
        public event SiteMapResolveEventHandler SiteMapResolve;
#endregion

#region Protected Methods
        protected virtual SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            if (SiteMapResolve != null)
                return SiteMapResolve(this, e);

            return SiteMap.CurrentNode;
        }

        protected virtual bool IsSamePage(HttpContext context1, HttpContext context2)
        {
            return ((Server.MapPath(context1.Request.AppRelativeCurrentExecutionFilePath) == Server.MapPath(context2.Request.AppRelativeCurrentExecutionFilePath)) && (context1.Request.QueryString == context2.Request.QueryString));
        }
#endregion

#region Event Handlers
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SiteMap.SiteMapResolve += new SiteMapResolveEventHandler(SiteMap_SiteMapResolve);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            SiteMap.SiteMapResolve -= new SiteMapResolveEventHandler(SiteMap_SiteMapResolve);
        }

        SiteMapNode SiteMap_SiteMapResolve(object sender, SiteMapResolveEventArgs e)
        {
            if (IsSamePage(Context, e.Context))
                return BindNavigation(e);

            return SiteMap.CurrentNode;
        }
#endregion

    }
}
