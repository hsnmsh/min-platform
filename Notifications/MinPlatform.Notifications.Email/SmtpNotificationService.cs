namespace MinPlatform.Notifications.Email.Smtp
{
    using MinPlatform.ConfigStore.Service;
    using MinPlatform.Logging.Service;
    using MinPlatform.Notifications.Abstractions;
    using MinPlatform.Notifications.Abstractions.Data;
    using MinPlatform.Notifications.Abstractions.Email;
    using MinPlatform.Notifications.Abstractions.Exceptions;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class SmtpNotificationService : NotificationService
    {
        private const string smtpConfig = nameof(smtpConfig);
        private readonly NotificationConfig notificationConfig;

        public SmtpNotificationService(LoggerManager loggerManager, ConfigStoreManager configStoreManager, NotificationConfig notificationConfig)
            : base(loggerManager, configStoreManager)
        {
            this.notificationConfig = notificationConfig;
        }

        public override async Task<NotificationResult> SendNotificationAsync(IDictionary<string, object> inputs,
            IDictionary<string, object> placeHolders, Notification notification)
        {
            IDictionary<string, object> emailConfig = await ConfigStoreManager.GetConfigAsync(smtpConfig);

            if (emailConfig is null)
            {
                throw new NotificationException("smtp config is not valid");
            }

            var configProperties = JsonConvert.DeserializeObject<IDictionary<string, object>>(emailConfig[NotificationConstants.Properties].ToString());

            var smtpClient = new SmtpClient(configProperties[NotificationConstants.HostName].ToString());
            smtpClient.Port = Convert.ToInt32(configProperties[NotificationConstants.Port]);
            smtpClient.EnableSsl = Convert.ToBoolean(configProperties["EnableSsl"]);

            if (Convert.ToBoolean(configProperties["UseDefaultCred"]))
            {
                smtpClient.UseDefaultCredentials = true;

            }
            else
            {
                smtpClient.Credentials = new NetworkCredential(configProperties[NotificationConstants.Username].ToString(), configProperties[NotificationConstants.Password].ToString()); ;
            }

            if (notificationConfig.ConfigInputs.TryGetValue(NotificationConstants.Environment, out object env) &&
                env.ToString() == NotificationConstants.Development &&
                configProperties.ContainsKey(NotificationConstants.DirectoryPath))
            {
                smtpClient.PickupDirectoryLocation = configProperties[NotificationConstants.DirectoryPath].ToString();
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;


            }

            EmailInfo emailInfo = JsonConvert.DeserializeObject<EmailInfo>(notification.Properties);
            MailAddress mailAddress = null;

            if (inputs.TryGetValue("From", out object sender))
            {
                mailAddress = new MailAddress(sender.ToString());
            }
            else if (!string.IsNullOrEmpty(emailInfo?.From))
            {
                mailAddress = new MailAddress(emailInfo?.From);

            }
            else if (notificationConfig.ConfigInputs.TryGetValue("SenderEmail", out object senderEmail))
            {
                mailAddress = new MailAddress(senderEmail.ToString());
            }
            else
            {
                throw new NotificationException("Email sender is not defined");
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

            var mailMessage = new MailMessage
            {
                From = mailAddress,
                Subject = subjectMessageBuilder.ToString(),
                Body = messageBuilder.ToString(),
                BodyEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = true,
            };

            if (inputs.TryGetValue(NotificationConstants.Recipients, out object inputRecipients))
            {
                foreach (var recipient in (IEnumerable<string>)inputRecipients)
                {
                    mailMessage.To.Add(recipient);
                }
            }

            if (emailInfo.To != null && emailInfo.To.Any())
            {
                foreach (var recipient in emailInfo.To)
                {
                    mailMessage.To.Add(recipient);
                }
            }

            if (!mailMessage.To.Any())
            {
                throw new NotificationException("Email recipients are not found");
            }

            if (inputs.TryGetValue("cc", out object inputCC))
            {
                foreach (var recipient in (IEnumerable<string>)inputCC)
                {
                    mailMessage.CC.Add(recipient);
                }
            }

            if (emailInfo.CC != null && emailInfo.CC.Any())
            {
                foreach (var recipient in emailInfo.CC)
                {
                    mailMessage.CC.Add(recipient);
                }

            }

            if (inputs.TryGetValue("Files", out object files))
            {
                foreach (var item in (IEnumerable<FileAttachment>)files)
                {
                    mailMessage.Attachments.Add(new Attachment(item.Path,
                        MediaTypeNames.Application.Octet));
                }
            }

            if (emailInfo?.Files != null && emailInfo.Files.Any())
            {
                foreach (var file in emailInfo.Files)
                {
                    byte[] bytes = File.ReadAllBytes(file.Path);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(bytes), file.FileName+"."+file.FileExtension));
                }
            }

            try
            {
                smtpClient.Send(mailMessage);

            }
            catch (Exception ex)
            {
                LoggerManager.Error("Inputs:" + JsonConvert.SerializeObject(inputs) + "-Notification:" + JsonConvert.SerializeObject(notification), ex);

                return new NotificationResult
                {
                    Errors = new List<string> { ex.Message }
                };
            }
            finally
            {
                smtpClient.Dispose();
                mailMessage.Dispose();
            }

            return new NotificationResult
            {
                Success = true,

            };

        }

    }
}
