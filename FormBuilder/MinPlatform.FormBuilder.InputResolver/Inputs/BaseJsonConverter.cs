namespace MinPlatform.FormBuilder.Elements.Inputs
{
    using MinPlatform.FormBuilder.Elements.Inputs.Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    using System;

    public sealed class BaseJsonConverter : CustomCreationConverter<BasePropertyInput>
    {
        private string inputType;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            var jobj = JToken.ReadFrom(reader);
            var t = jobj.Value<string>("Type");
            inputType = t.ToLower();

            return base.ReadJson(jobj.CreateReader(), objectType, existingValue, serializer);

        }
        public override BasePropertyInput Create(Type objectType)
        {
            switch (inputType)
            {
                case "text":
                    return new TextPropertyInput();
                case "number":
                    return new NumberPropertyInput();
                case "datetime":
                    return new DateTimePropertyInput();
                case "date":
                    return new DatePropertyInput();
                case "email":
                    return new EmailPropertyInput();
                case "telephone":
                    return new TelephonePropertyInput();
                case "textarea":
                    return new TextAreaPropertyInput();
                case "slider":
                    return new SliderPropertyInput();
                case "toggle":
                    return new TogglePropertyInput();
                case "checkbox":
                    return new CheckBoxPropertyInput();
                case "radio":
                    return new RadioPropertyInput();
                case "dropdownlist":
                    return new DropDownListPropertyInput();
                case "file":
                    return new FileUploaderPropertyInput();
                case "listview":
                    return new ListViewPropertyInput();
                default:
                    throw new InvalidOperationException("Target type is not found");
            }

        }


    }


}
