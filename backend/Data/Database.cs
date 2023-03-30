using Data.Records;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Data;

public interface IDatabase
{
    ISessionFactory SessionFactory { get; }
}

public sealed class Database : IDatabase
{
    public ISessionFactory SessionFactory { get; }

    public Database()
    {
        SessionFactory = Fluently.Configure()
            .Database(PostgreSQLConfiguration.Standard.ConnectionString(c => c
                .Host("localhost")
                .Port(5432)
                .Database("cross_stitch_viewer")
                .Username("postgres")
                .Password("password")))
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<_Records>())
            .BuildSessionFactory();
    }
}