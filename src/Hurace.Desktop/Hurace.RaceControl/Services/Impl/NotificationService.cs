using System;
using Hurace.Mvvm;
using MaterialDesignThemes.Wpf;

namespace Hurace.RaceControl.Services.Impl
{
    public class NotificationService : NotifyPropertyChanged, INotificationService
    {
        public SnackbarMessageQueue MessageQueue { get; private set; }
                = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(500));
        
        public void ShowMessage(string message)
        {
            this.MessageQueue.Enqueue(message);
        }
    }
}
