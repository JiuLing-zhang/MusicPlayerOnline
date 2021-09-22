﻿using System;
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
                }
                return _database;
            }
        }
        public static void InitTable()
        {
            Database.CreateTableAsync<MusicDetail>().Wait();
            Database.CreateTableAsync<Playlist>().Wait();
        }
    }
}
