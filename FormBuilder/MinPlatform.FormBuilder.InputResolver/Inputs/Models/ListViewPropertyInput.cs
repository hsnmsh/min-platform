namespace MinPlatform.FormBuilder.Elements.Inputs.Models
{
    using System.Collections.Generic;

    internal sealed class ListViewPropertyInput : BasePropertyInput
    {
        public override InputType Type { get => InputType.ListView; }

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

        public IEnumerable<BasePropertyInput> PropertyInputs
        {
            get;
            set;
        }
    }

    public enum ActionButton
    {
        Add,
        Update,
        Delete
    }

    public class ActionButtonProperty
    {
        public string Visible
        {
            get;
            set;
        }

        public string Disabled
        {
            get;
            set;
        }
    }
}
