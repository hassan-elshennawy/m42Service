using m42Service.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NT.Integration.SharedKernel.OracleManagedHelper;
using Oracle.ManagedDataAccess.Client;
using m42Service.M42Service.Entities;

namespace M42Service.DAL
{
    public class M42DAL
    {
        private readonly ConfigManager _configManager;
        private readonly File_Logger _logger;

        public M42DAL(ConfigManager configManager, File_Logger logger)
        {
            _configManager = configManager;
            _logger = File_Logger.GetInstance("M42DAL");
        }
        public DataTable GetAttachment(OracleManager oracleManager)
        {
            return oracleManager.GetDataTable("get_GH_ATTACHMENT_API", CommandType.StoredProcedure);
        }

        public void SetAttachment(OracleManager oracleManager, SetAttachmentDto attachmentDto)
        {
            oracleManager.CommandParameters.Add("p_seq", OracleDbType.Int64, attachmentDto.SeqNumber, ParameterDirection.Input);
            oracleManager.CommandParameters.Add("P_status", OracleDbType.Int16, attachmentDto.Status, ParameterDirection.Input);
            oracleManager.CommandParameters.Add("P_file", OracleDbType.Blob,attachmentDto.ModifiedPdf,ParameterDirection.Input);
            oracleManager.ExcuteNonQueryAsync("set_GH_ATTACHMENT_API", CommandType.StoredProcedure);
        }
    }
}
