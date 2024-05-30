using NJsonSchema.Converters;
using System.Runtime.Serialization;

namespace DataCollector.ReturnModels.Visuals
{

    [JsonInheritanceConverter(typeof(VisualsReturnModel), "discriminator")]
    [KnownType(typeof(VisualsBinaryReturnModel))]
    [KnownType(typeof(VisualsNumericReturnModel))]
    public class VisualsReturnModel
    {
        public String Icon { get; set; }
    }


}
