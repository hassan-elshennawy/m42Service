using m42Service.Entities;
using m42Service.Helpers;
using m42Service.M42Service.Entities;
using M42Service.DAL;
using M42Service.Helpers;
using NT.Integration.SharedKernel.Helper;
using NT.Integration.SharedKernel.OracleManagedHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace M42Service.DSL
{
    public class M42Dsl
    {
        private readonly ConfigManager _configuration;
        private readonly File_Logger _logger;
        private readonly M42DAL _m42DAL;

        public M42Dsl(ConfigManager configuration,M42DAL m42DAL) 
        { 
            _configuration = configuration;
            _logger = File_Logger.GetInstance("M42DSL");
            _m42DAL = m42DAL;
        }

        public async Task<AuthResponseWrapper> processAuthReq()
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
            var content = await responseMessage.Content.ReadAsStringAsync();
            var wrapper = new AuthResponseWrapper();
            if (responseMessage.IsSuccessStatusCode)
            {
                _logger.WriteToLogFile(ActionTypeEnum.Information, "Auth process succeed.");
                var success = JsonSerializer.Deserialize<AuthResponsDto>(content);
                wrapper.IsSuccess = true;
                wrapper.Success = success;
            }
            else
            {
                var error = JsonSerializer.Deserialize<AuthErrorDto>(content);
                wrapper.IsSuccess = false;
                wrapper.Error = error;
            }

            return wrapper;
        }
        public async Task processUpdatehReq()
        {
            _logger.WriteToLogFile(ActionTypeEnum.Information, "update process started.");
            var url = _configuration.UpdateApi;
            UpdateRequestDto body = new UpdateRequestDto();
            AuthResponseWrapper authResponseWrapper = new AuthResponseWrapper();
            int seqNumber = 0;

            try
            {
                using (var oracleManager = new OracleManager(_configuration.ConnectionStringLdm))
                {
                    _logger.WriteToLogFile(ActionTypeEnum.Information, "Opening Oracle connection to retrieve LDM records.");
                    await oracleManager.OpenConnectionAsync();
                    var dataTable = _m42DAL.GetAttachment(oracleManager);
                    _logger.WriteToLogFile(ActionTypeEnum.Information, $"Retrieved {dataTable.Rows.Count} rows from the database.");
                    string rawJson = dataTable.Rows[0]["P_JSON"]?.ToString();
                    seqNumber = (int)dataTable.Rows[0]["P_Seq_num"];
                    body = JsonSerializer.Deserialize<UpdateRequestDto>(rawJson);
                    _logger.WriteToLogFile(ActionTypeEnum.Information, "Closing Oracle connection after retrieving LDM records.");
                    oracleManager.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteToLogFile(ActionTypeEnum.Exception, $"Exception occurred while retrieving LDM records: {ex}");
            }
            
            string token = string.Empty;
            authResponseWrapper = processAuthReq().Result;
            if (authResponseWrapper.IsSuccess)
            {
                token = authResponseWrapper.Success.Access_token;
                _logger.WriteToLogFile(ActionTypeEnum.Information, $"got the token successfully: {token}");
            }
            else
            {
                _logger.WriteToLogFile(ActionTypeEnum.Information, $"error while getting token: {authResponseWrapper.Error.Error}");
            }

             var responseMessage = HttpHelper.UpdatePost<UpdateRequestDto>(url, body, token);
            var content = await responseMessage.Content.ReadAsStringAsync();
            if (responseMessage.IsSuccessStatusCode)
            {
                UpdateSuccessResponseDto updateSuccessResponseDto = JsonSerializer.Deserialize<UpdateSuccessResponseDto>(content);
                SetAttachmentDto attachmentDto = new SetAttachmentDto
                {
                    ModifiedPdf = updateSuccessResponseDto.ModifiedPdf,
                    SeqNumber = seqNumber,
                    Status = 1
                };
                _logger.WriteToLogFile(ActionTypeEnum.Information, "update process succeed.");
                try
                {
                    using (var oracleManager = new OracleManager(_configuration.ConnectionStringLdm))
                    {
                        await oracleManager.OpenConnectionAsync();
                        _m42DAL.SetAttachment(oracleManager, attachmentDto);
                        oracleManager.CloseConnection();
                    }
                }
                catch (Exception ex)
                {
                    _logger.WriteToLogFile(ActionTypeEnum.Exception, $"Exception occurred while retrieving LDM records: {ex}");
                }
            }
            else
            {
                UpdateErrorResponseDto updateErrorResponseDto = JsonSerializer.Deserialize<UpdateErrorResponseDto>(content);
                SetAttachmentDto attachmentDto = new SetAttachmentDto
                {
                    SeqNumber = seqNumber,
                    Status = 2
                };
                _logger.WriteToLogFile(ActionTypeEnum.Exception, $"error with status code {updateErrorResponseDto.ErrorStatusCode} and message {updateErrorResponseDto.ErrorMessage}");
                try
                {
                    using (var oracleManager = new OracleManager(_configuration.ConnectionStringLdm))
                    {
                        await oracleManager.OpenConnectionAsync();
                        _m42DAL.SetAttachment(oracleManager, attachmentDto);
                        oracleManager.CloseConnection();
                    }
                }
                catch (Exception ex)
                {
                    _logger.WriteToLogFile(ActionTypeEnum.Exception, $"Exception occurred while retrieving LDM records: {ex}");
                }
            }
        }

    }
}
