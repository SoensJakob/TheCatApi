using EO_JS.Models;
using EO_JS.Repositories;
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
    public partial class OverviewPage : ContentPage
    {
        //Global Properties
        public String Origin { get; set; }
        public String User { get; set; }
        public List<Breed> Breeds { get; set; }
        public List<String> Origins { get; set; }
        public OverviewPage(String origin, String user, List<Breed> breeds, List<String> origins)
        {
            InitializeComponent();
            //Set global properties
            this.Breeds = breeds;
            this.Origin = origin;
            this.User = user;
            this.Origins = origins;

            ShowPicker();
            ShowBreeds();
            
            pickOrigin.SelectedIndexChanged += PickOrigin_SelectedIndexChanged;
            
        }

        //Onchange picker load new breed list
        private void PickOrigin_SelectedIndexChanged(object sender, EventArgs e)
        {
            Origin = pickOrigin.Items[pickOrigin.SelectedIndex];
            ShowBreeds();
        }

        //Fill picker with global property Origins
        private void ShowPicker()
        {
            pickOrigin.ItemsSource = Origins;
            int index = Origins.FindIndex(x => x.StartsWith(Origin));
            pickOrigin.SelectedIndex = index;
        }

        //Get Breed list from api and show it in listview
        private async void ShowBreeds()
        {
            List<Breed> breeds = await CatRepository.GetBreedsByOriginAsync(Breeds, Origin);

            lvwBreed.ItemsSource = breeds;
        }

        //Onclick btnBack go to previous page
        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        
        //Onclick logoutbtn go back to loginpage
        private void Home_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        //Onclick Favouritebtn go to favouritepage
        private void Favourite_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FavouritePage(User));
        }
    

        //Onclick Listview item go to detailpage with selected breed as parameter
        private void LvwBreed_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            

            if (lvwBreed.SelectedItem != null)
            {

                Breed selected = (Breed)lvwBreed.SelectedItem;

                Navigation.PushAsync(new DetailPage(selected.Id, User));
                // DESELECT item
                lvwBreed.SelectedItem = null;
            }
        }
    }

}