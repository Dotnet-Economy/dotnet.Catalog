namespace dotnet.Catalog.Service.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; set; }
        public int Port { get; init; }
        public string ConnectionString => $"mongodb://{Host}:{Port}";

    }
}