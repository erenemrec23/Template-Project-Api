using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Domain.Abstractions;   // IBaseEntity
using QrAssignment.Persistance.Context;
using System.Linq.Expressions;

namespace QrAssignment.Persistance.Repositories;
 
internal abstract class GenericAppRepository<TEntity> where TEntity : class, IBaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _set;

    protected GenericAppRepository(AppDbContext context)
    {
        _context = context;
        _set = context.Set<TEntity>();
    }
     
    protected virtual IQueryable<TEntity> Query => _set.AsNoTracking();

    protected Task<Paginate<TDto>> PaginateAsync<TDto>(
        Expression<Func<TEntity, TDto>> projection, PageRequestBaseDto request, CancellationToken ct)
        => Query.ToPaginateAsync(request, projection, ct);

    protected Task<Paginate<TDto>> PaginatePassivedAsync<TDto>(
        Expression<Func<TEntity, TDto>> projection, PageRequestBaseDto request, CancellationToken ct)
        => Query.IgnoreQueryFilters(["SoftDeleteFilter"])
                .Where(e => e.IsPassived)
                .ToPaginateAsync(request, projection, ct);

    protected Task<List<TDto>> ListAsync<TDto>(
        Expression<Func<TEntity, TDto>> projection, PageRequestBaseDto request, CancellationToken ct)
        => Query.ToFilteredListAsync(request, projection, ct);

    protected Task<TDto?> SingleDtoByIdAsync<TDto>(
        Guid id, Expression<Func<TEntity, TDto>> projection, CancellationToken ct) where TDto : class
        => Query.Where(e => e.Id == id).Select(projection).SingleOrDefaultAsync(ct);

    protected Task<TDto?> SinglePassivedDtoByIdAsync<TDto>(
        Guid id, Expression<Func<TEntity, TDto>> projection, CancellationToken ct) where TDto : class
        => Query.IgnoreQueryFilters(["SoftDeleteFilter"])
                .Where(e => e.Id == id).Select(projection).SingleOrDefaultAsync(ct);


    protected Task<TEntity?> SinglePassivedByIdAsync(
        Guid id,CancellationToken ct) 
        => Query.IgnoreQueryFilters(["SoftDeleteFilter"])
                .Where(e => e.Id == id).SingleOrDefaultAsync(ct);



    protected async Task BulkDeleteByIdsAsync(List<Guid> ids, CancellationToken ct)
    {
        var entities = await _set
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(e => ids.Contains(e.Id)).ToListAsync(ct);
        if (entities.Count > 0)
            _set.RemoveRange(entities);
    }
    protected async Task DeleteByIdAsync(Guid id, CancellationToken ct)
    {
        var entity = await _set
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(e => e.Id == id).SingleOrDefaultAsync(ct);
        if (entity != null)
            _set.Remove(entity);
    }

    protected Task<List<TEntity>> GetByValuesAsync<TValue>(
        Expression<Func<TEntity, TValue>> selector,
        IReadOnlyCollection<TValue> values,
        CancellationToken ct)
        => Query.ToListByValuesAsync(selector, values, ct);

    protected async Task SetActiveByIdAsync(Guid id, CancellationToken ct)
    {
        var entity = await _set
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .FirstOrDefaultAsync(e => e.Id == id, ct);

        if (entity is not null)
            entity.IsPassived = false;
    }

    protected async Task SetPassiveByIdAsync(Guid id, CancellationToken ct)
    {
        var entity = await _set 
            .FirstOrDefaultAsync(e => e.Id == id, ct);

        if (entity is not null)
            entity.IsPassived = true;
    }
    protected async Task BulkSetActiveByIdsAsync(List<Guid> ids, CancellationToken ct)
    {
        if (ids.Count == 0)
            return;

        var entities = await _set
            .IgnoreQueryFilters(["SoftDeleteFilter"])
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(ct);

        foreach (var entity in entities)
            entity.IsPassived = false;
        //    await _set.AsQueryable().IgnoreQueryFilters(["SoftDeleteFilter"])                       // or _dbSet.Where(...)
        //.Where(x => ids.Contains(x.Id))
        //.ExecuteUpdateAsync(s => s.SetProperty(u => u.IsPassived, false), ct);
    }

    protected async Task BulkSetPassiveByIdsAsync(List<Guid> ids, CancellationToken ct)
    {
        if (ids.Count == 0)
            return;

        var entities = await _set 
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(ct);

        foreach (var entity in entities)
            entity.IsPassived = true;
    }
}

