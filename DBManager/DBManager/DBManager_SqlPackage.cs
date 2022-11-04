using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCore
{
    public partial class DBManager
    {
        void CreateExportDACPACBatchFile(DBInfo dbInfo, string applyDBName, string dacpacFileName)
        {
            string batchCommand = string.Empty;
            batchCommand = batchCommand + "@echo off" + Environment.NewLine;
            batchCommand = batchCommand + "@chcp 850" + Environment.NewLine;
            batchCommand = batchCommand + string.Format(@"sqlpackage /Action:Extract /SourceServerName:{0} /SourceDatabaseName:{1} /SourceUser:{2} /SourcePassword:{3} /TargetFile:{4}{5}",
                dbInfo._ip, applyDBName, dbInfo._userId, dbInfo._passwd, dacpacFileName, Environment.NewLine);
            batchCommand = batchCommand + "if errorlevel 1 goto errexit" + Environment.NewLine;
            batchCommand = batchCommand + "goto end" + Environment.NewLine;
            batchCommand = batchCommand + ":errexit" + Environment.NewLine;
            batchCommand = batchCommand + "echo " + _failExportDACPAC + Environment.NewLine;
            batchCommand = batchCommand + "goto end" + Environment.NewLine;
            batchCommand = batchCommand + ":end" + Environment.NewLine;
            batchCommand = batchCommand + "@echo on" + Environment.NewLine;
            File.WriteAllText(_batchFileName, batchCommand);
            File.SetAttributes(_batchFileName, FileAttributes.Hidden);
        }

        void CreateExportCreateSqlFile(DBInfo dbInfo, string dacpacFileName, string createSqlFileName)
        {
            string option1 = "/p:CreateNewDatabase=True";

            string batchCommand = string.Empty;
            batchCommand = batchCommand + "@echo off" + Environment.NewLine;
            batchCommand = batchCommand + string.Format(@"sqlpackage /Action:Script /SourceFile:{0} /TargetServerName:{1} /TargetDatabaseName:{2} /TargetUser:{3} /TargetPassword:{4} /OutputPath:{5} {6} {7}",
                dacpacFileName, dbInfo._ip, dbInfo._realDBName, dbInfo._userId, dbInfo._passwd, createSqlFileName, option1, Environment.NewLine);
            batchCommand = batchCommand + "if errorlevel 1 goto errexit" + Environment.NewLine;
            batchCommand = batchCommand + "goto end" + Environment.NewLine;
            batchCommand = batchCommand + ":errexit" + Environment.NewLine;
            batchCommand = batchCommand + "echo " + _failExportCreateSqlFile + Environment.NewLine;
            batchCommand = batchCommand + "goto end" + Environment.NewLine;
            batchCommand = batchCommand + ":end" + Environment.NewLine;
            batchCommand = batchCommand + "@echo on" + Environment.NewLine;
            File.WriteAllText(_batchFileName, batchCommand);
            File.SetAttributes(_batchFileName, FileAttributes.Hidden);
        }

        void CreateExportUpdateSqlFile(DBInfo dbInfo, string dacpacFileName, string updateSqlFileName)
        {
            string option1 = "/p:DropObjectsNotInSource=True";
            string option2 = "/p:BlockOnPossibleDataLoss=false";
            string option3 = "/p:CommentOutSetVarDeclarations=true";

            string batchCommand = string.Empty;
            batchCommand = batchCommand + "@echo off" + Environment.NewLine;
            batchCommand = batchCommand + "@chcp 850" + Environment.NewLine;
            batchCommand = batchCommand + string.Format(@"sqlpackage /Action:Script /SourceFile:{0} /TargetServerName:{1} /TargetDatabaseName:{2} /TargetUser:{3} /TargetPassword:{4} /OutputPath:{5} {6} {7} {8} {9}",
                dacpacFileName, dbInfo._ip, dbInfo._realDBName, dbInfo._userId, dbInfo._passwd, updateSqlFileName, option1, option2, option3, Environment.NewLine);
            batchCommand = batchCommand + "if errorlevel 1 goto errexit" + Environment.NewLine;
            batchCommand = batchCommand + "goto end" + Environment.NewLine;
            batchCommand = batchCommand + ":errexit" + Environment.NewLine;
            batchCommand = batchCommand + "echo " + _failExportUpdateSqlFile + Environment.NewLine;
            batchCommand = batchCommand + "goto end" + Environment.NewLine;
            batchCommand = batchCommand + ":end" + Environment.NewLine;
            batchCommand = batchCommand + "@echo on" + Environment.NewLine;
            File.WriteAllText(_batchFileName, batchCommand);
            File.SetAttributes(_batchFileName, FileAttributes.Hidden);
        }
    }
}
