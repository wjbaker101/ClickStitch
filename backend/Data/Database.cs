using Core.Settings;
using Data.Interceptors;
using Data.Types;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Data;

public interface IDatabase
{
    ISessionFactory SessionFactory { get; }
    IApiSession OpenSession(bool shouldOutputSql = true);
}

public sealed class Database : IDatabase
{
    public ISessionFactory SessionFactory { get; }

    public Database(AppSecrets secrets)
    {
        var database = secrets.Database;

        SessionFactory = Fluently.Configure()
            .Database(PostgreSQLConfiguration.Standard.ConnectionString(c => c
                .Host(database.Host)
                .Port(database.Port)
                .Database(database.Database)
                .Username(database.Username)
                .Password(database.Password)))
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<_Records>())
            .BuildSessionFactory();
    }

    public IApiSession OpenSession(bool shouldOutputSql = true)
    {
        var sessionBuilder = SessionFactory.WithOptions();

        #if DEBUG
        if (shouldOutputSql)
            sessionBuilder = sessionBuilder.Interceptor(new OutputSqlInterceptor());
        #endif

        return new ApiSession(sessionBuilder.OpenSession());
    }
}