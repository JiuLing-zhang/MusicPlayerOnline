﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Service
{
    public class MyFavoriteService : IMyFavoriteService
    {
        public async Task<MyFavorite> GetMyFavorite(string id)
        {
            return await DatabaseProvide.DatabaseAsync.Table<MyFavorite>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<MyFavorite> GetMyFavoriteByName(string name)
        {
            return await DatabaseProvide.DatabaseAsync.Table<MyFavorite>().Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task Add(MyFavorite myFavorite)
        {
            if (await DatabaseProvide.DatabaseAsync.Table<MyFavorite>().Where(x => x.Name == myFavorite.Name).CountAsync() > 0)
            {
                return;
            }

            await DatabaseProvide.DatabaseAsync.InsertAsync(myFavorite);
        }

        public async Task Update(MyFavorite myFavorite)
        {
            await DatabaseProvide.DatabaseAsync.UpdateAsync(myFavorite);
        }

        public async Task<(bool Succeed, string Message)> DeleteMyFavorite(string id)
        {
            var count = await DatabaseProvide.DatabaseAsync.DeleteAsync<MyFavorite>(id);
            if (count == 0)
            {
                return (false, "删除失败");
            }

            await DatabaseProvide.DatabaseAsync.Table<MyFavoriteDetail>().DeleteAsync(m => m.MyFavoriteId == id);
            return (true, "删除成功");
        }

        public async Task<List<MyFavorite>> GetMyFavoriteList()
        {
            return await DatabaseProvide.DatabaseAsync.Table<MyFavorite>().ToListAsync();
        }

        public async Task<(bool Succeed, string Message)> AddToMyFavorite(MusicDetail music, string myFavoriteId)
        {
            if (await DatabaseProvide.DatabaseAsync.Table<MyFavoriteDetail>().Where(x => x.MyFavoriteId == myFavoriteId && x.MusicId == music.Id).CountAsync() > 0)
            {
                return (false, "不能重复添加");
            }
            string id = Guid.NewGuid().ToString("d");
            var obj = new MyFavoriteDetail()
            {
                Id = id,
                MyFavoriteId = myFavoriteId,
                MusicId = music.Id,
                Platform = music.Platform,
                Name = music.Name,
                Artist = music.Artist,
                Album = music.Album
            };
            await DatabaseProvide.DatabaseAsync.InsertAsync(obj);

            //添加后，歌曲数量+1
            var myFavorite = await DatabaseProvide.DatabaseAsync.GetAsync<MyFavorite>(myFavoriteId);
            myFavorite.MusicCount++;
            if (myFavorite.ImageUrl.IsEmpty())
            {
                myFavorite.ImageUrl = music.ImageUrl;
            }

            await DatabaseProvide.DatabaseAsync.UpdateAsync(myFavorite);
            return (true, "添加成功");
        }

        public async Task<List<MyFavoriteDetail>> GetMyFavoriteDetail(string myFavoriteId)
        {
            return await DatabaseProvide.DatabaseAsync.Table<MyFavoriteDetail>().Where(x => x.MyFavoriteId == myFavoriteId).ToListAsync();
        }
    }
}
