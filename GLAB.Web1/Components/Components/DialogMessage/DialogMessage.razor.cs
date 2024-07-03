using Microsoft.AspNetCore.Components;

namespace GLAB.Web1.Components.Components.DialogMessage;

partial class DialogMessage
{
    [Parameter] public List<string> messages { get; set; } 
    [Parameter] public bool isError { get; set; } 
}