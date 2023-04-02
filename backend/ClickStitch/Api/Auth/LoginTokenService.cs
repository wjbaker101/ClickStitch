﻿using ClickStitch.Models;
using Core.Types;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClickStitch.Api.Auth;

public interface ILoginTokenService
{
    string Create(UserModel user);
    Result<Guid> GetUserReferenceByToken(string loginToken);
}

public sealed class LoginTokenService : ILoginTokenService
{
    private const string SECRET_KEY = "ac8763b1-eeac-4965-8a60-8e0ccb5f9a3d";
    private const string CLAIM_USER_REFERENCE = "userReference";

    public string Create(UserModel user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(CLAIM_USER_REFERENCE, user.Reference.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY)), SecurityAlgorithms.HmacSha512Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public Result<Guid> GetUserReferenceByToken(string loginToken)
    {
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY))
        };

        try
        {
            var principal = new JwtSecurityTokenHandler().ValidateToken(loginToken, parameters, out var _);

            var userReference = principal.Claims.Single(x => x.Type == CLAIM_USER_REFERENCE).Value;

            return Guid.Parse(userReference);
        }
        catch (Exception)
        {
            return Result<Guid>.Failure("Unable to parse login token.");
        }
    }
}