using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Service
{
    public class MyFavoriteService : IMyFavoriteService
    {
        public async Task<List<MyFavorite>> GetMyFavoriteList()
        {
            return await DatabaseProvide.Database.Table<MyFavorite>().ToListAsync();
        }

        public async Task<(bool Succeed, string Message)> AddToMyFavorite(MusicDetail music, string myFavoriteId)
        {
            if (await DatabaseProvide.Database.Table<MyFavoriteDetail>().Where(x => x.MyFavoriteId == myFavoriteId && x.MusicId == music.Id).CountAsync() > 0)
            {
                //TODO 检查页面返回
                // DependencyService.Get<IToast>().Show("");
                // await Navigation.PopPopupAsync();
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
            int count = await DatabaseProvide.Database.InsertAsync(obj);
            if (count == 0)
            {
                // DependencyService.Get<IToast>().Show("添加失败");
                // await Navigation.PopPopupAsync();
                //TODO 检查页面返回
                return (false, "添加失败");
            }

            //添加后，歌曲数量+1
            var myFavorite = await DatabaseProvide.Database.GetAsync<MyFavorite>(myFavoriteId);
            myFavorite.MusicCount = myFavorite.MusicCount + 1;
            if (myFavorite.ImageUrl.IsEmpty())
            {
                myFavorite.ImageUrl = music.ImageUrl;
            }

            await DatabaseProvide.Database.UpdateAsync(myFavorite);
            return (true, "添加成功");
        }
    }
}
