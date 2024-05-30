namespace DAL.Entities
{
    [HyperTableAttributes(ChunkSize = 168, RetentionPeriod = -1)]
    public class NumericSample : HyperEntity
    {
        public float Value { get; set; }
    }
}
