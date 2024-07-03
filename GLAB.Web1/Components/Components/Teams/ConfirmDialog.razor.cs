using Microsoft.AspNetCore.Components;

namespace GLAB.Web1.Components.Components.Teams;

partial class ConfirmDialog
{
    [Parameter] public string dialogMessage { get; set; }
    [Parameter] public Object target { get; set; }
    [Parameter] public EventCallback<Object> response { get; set; }
    [Parameter] public string cancelButtonLabel { get; set; }
    [Parameter] public string confirmButtonLabel { get; set; }  
  
    
    private async Task closeDialog()
    {
        await response.InvokeAsync(false);
    }

    private async Task confirm()
    {
        await response.InvokeAsync(target);
    }
}