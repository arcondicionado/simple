using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Example.Project.Config;
using Simple;
using Simple.IO;
using Example.Project.Domain;
using Simple.Migrator;
using Simple.Generator;
using System.IO;
using Example.Project.Database;

namespace Example.Project.Tools.Database
{
    public class MigrateTool : ICommand
    {
        public long? Version { get; set; }
        public string FilePath { get; set; }
        public bool DryRun { get; set; }
        public string Environment { get; set; }

        #region ICommand Members

        protected bool Is(string env)
        {
            return new Configurator(Environment).Is(env);
        }

        public void Execute()
        {
            
            if (!Configurator.IsProduction)
            {
                if (Is(Configurator.Development))
                    using (Context.Development)
                        Migrate(Version);

                if (Is(Configurator.Test))
                    using (Context.Test)
                        Migrate(Version);
            }
            else
            {
                Migrate(Version);
            }

        }

        private void Migrate(long? version)
        {
            Action<string> action = null;
            var builder = new StringBuilder();
            if (FilePath != null || DryRun) 
               action = x=>builder.AppendLine(x);


            var config = Simply.Do.GetConfig<ApplicationConfig>();

            var options = new MigratorOptions(config.ADOProvider, Simply.Do.GetConnectionString())
                .FromAssembly(typeof(DataLists).Assembly)
                .WithSchemaTable(config.SchemaInfoTable)
                .WriteWith(action);

            new DbMigrator(options).Migrate(version);

            if (FilePath != null)
                File.WriteAllText(FilePath, builder.ToString());
        }
       

        #endregion
    }
}
