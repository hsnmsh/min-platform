namespace MinPlatform.Data.Abstractions.Models
{
    using System;

    public abstract class AbstractEntity<IdType>
    {
        public abstract IdType Id 
        {
            get;
            set; 
        }

        public DateTime? CreatedOn
        {
            get;
            set;
        }

        public string CreatedBy
        {
            get;
            set;
        }

        public DateTime? ModifiedOn
        {
            get;
            set;
        }

        public string ModifiedBy
        {
            get;
            set;
        }
    }
}
