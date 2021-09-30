using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Service
{
    public interface IMyFavoriteService
    {
        Task<MyFavorite> GetMyFavorite(string id);
        Task Add(MyFavorite myFavorite);
        Task Update(MyFavorite myFavorite);
        Task<(bool Succeed, string Message)> DeleteMyFavorite(string id);

        Task<List<MyFavorite>> GetMyFavoriteList();

        Task<(bool Succeed, string Message)> AddToMyFavorite(MusicDetail music, string myFavoriteId);
    }
}
