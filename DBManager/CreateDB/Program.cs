﻿using DBCore;

string errorMessage = string.Empty;
DBCore.DBInfoJson _dbInfoJson = new DBCore.DBInfoJson();
if (_dbInfoJson.LoadJson(ref errorMessage) == false)
{
    Console.WriteLine("Error");
}

List<string> ah = _dbInfoJson.GetDBList();

foreach(string a in ah)
{
    Console.WriteLine(a);
}

if (args.Length == 0)
{
    Console.WriteLine("CreateDB [ DB Name ]");
    List<string> dbList = _dbInfoJson.GetDBList();
    foreach (string dbName in dbList)
        Console.WriteLine(dbName);
    return;
}

DBCore.DBManager _dbPatcher = new DBCore.DBManager();
if (_dbPatcher.IsEnableDBPatcher(_dbInfoJson.GetDBInfoGroup()) == false)
{
    Console.WriteLine("Not Installed sqlpackage.exe");
}

if (args[0].ToLower().Equals("_server_") == true)
{
    _dbPatcher.CreateDB();
}
else
{
    _dbPatcher.CreateDB(args[0]);
}