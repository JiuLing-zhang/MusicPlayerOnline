using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Service
{
    public interface IMyFavoriteService
    {
        Task<List<MyFavorite>> GetMyFavoriteList();

        Task<(bool Succeed, string Message)> AddToMyFavorite(MusicDetail music, string myFavoriteId);
    }
}
