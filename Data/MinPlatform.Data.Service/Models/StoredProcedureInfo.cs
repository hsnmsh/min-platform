namespace MinPlatform.Data.Service.Models
{
    using System.Collections.Generic;
    using System.Data;


    public sealed class StoredProcedureInfo
    {
        public string SPName 
        { 
            get; 
            set; 
        }

        public IDictionary<string, SPParameter> Parameters 
        {
            get;
            set; 
        }

        public ReturnType ReturnType
        {
            get;
            set;
        }

    }

    public enum ReturnType 
    { 
        None,
        SingleResult,
        MultipleResult,
        ScalarValue
    }

    public sealed class SPParameter
    {
       
        public object Value 
        { 
            get;
            set;
        }

        public DbType DbType 
        { 
            get;
            set;
        }

        public ParameterDirection ParameterDirection 
        { 
            get;
            set; 
        }

        public int? Size
        {
            get;
            set;
        }

    }

}
