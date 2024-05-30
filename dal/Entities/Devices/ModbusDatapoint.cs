namespace DAL.Entities.Devices
{
    public class ModbusDataPoint : DataPoint
    {
        public RegisterType RegisterType { get; set; }

        public ReadingType ReadingType { get; set; }

        public int Register { get; set; }

        public int RegisterCount { get; set; }
    }

    public enum RegisterType
    {
        InputRegister,
        HoldingRegister,
        Coil,
        InputStatus,
        WriteSingleCoil,
        WriteSingleRegister
    }

    public enum ReadingType
    {
        LowToHigh,
        HighToLow,
    }
}
