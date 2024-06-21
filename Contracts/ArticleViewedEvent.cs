namespace Contracts;

public record ArticleViewedEvent(Guid Id, DateTime ViewedOnUtc);