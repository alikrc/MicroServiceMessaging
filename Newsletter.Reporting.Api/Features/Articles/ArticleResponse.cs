namespace Newsletter.Reporting.Api.Features.Articles;

public record ArticleResponse(Guid id, DateTime CreatedOnUtc, DateTime? PublishedOnUtc, List<ArticleEventResponse> Events);