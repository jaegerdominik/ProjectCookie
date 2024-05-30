using dal;
using dal.interfaces;
using dal.MongoDB;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using services.interfaces;
using services.ModelState;
using services.ModelState.interfaces;
using services.Response;
using utils.interfaces;

namespace services;
// hü
// delete, get, getall, update implementieren
// einzelnen services implementieren --> aquariumitem usw siehe moodle
// login ist mit authentication handler geschrieben
// getforuser --> aquarien pro benutzer zurückgeben
// keine interfaces notwendig

// login
// login request als parameter
// picture machen wir nächstes mal

// request ordner im service für login und picture request
public abstract class Service<TEntity> : IService<TEntity> where TEntity : Entity
{
    protected IUnitOfWork UnitOfWork;
    protected IMongoRepository<TEntity> Repo;
    protected Serilog.ILogger Logger;
    
    // ModelStateWrapper prüft, ob Model valide ist
    protected IModelStateWrapper ValidationDictionary;
    protected ModelStateDictionary ModelStateDictionary;
    protected IAquariumLogger AquariumLogger;
    
    // einem REST Controller kann ein Claim mitgegeben werden
    protected User CurrentUser { get; private set; }
    
    public Service(IUnitOfWork unitOfWork, IMongoRepository<TEntity> repo, IAquariumLogger logger)
    {
        UnitOfWork = unitOfWork;
        Repo = repo;
        Logger = logger.ContextLog<Service<TEntity>>();
        AquariumLogger = logger;
    }

    public abstract Task<bool> Validate(TEntity entity);
    
    public async Task<ItemResponseModel<TEntity>> Create(TEntity entry)
    {
        bool validated = await Validate(entry);
        ItemResponseModel<TEntity> response = new ItemResponseModel<TEntity>();
        
        if (validated)
        {
            TEntity result = await Repo.InsertOneAsync(entry);
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

        return response;
    }

    public async Task<ItemResponseModel<TEntity>> Update(string id, TEntity entry)
    {
        bool validated = await Validate(entry);
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
        }
        
        return response;
    }

    public async Task<ActionResultResponseModel> Delete(string id)
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
        
        return model;
    }

    public async Task SetModelState(ModelStateDictionary validation)
    {
        ValidationDictionary = new ModelStateWrapper(validation, AquariumLogger);
        ModelStateDictionary = validation;
    }

    // Fragt den aktuellen Nutzer ab
    public async Task Load(string email)
    {
        CurrentUser = await UnitOfWork.User.FindOneAsync(x => x.Email.ToLower().Equals(email.ToLower()));
    }

    public async Task<TEntity> Get(string id)
    {
        return await Repo.FindByIdAsync(id);
    }

    public async Task<List<TEntity>> Get()
    {
        return await Task.FromResult(Repo.FilterBy(doc => doc != null).ToList());
        //return await Task.Run(() => Repo.FilterBy(doc => doc != null).ToList());
    }
}