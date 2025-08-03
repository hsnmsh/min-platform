namespace MinPlatform.Data.Service.SqlStatmentBuilder
{
    public sealed class SqlStatmentBuilderCommands
    {
        public ICreateCommandGenerator CreateCommandGenerator
        {
            get;
            set;
        }

        public IUpdateCommandGenerator UpdateCommandGenerator
        {
            get;
            set;
        }

        public IDeleteCommandGenerator DeleteCommandGenerator
        {
            get;
            set;
        }
    }
}
