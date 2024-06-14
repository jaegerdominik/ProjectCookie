using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectCookie.DAL.BaseClasses;
using ProjectCookie.Services.Response;

namespace ProjectCookie.Services.BaseInterfaces;

public interface IService<TEntity> where TEntity : Entity
{
    public abstract Task<ItemResponseModel<TEntity>> Create(TEntity entry);
    public abstract Task<ItemResponseModel<TEntity>> Update(int id, TEntity entry);
    
    public Task<ActionResultResponseModel> Delete(int id);
    public Task SetModelState(ModelStateDictionary validation);
    public Task<TEntity> Get(int id);
    public Task<List<TEntity>> Get();
}