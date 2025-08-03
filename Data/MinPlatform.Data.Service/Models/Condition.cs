namespace MinPlatform.Data.Service.Models
{
    public class Condition
    {
        public string Property 
        { 
            get;
            set; 
        }

        public ConditionalOperator ConditionalOperator 
        { 
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }
    }
}
