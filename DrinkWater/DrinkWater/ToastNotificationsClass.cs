namespace DrinkWater
{
    using Microsoft.Toolkit.Uwp.Notifications;
    using Windows.Data.Xml.Dom;
    using Windows.UI.Notifications;

    public class ToastNotificationsClass
    {
        private ToastContent toastContent;

        public int Timer { get; set; }

        public ToastNotificationsClass()
        {
            toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                        new AdaptiveText()
                            {
                            Text = "Water drink reminder",
                            },
                        new AdaptiveText()
                            {
                            Text = "Hey, have you drunk enought today?",
                            },
                        },
                    },
                },
            };
        }

        public void ShowNot()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(toastContent.GetContent());
            var toast = new ToastNotification(xmlDoc);
            ToastNotificationManager.CreateToastNotifier(@"{1AC14E77-02E7-4E5D-B744-2EB1AE5198B7}\WindowsPowerShell\v1.0\powershell.exe").Show(toast); // Display
        }
    }
}
