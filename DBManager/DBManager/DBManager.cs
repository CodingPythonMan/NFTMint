using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCore
{
    public partial class DBManager
    {
        //CMD 명령어를 실행을 위한 클래스.
        ProcessStartInfo _processStartInfo = null!;

        DBInfoGroup _dbInfoGroup = null!;
        public bool _isExistSqlpackage { get; protected set; } = true;
        public bool _useTest { get; protected set; }

        string _tempScriptDirectory = @".\tempScript";
        string _scriptDirectory = @"..\Script";
        string _updateScriptDirectory = @".\SQLScript";
        string _logDirectory = @".\Log";
        string _tempDacpacDirectory = @".\tempDacpac";
        string _batchFileName;

        readonly string _createDB = "Create DB";
        readonly string _failCreateDB = "Fail CreateDB";

        string _updateDB = "Update DB";
        string _failUpdateDB = "Fail Update DB";

        string _backupDB = "Backup DB";
        string _failBackupDB = "Fail Backup DB";

        readonly string _failExportDACPAC = "Fail Export DACPAC";
        readonly string _failExportCreateSqlFile = "Fail Export Create Sql File";
        readonly string _failExportUpdateSqlFile = "Fail Export Update Sql File";

        string _notInstalledSqlPackage = "Not Installed sqlpackage.exe";

        public DBManager()
        {
            _batchFileName = _tempScriptDirectory + @"\DBManager.bat";
            _processStartInfo = new ProcessStartInfo();
            _processStartInfo.Verb = "runas";
            _processStartInfo.FileName = @"cmd";
            _processStartInfo.CreateNoWindow = false;
            _processStartInfo.UseShellExecute = false;
            _processStartInfo.RedirectStandardOutput = true;
            _processStartInfo.RedirectStandardInput = true;
            _processStartInfo.RedirectStandardError = true;
            CheckDirectory();
        }

        //DataBase 생성 쿼리 경로
        string GetCreateDefaultDBSqlFIleName(string defDBName) { return string.Format(@"{0}\{1}_Create.sql", _scriptDirectory, defDBName); }

        //DataBase 로그 경로
        string GetCreateDefaultDBLogFileName(string defDBName) { return string.Format(@"{0}\{1}_Create.log", _logDirectory, defDBName); }

        string GetCreateRealDBSqlFileName(string realDBName) { return string.Format(@"{0}\{1}_Create.sql", _tempScriptDirectory, realDBName); }
        string GetCreateRealDBLogFileName(string realDBName) { return string.Format(@"{0}\{1}_Create.log", _logDirectory, realDBName); }


        string GetUpdateRealDBSqlFileName(string realDBName, bool userTemp = true) { return string.Format(@"{0}\{1}_Update.sql", userTemp ? _tempScriptDirectory : _updateScriptDirectory, realDBName); }

        string GetUpdateRealDBLogFileName(string realDBName) { return string.Format(@"{0}\{1}_Update.log", _logDirectory, realDBName); }

        string GetBackupDBSqlFileName() { return string.Format(@"{0}\ProjectR_BackupDB.sql", _tempScriptDirectory); }
        string GetBackupDBLogFileName(string realDBName) { return string.Format(@"{0}\{1}_BackupDB.log", _logDirectory, realDBName); }

        //데이타베이스 스키마 얻기.
        string GetDacpacFileName(string realDBname) { return string.Format(@"{0}\{1}.dacpac", _tempDacpacDirectory, realDBname); }

        #region [Console Message]

        void StartConsoleMessage(string process, string targetName)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine(string.Format("Start {0} - {1}", process, targetName));
        }

        void SuccessConsoleMessage(string process, string targetName)
        {
            Console.WriteLine(string.Format("Success {0} {1}", process, targetName));
            Console.WriteLine("==================================================");
        }

        void FailConsoltMessage(string process, string targetName, string logFileName)
        {
            Console.WriteLine(string.Format("Fail {0} {1} - {2}", process, targetName, logFileName));
            Console.WriteLine("==================================================");
        }

        void FailConsoltMessage(string process, string targetName)
        {
            Console.WriteLine(string.Format("Fail {0} {1}", process, targetName));
            Console.WriteLine("==================================================");
        }
        #endregion [Console Message]

        public bool IsEnableDBManager(DBInfoGroup dbInfoGroup)
        {
            bool result = false;
            do
            {
                result = DeleteBatchFile();
                if (result == false)
                    break;

                result = DeleteTempScriptFile();
                if (result == false)
                    break;

                _dbInfoGroup = dbInfoGroup;

                _useTest = !string.IsNullOrEmpty(_dbInfoGroup._useTest);

                MakeCheckInstalledSqlPackageBatchFile();
                result = CheckCmd(_notInstalledSqlPackage);

            } while (false);
            return result;
        }

        #region [private]
        void CheckDirectory()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(_tempScriptDirectory);
            if (directoryInfo.Exists == false)
                directoryInfo.Create();
            directoryInfo = null!;

            directoryInfo = new DirectoryInfo(_scriptDirectory);
            if (directoryInfo.Exists == false)
                directoryInfo.Create();
            directoryInfo = null!;

            directoryInfo = new DirectoryInfo(_logDirectory);
            if (directoryInfo.Exists == false)
                directoryInfo.Create();
            directoryInfo = null!;

            directoryInfo = new DirectoryInfo(_tempDacpacDirectory);
            if (directoryInfo.Exists == false)
                directoryInfo.Create();
            directoryInfo = null!;

        }

        // _batchFileName의 변수명에 정해진 파일을 지우는 함수
        bool DeleteBatchFile()
        {
            bool result = false;
            do
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(_batchFileName);
                    if (fileInfo.Exists == true)
                        fileInfo.Delete();
                    fileInfo = null!;
                    result = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    result = false;
                }

            } while (false);

            return result;
        }

        //새로운 데이타베이스 리스트를 만들기 기존에 백업을 해두었던 기존 것을 지운다.
        bool DeleteTempScriptFile()
        {
            bool result = false;
            do
            {
                try
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(_tempScriptDirectory);
                    if (directoryInfo.Exists == false)
                    {
                        directoryInfo.Create();
                    }
                    else
                    {
                        FileInfo[] fileInfoList = directoryInfo.GetFiles();
                        foreach (FileInfo fileInfo in fileInfoList)
                            fileInfo.Delete();
                    }
                    result = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    result = false;
                }
            } while (false);
            return result;
        }

        // sqlpackage.exe이 설치가 되었는지 체크하는 배치 파일 만드는 함수
        /*            
            @echo off
            sqlpackage/?
            if errorlevel 1 goto errexit
            goto end
            :errexit
            echo Not Installed sqlpackage.exe
            goto end
            :end
            @echo on
        */
        void MakeCheckInstalledSqlPackageBatchFile()
        {
            string batchCommand = string.Empty;
            batchCommand = batchCommand + "@echo off" + Environment.NewLine;
            batchCommand = batchCommand + @"start ""C:\\Program Files\\Microsoft SQL Server\\150\DAC\\bin\\sqlpackage.exe"" /?" + Environment.NewLine;
            batchCommand = batchCommand + @"if not %ERRORLEVEL% == 0 goto errexit" + Environment.NewLine;
            batchCommand = batchCommand + "goto end" + Environment.NewLine;
            batchCommand = batchCommand + Environment.NewLine;
            batchCommand = batchCommand + ":errexit" + Environment.NewLine;
            batchCommand = batchCommand + "echo %ERRORLEVEL% " + Environment.NewLine;
            batchCommand = batchCommand + "echo " + _notInstalledSqlPackage + Environment.NewLine;
            batchCommand = batchCommand + "goto end" + Environment.NewLine;
            batchCommand = batchCommand + Environment.NewLine;
            batchCommand = batchCommand + ":end" + Environment.NewLine;
            batchCommand = batchCommand + "@echo on" + Environment.NewLine;
            File.WriteAllText(_batchFileName, batchCommand);
            File.SetAttributes(_batchFileName, FileAttributes.Hidden);
        }

        //sqlpackage.exe가 설치가 되었는지 체크하는 배치 파일을 실행하는 함수.
        //배치파일 결과 문자열에 "Not Installed sqlpackage.exe"; 있다면 설치가 되지않으면 return false;
        bool CheckCmd(string failMessage)
        {
            bool result = true;

            Process process = new Process();
            process.StartInfo = _processStartInfo;
            process.Start();
            process.StandardInput.WriteLine(_batchFileName);
            process.StandardInput.Close();

            if (string.IsNullOrEmpty(failMessage) == false)
            {
                string returnValue = string.Empty;
                while (result == true)
                {
                    returnValue = process.StandardOutput.ReadLine()!;
                    if (returnValue == null)
                    {
                        //process 종료 됨.
                        break;
                    }
                    //"Not Installed sqlpackage.exe" 문자열이 있다면 result = false 이다.
                    if (returnValue.ToLower().Equals(failMessage.ToLower()) == true)
                        result = false;
                }
            }

            process.WaitForExit();
            process.Close();
            //sqlpackage.exe이 설치가 되었는지 체크하는 배치 파일을 삭제하는 함수.
            DeleteBatchFile();
            return result;
        }

        #endregion [private]
    }
}
