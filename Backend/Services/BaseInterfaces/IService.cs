using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectCookie.DAL.BaseClasses;
using ProjectCookie.Services.Response;

namespace ProjectCookie.Services.BaseInterfaces;

public interface IService<TEntity> where TEntity : Entity
{
    public abstract Task<ItemResponseModel<TEntity>> Create(TEntity entry);
    public abstract Task<TEntity?> Get(int id);
    public abstract Task<List<TEntity>> Get();
    
    public Task SetModelState(ModelStateDictionary validation);
}