using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.Devices
{
    public class MQTTDevice : Device
    {
        [Column("Host")]
        public string Host { get; set; }
        [Column("Port")]
        public int Port { get; set; }


    }
}
