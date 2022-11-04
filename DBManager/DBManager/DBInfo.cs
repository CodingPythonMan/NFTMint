namespace DBCore
{
    //모든 데이타베이스의 설정에 사용되는 클래스. ( 예를 들어서 데이터베이스 이름의 접두어,접미어 역활. )
    public class DBName
    {
        // ADB 데이타 베이스 이름.
        public string _adbName = string.Empty;

        // GDB 데이타 베이스 이름.
        public string _gdbName = string.Empty;

        public string _additionName = string.Empty;
        public DBName()
        {
            _adbName = "ProjectR_ADB";
            _gdbName = "ProjectR_GDB";
            _additionName = "DEV";
        }
    }

    //실질적으로 하나의 데이터베이스의 설정값 정보를 가지고 있는 클래스.
    public class DBInfo
    {
        public int _id = 0;

        //스키마를 가지고 있는 데이타베이스 
        //데이타베이스 업데이트 순서는 
        // 1._realDBName의 데이타 베이스를 백업
        // 2._defDBName데이타 베이스를 날리고 스크립트에 있는 최신 데이타베이스로 생성을 새로 한다.
        // 3._realDBName과 _defDBName을 비교해서 _realDBName를 타겟으로 하는 update쿼리를 만든다.
        // 4._realDBName를 타겟으로 하는 update쿼리를 실행한다.

        //업데이트를 할때 사용될 최신 스키마로 유지되는 데이타베이스 이름 
        //규칙 : _DEV , _LIVE 등이 없는 것이다.
        public string _defDBName = string.Empty;

        //데이타베이스 이름 뒤에 붙는 접미사
        public string _additionName = string.Empty;

        //string.Format("{0}_{1}_{2}", _dbName._gdbName, dbInfo._worldId.ToString("D3"), dbInfo._additionName);
        //GDB_000_DEV 같은 실제 MSSQL에 설치되어진 Database 이름.
        public string _realDBName = string.Empty;
        public int _worldId = 0;
        public string _ip = string.Empty;
        public string _userId = string.Empty;
        public string _passwd = string.Empty;
    }
    public class DBInfoGroup
    {
        public string _useTest { get; set; } = null!;
        public List<DBInfo> _dbList = new List<DBInfo>();
        public void Clear() { _dbList.Clear(); }
    }
}