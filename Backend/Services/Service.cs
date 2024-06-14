using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectCookie.DAL.BaseClasses;
using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.Services.BaseInterfaces;
using ProjectCookie.Services.Response;
using ProjectCookie.Utils;
using ProjectCookie.Utils.Logging;

namespace ProjectCookie.Services;

public abstract class Service<TEntity> : IService<TEntity> where TEntity : Entity
{
    protected IUnitOfWork UnitOfWork;
    protected IPostgresRepository<TEntity> Repo;
    protected Serilog.ILogger Logger;

    protected IModelStateWrapper ValidationDictionary;
    protected ModelStateDictionary ModelStateDictionary;
    protected ICookieLogger CookieLogger;
    
    public Service(IUnitOfWork unitOfWork, IPostgresRepository<TEntity> repo, ICookieLogger logger)
    {
        UnitOfWork = unitOfWork;
        Repo = repo;
        Logger = logger.ContextLog<Service<TEntity>>();
        CookieLogger = logger;
    }

    public abstract Task<bool> Validate(TEntity entity);
    
    public async Task<ItemResponseModel<TEntity>> Create(TEntity entry)
    {
        bool validated = await Validate(entry);
        ItemResponseModel<TEntity> response = new ItemResponseModel<TEntity>();
        
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

    public async Task<ItemResponseModel<TEntity>> Update(int id, TEntity entry)
    {
         bool validated = await Validate(entry);
         ItemResponseModel<TEntity> response = new ItemResponseModel<TEntity>();

         if (validated)
         {
             await Repo.UpdateAsync(entry);
             response.Data = entry;
             response.HasError = false;
         }
         else
         {
             response.Data = default(TEntity);
             response.HasError = true;
             response.ErrorMessages = ValidationDictionary.Errors;
         }
        
         return null;
    }

    public async Task<ActionResultResponseModel> Delete(int id)
    {
        ActionResultResponseModel model = new ActionResultResponseModel();
        model.Success = true;

        try
        {
            await Repo.DeleteByIdAsync(id);
        }
        catch (Exception e)
        {
            model.Success = false;
            Console.WriteLine(e);
        }
        
        return null;
    }

    public async Task SetModelState(ModelStateDictionary validation)
    {
        ValidationDictionary = new ModelStateWrapper(validation);
        ModelStateDictionary = validation;
    }

    public async Task<TEntity> Get(int id)
    {
        return await Repo.FindByIdAsync(id);
    }

    public async Task<List<TEntity>> Get()
    {
        return await Task.FromResult(Repo.FilterBy(doc => doc != null).ToList());
    }
}