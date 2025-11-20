using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Yugen.HomeBudget.Server.Components
{
    public partial class App
    {
        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        private IComponentRenderMode? RenderModeForPage => HttpContext.Request.Path.StartsWithSegments("/Account")
            ? null
            : new InteractiveServerRenderMode(false);
    }
}
