using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace m42Service.Helpers
{
    public class File_Logger
    {
        public static event Func<string> GetLogFilePath_Event;

        private File_Logger(string _subFolderLogName)
        {
            this._subFolderLogName = _subFolderLogName;
        }

        private static readonly string _executingAssemblyVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();

        private readonly string _subFolderLogName;
        private static readonly Dictionary<string, File_Logger> _loggerDictionary = new Dictionary<string, File_Logger>();

        public static File_Logger GetInstance(string? subFolderLogName = null)
        {

           /* if (string.IsNullOrWhiteSpace(subFolderLogName))
            {
                subFolderLogName = "General";
            }*/

            lock (_loggerDictionary)
            {
                var file_logger = _loggerDictionary.FirstOrDefault(x => x.Key.Trim().ToLower() == subFolderLogName.Trim().ToLower()).Value;
                if (file_logger == null)
                {
                    file_logger = new File_Logger(subFolderLogName);
                    _loggerDictionary.Add(subFolderLogName, file_logger);
                }
                return _loggerDictionary[subFolderLogName];
            }
        }

        public string GetLogFilePath()
        {
            return GetLogFilePath_Event();
        }

        public void WriteToLogFile(Exception exception, string exceptionDetail, bool newLine = true)
        {
            WriteToLogFile(ActionTypeEnum.Exception, exceptionDetail + Environment.NewLine + exception.ToString(), newLine);
        }

        public void WriteToLogFile(ActionTypeEnum logAction, string message, bool newLine = true,
                                  [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
        {
            try
            {
                // get base log file path
                string baseLogPath = GetLogFilePath();

                string directoryPath = Path.Combine(baseLogPath, DateTime.Now.ToString("yyyy-MM-dd"), _subFolderLogName);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string filePath = Path.Combine(directoryPath, DateTime.Now.ToString("yyyy-MM-dd HH") +
                                                              (logAction == ActionTypeEnum.Exception ? "_Exception" : string.Empty) +
                                                              $"_V{_executingAssemblyVersion}.txt");

                BackUpLogFileWhenSizeExceeded(filePath);

                using (StreamWriter streamWriter = new StreamWriter(filePath, true))
                {
                    string[] str = new string[8];
                    str[0] = DateTime.Now.ToString("HH:mm:ss.fff");
                    str[1] = " || ";
                    str[2] = methodName.PadRight(33);
                    str[3] = " || ";
                    str[4] = logAction.ToString().PadRight(11);
                    str[5] = " || ";
                    str[6] = message;
                    if (logAction == ActionTypeEnum.Exception && newLine)
                        str[7] = Environment.NewLine + "==============================================" + Environment.NewLine;
                    streamWriter.WriteLine(string.Concat(str));
                }
            }
            catch (Exception)
            {
            }
        }

        private static void BackUpLogFileWhenSizeExceeded(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return;
                }
                if (File.ReadAllBytes(filePath).Length > 5242880) //5 Mega
                {
                    File.Move(filePath, Path.Combine(Path.GetDirectoryName(filePath),
                                                     Path.GetFileNameWithoutExtension(filePath) + "_" + DateTime.Now.ToString("HH_mm_ss") + ".txt"));
                }
            }
            catch (Exception) { }
        }
    }

    public enum ActionTypeEnum
    {
        Information = 1,
        Action = 2,
        Exception = 3
    }
}