using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NJsonSchema.Converters;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DAL.Entities
{
    [Index(nameof(Time), nameof(FK_Datapoint), AllDescending = true)]
    // [JsonConverter(typeof(JsonInheritanceConverter), "discriminator")]
    [JsonInheritanceConverter(typeof(HyperEntity), "discriminator")]
    [KnownType(typeof(NumericSample))]
    [KnownType(typeof(BinarySample))]
    public abstract class HyperEntity
    {
        [HyperTableColumn]
        public DateTimeOffset Time { get; set; } = DateTimeOffset.UtcNow;

        [ForeignKey("FK_Datapoint")]
        //  [NotMapped]
        [JsonIgnore]
        [JsonProperty(Required = Required.Default)]
        public DataPoint DataPoint { get; set; }

        public int FK_Datapoint { get; set; }
        public int? Status { get; set; }
        public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;
    }
}
