namespace MinPlatform.Notifications.Service.Data
{
    using MinPlatform.Data.Service;
    using MinPlatform.Data.Service.Models;
    using MinPlatform.Notifications.Abstractions;
    using MinPlatform.Notifications.Abstractions.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class NotificationDataService : INotificationDataService
    {
        private readonly DataManager dataManager;

        public NotificationDataService(DataManager dataManager)
        {
            this.dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(string notificationName)
        {
            var notificationEntities = await dataManager.SearchEntitiesAsync(GenerateNotificationQuery(notificationName, null));

            return notificationEntities.Select(notificationEntity=>ToNotification(notificationEntity));
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(string notificationName, string languageCode)
        {
            var notificationEntities = await dataManager.SearchEntitiesAsync(GenerateNotificationQuery(notificationName, languageCode));

            return notificationEntities.Select(notificationEntity => ToNotification(notificationEntity));
        }

        private static QueryData GenerateNotificationQuery(string name, string languageCode)
        {
            QueryData notificationBaseQuery = new QueryData
            {
                Columns = new List<string>
                {
                    "Name",
                    "NotificationTemplate.Subject",
                    "NotificationTemplate.MessageContent",
                    "NotificationTemplate.NotificationType",
                    "NotificationTemplate.LanguageCode",
                    "NotificationTemplate.IsActive",
                    "NotificationTemplate.Properties",
                },

                Entity = Constants.Notification,
                JoinEntity = new List<Join>
                {
                    new Join
                    {
                        JoinType=JoinType.Inner,
                        SourceEntity=new JoinEntityInfo
                        {
                            Name="mainTable",
                            ColumnName="Id"
                        },
                        TargetEntity=new JoinEntityInfo
                        {
                            Name=Constants.NotificationTemplate,
                            ColumnName=Constants.NotificationId,
                        }
                    }
                },
                Conditions = new List<ConditionGroup>
                {
                    new ConditionGroup
                    {
                        Condition=new List<Condition>
                        {
                            new Condition
                            {
                                Property=Constants.Name,
                                ConditionalOperator=ConditionalOperator.Equal,
                                Value=name
                            }
                        }
                    }
                },
            };

            if(!string.IsNullOrEmpty(languageCode))
            {
                notificationBaseQuery.Conditions[0].Condition.Add(new Condition
                {
                    Property = Constants.LanguageCode,
                    ConditionalOperator = ConditionalOperator.Equal,
                    Value = languageCode
                });

                notificationBaseQuery.Conditions[0].Operator=LogicalOperator.And;

            }

            return notificationBaseQuery;
        }

        private static Notification ToNotification(IDictionary<string, object> notificationEntity)
        {
            return new Notification
            {
                Id = Convert.ToInt32(notificationEntity["Id"]),
                Name = notificationEntity["Name"].ToString(),
                Subject = notificationEntity["NotificationTemplate_Subject"].ToString(),
                MessageContent = notificationEntity["NotificationTemplate_MessageContent"].ToString(),
                IsActive = Convert.ToBoolean(notificationEntity["NotificationTemplate_IsActive"]),
                LanguageCode = notificationEntity["NotificationTemplate_LanguageCode"].ToString(),
                NotificationType = (NotificationTypes)Enum.Parse(typeof(NotificationTypes), notificationEntity["NotificationTemplate_NotificationType"].ToString()),
                Properties= notificationEntity["NotificationTemplate_Properties"].ToString(),

            };
        }


    }
}
