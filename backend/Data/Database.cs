using Core.Settings;
using Data.Interceptors;
using Data.Types;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Data;

public interface IDatabase
{
    IApiSession OpenSession();
    IApiStatelessSession OpenStatelessSession();
}

public sealed class Database : IDatabase
{
    private readonly ISessionFactory _sessionFactory;

    public Database(AppSecrets secrets)
    {
        var database = secrets.Database;

        _sessionFactory = Fluently.Configure()
            .Database(PostgreSQLConfiguration.Standard.ConnectionString(c => c
                .Host(database.Host)
                .Port(database.Port)
                .Database(database.Database)
                .Username(database.Username)
                .Password(database.Password)))
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<_Records>())
            .BuildSessionFactory();
    }

    public IApiSession OpenSession()
    {
        var sessionBuilder = _sessionFactory.WithOptions();

        #if DEBUG
        sessionBuilder = sessionBuilder.Interceptor(new OutputSqlInterceptor());
        #endif

        return new ApiSession(sessionBuilder.OpenSession());
    }

    public IApiStatelessSession OpenStatelessSession()
    {
        return new ApiStatelessSession(_sessionFactory.OpenStatelessSession());
    }
}