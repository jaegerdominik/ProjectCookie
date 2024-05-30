using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{

    public class DataPoint : Entity
    {
        public String DatapointType { get; set; }
        public string Name { get; set; }

        public DataType DataType { get; set; }

        public int Offset { get; set; }

        public String? Description { get; set; }

        public String? Icon { get; set; } = "";

        public String? Unit { get; set; } = "";

        [ForeignKey("FK_Device")]
        [JsonIgnore][JsonProperty(Required = Required.Default)] public Device Device { get; set; }

        public int FK_Device { get; set; }



    }
}
