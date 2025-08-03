#region License
// Copyright (c) 2007-2024, Fluent Migrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

namespace MinPlatform.Schema.Migrators.Runner.Processors
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using MinPlatform.Schema.Migrators.Runner.Infrastructure;
    using JetBrains.Annotations;
    using System.Data.Common;

    public class ReflectionBasedDbFactory : DbFactoryBase
    {
        private readonly Func<Type, object> typeResolver;
        private readonly TestEntry[] testEntries;

        private DbProviderFactory _instance;

        [Obsolete]
        public ReflectionBasedDbFactory(string assemblyName, string dbProviderFactoryTypeName)
            : this(new TestEntry(assemblyName, dbProviderFactoryTypeName))
        {
        }

        [Obsolete]
        protected ReflectionBasedDbFactory(params TestEntry[] testEntries)
        {
            if (testEntries.Length == 0)
            {
                throw new ArgumentException(@"At least one test entry must be specified", nameof(testEntries));
            }

            this.testEntries = testEntries;
        }

        protected ReflectionBasedDbFactory(Func<Type, object> typeResolver, params TestEntry[] testEntries)
        {
            if (testEntries.Length == 0)
            {
                throw new ArgumentException(@"At least one test entry must be specified", nameof(testEntries));
            }

            this.typeResolver = typeResolver;
            this.testEntries = testEntries;
        }

        protected override DbProviderFactory CreateFactory()
        {
            if (_instance != null)
            {
                return _instance;
            }

            var exceptions = new List<Exception>();
            if (TryCreateFactory(this.typeResolver, testEntries, exceptions, out var factory))
            {
                _instance = factory;
                return factory;
            }

            var assemblyNames = string.Join(", ", testEntries.Select(x => x.AssemblyName));
            var fullExceptionOutput = string.Join(Environment.NewLine, exceptions.Select(x => x.ToString()));

            throw new AggregateException($"Unable to load the driver. Attempted to load: {assemblyNames}, with {fullExceptionOutput}", exceptions);
        }

        protected static bool TryCreateFactory(
             Func<Type, object> typeResolver,
            [NotNull, ItemNotNull] IEnumerable<TestEntry> entries,
            [NotNull, ItemNotNull] ICollection<Exception> exceptions,
            out DbProviderFactory factory)
        {
            var entriesCollection = entries.ToList();

            foreach (var entry in entriesCollection)
            {
                if (TryCreateFromCurrentDomain(entry, exceptions, out factory))
                {
                    return true;
                }
            }

            foreach (var entry in entriesCollection)
            {
                if (TryCreateFactoryFromRuntimeHost(entry, exceptions, typeResolver, out factory))
                {
                    return true;
                }
            }

            foreach (var entry in entriesCollection)
            {
                if (TryCreateFromAppDomainPaths(entry, exceptions, out factory))
                {
                    return true;
                }
            }

            foreach (var entry in entriesCollection)
            {
                if (TryCreateFromGac(entry, exceptions, out factory))
                {
                    return true;
                }
            }

            factory = null;
            return false;
        }

        protected static bool TryCreateFromAppDomainPaths(
            [NotNull] TestEntry entry,
            [NotNull, ItemNotNull] ICollection<Exception> exceptions,
            out DbProviderFactory factory)
        {
            if (TryLoadAssemblyFromAppDomainDirectories(entry.AssemblyName, exceptions, out var assembly))
            {
                try
                {
                    var type = assembly.GetType(entry.DBProviderFactoryTypeName, true);
                    if (TryGetInstance(type, out factory))
                    {
                        return true;
                    }

                    factory = (DbProviderFactory) Activator.CreateInstance(type);
                    return true;
                }
                catch (Exception ex)
                {
                    // Ignore
                    exceptions.Add(ex);
                }
            }

            factory = null;
            return false;
        }

        protected static bool TryCreateFactoryFromRuntimeHost(
            [NotNull] TestEntry entry,
            [NotNull, ItemNotNull] ICollection<Exception> exceptions,
            Func<Type, object> typeResolver,
            out DbProviderFactory factory)
        {
            try
            {
                factory = (DbProviderFactory)RuntimeHost.Current.CreateInstance(
                    typeResolver,
                    entry.AssemblyName,
                    entry.DBProviderFactoryTypeName);
                return true;
            }
            catch (Exception ex)
            {
                // Ignore, check if we could load the assembly
                exceptions.Add(ex);
            }

            // Try to create from current domain in case of a successfully loaded assembly
            return TryCreateFromCurrentDomain(entry, exceptions, out factory);
        }

        protected static bool TryLoadAssemblyFromAppDomainDirectories(
            [NotNull] string assemblyName,
            [NotNull, ItemNotNull] ICollection<Exception> exceptions,
            out Assembly assembly)
        {
            return TryLoadAssemblyFromDirectories(
                GetPathsFromAppDomain(),
                assemblyName,
                exceptions,
                out assembly);
        }

        protected static bool TryLoadAssemblyFromDirectories(
            [NotNull, ItemNotNull] IEnumerable<string> directories,
            [NotNull] string assemblyName,
            [NotNull, ItemNotNull] ICollection<Exception> exceptions,
            out Assembly assembly)
        {
            var alreadyTested = new HashSet<string>(StringComparer.InvariantCulture);
            var assemblyFileName = $"{assemblyName}.dll";
            foreach (var directory in directories)
            {
                var path = Path.Combine(directory, assemblyFileName);
                if (!alreadyTested.Add(path))
                {
                    continue;
                }

                try
                {
                    assembly = Assembly.LoadFile(path);
                    return true;
                }
                catch (Exception ex)
                {
                    exceptions.Add(new Exception($"Failed to load file {path}", ex));
                }
            }

            assembly = null;
            return false;
        }

        private static bool TryCreateFromGac(
            [NotNull] TestEntry entry,
            [NotNull, ItemNotNull] ICollection<Exception> exceptions,
            out DbProviderFactory factory)
        {
            var asmNames = FindAssembliesInGac(entry.AssemblyName);
            var asmName = asmNames.OrderByDescending(n => n.Version).FirstOrDefault();

            if (asmName == null)
            {
                factory = null;
                return false;
            }

            try
            {
                var assembly = Assembly.Load(asmName);
                var type = assembly.GetType(entry.DBProviderFactoryTypeName, true);
                if (TryGetInstance(type, out factory))
                {
                    return true;
                }

                factory = (DbProviderFactory) Activator.CreateInstance(type);
                return true;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
                factory = null;
                return false;
            }
        }

        private static bool TryCreateFromCurrentDomain(
            [NotNull] TestEntry entry,
            [NotNull, ItemNotNull] ICollection<Exception> exceptions,
            out DbProviderFactory factory)
        {
            if (TryLoadAssemblyFromCurrentDomain(entry.AssemblyName, exceptions, out var assembly))
            {
                try
                {
                    var type = assembly.GetType(entry.DBProviderFactoryTypeName, true);
                    if (TryGetInstance(type, out factory))
                    {
                        return true;
                    }

                    factory = (DbProviderFactory) Activator.CreateInstance(type);
                    return true;
                }
                catch (Exception ex)
                {
                    // Ignore
                    exceptions.Add(ex);
                }
            }

            factory = null;
            return false;
        }

        private static bool TryLoadAssemblyFromCurrentDomain(
            [NotNull] string assemblyName,
            [NotNull, ItemNotNull] ICollection<Exception> exceptions,
            out Assembly assembly)
        {
            try
            {
                assembly = AppDomain.CurrentDomain.Load(new AssemblyName(assemblyName));
                return true;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
                assembly = null;
                return false;
            }
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<AssemblyName> FindAssembliesInGac([NotNull, ItemNotNull] params string[] names)
        {
            foreach (var name in names)
            {
                foreach (var assemblyName in RuntimeHost.FindAssemblies(name))
                {
                    yield return assemblyName;
                }
            }
        }

        private static bool TryGetInstance(
            [NotNull] Type factoryType,
            out DbProviderFactory factory)
        {
            var instanceField = factoryType.GetField(
                "Instance",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);

            if (instanceField != null && TryCastInstance(instanceField.GetValue(null), out factory))
            {
                return true;
            }

            var instanceProperty = factoryType.GetProperty(
                "Instance",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);
            if (instanceProperty != null && TryCastInstance(instanceProperty.GetValue(null, null), out factory))
            {
                return true;
            }

            factory = null;
            return false;
        }

        private static bool TryCastInstance(
            [NotNull] object value,
            out DbProviderFactory factory)
        {
            factory = value as DbProviderFactory;
            return factory != null;
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<string> GetPathsFromAppDomain()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                string assemblyDirectory;
                try
                {
                    assemblyDirectory = Path.GetDirectoryName(assembly.Location);
                    if (assemblyDirectory == null)
                    {
                        continue;
                    }
                }
                catch
                {
                    // Ignore error caused by dynamic assembly
                    continue;
                }

                yield return assemblyDirectory;
            }
        }

        protected class TestEntry
        {
            public TestEntry(
                [NotNull] string assemblyName,
                [NotNull] string dbProviderFactoryTypeName)
            {
                AssemblyName = assemblyName;
                DBProviderFactoryTypeName = dbProviderFactoryTypeName;
            }

            [NotNull]
            public string AssemblyName { get; }

            [NotNull]
            public string DBProviderFactoryTypeName { get; }
        }
    }
}
