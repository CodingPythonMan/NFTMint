using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCore
{
    public partial class DBManager
    {
        //데이타 베이스 생성 쿼리 만들기.
        void CreateSqlCmdBatchFile(DBInfo dbInfo, string applyDBName, string executeSqlFileName, string outputFileName, string failMessage)
        {
            string batchCommand = string.Empty;
            batchCommand = batchCommand + "@echo off" + Environment.NewLine;
            batchCommand = batchCommand + "@chcp 850" + Environment.NewLine;
            batchCommand = batchCommand + string.Format(@"SET SqlCmdOption=-U {0} -P {1} -S {2} -f 65001 -o {3}{4}", dbInfo._userId, dbInfo._passwd, dbInfo._ip, outputFileName, Environment.NewLine);
            if (string.IsNullOrEmpty(applyDBName) != true)
                batchCommand = batchCommand + string.Format(@"SET DatabaseName={0}{1}", applyDBName, Environment.NewLine);
            batchCommand = batchCommand + string.Format(@"sqlcmd %SqlCmdOption% -i {0}{1}", executeSqlFileName, Environment.NewLine);
            batchCommand = batchCommand + "if errorlevel 1 goto errexit" + Environment.NewLine;
            batchCommand = batchCommand + "goto end" + Environment.NewLine;
            batchCommand = batchCommand + ":errexit" + Environment.NewLine;
            if (string.IsNullOrEmpty(failMessage) != true && failMessage.Length > 0)
                batchCommand = batchCommand + "echo " + failMessage + Environment.NewLine;
            batchCommand = batchCommand + "goto end" + Environment.NewLine;
            batchCommand = batchCommand + ":end" + Environment.NewLine;
            batchCommand = batchCommand + "@echo on" + Environment.NewLine;
            File.WriteAllText(_batchFileName, batchCommand);
            File.SetAttributes(_batchFileName, FileAttributes.Hidden);
        }

        //데이타 베이스 백업 쿼리 만들기.
        void CreateBackupDBQuery()
        {
            string queryCommand = string.Empty;
            queryCommand = queryCommand + "GO" + Environment.NewLine;
            queryCommand = queryCommand + "SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;" + Environment.NewLine;
            queryCommand = queryCommand + "SET NUMERIC_ROUNDABORT OFF;" + Environment.NewLine;
            queryCommand = queryCommand + "GO" + Environment.NewLine;
            queryCommand = queryCommand + ":on error exit" + Environment.NewLine;
            queryCommand = queryCommand + ":setvar __IsSqlCmdEnabled \"True\"" + Environment.NewLine;
            queryCommand = queryCommand + "GO" + Environment.NewLine;
            queryCommand = queryCommand + "IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True' " + Environment.NewLine;
            queryCommand = queryCommand + "BEGIN" + Environment.NewLine;
            queryCommand = queryCommand + "PRINT N'To use the Database Engine Query Editor to write or edit SQLCMD scripts, you must enable the SQLCMD scripting mode.';" + Environment.NewLine;
            queryCommand = queryCommand + "SET NOEXEC ON;" + Environment.NewLine;
            queryCommand = queryCommand + "END" + Environment.NewLine;
            queryCommand = queryCommand + "GO" + Environment.NewLine;
            queryCommand = queryCommand + "USE[master];" + Environment.NewLine;
            queryCommand = queryCommand + "GO" + Environment.NewLine;
            queryCommand = queryCommand + "EXEC[master].[dbo].[sspFullBackup] N'$(DatabaseName)';" + Environment.NewLine;
            queryCommand = queryCommand + "GO" + Environment.NewLine;
            File.WriteAllText(GetBackupDBSqlFileName(), queryCommand);
            File.SetAttributes(GetBackupDBSqlFileName(), FileAttributes.Hidden);
        }

        //데이타 베이스 백업 쿼리 만든 것 지우기.
        void DeleteBackupDBQuery()
        {
            FileInfo fileInfo = new FileInfo(GetBackupDBSqlFileName());
            if (fileInfo.Exists == true)
                fileInfo.Delete();
            fileInfo = null!;
        }
    }
}
