using System;

namespace Yugen.Home.Budget.Server.Components.Shared.LoadingSpinner;

public interface ILoadingSpinnerService
{
    void Wait();

    void Resume();

    event Action<bool> OnSpinnerStateChanged;
}
