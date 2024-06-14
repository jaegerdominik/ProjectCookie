using DAL.UnitOfWork;

namespace Utils
{
    public class DataSettings : ISettings
    {
        public TimeScaleDBSettings TimeScaleDBSettings { get; set; }
        public string LoggerSettings { get; set; }
    }
}
