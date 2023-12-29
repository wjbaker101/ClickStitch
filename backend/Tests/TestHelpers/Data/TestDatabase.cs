﻿using Data;
using Data.Types;
using NHibernate;

namespace TestHelpers.Data;

public sealed class TestDatabase : IDatabase
{
    public List<IDatabaseRecord> Records { get; set; } = new();

    public ISessionFactory SessionFactory { get; }

    public IApiSession OpenSession()
    {
        return new TestApiSession(Records);
    }
}

public sealed class TestApiSession : IApiSession
{
    private readonly List<IDatabaseRecord> _records;

    public TestApiSession(List<IDatabaseRecord> records)
    {
        _records = records;
    }

    public Task<IApiTransaction> BeginTransaction(CancellationToken cancellationToken)
    {
        return Task.FromResult<IApiTransaction>(new TestApiTransaction());
    }

    public IQueryable<TRecord> Query<TRecord>() where TRecord : IDatabaseRecord
    {
        return new TestQueryable<TRecord>(_records.OfType<TRecord>().AsQueryable());
    }

    public void Dispose()
    {
    }
}

public sealed class TestApiTransaction : IApiTransaction
{
    public Task Commit(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}