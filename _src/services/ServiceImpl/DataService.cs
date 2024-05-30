using DAL.Entities;
using DAL.Repository;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectCookie._src.dal.UnitOfWork;
using ProjectCookie._src.utils.Logging;
using Services.Response.Basis;
using Services.Utils;
using Utilities.Logging;

namespace Services
{
    public abstract class DataService<TEntity> where TEntity : Entity
    {

        protected Serilog.ILogger log = null;


        protected DataService(ICookieLogger logger)
        {
            //TODO log = logger.ContextLog<DataService<TEntity>>();
        }

        protected UnitOfWork UnitOfWork = null;
        protected IEntityRepository<TEntity> Repository = null;
        protected ModelStateDictionary Validation = null;
        protected GlobalService GlobalService;

        protected IModelStateWrapper validationDictionary;
        public abstract Task<bool> Validate(TEntity entry);

        protected abstract Task<ItemResponseModel<TEntity>> Create(TEntity entry);
        public abstract Task<ItemResponseModel<TEntity>> Update(int id, TEntity entry);
        public virtual async Task<ActionResultResponseModel> Delete(int id)
        {
            await Repository.Delete(id);

            Boolean success = await Commit();
            ActionResultResponseModel model = new ActionResultResponseModel();
            model.Success = success;

            return model;
        }


        private async Task<Boolean> Commit()
        {
            try
            {
                await UnitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                log.Error(e, "Error during commiting");
            }

            return false;
        }


        public async Task SetModelState(ModelStateDictionary validation)
        {
            validationDictionary = new ModelStateWrapper(validation);
            this.Validation = validation;
        }

        public DataService(UnitOfWork uow, IEntityRepository<TEntity> repo, GlobalService service)
        {
            UnitOfWork = uow;
            Repository = repo;
            GlobalService = service;
        }


        public void ClearValidation()
        {
            if (Validation != null)
            {
                validationDictionary = new ModelStateWrapper(Validation);
            }
        }


        public virtual async Task<ItemResponseModel<TEntity>> CreateHandler(TEntity entry)
        {
            ClearValidation();
            ItemResponseModel<TEntity> returnval = new ItemResponseModel<TEntity>();

            try
            {

                if (await Validate(entry))
                {

                    ItemResponseModel<TEntity> ent = await Create(entry);

                    Boolean result = await Commit();

                    if (ent != null)
                    {
                        if (ent.HasError == false)
                        {

                            returnval.Data = ent.Data;
                            returnval.HasError = false;
                        }
                        else
                        {
                            return ent;
                        }
                    }
                    else
                    {
                        returnval.Data = default;
                        returnval.HasError = true;
                        returnval.ErrorMessages.Add("Empty", "Object was empty");
                    }

                }
                else
                {
                    returnval.Data = default;
                    returnval.HasError = true;
                    returnval.ErrorMessages = validationDictionary.Errors;
                }
            }
            catch (Exception ex)
            {
                returnval.Data = default;
                returnval.HasError = true;
                returnval.ErrorMessages.Add("Error", "Error during create of element");

                log.Warning("Error during creation");
                log.Debug(ex, "Error during creation");

            }


            return returnval;
        }



        public virtual async Task<ItemResponseModel<TEntity>> UpdateHandler(int id, TEntity entry)
        {
            ClearValidation();
            ItemResponseModel<TEntity> returnval = new ItemResponseModel<TEntity>();
            try
            {
                if (await Validate(entry))
                {
                    try
                    {

                        ItemResponseModel<TEntity> ent = await Update(id, entry);
                        if (ent != null && ent.Data != null)
                        {
                            if (ent.HasError == false)
                            {

                                ent.Data.ID = id;
                                await Repository.Update(ent.Data);
                                Boolean result = await Commit();
                                returnval.Data = ent.Data;
                                returnval.HasError = false;

                            }
                            else
                            {
                                return ent;
                            }
                        }
                        else
                        {
                            returnval.Data = default;
                            returnval.HasError = true;
                            returnval.ErrorMessages.Add("Empty", "Object was empty");
                        }
                    }

                    catch (Exception ex)
                    {
                        returnval.Data = default;
                        returnval.HasError = true;
                        returnval.ErrorMessages.Add("Error", "Error during create of element");

                        log.Warning("Error during creation");
                        log.Debug(ex, "Error during creation");

                    }
                }
                else
                {
                    returnval.Data = default;
                    returnval.HasError = true;
                    returnval.ErrorMessages = validationDictionary.Errors;
                }

            }
            catch (Exception ex)
            {
                returnval.Data = default;
                returnval.HasError = true;
                returnval.ErrorMessages.Add("Error", "Error during create of element");

                log.Warning("Error during creation");
                log.Debug(ex, "Error during creation");

            }


            return returnval;
        }


        public async Task<TEntity> Get(int id)
        {
            TEntity ent = await Repository.Find(x => x.ID == id);

            return ent;
        }


        public async Task<List<TEntity>> Get()
        {
            List<TEntity> ent = await Repository.Get(x => true);

            return ent;
        }
    }
}