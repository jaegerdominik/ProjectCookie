using dal.MongoDB;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using services.Response;

namespace services.interfaces;

public interface IService<TEntity> where TEntity : Entity
{
    // nach create und update wird etwas zurück gegeben
    public abstract Task<ItemResponseModel<TEntity>> Create(TEntity entry);
    public abstract Task<ItemResponseModel<TEntity>> Update(string id, TEntity entry);
    
    // bei delete kommt nichts zurück
    public Task<ActionResultResponseModel> Delete(String id);
    public Task SetModelState(ModelStateDictionary validation);
    public Task Load(String email);
    public Task<TEntity> Get(String id);
    public Task<List<TEntity>> Get();
}