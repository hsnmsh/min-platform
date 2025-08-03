namespace MinPlatform.Notifications.Abstractions
{
    using MinPlatform.ConfigStore.Service;
    using MinPlatform.Logging.Service;
    using MinPlatform.Notifications.Abstractions.Data;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class NotificationService
    {
        protected readonly LoggerManager LoggerManager;
        protected readonly ConfigStoreManager ConfigStoreManager;

        protected NotificationService(LoggerManager loggerManager, ConfigStoreManager configStoreManager)
        {
            this.LoggerManager = loggerManager;
            this.ConfigStoreManager = configStoreManager;
        }

        public abstract Task<NotificationResult> SendNotificationAsync(IDictionary<string, object> inputs, IDictionary<string, object> placeHolders, Notification notification);

        public virtual void OnNotificationSend(IDictionary<string, object> inputs, Notification notification)
        {
            string serializedInputs = JsonConvert.SerializeObject(inputs);

            var logMessageBuilder = new StringBuilder();

            logMessageBuilder
                .Append("Inputs:")
                .Append(serializedInputs)
                .Append("--")
                .Append("Notification:")
                .Append(notification.Name)
                .Append("-")
                .Append(notification.NotificationType.ToString())
                .Append("-")
                .Append(notification.LanguageCode);


            LoggerManager.Information(logMessageBuilder.ToString());


        }
    }
}
