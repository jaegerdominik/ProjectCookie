namespace DataCollector.ReturnModels
{
    public class ValueReturnModelMultiple
    {
        public List<ValueReturnNumericModel> Numeric { get; set; } = new();
        public List<ValueReturnBinaryModel> Binary { get; set; } = new();
    }
}
