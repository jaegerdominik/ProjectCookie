using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectCookie.DAL.BaseClasses;
using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.Services.BaseInterfaces;
using ProjectCookie.Services.Response;
using ProjectCookie.Utils;
using Serilog;

namespace ProjectCookie.Services;

public abstract class Service<TEntity> : IService<TEntity> where TEntity : Entity
{
    protected IPostgresRepository<TEntity> Repo;
    protected IModelStateWrapper ValidationDictionary;
    protected ModelStateDictionary ModelStateDictionary;
    
    public Service(IPostgresRepository<TEntity> repo)
    {
        Repo = repo;
        ModelStateDictionary = new ModelStateDictionary();
        ValidationDictionary = new ModelStateWrapper(ModelStateDictionary);
    }

    public abstract Task<bool> Validate(TEntity entity);
    
    public async Task<ItemResponseModel<TEntity>> Create(TEntity entry)
    {
        Log.Information("Create new entry: " + entry);
        bool validated = await Validate(entry);
        ItemResponseModel<TEntity> response = new();
        
        if (validated)
        {
            await Repo.InsertAsync(entry);
            response.Data = entry;
            response.HasError = false;
        }
        else
        {
            response.Data = default(TEntity);
            response.HasError = true;
            response.ErrorMessages = ValidationDictionary.Errors;
        }

        return response;
    }
    public async Task<List<TEntity>> Get() => Repo.FilterBy(e => e != null).ToList();
    public async Task<TEntity?> Get(int id) => await Repo.FindByIdAsync(id);
    
    public async Task SetModelState(ModelStateDictionary validation)
    {
        ValidationDictionary = new ModelStateWrapper(validation);
        ModelStateDictionary = validation;
    }
}