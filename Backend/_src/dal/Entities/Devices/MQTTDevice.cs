using System.ComponentModel.DataAnnotations.Schema;
using ProjectCookie._src.dal.Entities;

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
