using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProjectCookie.Utils;

public class ModelStateWrapper : IModelStateWrapper
{
    private ModelStateDictionary _modelState;


    public ModelStateWrapper(ModelStateDictionary modelState)
    {
        _modelState = modelState;
        Clear();
    }

    public void AddError(string key, string errorMessage)
    {
        _modelState.AddModelError(key, errorMessage);
    }

    public bool IsValid
    {
        get { return _modelState.IsValid; }
    }

    public void Clear()
    {
        _modelState.Clear();
    }

    public Dictionary<string, string> Errors
    {

        get
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            foreach (KeyValuePair<string, ModelStateEntry> err in _modelState)
            {
                ModelStateEntry modelStateEntry = err.Value;

                string errormessage = "";

                ModelErrorCollection coll = modelStateEntry.Errors;

                foreach (ModelError error in coll)
                {
                    errormessage += error.ErrorMessage;
                }

                errors.Add(err.Key, errormessage);
            }

            return errors;
        }
    }
}