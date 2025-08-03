namespace MinPlatform.DI.Abstractions
{
    using System;
    using System.Collections.Generic;

    public abstract class InstanceManager<ResolverType>
    {
        protected readonly ResolverType resolverType;
        protected readonly Dictionary<Type, object> Instances;

        public InstanceManager(ResolverType resolverType)
        {
            Instances = new Dictionary<Type, object>();

            this.resolverType = resolverType;
        }

        public abstract T Resolve<T>();

        public T GetInstance<T>()
        {
            var type = typeof(T);

            if (!Instances.ContainsKey(type))
            {
                var instance = Resolve<T>();

                Instances[type] = instance;
            }

            return (T)Instances[type];
        }


    }
}
