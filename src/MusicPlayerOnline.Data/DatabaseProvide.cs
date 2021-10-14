using System;
using MusicPlayerOnline.Model;
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
        private static SQLiteAsyncConnection _database;
        public static SQLiteAsyncConnection Database
        {
            get
            {
                if (_database == null)
                {
                    if (string.IsNullOrEmpty(_dbPath))
                    {
                        throw new Exception("非法的数据库路径");
                    }

                    _database = new SQLiteAsyncConnection(_dbPath);
                    InitTable();
                }
                return _database;
            }
        }
        private static void InitTable()
        {
            Database.CreateTableAsync<MusicDetail>().Wait();
            Database.CreateTableAsync<Playlist>().Wait();
            Database.CreateTableAsync<MyFavorite>().Wait();
            Database.CreateTableAsync<MyFavoriteDetail>().Wait();

            Database.CreateTableAsync<GeneralConfig>().Wait();
            Database.CreateTableAsync<PlatformConfig>().Wait();
            Database.CreateTableAsync<PlayConfig>().Wait();
            Database.CreateTableAsync<PlayerConfig>().Wait();

            //配置表不存在时创建
            if (Database.Table<GeneralConfig>().CountAsync().Result == 0)
            {
                Database.InsertAsync(new GeneralConfig());
            }
            if (Database.Table<PlatformConfig>().CountAsync().Result == 0)
            {
                Database.InsertAsync(new PlatformConfig());
            }
            if (Database.Table<PlayConfig>().CountAsync().Result == 0)
            {
                Database.InsertAsync(new PlayConfig());
            }
            if (Database.Table<PlayerConfig>().CountAsync().Result == 0)
            {
                Database.InsertAsync(new PlayerConfig());
            }
        }
    }
}
