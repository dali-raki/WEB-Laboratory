using GLAB.Domains.Models.Teams;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GLAB.Web1.Components.Components.Teams;

partial class CardMenu
{
    [Inject] private IJSRuntime js { get; set; }
    [Parameter] public EventCallback closeCardMenu { get; set; }
    [Parameter] public EventCallback removeCardEvent { get; set; }
    
    
    private ElementReference cardMenuContainer;
    private async Task hideMenu()
    {
       await js.InvokeVoidAsync("hideMenu",cardMenuContainer);
       await Task.Delay(450);
       await closeCardMenu.InvokeAsync();
    }

    private async Task removeCard()
    {
      await  removeCardEvent.InvokeAsync();
    }

    
}