using Microsoft.AspNetCore.Components;

namespace Client.Components.Shared
{
    public partial class Toast : ComponentBase
    {
        [Parameter] public bool IsError { get; set; }
        [Parameter] public string ToastTitle { get; set; }
        [Parameter] public string ToastBody { get; set; }
        [Parameter] public EventCallback ParentMethodToCallOnClickBtnClose { get; set; }

        private string _showClass = string.Empty;
        private string _time = string.Empty;

        protected void OnAfterRenderAsync()
        {
            _time = DateTime.Now.ToString("hh:mm:ss tt");
            _showClass = "show";
        }
    }
}
