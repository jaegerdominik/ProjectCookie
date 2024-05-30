namespace utils.interfaces;

public interface ISettings
{
    public String LoggerSettings { get; set; }
    
    public MongoDBSettings MongoDbSettings { get; set; }
}