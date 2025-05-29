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
            //ConnectionStringLdm = GetKey<string>("ConnectionStringLdm", true);
            //ConnectionStringClinics = GetKey<string>("ConnectionStringClinics", true);
            LdmStartTime = GetKey<int>("Ldm_startAfterSeconds");
            //VMsStartTime = GetKey<int>("VmsStartTime");
            //PrescriptionStartTime = GetKey<int>("PrescriptionStartTime");
            IntervalTime = GetKey<int>("Ldm_intervalInSeconds");
            //PackageName = GetKey<string>("PackageName");
            //UserName = GetKey<string>("UserName");
            //Password = GetKey<string>("Password");
            UpdateApi = GetKey<string>("update_url");
            TokenApi = GetKey<string>("token_url");

            GrantType = GetKey<string>("grant-type");
            ClientAuthMethod = GetKey<string>("client_authentication_method");
            ClientSecret = GetKey<string>("client_secret");
            ClientId = GetKey<int>("client_id");


        }
        public string LogPath { get; }
        //public string ConnectionStringLdm { get; }
        //public string ConnectionStringClinics { get; }
        //public string PackageName { get; set; }
        //public string UserName { get; }
        //public string Password { get; }
        public string UpdateApi { get; }
        public string TokenApi { get; }
        public int LdmStartTime { get; }
        //public int VMsStartTime { get; }
       // public int PrescriptionStartTime { get; }
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
