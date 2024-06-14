using DAL.UnitOfWork;

namespace Utils
{
    public interface ISettings
    {
        public TimeScaleDBSettings TimeScaleDBSettings { get; set; }

        public String LoggerSettings { get; set; }

    }
}
