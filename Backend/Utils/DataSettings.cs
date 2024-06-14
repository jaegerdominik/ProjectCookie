using ProjectCookie.Utils.SettingItems;

namespace ProjectCookie.Utils;

public class DataSettings : ISettings
{
    public TimeScaleDBSettings TimeScaleDBSettings { get; set; }
    public string LoggerSettings { get; set; }
}