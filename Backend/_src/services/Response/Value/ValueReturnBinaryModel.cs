using DAL.Entities;

namespace DataCollector.ReturnModels
{
    public class ValueReturnBinaryModel : ValueReturnModelSingle
    {
        public bool Sample { get; set; }

        public String TextForTrue { get; set; }
        public String TextForFalse { get; set; }

    }
}