using utils.interfaces;

namespace utils;

public class AquariumSettings : ISettings
{
    public required string LoggerSettings { get; set; }
    
    public MongoDBSettings MongoDbSettings { get; set; }
}