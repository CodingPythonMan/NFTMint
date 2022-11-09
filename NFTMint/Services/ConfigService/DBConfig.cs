namespace NFTMint.Services.Config
{
    public class DBConfig
    {
        public MSSql MsSql { get; set; } = null!;
    }

    public class MSSql
    {
        public DB1[] DB { get; set; } = null!;
    }

    public class DB1
    {
        public int TypeId { get; set; }

        public string Embeded { get; set; } = null!;

        public string DBName { get; set; } = null!;

        public string IP { get; set; } = null!;

        public int Port { get; set; }

        public string ID { get; set; } = null!;

        public string Pass { get; set; } = null!;

        public int MaxRange { get; set; }
    }
}
