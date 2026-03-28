using System;

namespace Yugen.Home.Budget.Server.Components.Shared.LoadingSpinner;

public class LoadingSpinnerService : ILoadingSpinnerService
{
    public event Action<bool> OnSpinnerStateChanged;

    public void Wait() => SetState(true);

    public void Resume() => SetState(false);

    private void SetState(bool isBusy) => 
        OnSpinnerStateChanged?.Invoke(isBusy);
}