using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newsletter.Reporting.Api.Database;
using Shared;

namespace Newsletter.Reporting.Api.Features.Articles;

public static class GetArticle
{
    public class Query : IRequest<Result<ArticleResponse>>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<ArticleResponse>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<ArticleResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var articleResponse = await _dbContext
                .Articles
                .AsNoTracking()
                .Where(a => a.Id == request.Id)
                .Select(a => new ArticleResponse(a.Id, a.CreatedOnUtc, a.PublishedOnUtc, _dbContext
                        .ArticleEvents
                        .Where(articleEvent => articleEvent.ArticleId == a.Id)
                        .Select(articleEvent => new ArticleEventResponse(articleEvent.Id,
                                                                         articleEvent.CreatedOnUtc,
                                                                         articleEvent.EventType)).ToList()))
                        .FirstOrDefaultAsync(cancellationToken);

            if (articleResponse is null)
            {
                return Result.Failure<ArticleResponse>(new Error(
                    "GetArticle.Null",
                    "The article with the specified ID was not found"));
            }

            return articleResponse;
        }
    }
}

public class GetArticleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/articles/{id}", async (Guid id, ISender sender) =>
        {
            var query = new GetArticle.Query { Id = id };

            var result = await sender.Send(query);

            if (result.IsFailure)
            {
                return Results.NotFound(result.Error);
            }

            return Results.Ok(result.Value);
        });
    }
}