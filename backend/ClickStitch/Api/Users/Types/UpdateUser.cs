﻿using ClickStitch.Models;

namespace ClickStitch.Api.Users.Types;

public sealed class UpdateUserRequest
{
    public required string Username { get; init; }
}

public sealed class UpdateUserResponse
{
    public required UserModel User { get; init; }
}