using Microsoft.AspNetCore.Components;

namespace Client.Components.Public.Portfolio;

public partial class ExamplePortfolioProjectModalWindow : ComponentBase
{
    [Parameter] public EventCallback ParentMethodToCallbackOnClickBtnClose { get; set; }
    private string _modalDisplay = null;
    private string _modalClass = null;
    private bool _showBackdrop = false;

    protected override void OnInitialized() => ShowModal();

    private void ShowModal()
    {
        _modalDisplay = "block;";
        _modalClass = "show";
        _showBackdrop = true;
    }
    
    private void CloseModal()
    {
        _modalDisplay = "none";
        _modalClass = string.Empty;
        _showBackdrop = false;
        ParentMethodToCallbackOnClickBtnClose.InvokeAsync();
    }
}