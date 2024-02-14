using com.symantec.scanengine.api;
using Microsoft.Extensions.Logging;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OPM.SFS.Web.Shared
{

    public class VirusScannerResult
    {
        public bool WasOriginalClean { get; set; }
        public bool IsResultClean { get; set; }
        public bool IsSendSuccessful { get; set; }

        public List<string> Threats { get; set; }
        public Dictionary<string, string> FailedScanServers { get; set; }

        public string ScanServer;

        public byte[] ResultFile;

        public override string ToString()
        {
            var result = string.Empty;
            if ((IsSendSuccessful))
            {
                if (!(IsResultClean))
                {
                    foreach (string item in Threats)
                    {
                        result = @"Threats Found:\\n";
                        result = string.Format(@"01\\n", result, item);
                    }
                }
                else
                    result = "Result File is clean";
            }
            else
                result = "Scan Failed";
            return result;
        }


        public string ToLongString()
        {
            var result = string.Empty;
            result = string.Format("0IsSendSuccessful: 1 \r\n", result, IsSendSuccessful);
            result = string.Format("0IsResultClean: 1 \r\n", result, IsResultClean);
            result = string.Format("0WasOriginalClean: 1 \r\n", result, WasOriginalClean);
            result = string.Format("0ScanServer: 1 \r\n", result, ScanServer);
            result = string.Format("0FailedScanServers: 1 \r\n", result, FailedScanServers.Count);
            foreach (KeyValuePair<string, string> lScanSever in FailedScanServers)
                result = string.Format("0   1 - 2 \r\n", result, lScanSever.Key, lScanSever.Value);
            result = string.Format("0Threats: 1 \r\n", result, Threats.Count);

            foreach (string lthreat in Threats)
                result = string.Format("0   1 \r\n", result, lthreat);

            return result;
        }
    }

    public interface IVirusScanner
    {
        VirusScannerResult FakeCleanedResult(Stream inputStream, string userID);
        VirusScannerResult GetEmptyVirusScannerResult();
        bool InitalizeVirusScanner(string ServerCSVList, string userID);
        VirusScannerResult ScanForVirus(byte[] buffer, string userID);
        VirusScannerResult ScanForVirus(Stream inputStream, string userID);
        Stream VirusStream();
        byte[] VirusTestBuffer();
    }

    public class VirusScanner : IVirusScanner
    {
        private const int DefaultScannerPort = 1344;
        private static List<ScanEngineInfo> ScanServers = new List<ScanEngineInfo>();
        private static ScanRequestManager ScanManager = new ScanRequestManager();
        private static int _serverCount;
        private string _virusTestBufferString = @"X5O!P%@AP[4\\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*";
        private readonly ILogger<VirusScanner> _logger;

        public VirusScanner(ILogger<VirusScanner> logger)
        {
            _logger = logger;
        }

        public VirusScannerResult GetEmptyVirusScannerResult()
        {
            
            VirusScannerResult ScannerResult = new VirusScannerResult()
            {
                WasOriginalClean = false,
                IsResultClean = false,
                IsSendSuccessful = false,
                Threats = new List<string>(),
                FailedScanServers = new Dictionary<string, string>(),
                ScanServer = string.Empty
            };

            return ScannerResult;
        }

        public int GetServerCount()
        {
            return _serverCount;
        }

        public bool IsVirusScannerInitialized()
        {            
            return _serverCount > 0;
        }

        public byte[] VirusTestBuffer()
        {
            return Encoding.ASCII.GetBytes(_virusTestBufferString);
        }
        public Stream VirusStream()
        {
            var lstream = new MemoryStream(VirusTestBuffer());
            return lstream;
        }

        public bool InitalizeVirusScanner(string ServerCSVList, string userID)
        {
            _logger.LogInformation($"Symantec AV Info: Initalizing Virus Scanner server - userID: {userID}");
            if (IsVirusScannerInitialized())
                return true;

            if ((string.IsNullOrEmpty(ServerCSVList)))
            {
                _logger.LogInformation($"Symantec AV Info: ServerCSVList is nothing or empty - userID: {userID}");
                throw new ArgumentException("ServerCSVList is nothing or empty.", "ServerCSVList");
            }
            var scanServerList = ServerCSVList.Split(",");
            ScanServers = new List<ScanEngineInfo>();
            ScanManager = new ScanRequestManager();
            foreach (string scanServer in scanServerList)
            {
                var serverParams = scanServer.Split(":");
                if ((serverParams.Length == 0))
                {
                    _logger.LogInformation($"Symantec AV Info: ServerCSVList has an invalid format. userID: {userID}");
                    throw new ArgumentException("ServerCSVList has an invalid format.", "ServerCSVList");
                }
                try
                {
                    if ((serverParams.Length > 1))
                    {
                        _serverCount = _serverCount + 1;
                        ScanServers.Add(new ScanEngineInfo(serverParams[0], int.Parse(serverParams[1])));
                        _logger.LogInformation($"Symantec AV Info: Adding server {serverParams[0]}. UserID {userID}");
                    }
                    else
                    {
                        _serverCount = _serverCount + 1;
                        ScanServers.Add(new ScanEngineInfo(serverParams[0], DefaultScannerPort));
                        _logger.LogInformation($"Symantec AV Info: Adding server {serverParams[0]}. UserID {userID}");
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogInformation($"Symantec AV Info: {ex.Message}. UserID {userID}");
                    _logger.LogInformation($"Symantec AV Info: {ex.StackTrace}. UserID  {userID}");
                    _serverCount = 0;
                    ScanServers.Clear();
                    ScanServers = null;
                    ScanManager = null;
                    throw;
                }
            }
            ScanManager.PrepareForScan(ScanServers, 10000, 5);
            return IsVirusScannerInitialized();
        }

        public VirusScannerResult FakeCleanedResult(Stream inputStream, string userID)
        {
            VirusScannerResult result;
            result = ScanForVirus(StreamExtensions.StreamToByteArray(inputStream), userID);
            if ((result.IsResultClean && result.WasOriginalClean))
            {
                result.WasOriginalClean = false;
                result.Threats.Add("Test Cleanable Virus");
            }
            return result;
        }


        public VirusScannerResult ScanForVirus(Stream inputStream, string userID)
        {
            _logger.LogInformation($"Symantec AV Info: Starting AV scanning. UserID: {userID}");
            return ScanForVirus(StreamExtensions.StreamToByteArray(inputStream), userID);
        }

        public VirusScannerResult ScanForVirus(byte[] buffer, string userID)
        {            
            var myScannerResult = GetEmptyVirusScannerResult();
            if (!IsVirusScannerInitialized())
            {
                _logger.LogInformation($"Symantec AV Info: Virus Scanner not initialized. UserID: {userID}");
                return myScannerResult;
            }

            var ScanRequest1 = ScanManager.CreateStreamScanRequest(Policy.DEFAULT);
            ScanResult ScanResult1 = null;
            try
            {
                if ((ScanRequest1.Start(string.Empty, string.Empty)))
                {
                    if ((ScanRequest1.Send(buffer)))
                    {
                        using (MemoryStream ResultFile = new MemoryStream())
                        {
                            ScanResult1 = ScanRequest1.Finish(ResultFile);
                            myScannerResult.ResultFile = ResultFile.ToArray();
                            myScannerResult.IsSendSuccessful = true;
                        }
                    }
                    else
                        myScannerResult.IsSendSuccessful = false;
                }
                else
                    myScannerResult.IsSendSuccessful = false;
            }
            catch (ScanException ex)
            {
                _logger.LogError($"Symantec AV error: {ex.Message}. UserID {userID}");
                _logger.LogError($"Symantec AV error stack: {ex.StackTrace}. UserID {userID}");
                if (ex.Message == "ERR_INITIALIZING_STREAM_REQUEST")
                    myScannerResult.IsSendSuccessful = false;
            }
            if (ScanResult1 != null)
            {
                foreach (ScanResult.ConnectionInfo connectionInfo in ScanResult1.connTriesInfo)
                {
                    _logger.LogInformation($"Symantec AV Info: Possible problem {connectionInfo.problemEncountered}. UserID: {userID}");
                    if ((connectionInfo.problemEncountered == ErrorCode.ERR_SUCCESSFUL_CONN))
                    {                       
                        myScannerResult.ScanServer = string.Format("{0}:{1}", connectionInfo.scanHost, connectionInfo.port);
                    }
                    else
                    {
                        myScannerResult.FailedScanServers.Add(string.Format("{0}:{1}", connectionInfo.scanHost, connectionInfo.port), connectionInfo.problemEncountered.ToString());
                    }
                }

                foreach (ScanResult.ThreatInfo threatInfo in ScanResult1.threat)
                {
                    _logger.LogInformation($"Symantec AV Info: Threats found {threatInfo.threatCategory}, {threatInfo.violationName}. UserID: {userID}");
                    myScannerResult.Threats.Add(string.Format("{0} {1}", threatInfo.threatCategory, threatInfo.violationName));
                }

                if ((myScannerResult.IsSendSuccessful))
                {
                    _logger.LogInformation($"Symantec AV Info: Successful scan. UserID: {userID}");
                    myScannerResult.IsResultClean = ScanResult1.fileStatus == FileScanStatus.CLEAN;
                    myScannerResult.WasOriginalClean = ScanResult1.totalInfection == 0;
                }
            }

            return myScannerResult;
        }
    }
}
