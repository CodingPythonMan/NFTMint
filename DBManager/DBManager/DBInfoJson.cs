using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCore
{
    public class DBInfoJson
    {
        //DB리스트의 폴더 경로.
        string _dbListDirectory = string.Empty;

        //DB리스트의 설정 파일 이름.
        string _dbListFileName = "DBList";

        //DB리스트의 설정 경로 + 파일명.
        string _dbListSaveFileName = string.Empty;

        //DB리스트가 개발자 모드인지.
        public bool _isLoadDevJson { get; } = false;

        DBInfoGroup _dbInfoGroup = null!;
        DBName _dbName = null!;

        public DBInfoJson()
        {
            _dbListDirectory = @"..\..\Config";
            {
                //DBListDev.json이 있으면 개발자 모드.
                string dbListSaveFileName = _dbListDirectory + @"\" + _dbListFileName + "Dev.json";
                FileInfo fileInfo = new FileInfo(dbListSaveFileName);
                if (fileInfo.Exists == true)
                {
                    _dbListFileName = _dbListFileName + "Dev";
                    _isLoadDevJson = true;
                }
            }
            _dbListSaveFileName = _dbListDirectory + @"\" + _dbListFileName + ".json";
            _dbInfoGroup = new DBInfoGroup();
            _dbName = new DBName();

            Clear();
            DirectoryCheck();
        }

        public void Clear() { _dbInfoGroup.Clear(); }

        void DirectoryCheck()
        {
            // _dbListDirectory 폴더가 없으면 만들어 주자.
            DirectoryInfo directoryInfo = new DirectoryInfo(_dbListDirectory);
            if (directoryInfo.Exists == false)
                directoryInfo.Create();
        }

        // _dbInfoGroup의 모든 DBInfo의 아이디를 1부터 증가된 값으로 셋팅을 해준다.
        void RangeDBId()
        {
            int id = 1;
            foreach (DBInfo dbInfo in _dbInfoGroup._dbList)
                dbInfo._id = id++;
        }

        public List<DBInfo> GetDBInfoList() { return _dbInfoGroup._dbList; }

        //dbId인 아이디의 DBInfo를 얻는 함수.
        public DBInfo GetDBInfo(int dbId)
        {
            DBInfo dbInfo = null!;
            for (int i = 0; i < _dbInfoGroup._dbList.Count; ++i)
            {
                if (_dbInfoGroup._dbList[i]._id == dbId)
                {
                    dbInfo = _dbInfoGroup._dbList[i];
                    break;
                }
            }
            return dbInfo;
        }

        //DBInfoGroup의 DataBase명들만 따로 리스트로 해서 얻는 함수.
        public List<string> GetDBList()
        {
            List<string> dbList = new List<string>();
            for (int i = 0; i < _dbInfoGroup._dbList.Count; ++i)
                dbList.Add(_dbInfoGroup._dbList[i]._realDBName);
            return dbList;
        }

        public DBInfoGroup GetDBInfoGroup() { return _dbInfoGroup; }
        public string GetADBName() { return _dbName._adbName; }
        public string GetGDBName() { return _dbName._gdbName; }

        // realDBName를 가지고 있는 DBInfo가 있는지 체크.
        public bool IsExistDBInfo(string realDBName)
        {
            foreach (DBInfo dbInfo in _dbInfoGroup._dbList)
            {
                if (dbInfo._realDBName.ToLower().Equals(realDBName.ToLower()) == true)
                    return true;
            }
            return false;
        }

        #region [Json]
        //Json 파일을 로딩.
        public bool LoadJson(ref string errorMessage)
        {
            Clear();
            bool result = false;
            try
            {
                FileInfo fileInfo = new FileInfo(_dbListSaveFileName);
                if (fileInfo.Exists == true)
                {
                    _dbInfoGroup = JsonConvert.DeserializeObject<DBInfoGroup>(File.ReadAllText(_dbListSaveFileName))!;
                    result = true;
                }

                if (false == string.IsNullOrEmpty(_dbInfoGroup._useTest))
                {
                    List<DBInfo> _testDBList = new List<DBInfo>();

                    foreach (var dbInfo in _dbInfoGroup._dbList)
                    {
                        _testDBList.Add(new DBInfo()
                        {
                            _additionName = _dbInfoGroup._useTest,
                            _defDBName = dbInfo._defDBName,
                            _id = dbInfo._id + 100,
                            _ip = dbInfo._ip,
                            _passwd = dbInfo._passwd,
                            _realDBName = dbInfo._realDBName.Replace(dbInfo._additionName, _dbInfoGroup._useTest),
                            _userId = dbInfo._userId,
                            _worldId = dbInfo._worldId
                        });
                    }
                    _dbInfoGroup._dbList.AddRange(_testDBList);
                }
            }
            catch (Exception ex)
            {
                result = false;
                errorMessage = ex.Message;
            }
            return result;
        }

        //기존에 있는 파일을 backup 파일로 옮긴다.
        void SaveBackupJson()
        {
            FileInfo fileInfo = new FileInfo(_dbListSaveFileName);
            if (fileInfo.Exists == true)
            {
                string dbListSaveFileName = _dbListDirectory + @"\" + string.Format("{0:yyyy_MM_dd_HHmmss_ffff}", DateTime.Now) + "_" + _dbListFileName + ".json.bak";
                fileInfo.MoveTo(dbListSaveFileName);
            }
        }

        //_dbInfoGroup의 내용을 _dbListSaveFileName 경로의 파일이름의 json으로 저장을 한다.
        public bool SaveJson(ref string errorMessage)
        {
            bool result = false;
            // 기존에 있는 파일을 backup 파일로 옮긴다.
            SaveBackupJson();
            //_dbInfoGroup의 모든 DBInfo의 아이디를 1부터 증가된 값으로 셋팅을 해준다.
            RangeDBId();
            try
            {
                File.WriteAllText(_dbListSaveFileName, JsonConvert.SerializeObject(_dbInfoGroup));
                result = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return result;
        }

        //초기화된 기본 json파일 생성하는 함수.
        public void SaveClearJson()
        {
            // 기존에 있는 파일을 backup 파일로 옮긴다.
            SaveBackupJson();
            Clear();
            {
                DBInfo dbInfo = new DBInfo();
                dbInfo._id = 1;
                dbInfo._worldId = 0;
                dbInfo._defDBName = _dbName._adbName;
                dbInfo._additionName = _dbName._additionName;
                dbInfo._realDBName = string.Format("{0}_{1}", _dbName._adbName, _dbName._additionName);
                _dbInfoGroup._dbList.Add(dbInfo);
            }
            {
                DBInfo dbInfo = new DBInfo();
                dbInfo._id = 2;
                dbInfo._worldId = 1;
                dbInfo._defDBName = _dbName._gdbName;
                dbInfo._additionName = _dbName._additionName;
                dbInfo._realDBName = string.Format("{0}_{1}_{2}", _dbName._gdbName, dbInfo._worldId.ToString("D3"), dbInfo._additionName);
                _dbInfoGroup._dbList.Add(dbInfo);
            }
            File.WriteAllText(_dbListSaveFileName, JsonConvert.SerializeObject(_dbInfoGroup, Formatting.Indented));
        }
        #endregion [Json]     
        #region [Change DBInfo]
        //DBInfo 추가. GDB 만 가능하다.
        public void InsertDBInfo(DBInfo dbInfo)
        {
            _dbInfoGroup._dbList.Add(dbInfo);
            RangeDBId();
        }
        //DBInfo 업데이트.
        public void UpdateDBInfo(DBInfo dbInfo)
        {
            for (int i = 0; i < _dbInfoGroup._dbList.Count; ++i)
            {
                if (_dbInfoGroup._dbList[i]._id == dbInfo._id)
                {
                    _dbInfoGroup._dbList[i] = dbInfo;
                    break;
                }
            }
            RangeDBId();
        }
        //DBInfo 삭제.
        public void DeleteDBInfo(int dbId)
        {
            for (int i = 0; i < _dbInfoGroup._dbList.Count; ++i)
            {
                if (_dbInfoGroup._dbList[i]._id == dbId)
                {
                    _dbInfoGroup._dbList.RemoveAt(i);
                    break;
                }
            }
            RangeDBId();
        }
        #endregion [Change DBInfo]        
    }
}
