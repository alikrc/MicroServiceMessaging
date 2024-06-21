using Newsletter.Reporting.Api.Entities;

namespace Newsletter.Reporting.Api.Features.Articles;

public record ArticleEventResponse(Guid Id, DateTime CreatedOnUtc, ArticleEventType ArticleEventType);