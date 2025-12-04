using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yugen.HomeBudget.Server.Helpers.Icons;

namespace Yugen.HomeBudget.Server.Components.Shared;

public partial class IconsDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }

    [Parameter]
    public string ButtonText { get; set; }

    [Parameter]
    public string IconName { get; set; }

    private List<MudIcons> MudIconList { get; set; } = [];

    private void Submit() => MudDialog.Close(DialogResult.Ok(IconName));

    private void Cancel() => MudDialog.Cancel();

    public Color IsActive(string iconName)
    {
        return IconName == iconName
            ? Color.Primary
            : Color.Default;
    }

    public void SetIcon(string iconName)
    {
        IconName = iconName;
    }

    protected override Task OnInitializedAsync()
    {
        MudIconList = IconHelper.GetMudIconsByType(IconType.Filled);

        return base.OnInitializedAsync();
    }
}
