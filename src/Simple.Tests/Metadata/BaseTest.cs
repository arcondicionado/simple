﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Simple.Migrator;
using Simple.Metadata;
using Simple.Migrator.Fluent;
using Simple.Migrator.Providers;

namespace Simple.Tests.Metadata
{
    public abstract class BaseTest
    {
        public DatabasesXml.Entry Database { get; protected set; }
        public BaseTest(DatabasesXml.Entry entry)
        {
            Database = entry;
        }

        public abstract IEnumerable<Type> GetMigrations();
        public abstract IEnumerable<TableAddAction> GetTableDefinitions();

        protected TableAddAction TableDef(string name, Action<TableAddAction> definition)
        {
            var table = new TableAddAction(null, name);
            definition(table);
            return table;
        }

        protected DbMigrator GetMigrator()
        {
            return new DbMigrator(Database.Provider, Database.ConnectionString, GetMigrations().ToArray());
        }
        protected DbSchema GetSchema()
        {
            return new DbSchema(Database.Provider, Database.ConnectionString);
        }

        protected Dialect GetDialect()
        {
            return ProviderFactory.GetDialect(Database.Provider);
        }

        public virtual void Check()
        {
            var schema = GetSchema();
            var dialect = GetDialect();
            new TableAssertionHelper(schema, dialect).AssertTables(GetTableDefinitions());
        }


        public virtual void Setup()
        {
            GetMigrator().MigrateToLastVersion("SchemaInfo");
        }

        public virtual void ExecuteAll()
        {
            try
            {
                Setup();
                Check();
            }
            finally
            {
                Teardown();
            }
        }

        public virtual void Teardown()
        {
            GetMigrator().MigrateTo(0, "SchemaInfo");
        }

    }
}
