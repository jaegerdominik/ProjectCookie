using DAL.Entities;
using NJsonSchema.Converters;
using System.Runtime.Serialization;
using ProjectCookie._src.dal.Entities;

namespace DataCollector.ReturnModels
{
    [JsonInheritanceConverter(typeof(ValueReturnModelSingle), "discriminator")]
    [KnownType(typeof(ValueReturnNumericModel))]
    [KnownType(typeof(ValueReturnBinaryModel))]
    public class ValueReturnModelSingle
    {
        public String DataPoint { get; set; }
        public DataType DataType { get; set; }
        public String Icon { get; set; }
    }
}
