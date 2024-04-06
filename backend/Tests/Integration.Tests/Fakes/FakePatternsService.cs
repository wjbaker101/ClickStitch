using ClickStitch.Api.Patterns;
using ClickStitch.Api.Patterns.DeletePattern.Types;
using ClickStitch.Api.Patterns.SearchPatterns.Types;
using ClickStitch.Api.Patterns.Types;
using ClickStitch.Api.Patterns.UpdatePattern.Types;
using ClickStitch.Api.Patterns.VerifyPattern.Types;
using ClickStitch.Models;
using Core.Types;
using DotNetLibs.Core.Types;
using Microsoft.AspNetCore.Http;

namespace Integration.Tests.Fakes;

public sealed class FakePatternsService : IPatternsService
{
    public async Task<Result<SearchPatternsResponse>> GetPatterns(RequestUser? requestUser, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        PatternModel pattern;
        if (requestUser == null)
        {
            pattern = new PatternModel
            {
                Reference = Guid.Parse("c3245143-c616-4b42-87dc-0ba20c52e4e2"),
                CreatedAt = new DateTime(2021, 04, 19, 15, 58, 03),
                Title = "TestTitle",
                Width = 3821,
                Height = 8704,
                Price = 4386,
                ThumbnailUrl = "TestThumbnailUrl",
                ThreadCount = 7880,
                StitchCount = 2731,
                BannerImageUrl = "TestBannerImageUrl",
                ExternalShopUrl = "TestExternalShopUrl",
                TitleSlug = "test-title-slug",
                AidaCount = 1335,
                Creator = null,
                User = new UserModel
                {
                    Reference = default,
                    CreatedAt = default,
                    Email = null!,
                    LastLoginAt = null
                }
            };
        }
        else
        {
            pattern = new PatternModel
            {
                Reference = Guid.Parse("2544630d-9502-4cd1-a777-86260451d138"),
                CreatedAt = new DateTime(2021, 04, 19, 15, 58, 03),
                Title = "TestTitle",
                Width = 3821,
                Height = 8704,
                Price = 4386,
                ThumbnailUrl = "TestThumbnailUrl",
                ThreadCount = 7880,
                StitchCount = 2731,
                BannerImageUrl = "TestBannerImageUrl",
                ExternalShopUrl = "TestExternalShopUrl",
                TitleSlug = "test-title-slug",
                AidaCount = 1335,
                Creator = null,
                User = new UserModel
                {
                    Reference = default,
                    CreatedAt = default,
                    Email = null!,
                    LastLoginAt = null
                }
            };
        }

        return new SearchPatternsResponse
        {
            Patterns = new List<PatternModel> { pattern }
        };
    }

    public Task<Result<UpdatePatternResponse>> UpdatePattern(RequestUser requestUser, Guid patternReference, UpdatePatternRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<UpdatePatternResponse>>(new UpdatePatternResponse
        {
            Pattern = new PatternModel
            {
                Reference = Guid.Parse("9b67266b-a1fa-4fc5-aecc-eb8fafc65433"),
                CreatedAt = new DateTime(2021, 04, 19, 15, 58, 03),
                Title = "TestTitle",
                Width = 3821,
                Height = 8704,
                Price = 4386,
                ThumbnailUrl = "TestThumbnailUrl",
                ThreadCount = 7880,
                StitchCount = 2731,
                BannerImageUrl = "TestBannerImageUrl",
                ExternalShopUrl = "TestExternalShopUrl",
                TitleSlug = "test-title-slug",
                AidaCount = 1335,
                Creator = null,
                User = new UserModel
                {
                    Reference = default,
                    CreatedAt = default,
                    Email = null!,
                    LastLoginAt = null
                }
            }
        });
    }

    public Task<Result> CreatePattern(
        RequestUser requestUser,
        CreatePatternRequest request,
        string patternData,
        IFormFile thumbnail,
        IFormFile? bannerImage,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<Result>(Result.Success());
    }

    public Result<VerifyPatternResponse> VerifyPattern(string patternData, CancellationToken cancellationToken)
    {
        return new VerifyPatternResponse();
    }

    public Task<Result<DeletePatternResponse>> DeletePattern(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<DeletePatternResponse>>(new DeletePatternResponse
        {
            Message = "TestMessage"
        });
    }
}