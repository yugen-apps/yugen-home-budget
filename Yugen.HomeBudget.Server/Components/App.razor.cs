using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;

namespace Yugen.HomeBudget.Server.Components
{
    public partial class App
    {
        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        //private IComponentRenderMode PageRenderMode => HttpContext.Request.Path.StartsWithSegments("/Account")
        //    ? null
        //    : new InteractiveServerRenderMode(false);

        private IComponentRenderMode PageRenderMode => HttpContext.AcceptsInteractiveRouting()
            ? new InteractiveServerRenderMode(false)
            : null;

        //private IComponentRenderMode PageRenderMode => HttpContext.AcceptsInteractiveRouting()
        //    ? new InteractiveAutoRenderMode()
        //    : null;
    }
}
