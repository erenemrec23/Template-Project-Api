using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Extensions;   // ToDynamic
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace QrAssignment.Persistance.Repositories;

internal static class QueryablePaginationExtensions
{
    public static IQueryable<T> ApplyDynamicFilters<T>(
        this IQueryable<T> query,
        DynamicQueryDto? dynamicFilter,
        GlobalSearchDto? globalSearch)
    {
        if (globalSearch != null && globalSearch.Fields.Any() && !string.IsNullOrWhiteSpace(globalSearch.Value))
        {
            string searchClause = string.Join(" || ", globalSearch.Fields.Select(field => $"{field}.Contains(@0)"));
            query = query.Where(searchClause, globalSearch.Value);
        }

        if (dynamicFilter != null)
            query = query.ToDynamic(dynamicFilter);

        return query;
    }

    public static async Task<Paginate<TDto>> ToPaginateAsync<T, TDto>(
        this IQueryable<T> query,
        PageRequestBaseDto request,
        Expression<Func<T, TDto>> projection,
        CancellationToken cancellationToken = default)
    {
        int totalItemCount = await query.CountAsync(cancellationToken);

        query = query.ApplyDynamicFilters(request.DynamicFilterAndSort, request.GlobalSearch);

        int totalFilteredItemCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip(request.PageIndex * request.PageSize)
            .Take(request.PageSize)
            .Select(projection)
            .ToListAsync(cancellationToken);

        return new Paginate<TDto>
        {
            Index = request.PageIndex,
            PageSize = request.PageSize,
            TotalItemCount = totalItemCount,
            TotalFilteredItemCount = totalFilteredItemCount,
            Items = items
        };
    }

    public static async Task<List<TDto>> ToFilteredListAsync<T, TDto>(
        this IQueryable<T> query,
        PageRequestBaseDto request,
        Expression<Func<T, TDto>> projection,
        CancellationToken cancellationToken = default)
    {
        query = query.ApplyDynamicFilters(request.DynamicFilterAndSort, request.GlobalSearch);

        return await query.Select(projection).ToListAsync(cancellationToken);
    }

    // values.Contains(selector(e)) → SQL IN (...). Null/boş listede DB'ye gitmez.
    public static Task<List<T>> ToListByValuesAsync<T, TValue>(
        this IQueryable<T> query,
        Expression<Func<T, TValue>> selector,
        IReadOnlyCollection<TValue>? values,
        CancellationToken cancellationToken = default)
    {
        if (values is null || values.Count == 0)
            return Task.FromResult(new List<T>());

        var contains = Expression.Call(
            typeof(Enumerable),
            nameof(Enumerable.Contains),
            new[] { typeof(TValue) },
            Expression.Constant(values, typeof(IEnumerable<TValue>)),
            selector.Body);

        var predicate = Expression.Lambda<Func<T, bool>>(contains, selector.Parameters);

        return query.Where(predicate).ToListAsync(cancellationToken);
    }
}