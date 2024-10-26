using TennisStoreClient.Pages.OtherPages;

namespace TennisStoreClient.Services
{
    public class MessageDialogService
    {
        public MessageDialog? messageDialog;
        public bool ShowBusyButton { get; set; }
        public bool ShowSaveButton { get; set; } = true;
        public Action? Action { get; set; }

        public async void SetMessageDialog()
        {
            await messageDialog!.ShowMessage();
            ShowBusyButton = false;
            ShowSaveButton = !ShowBusyButton;
            Action?.Invoke();
        }
    }
}
