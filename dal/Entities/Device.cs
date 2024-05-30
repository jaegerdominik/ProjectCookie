namespace DAL.Entities
{


    public class Device : Entity
    {

        public String DeviceType { get; set; }

        public string DeviceName { get; set; }

        public String? DeviceDescription { get; set; }

        public bool Active { get; set; }

        public string Aquarium { get; set; }

    }


}
