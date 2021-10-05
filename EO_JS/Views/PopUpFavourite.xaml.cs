using EO_JS.Models;
using EO_JS.Repositories;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EO_JS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupFavourite
    {
        public Favourite Favourite { get; set; }
        public PopupFavourite(Favourite item)
        {
            
            InitializeComponent();
            this.Favourite = item;
            imgSelected.Source = Favourite.Image.Url;
            lblCreated.Text = Favourite.Timestamp.ToString("dddd, dd MMMM yyyy");
        }



        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private async void BtnFav_Tapped(object sender, EventArgs e)
        {
            await CatRepository.DeleteFavouriteAsync(Favourite.FavouriteId);
            MessagingCenter.Send<PopupFavourite>(this, "RefreshMainPage");
            await PopupNavigation.Instance.PopAsync();
        }
    }
}