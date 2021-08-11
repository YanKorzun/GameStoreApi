namespace GameStore.WEB.Settings
{
    public class DatabaseSettings
    {
        public string DataSource { get; set; }
        public string InitialCatalog { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public int ConnectTimeout { get; set; }
        public bool Encrypt { get; set; }
        public bool TrustedServerCertificate { get; set; }
        public string ApplicationIntent { get; set; }
        public bool MultiSubnetFailover { get; set; }

        public string ConnectionString =>
            $"Data Source={DataSource};" +
            $"Initial Catalog={InitialCatalog};" +
            $"User ID={UserID};" +
            $"Password={Password};" +
            $"Connect Timeout={ConnectTimeout};" +
            $"Encrypt={Encrypt};" +
            $"TrustServerCertificate={TrustedServerCertificate};" +
            $"ApplicationIntent={ApplicationIntent};" +
            $"MultiSubnetFailover={MultiSubnetFailover}";
    }
}