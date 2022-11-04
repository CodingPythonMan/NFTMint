using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCore
{
    public partial class DBManager
    {
        public bool CreateDB()
        {
            foreach (DBInfo dbInfo in _dbInfoGroup._dbList)
            {
                if (CreateDB(dbInfo) == false)
                    return false;
            }
            return true;
        }

        public bool CreateDB(string dbName)
        {
            foreach (DBInfo dbInfo in _dbInfoGroup._dbList)
            {
                if (dbInfo._realDBName.ToLower().Equals(dbName.ToLower()) == true)
                {
                    if (CreateDB(dbInfo) == false)
                        return false;
                    return true;
                }
            }
            return false;
        }

        bool CreateDB(DBInfo dbInfo)
        {
            bool result = false;

            string sqlFileName = GetCreateDefaultDBSqlFIleName(dbInfo._defDBName);
            string logFileName = GetCreateDefaultDBLogFileName(dbInfo._defDBName);
            string dacpacFileName = GetDacpacFileName(dbInfo._realDBName);
            do
            {
                StartConsoleMessage(_createDB, dbInfo._realDBName);

                //Databases.sln 에서 생성된 쿼리를 실행해주는 배치 파일 만들기.                
                CreateSqlCmdBatchFile(dbInfo, string.Empty, sqlFileName, logFileName, _failCreateDB);
                //배치파일 실행을 해서 데이타베이스 생성하기.
                result = CheckCmd(_failCreateDB);
                if (result == false)
                {
                    FailConsoltMessage(_createDB, dbInfo._realDBName, logFileName);
                    break;
                }

                //dacpac 파일 추출.
                CreateExportDACPACBatchFile(dbInfo, dbInfo._defDBName, dacpacFileName);
                //생성된 데이타베이스 정보 얻기( 스키마 , 프로시져 등등 .. )
                result = CheckCmd(_failExportDACPAC);
                if (result == false)
                {
                    FailConsoltMessage(_createDB, dbInfo._realDBName, logFileName);
                    break;
                }

                //추출된 dacpac 파일로 해당 DB 의 생성 스크립트 만들기.
                sqlFileName = GetCreateRealDBSqlFileName(dbInfo._realDBName);
                CreateExportCreateSqlFile(dbInfo, dacpacFileName, sqlFileName);
                result = CheckCmd(_failExportCreateSqlFile);
                if (result == false)
                {
                    FailConsoltMessage(_createDB, dbInfo._realDBName);
                    break;
                }

                logFileName = GetCreateRealDBLogFileName(dbInfo._realDBName);
                CreateSqlCmdBatchFile(dbInfo, string.Empty, sqlFileName, logFileName, _failCreateDB);
                result = CheckCmd(_failCreateDB);
                if (result == false)
                {
                    FailConsoltMessage(_createDB, dbInfo._realDBName, logFileName);
                    break;
                }
                else
                {
                    SuccessConsoleMessage(_createDB, dbInfo._realDBName);
                }

                result = true;
            } while (false);
            return result;
        }
    }
}
