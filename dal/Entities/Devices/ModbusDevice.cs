using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.Devices
{
    public class ModbusDevice : Device
    {
        [Column("Host")]
        public string Host { get; set; }
        [Column("Port")]
        public int Port { get; set; }

        public int SlaveID { get; set; }
    }


}
