namespace MinPlatform.Notifications.Service.Validations
{
    using MinPlatform.Notifications.Abstractions;
    using MinPlatform.Notifications.Abstractions.Data;
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Extensions;
    using MinPlatform.Validators.Service.ValidationExecutor;
    using System.Collections.Generic;

    public sealed class NotificationInputsValidatorExecutor : AbstractValidatorExecutor<NotificationInputs>
    {
        public NotificationInputsValidatorExecutor(NotificationInputs model) : base(model)
        {
            CreateDefinition(new CustomDefinition<IDictionary<string, object>>()
            {
                Key = nameof(model.Inputs),
                FunctionErrors = new Dictionary<string, string>()
                {
                    {"InputsError", "Inputs must not be null" },

                }
            }).Function((model) =>
            {
                var errorKeys = new List<string>();

                if (model is null)
                {
                    errorKeys.Add("InputsError");
                }

                return errorKeys;
            });
            CreateDefinition(new CustomDefinition<IDictionary<string, object>>()
            {
                Key = nameof(model.PlaceHolders),
                FunctionErrors = new Dictionary<string, string>()
                {
                    {"PlaceHoldersError", "PlaceHolders must not be null" },

                }
            }).Function((model) =>
            {
                var errorKeys = new List<string>();

                if (model is null)
                {
                    errorKeys.Add("PlaceHoldersError");
                }

                return errorKeys;
            });
            CreateDefinition(new CustomDefinition<Notification>()
            {
                Key = nameof(model.Notification),
                FunctionErrors = new Dictionary<string, string>()
                {
                    {"NotificationError", "Notification must not be null" },
                    {"NameError", "Notification Name must not be null" },
                    {"SubjectError", "Notification Subject must not be null" },
                    {"MessageContentError", "Notification Message Content must not be null" },
                    {"PropertiesError", "Notification Properties must not be null" },

                }
            }).Function((model) =>
            {
                var errorKeys = new List<string>();

                if (model is null)
                {
                    errorKeys.Add("NotificationError");
                }

                if (string.IsNullOrEmpty(model?.Name))
                {
                    errorKeys.Add("NameError");
                }

                if (string.IsNullOrEmpty(model?.Subject))
                {
                    errorKeys.Add("SubjectError");
                }

                if (string.IsNullOrEmpty(model?.MessageContent))
                {
                    errorKeys.Add("MessageContentError");
                }

                if (string.IsNullOrEmpty(model?.Properties))
                {
                    errorKeys.Add("PropertiesError");
                }

                return errorKeys;

            });

        }
    }
}
