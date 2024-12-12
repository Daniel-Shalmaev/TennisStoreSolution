using TennisStoreClient.Pages.OtherPages;

namespace TennisStoreClient.Services
{
    public class MessageDialogService
    {
        #region Properties

        public MessageDialog? messageDialog;
        public Action? Action { get; set; }
        public bool ShowBusyButton { get; set; }
        public bool ShowSaveButton { get; set; } = true;

        #endregion

        #region Methods

        public async void SetMessageDialog()
        {
            if (messageDialog == null)
                throw new InvalidOperationException("MessageDialog is not initialized.");

            await messageDialog.ShowMessage();

            ShowBusyButton = false;
            ShowSaveButton = !ShowBusyButton;

            Action?.Invoke();
        }

        #endregion
    }
}
