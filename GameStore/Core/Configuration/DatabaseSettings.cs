namespace GameStore.Core.Configuration
{
    public class DatabaseSettings
    {
        public string Server { get; set; }

        public string Database { get; set; }

        public bool TrustedConnection{ get; set; }

        public bool MultipleActiveResultSets { get; set; }

        public string ConnectionString => $"Server={Server};Database={Database};Trusted_Connection={TrustedConnection};MultipleActiveResultSets={MultipleActiveResultSets}";
    }
}
