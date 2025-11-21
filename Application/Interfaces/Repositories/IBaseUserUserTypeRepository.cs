using System.Linq.Expressions;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Repositories;

public interface IBaseUserUserTypeRepository<TEntity, TPEntity> : IBaseRepository<TEntity> where TEntity : UserUserType where TPEntity : class
{
    string GetDiscriminator();
    Task<Guid> GetUserTypeIdAsync();
    Expression<Func<TEntity, bool>> IdentifierFilter(Guid identifier);
    Expression<Func<TEntity, bool>> IdentifierContainsFilter(IEnumerable<Guid> identifiers);
    Expression<Func<TEntity, TPEntity>> IncludeEntity();
    Expression<Func<TPEntity, bool>> IsAvailableFilter(Guid? userId);
    Expression<Func<TEntity, bool>> RoleAvailableFilter(Guid identifier);
    void SetIdentifier(TEntity entity, Guid identifier);
    Task<Guid?> GetUserIdAsync(Guid identifier);
    Task<bool> IsExistAsync(Guid identifier, Guid roleId);
    Task<bool> IsAttachedAsync(Guid identifier);
    Task<bool> IsExistAsync(Guid identifier, string roleCode);
    Task<TEntity?> GetAsync(Guid identifier, Guid roleId, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);
    Task<TEntity?> GetByUserIdAsync(Guid userId);
    Task<HashSet<Guid>> GetEnabledRoles(Guid identifier, IEnumerable<Guid> roleIds);
    Task<bool> AssignUserAsync(Guid userId, Guid identifier);
    Task UnassignUserAsync(Guid userId);
    Task<Dictionary<Guid, int>> GetRoleCountsAsync(IEnumerable<Guid> identifiers);
}
