namespace MinPlatform.Schema.Builder
{
    public abstract class BaseSchemaMigrator
    {
        public abstract string Name
        {
            get;
        }

        public virtual void Up(BaseSchemaBuilder schemaBuilder) { }
        public virtual void Down(BaseSchemaBuilder schemaBuilder) { }

    }
}
