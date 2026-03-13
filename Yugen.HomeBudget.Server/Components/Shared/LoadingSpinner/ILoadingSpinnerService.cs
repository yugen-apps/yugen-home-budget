using System;

namespace Yugen.HomeBudget.Server.Components.Shared.LoadingSpinner;

public interface ILoadingSpinnerService
{
    void Wait();

    void Resume();

    event Action<bool> OnSpinnerStateChanged;
}
