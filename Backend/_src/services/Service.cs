using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectCookie._src.dal;
using ProjectCookie._src.dal.Entities;
using ProjectCookie._src.dal.Repository.Interface;
using ProjectCookie._src.services.interfaces;
using ProjectCookie._src.utils.Logging;
using Services.Response.Basis;
using Services.Utils;

namespace ProjectCookie._src.services;

// request ordner im service für login und picture request
public abstract class Service<TEntity> : IService<TEntity> where TEntity : Entity
{
    protected IUnitOfWork UnitOfWork;
    protected IPostgresRepository<TEntity> Repo;
    protected Serilog.ILogger Logger;
    
    // ModelStateWrapper prüft, ob Model valide ist
    protected IModelStateWrapper ValidationDictionary;
    protected ModelStateDictionary ModelStateDictionary;
    protected ICookieLogger CookieLogger;
    
    // einem REST Controller kann ein Claim mitgegeben werden
    protected User CurrentUser { get; private set; }
    
    public Service(IUnitOfWork unitOfWork, IPostgresRepository<TEntity> repo, ICookieLogger logger)
    {
        UnitOfWork = unitOfWork;
        Repo = repo;
        //TODO Logger = logger.ContextLog<Service<TEntity>>();
        CookieLogger = logger;
    }

    public abstract Task<bool> Validate(TEntity entity);
    
    public async Task<ItemResponseModel<TEntity>> Create(TEntity entry)
    {
      /**  bool validated = await Validate(entry);
        ItemResponseModel<TEntity> response = new ItemResponseModel<TEntity>();
        
        if (validated)
        {
            TEntity result = await Repo.InsertAsync(entry);
            response.Data = result;
            response.HasError = false;
        }
        else
        {
            // Fehler werden nun über Validation Dictionary zurück an den User gegeben
            response.Data = default(TEntity);
            response.HasError = true;
            response.ErrorMessages = ValidationDictionary.Errors;
        }
**/
        return null;
    }

    public async Task<ItemResponseModel<TEntity>> Update(string id, TEntity entry)
    {
       /** bool validated = await Validate(entry);
        ItemResponseModel<TEntity> response = new ItemResponseModel<TEntity>();

        if (validated)
        {
            TEntity result = await Repo.UpdateOneAsync(entry);
            response.Data = result;
            response.HasError = false;
        }
        else
        {
            response.Data = default(TEntity);
            response.HasError = true;
            response.ErrorMessages = ValidationDictionary.Errors;
        }**/
        
        return null;
    }

    public async Task<ActionResultResponseModel> Delete(string id)
    {
        /**ActionResultResponseModel model = new ActionResultResponseModel();
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
        **/
        return null;
    }

    public async Task SetModelState(ModelStateDictionary validation)
    {
        ValidationDictionary = new ModelStateWrapper(validation);
        ModelStateDictionary = validation;
    }

    // Fragt den aktuellen Nutzer ab
    public async Task Load(string email)
    {
        // CurrentUser = await UnitOfWork.Users.FindOneAsync(x => x.Email.ToLower().Equals(email.ToLower()));
    }

    public async Task<TEntity> Get(string id)
    {
        //return await Repo.FindByIdAsync(id);
        return null;
    }

    public async Task<List<TEntity>> Get()
    {
        return await Task.FromResult(Repo.FilterBy(doc => doc != null).ToList());
        //return await Task.Run(() => Repo.FilterBy(doc => doc != null).ToList());
    }
}