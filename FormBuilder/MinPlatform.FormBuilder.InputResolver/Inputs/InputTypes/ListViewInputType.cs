namespace MinPlatform.FormBuilder.Elements.Inputs.InputTypes
{
    using MinPlatform.FormBuilder.Elements.Inputs.Models;
    using System.Collections.Generic;

    public sealed class ListViewInputType : BaseInputType
    {
        public int MinRows
        {
            get;
            set;
        }

        public int MaxRows
        {
            get;
            set;
        }

        public IDictionary<ActionButton, ActionButtonProperty> ActionProperties
        {
            get;
            set;
        }

        public IEnumerable<BaseInputType> PropertyInputs
        {
            get;
            set;
        }
    }

    public class ActionButtonProperty
    {
        public bool Visible
        {
            get;
            set;
        }

        public bool Disabled
        {
            get;
            set;
        }
    }
}
