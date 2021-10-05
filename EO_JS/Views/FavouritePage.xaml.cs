using EO_JS.Helper;
using EO_JS.Models;
using EO_JS.Repositories;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EO_JS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavouritePage : ContentPage
    {
        //Global Properties
        public bool IsDetailPrevPage { get; set; }
        public String User { get; set; }
        public FavouritePage(String user, bool isdetailprevpage = false)
        {
            this.User = user;
            // value is true if previous page was detailpage
            this.IsDetailPrevPage = isdetailprevpage;
            InitializeComponent();

            ShowFavourites();

            //On recieve message "RefreshMainPage" from popupfavourite page refresh the favourite list
            MessagingCenter.Subscribe<PopupFavourite>(this, "RefreshMainPage", (sender) => {
                ShowFavourites();
            });
        }


        //Show Favourites with data from global property
        private async void ShowFavourites()
        {
            List<Favourite> favourites = await CatRepository.GetFavouritesAsync(User);
            foreach(Favourite f in favourites)
            {
                f.Icon = IconFont.Heart;
            }
            BindableLayout.SetItemsSource(flexLayout, favourites);
        }

        //Onclick go back to previous page
        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }


        //Onclick image in listview create popup page with that image
        private async void Image_Tapped(object sender, EventArgs e)
        {
            Favourite fav = (Favourite)((Image)sender).BindingContext;
            await PopupNavigation.Instance.PushAsync(new PopupFavourite(fav));
        }

        //Onclick logoutbtn go back to loginpage
        private void Home_Tapped(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }

        //Onclick searchbtn go back to search page
        private async void Search_Tapped(object sender, EventArgs e)
        {
            //Check if detailpage was previous page if popasync twice
            if (IsDetailPrevPage)
            {
                MessagingCenter.Send<FavouritePage>(this, "PopAsync");
                await Navigation.PopAsync();

            }
            // else just go back to previous page
            else
            {
                await Navigation.PopAsync();
            }
        }
    }
}