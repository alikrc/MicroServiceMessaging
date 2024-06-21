using Contracts;
using MassTransit;
using Newsletter.Reporting.Api.Database;
using Newsletter.Reporting.Api.Entities;

namespace Newsletter.Reporting.Api.Features.Articles;

public sealed class ArticleCreatedConsumer : IConsumer<ArticleCreatedEvent>
{
    private readonly ApplicationDbContext _dbContext;

    public ArticleCreatedConsumer(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ArticleCreatedEvent> context)
    {
        var article = new Article
        {
            Id = context.Message.Id,
            CreatedOnUtc = context.Message.CreatedOnUtc,
        };

        _dbContext.Add(article);

        await _dbContext.SaveChangesAsync();
    }
}