using System.Text;

namespace NFTMint.Services.Config
{
    public class ConfigService
    {
        static public WebServerConfig _webServerConfig = null!;
        static public DBConfig _dbConfig = null!;

        // config 기본 폴더 설정
        private static string _basePathDir = string.Empty;
        const string WEB_FILE_NAME = "WebServerConfig";
        const string DB_FILE_NAME = "DB";

        public ConfigService()
        {
            InitPath();
            WebLoadConfig();
            DBLoadConfig();
        }

        static public void InitPath()
        {
            Console.WriteLine($"Windows Current PWD {Directory.GetCurrentDirectory().ToString()}");
            int index = Directory.GetCurrentDirectory().LastIndexOf("NFTMint");
            if (0 < index)
            {
                _basePathDir = Directory.GetCurrentDirectory().Remove(Directory.GetCurrentDirectory().LastIndexOf("NFTMint"));
                _basePathDir += @"Config/";
            }
        }

        static public void WebLoadConfig()
        {
            var builder = new StringBuilder();
            var checkBuilder = new StringBuilder();

            try
            {
                checkBuilder.AppendFormat("{0}{1}Dev.json", _basePathDir, WEB_FILE_NAME);
                
                if(File.Exists(checkBuilder.ToString()))
                {
                    builder.AppendFormat("{0}{1}Dev.json", _basePathDir, WEB_FILE_NAME);
                }
                else
                {
                    builder.AppendFormat("{0}{1}.json", _basePathDir, WEB_FILE_NAME);
                }
                Console.WriteLine("{0} load config .. {1}", WEB_FILE_NAME, builder.ToString());

                string jsonString = System.IO.File.ReadAllText(builder.ToString());
                _webServerConfig = System.Text.Json.JsonSerializer.Deserialize<WebServerConfig>(jsonString)!;

                builder.Clear();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }

        static public void DBLoadConfig()
        {
            var builder = new StringBuilder();
            var checkBuilder = new StringBuilder();

            try
            {
                checkBuilder.AppendFormat("{0}{1}Dev.json", _basePathDir, DB_FILE_NAME);

                if (File.Exists(checkBuilder.ToString()))
                {
                    builder.AppendFormat("{0}{1}Dev.json", _basePathDir, DB_FILE_NAME);
                }
                else
                {
                    builder.AppendFormat("{0}{1}.json", _basePathDir, DB_FILE_NAME);
                }
                Console.WriteLine("{0} load config .. {1}", DB_FILE_NAME, builder.ToString());

                string jsonString = System.IO.File.ReadAllText(builder.ToString());
                _dbConfig = System.Text.Json.JsonSerializer.Deserialize<DBConfig>(jsonString)!;

                builder.Clear();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
    }
}
