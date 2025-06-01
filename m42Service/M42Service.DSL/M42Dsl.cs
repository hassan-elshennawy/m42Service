using AdafcaJahizyaService.Helpers;
using M42Service.Helpers;
using m42Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using m42Service.M42Service.Entities;

namespace M42Service.DSL
{
    public class M42Dsl
    {
        private readonly ConfigManager _configuration;
        private readonly File_Logger _logger;

        public M42Dsl(ConfigManager configuration) {
        _configuration = configuration;
        _logger = File_Logger.GetInstance("M42DSL");
        }

        public async Task processAuthReq()
        {
            _logger.WriteToLogFile(ActionTypeEnum.Information, "Auth process started.");
            var url = _configuration.TokenApi;
            var body = new Dictionary<string, string>
            {
                 { "grant_type", _configuration.GrantType },
                { "client_authentication_method",  _configuration.ClientAuthMethod},
                {"client_secret", _configuration.ClientSecret},
                {"client_id", _configuration.ClientId.ToString()}
            };
            var responseMessage = HttpHelper.AuthPost(url, body);

        }

        // will use Await when retriving from Database
        public async Task processUpdatehReq()
        {
            _logger.WriteToLogFile(ActionTypeEnum.Information, "update process started.");
            var url = _configuration.UpdateApi;

            // will use here DAL instead to read from the database
            var body = new UpdateRequestDto
            {
                pdf = "rand",
                data =
                {
                    dob = new DateOnly(2024,5,29),
                    mrn = "gfhfg",
                    name = "Jhon Doe"
                },
                templsteId = "Test1234"
                
            };
            var token = "";

            var responseMessage = HttpHelper.UpdatePost<UpdateRequestDto>(url, body,token);

        }

    }
}
