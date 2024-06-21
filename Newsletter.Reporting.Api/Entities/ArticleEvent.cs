namespace Newsletter.Reporting.Api.Entities;

public record ArticleEvent(Guid Id, Guid ArticleId, ArticleEventType EventType, DateTime CreatedOnUtc);