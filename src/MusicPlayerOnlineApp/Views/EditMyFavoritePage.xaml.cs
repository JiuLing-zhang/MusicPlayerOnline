using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnlineApp.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditMyFavoritePage : PopupPage
    {
        private readonly EditMyFavoritePageViewModel _myModel = new EditMyFavoritePageViewModel();
        private MyFavorite _myFavorite;
        public Action EditFinished;
        public EditMyFavoritePage()
        {
            InitializeComponent();
            BindingContext = _myModel;
        }
        public void Initialize(MyFavorite myFavorite)
        {
            _myFavorite = myFavorite;
            _myModel.MyFavoriteId = myFavorite.Id;
            _myModel.Name = myFavorite.Name;
        }

        private async void BtnRename_Clicked(object sender, EventArgs e)
        {
            string name = _myModel.NewName;
            if (name.IsEmpty())
            {
                DependencyService.Get<IToast>().Show("输入歌单名称");
                return;
            }

            _myFavorite.Name = name;
            var count = await DatabaseProvide.Database.UpdateAsync(_myFavorite);
            if (count == 0)
            {
                DependencyService.Get<IToast>().Show("保存失败");
                return;
            }
            await Navigation.PopPopupAsync();
            EditFinished.Invoke();
            DependencyService.Get<IToast>().Show("重命名成功");
        }

        private async void BtnRemove_Clicked(object sender, EventArgs e)
        {
            var isOk = await DisplayAlert("提示", "确定要删除该歌单吗？", "确定", "取消");
            if (isOk == false)
            {
                return;
            }
            var count = await DatabaseProvide.Database.DeleteAsync<MyFavorite>(_myFavorite.Id);
            if (count == 0)
            {
                DependencyService.Get<IToast>().Show("删除失败");
                return;
            }
            
            await DatabaseProvide.Database.Table<MyFavoriteDetail>().DeleteAsync(m => m.MyFavoriteId == _myFavorite.Id);

            await Navigation.PopPopupAsync();
            EditFinished.Invoke();
            DependencyService.Get<IToast>().Show("删除成功");
        }
    }
}