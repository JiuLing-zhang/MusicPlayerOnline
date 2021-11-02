using System;
using System.Linq;
using MusicPlayerOnline.Model.Model;
using SQLite;

namespace MusicPlayerOnline.Data
{
    public class DatabaseProvide
    {
        private static string _dbPath = "";
        public static void SetConnection(string dbPath)
        {
            _dbPath = dbPath;
        }
        private static SQLiteConnection _database;
        public static SQLiteConnection Database
        {
            get
            {
                if (_database == null)
                {
                    if (string.IsNullOrEmpty(_dbPath))
                    {
                        throw new Exception("非法的数据库路径");
                    }

                    _database = new SQLiteConnection(_dbPath);
                    InitTable();
                }
                return _database;
            }
        }

        private static void InitTable()
        {
            Database.CreateTable<MusicDetail>();
            Database.CreateTable<Playlist>();
            Database.CreateTable<MyFavorite>();
            Database.CreateTable<MyFavoriteDetail>();
            Database.CreateTable<LogDetail>();

            Database.CreateTable<GeneralConfig>();
            Database.CreateTable<SearchConfig>();
            Database.CreateTable<PlayConfig>();
            Database.CreateTable<PlayerConfig>();

            //配置表不存在时创建
            if (!Database.Table<GeneralConfig>().Any())
            {
                Database.Insert(new GeneralConfig());
            }
            if (!Database.Table<SearchConfig>().Any())
            {
                Database.Insert(new SearchConfig());
            }
            if (!Database.Table<PlayConfig>().Any())
            {
                Database.Insert(new PlayConfig());
            }
            if (!Database.Table<PlayerConfig>().Any())
            {
                Database.Insert(new PlayerConfig());
            }
        }

        private static SQLiteAsyncConnection _databaseAsync;
        public static SQLiteAsyncConnection DatabaseAsync
        {
            get
            {
                if (_databaseAsync == null)
                {
                    if (string.IsNullOrEmpty(_dbPath))
                    {
                        throw new Exception("非法的数据库路径");
                    }

                    _databaseAsync = new SQLiteAsyncConnection(_dbPath);
                    InitTableAsync();
                }
                return _databaseAsync;
            }
        }
        private static void InitTableAsync()
        {
            DatabaseAsync.CreateTableAsync<MusicDetail>().Wait();
            DatabaseAsync.CreateTableAsync<Playlist>().Wait();
            DatabaseAsync.CreateTableAsync<MyFavorite>().Wait();
            DatabaseAsync.CreateTableAsync<MyFavoriteDetail>().Wait();
            DatabaseAsync.CreateTableAsync<LogDetail>().Wait();

            DatabaseAsync.CreateTableAsync<GeneralConfig>().Wait();
            DatabaseAsync.CreateTableAsync<SearchConfig>().Wait();
            DatabaseAsync.CreateTableAsync<PlayConfig>().Wait();
            DatabaseAsync.CreateTableAsync<PlayerConfig>().Wait();

            //配置表不存在时创建
            if (DatabaseAsync.Table<GeneralConfig>().CountAsync().Result == 0)
            {
                DatabaseAsync.InsertAsync(new GeneralConfig()).Wait();
            }
            if (DatabaseAsync.Table<SearchConfig>().CountAsync().Result == 0)
            {
                DatabaseAsync.InsertAsync(new SearchConfig()).Wait();
            }
            if (DatabaseAsync.Table<PlayConfig>().CountAsync().Result == 0)
            {
                DatabaseAsync.InsertAsync(new PlayConfig()).Wait();
            }
            if (DatabaseAsync.Table<PlayerConfig>().CountAsync().Result == 0)
            {
                DatabaseAsync.InsertAsync(new PlayerConfig()).Wait();
            }
        }

        private class LogDetail
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }
            public Int64 Timestamp { get; set; }
            public LogTypeEnum LogType { get; set; }
            public string Message { get; set; }
        }
        private enum LogTypeEnum
        {
            消息 = 0,
            警告 = 1,
            错误 = 2
        }
    }
}
