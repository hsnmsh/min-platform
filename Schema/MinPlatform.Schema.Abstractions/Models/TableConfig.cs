namespace MinPlatform.Schema.Abstractions.Models
{
    using System.Collections.Generic;

    public sealed class TableConfig
    {
        public string Schema 
        { 
            get; 
            set;
        }

        public string Name 
        { 
            get;
            set;
        }

        public string Alias
        {
            get;
            set;
        }

        public IList<Column> Columns
        {
            get;
            set;
        }

        public IList<LinkedEntity> LinkedEntities
        {
            get;
            set;
        }

        public IList<Trigger> Triggers
        {
            get;
            set;
        }
    }
}
