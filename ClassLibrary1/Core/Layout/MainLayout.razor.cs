using Client.Core.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Services;
using System.Globalization;

namespace Client.Core.Layout;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [Inject] private LayoutService LayoutService { get; set; } = null!;

    [Inject] private IBreakpointService BreakpointListener { get; set; } = null!;

    private MudThemeProvider? _mudThemeProvider;

    private Breakpoint _breakpoint = new();
    private Guid _subscriptionId;

  

    private CultureInfo[] supportedCultures = new[]
   {
        new CultureInfo("en-US"),
        new CultureInfo("ru-RU"),
        new CultureInfo("uz-Latn"),
    };

    private CultureInfo Culture
    {
        get => CultureInfo.CurrentCulture;
        set
        {
            if (CultureInfo.CurrentCulture == value)
            {
                return;
            }
            var uri = new Uri(Navigation.Uri)
                .GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
            var cultureEscaped = Uri.EscapeDataString(value.Name);
            var uriEscaped = Uri.EscapeDataString(uri);
            JSRuntime.InvokeVoidAsyncIgnoreErrors("blazorCulture.set", value.Name);
            Navigation.NavigateTo(
                $"Culture/Set?culture={cultureEscaped}&redirectUri={uriEscaped}",
                forceLoad: true);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await ApplyUserPreferences();

            var subscriptionResult = await BreakpointListener.SubscribeAsync((breakpoint) =>
            {
                _breakpoint = breakpoint;
                InvokeAsync(StateHasChanged);
            }, new ResizeOptions
            {
                ReportRate = 250,
                NotifyOnBreakpointOnly = true,
            });
            _subscriptionId = subscriptionResult.SubscriptionId;
            _breakpoint = subscriptionResult.Breakpoint;
            StateHasChanged();
        }
     }

    private async Task ApplyUserPreferences()
    {
        if (_mudThemeProvider != null)
        {

            var defaultDarkMode = await _mudThemeProvider.GetSystemPreference();
            await LayoutService.ApplyUserPreferences(defaultDarkMode);
        }

    }

    

    
}
