using ClickStitch.Api.Admin.Types;
using Data.Repositories.Admin;
using Data.Repositories.Admin.Types;

namespace ClickStitch.Api.Admin;

public interface IAdminService
{
    Task<Result<GetUsersResponse>> GetUsers(int pageNumber, int pageSize, CancellationToken cancellationToken);
}

public sealed class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;

    public AdminService(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    public async Task<Result<GetUsersResponse>> GetUsers(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var getUsers = await _adminRepository.GetUsers(new GetUsersParameters
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        }, cancellationToken);

        return new GetUsersResponse
        {
            Users = getUsers.Users.ConvertAll(UserMapper.Map),
            Pagination = PaginationModel.Create(pageNumber, pageSize, getUsers.TotalCount)
        };
    }
}