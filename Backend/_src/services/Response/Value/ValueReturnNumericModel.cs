using DAL.Entities;

namespace DataCollector.ReturnModels
{
    public class ValueReturnNumericModel : ValueReturnModelSingle
    {

        public float Sample { get; set; }
        public String Unit { get; set; }

    }
}