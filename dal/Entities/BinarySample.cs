namespace DAL.Entities
{
    [HyperTableAttributes(ChunkSize = 168, RetentionPeriod = -1)]
    public class BinarySample : HyperEntity
    {
        public Boolean Value { get; set; }
    }
}
