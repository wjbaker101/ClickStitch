using Core.Types;
using Data.Records;
using Data.Repositories.User;
using DotNetLibs.Core.Types;
using TestHelpers.Fakes;

namespace Integration.Tests.Fakes;

public sealed class FakeUserRepository : FakeRepository<UserRecord>, IUserRepository
{
    public FakeUserRepository() : base(null!)
    {
    }

    public Task<UserRecord> GetByRequestUser(RequestUser requestUser, CancellationToken cancellationToken)
    {
        return Task.FromResult(new UserRecord
        {
            Id = requestUser.Id,
            Reference = requestUser.Reference,
            CreatedAt = new DateTime(2014, 12, 03),
            Email = "test@email.com",
            Password = "",
            PasswordSalt = "",
            LastLoginAt = null,
            Permissions = requestUser.Permissions.ConvertAll(x => new PermissionRecord
            {
                Type = (PermissionType)x,
                Name = x.ToString()
            })
        });
    }

    public Task<Result<UserRecord>> GetWithPermissionsByReferenceAsync(Guid userReference, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<UserRecord>>(new UserRecord
        {
            Id = 2338,
            Reference = userReference,
            CreatedAt = new DateTime(2014, 12, 03),
            Email = "test@email.com",
            Password = "",
            PasswordSalt = "",
            LastLoginAt = null,
            Permissions = new List<PermissionRecord>()
        });
    }

    public Task<Result<UserRecord>> GetByReferenceAsync(Guid userReference, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<UserRecord>>(new UserRecord
        {
            Id = 2338,
            Reference = userReference,
            CreatedAt = new DateTime(2014, 12, 03),
            Email = "test@email.com",
            Password = "",
            PasswordSalt = "",
            LastLoginAt = null,
            Permissions = new List<PermissionRecord>()
        });
    }

    public Task<Result<UserRecord>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<UserRecord>>(new UserRecord
        {
            Id = 2338,
            Reference = Guid.Parse("81c2d201-13be-4573-aab9-a75a7d2620bd"),
            CreatedAt = new DateTime(2014, 12, 03),
            Email = email,
            Password = "",
            PasswordSalt = "",
            LastLoginAt = null,
            Permissions = new List<PermissionRecord>()
        });
    }
}