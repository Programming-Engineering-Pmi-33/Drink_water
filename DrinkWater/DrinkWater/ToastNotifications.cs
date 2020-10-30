using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace DrinkWater
{
    public class ToastNotificationsClass
    {
        public ToastContent toastContent;
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
                            Text = "Water drink reminder"
                            },
                            new AdaptiveText()
                            {
                            Text = "Hey, have you drunk enought today?"
                            }
                        },
                    }
                }
            };
        }
        public void ShowNot()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(toastContent.GetContent());
            var toast = new ToastNotification(xmlDoc);
            ToastNotificationManager.CreateToastNotifier("Drink Water").Show(toast); // Display 
        }
    }
}
