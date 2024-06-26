﻿namespace ClickStitch.Api.Auth.LogIn.Types;

public sealed class LogInRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public sealed class LogInResponse
{
    public required Guid Reference { get; init; }
    public required string LoginToken { get; init; }
    public required string Email { get; init; }
    public required List<PermissionModel> Permissions { get; init; }
}