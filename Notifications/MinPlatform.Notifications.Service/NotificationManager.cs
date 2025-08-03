namespace MinPlatform.Notifications.Service
{
    using MinPlatform.Notifications.Abstractions;
    using MinPlatform.Notifications.Abstractions.Data;
    using MinPlatform.Notifications.Abstractions.Exceptions;
    using MinPlatform.Notifications.Service.Validations;
    using MinPlatform.Validators.Service;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class NotificationManager
    {
        private readonly INotificationDataService notificationDataService;
        private readonly INotificationServiceResolver notificationServiceResolver;
        private readonly ValidatorManager validatorManager;

        public NotificationManager(INotificationDataService notificationDataService,
            INotificationServiceResolver notificationServiceResolver,
            ValidatorManager validatorManager)
        {
            this.notificationDataService = notificationDataService;
            this.notificationServiceResolver = notificationServiceResolver;
            this.validatorManager = validatorManager;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return await notificationDataService.GetNotificationsAsync(name);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(string name, string languageCode)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(languageCode))
            {
                throw new ArgumentNullException(nameof(languageCode));
            }

            return await notificationDataService.GetNotificationsAsync(name, languageCode);

        }

        public async Task<NotificationResult> SendNotificationAsync(NotificationInputs notificationInputs)
        {
            var notificationValidator = new NotificationInputsValidatorExecutor(notificationInputs);
            var validationResult = validatorManager.ExecuteValidation(notificationValidator);

            if (validationResult.Any(result => !result.IsValid))
            {
                throw new NotificationException(string.Join(',', validationResult.SelectMany(result => result.ErrorDescriptions)));
            }

            if (notificationInputs.Notification.IsActive)
            {
                var notificationService = notificationServiceResolver.Create(notificationInputs.Notification.NotificationType);
                var notificationResult = await notificationService.SendNotificationAsync(notificationInputs.Inputs, 
                    notificationInputs.PlaceHolders, notificationInputs.Notification);

                notificationService.OnNotificationSend(notificationInputs.Inputs, notificationInputs.Notification);

                return notificationResult;

            }

            return new NotificationResult
            {
                Errors = new List<string>() { notificationInputs.Notification.Name + " is not active For " + notificationInputs.Notification.NotificationType.ToString()}
            };


        }

        public async Task<IEnumerable<NotificationResult>> SendNotificationAsync(IEnumerable<NotificationInputs> notificationInputs)
        {
            var notificationResults = new List<NotificationResult>();

            foreach (NotificationInputs notificationInput in notificationInputs)
            {
                var result= await SendNotificationAsync(notificationInput);

                notificationResults.Add(result);
            }

            return notificationResults;

        }
    }
}
