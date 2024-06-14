using ProjectCookie.Utils.SettingItems;

namespace ProjectCookie.Utils;

public interface ISettings
{
    public TimeScaleDBSettings TimeScaleDBSettings { get; set; }

    public string LoggerSettings { get; set; }
}