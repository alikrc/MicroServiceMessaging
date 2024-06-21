using Carter;
using Contracts;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Contracts;
using Newsletter.Api.Database;
using Shared;

namespace Newsletter.Api.Features.Articles;

public class GetArticle
{
    public class Query : IRequest<Result<ArticleResponse>>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<ArticleResponse>>
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly IPublishEndpoint _publishEndpoint;

        public Handler(ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Result<ArticleResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var articleResponse = await _dbContext
                .Articles
                .AsNoTracking()
                .Where(a => a.Id == request.Id)
                .Select(a => new ArticleResponse
                {
                    Id = a.Id,
                    Title = a.Title,
                    Content = a.Content,
                    Tags = a.Tags,
                    CreatedOnUtc = a.CreatedOnUtc,
                    PublishedOnUtc = a.PublishedOnUtc
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (articleResponse is null)
            {
                return Result.Failure<ArticleResponse>(new Error("GetArticle.Null", "The article with the specified ID was not found"));
            }

            await _publishEndpoint.Publish(new ArticleViewedEvent(articleResponse.Id, DateTime.UtcNow), cancellationToken);

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