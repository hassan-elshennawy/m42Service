using Microsoft.Extensions.Configuration;
using NT.Integration.Shield;
namespace m42Service.Helpers
{

    public class ConfigManager
    {
        private readonly IConfiguration _configuration;
        public ConfigManager(IConfiguration IConfiguration)
        {
            _configuration = IConfiguration;
            LogPath = GetKey<string>("LogPath");

            LdmStartTime = GetKey<int>("Ldm_startAfterSeconds");
            IntervalTime = GetKey<int>("Ldm_intervalInSeconds");

            UpdateApi = GetKey<string>("update_url");
            TokenApi = GetKey<string>("token_url");

            GrantType = GetKey<string>("grant-type");
            ClientAuthMethod = GetKey<string>("client_authentication_method");
            ClientSecret = GetKey<string>("client_secret");
            ClientId = GetKey<int>("client_id");
            ConnectionStringLdm = GetKey<string>("ConnectionStringLdm", true);


        }
        public string ConnectionStringLdm { get; }
        public string LogPath { get; }
        public string UpdateApi { get; }
        public string TokenApi { get; }
        public int LdmStartTime { get; }
        public int IntervalTime { get; }
        public string GrantType { get; set; }
        public string ClientAuthMethod { get; set; }
        public string ClientSecret { get; set; }
        public int ClientId { get; set; }



        private T GetKey<T>(string key, bool isConnectionString = false)
        {
            if (isConnectionString)
                return Decrypt<T>(_configuration.GetConnectionString(key));

            return Decrypt<T>(_configuration.GetSection(key).Value);
        }
        private T Decrypt<T>(string encryptedValue)
        {
            try
            {
                return (T)Convert.ChangeType(EncryptionHandler.DecryptText(encryptedValue, true), typeof(T));
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
