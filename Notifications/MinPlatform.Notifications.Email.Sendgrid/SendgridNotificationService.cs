namespace MinPlatform.Notifications.Email.Sendgrid
{
    using MinPlatform.ConfigStore.Service;
    using MinPlatform.Logging.Service;
    using MinPlatform.Notifications.Abstractions;
    using MinPlatform.Notifications.Abstractions.Data;
    using MinPlatform.Notifications.Abstractions.Email;
    using MinPlatform.Notifications.Abstractions.Exceptions;
    using Newtonsoft.Json;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class SendgridNotificationService : NotificationService
    {
        private const string sendgridConfig = nameof(sendgridConfig);
        private const string apiKey = nameof(apiKey);

        private readonly NotificationConfig notificationConfig;

        public SendgridNotificationService(LoggerManager loggerManager, ConfigStoreManager configStoreManager, NotificationConfig notificationConfig)
            : base(loggerManager, configStoreManager)
        {
            this.notificationConfig = notificationConfig;
        }

        public override async Task<NotificationResult> SendNotificationAsync(IDictionary<string, object> inputs, IDictionary<string, object> placeHolders, Notification notification)
        {
            IDictionary<string, object> emailConfig = await ConfigStoreManager.GetConfigAsync(sendgridConfig);

            if (emailConfig is null)
            {
                throw new NotificationException("sendgrid config is not valid");
            }

            var configProperties = JsonConvert.DeserializeObject<IDictionary<string, object>>(emailConfig[NotificationConstants.Properties].ToString());
            string key = configProperties[apiKey].ToString();

            if (string.IsNullOrEmpty(key))
            {
                throw new NotificationException("Sendgrid api key is not founf");

            }
            var client = new SendGridClient(apiKey);

            EmailInfo emailInfo = JsonConvert.DeserializeObject<EmailInfo>(notification.Properties);
            EmailAddress emailAddress = null;

            if (inputs.TryGetValue("From", out object sender))
            {
                emailAddress = new EmailAddress(sender.ToString());
            }
            else if (!string.IsNullOrEmpty(emailInfo?.From))
            {
                emailAddress = new EmailAddress(emailInfo?.From);

            }
            else if (notificationConfig.ConfigInputs.TryGetValue("SenderEmail", out object senderEmail))
            {
                emailAddress = new EmailAddress(senderEmail.ToString());
            }
            else
            {
                throw new NotificationException("Email sender is not defined");
            }

            var tos = new List<EmailAddress>();

            if (inputs.TryGetValue(NotificationConstants.Recipients, out object inputRecipients))
            {
                foreach (var recipient in (IEnumerable<string>)inputRecipients)
                {
                    tos.Add(new EmailAddress(recipient));
                }
            }

            if (emailInfo.To != null && emailInfo.To.Any())
            {
                foreach (var recipient in emailInfo.To)
                {
                    tos.Add(new EmailAddress(recipient));
                }
            }

            if (notificationConfig.ConfigInputs.TryGetValue(NotificationConstants.Environment, out object env) &&
               env.ToString() == NotificationConstants.Development &&
               configProperties.ContainsKey("recipientTestEmail"))
            {
                tos.Add(new EmailAddress(configProperties["recipientTestEmail"].ToString()));
            }

            if (!tos.Any())
            {
                throw new NotificationException("Email recipients are not found");
            }

            var ccs = new List<EmailAddress>();

            if (inputs.TryGetValue("cc", out object inputCC))
            {
                foreach (var recipient in (IEnumerable<string>)inputCC)
                {
                    ccs.Add(new EmailAddress(recipient));
                }
            }

            if (emailInfo.CC != null && emailInfo.CC.Any())
            {
                foreach (var recipient in emailInfo.CC)
                {
                    ccs.Add(new EmailAddress(recipient));
                }
            }

            var attachments = new List<FileAttachment>();

            if (inputs.TryGetValue("Files", out object files))
            {
                attachments.AddRange((IEnumerable<FileAttachment>)files);
            }

            if (emailInfo?.Files != null && emailInfo.Files.Any())
            {
                attachments.AddRange(emailInfo.Files);
            }

            var subjectMessageBuilder = new StringBuilder();
            subjectMessageBuilder.Append(notification.Subject);

            foreach (var pair in placeHolders)
            {
                subjectMessageBuilder.Replace("{" + pair.Key + "}", pair.Value.ToString());
            }

            var messageBuilder = new StringBuilder();
            messageBuilder.Append(notification.MessageContent);

            foreach (var pair in placeHolders)
            {
                messageBuilder.Replace("{" + pair.Key + "}", pair.Value.ToString());
            }

            var msg = new SendGridMessage()
            {
                Subject = subjectMessageBuilder.ToString(),
                HtmlContent = messageBuilder.ToString(),
                From = emailAddress,
            };

            msg.AddTos(tos);
            msg.AddCcs(ccs);

            foreach (var file in attachments)
            {
                using (var fileStream = File.OpenRead(file.Path))
                {
                    await msg.AddAttachmentAsync(file.FileName, fileStream);
                }
            }

            try
            {
                var response = await client.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    return new NotificationResult
                    {
                        Success = true
                    };
                }

                return new NotificationResult
                {
                    Errors = new List<string>() { await response.Body.ReadAsStringAsync() }
                };


            }
            catch (Exception ex)
            {

                LoggerManager.Error("Inputs:" + JsonConvert.SerializeObject(inputs) + "-Notification:" + JsonConvert.SerializeObject(notification), ex);

                return new NotificationResult
                {
                    Errors = new List<string> { ex.Message }
                };
            }

        }
    }
}
