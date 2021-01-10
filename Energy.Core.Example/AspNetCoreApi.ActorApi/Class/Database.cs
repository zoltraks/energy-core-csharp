namespace AspNetCoreApi.ActorApi.Class
{
    public class Database
    {
        public string ConnectionString { get; set; }

        public Energy.Source.Connection Source { get; set; }
    }
}