using MinPlatform.Data.Service.DataAttributes;

namespace MinPlatform.Data.Service
{
    public enum OrderOperator
    {
        [OrderBy("ASC")]
        Ascending,
        [OrderBy("DESC")]
        Descending,
    }
}
