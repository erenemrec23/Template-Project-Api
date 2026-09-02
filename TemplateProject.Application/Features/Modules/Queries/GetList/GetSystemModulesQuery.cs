using MediatR;
using QrAssignment.Domain.Shared;

public sealed record GetSystemModulesQuery(string? PageKey = null) : IRequest<Result<List<PageCatalogItemDto>>>;