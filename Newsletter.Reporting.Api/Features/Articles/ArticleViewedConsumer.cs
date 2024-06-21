using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newsletter.Reporting.Api.Database;
using Newsletter.Reporting.Api.Entities;

namespace Newsletter.Reporting.Api.Features.Articles;

public sealed class ArticleViewedConsumer : IConsumer<ArticleViewedEvent>
{
    private readonly ApplicationDbContext _dbContext;

    public ArticleViewedConsumer(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ArticleViewedEvent> context)
    {
        var article = await _dbContext.Articles.FirstOrDefaultAsync(a => a.Id == context.Message.Id);

        if (article is null)
        {
            return;
        }

        var articleEvent = new ArticleEvent(Guid.NewGuid(), article.Id, ArticleEventType.View, context.Message.ViewedOnUtc);

        _dbContext.Add(articleEvent);

        await _dbContext.SaveChangesAsync();
    }
}