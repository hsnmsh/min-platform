#region License
//
// Copyright (c) 2007-2024, Fluent Migrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

#if NETFRAMEWORK

namespace MinPlatform.Schema.Migrators.Runner.Infrastructure.Hosts
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    internal class NetFrameworkHost : IHostAbstraction
    {
        public string BaseDirectory
            => AppDomain.CurrentDomain.BaseDirectory;

        public object CreateInstance(Func<Type, object> typeResolver, string assemblyName, string typeName)
        {
            if (serviceProvider != null)
            {
                try
                {
                    var asm = AppDomain.CurrentDomain.Load(new AssemblyName(assemblyName));
                    var type = asm.GetType(typeName, true);
                    var result = typeResolver;
                    if (result != null)
                        return result;
                }
                catch
                {
                    // Ignore, fall back to legacy method
                }
            }

            return AppDomain.CurrentDomain.CreateInstanceAndUnwrap(assemblyName, typeName);
        }

        public IEnumerable<Assembly> GetLoadedAssemblies()
            => AppDomain.CurrentDomain.GetAssemblies();
    }
}
#endif
