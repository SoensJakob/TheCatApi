using EO_JS.Helper;
using EO_JS.Models;
using EO_JS.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EO_JS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        //Global Properties
        public String BreedId { get; set; }
        public Favourite Firstobj { get; set; }
        public String User { get; set; }
        public List<Favourite> AllImages { get; set; }

        public DetailPage(String id, String user)
        {
            InitializeComponent();
            this.User = user;
            this.BreedId = id;

            LoadData();

            //when message "PopAsync recieved from FavouritePage do a popasync to previouspage"
            MessagingCenter.Subscribe<FavouritePage>(this, "PopAsync", (sender) => {
                Navigation.PopAsync();
            });
        }

        //Load in data from global properties
        private void LoadData()
        {
            ShowImages();
            ShowData();
        }

        //Show the detailed data about the breed
        private async void ShowData()
        {
            BreedInfo data = await CatRepository.GetBreedByIdAsync(BreedId);

            lblTitle.Text = data.Name;
            lblDesc.Text = data.Description;
            lblTemperament.Text = data.Temperament;
            Debug.WriteLine(data.Temperament);
            lblLifespan.Text = data.Lifespan + "Y";
            lblWeight.Text = data.WeightMetric + "kg";
            imgAffection.Source = GetImageSource(data.Affection);
            imgIntelligence.Source = GetImageSource(data.Intelligence);
            imgEnergy.Source = GetImageSource(data.Energy);
            imgSocial.Source = GetImageSource(data.Social);
            imgVocalisation.Source = GetImageSource(data.Vocalisation);
        }

        //helper method to select right picture according to the data for "Affection","Intelligence","Energy","Social","Vocalisation"
        private ImageSource GetImageSource(int value)
        {
            return ImageSource.FromResource($"EO_JS.Assets.{value}.png");
        }
        
        // Get images from api and set them in carouselview
        private async void ShowImages()
        {
            List<Favourite> favourites = await CatRepository.GetFavouritesAsync(User);
            List<ImageCat> images = await CatRepository.GetImagesByBreedAsync(BreedId);
            List<Favourite> newImages = new List<Favourite>();


            foreach (ImageCat i in images)
            {
                Favourite favourite = new Favourite
                {
                    Image = i
                };

                foreach (Favourite f in favourites)
                {
                    
                    if (f.Image.Id == i.Id)
                    {
                        favourite.FavouriteId = f.FavouriteId;
                        favourite.User = f.User;
                        favourite.Icon = IconFont.Heart;
                    }
                    

                }
                newImages.Add(favourite);
            }
            AllImages = newImages;

            Firstobj = newImages[0];
            IvwCat.ItemsSource = newImages;
        }

        //Onclick go back to previous page
        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        //Onclick FavImagebtn Send image to favourite list in the api and change the icon 
        private async void Btn_favImage_Clicked(object sender, EventArgs e)
        {

            Favourite image = (Favourite)IvwCat.CurrentItem;
            if (image == null)
            {
                image = Firstobj;
            }

            
            Image img = (Image)sender;
            FontImageSource imageSource = (FontImageSource)img.Source;


            if (image.Icon == IconFont.HeartOutline)
            {
                image.Icon = IconFont.Heart;

                imageSource.Glyph = IconFont.Heart;
                JObject json = new JObject(
                new JProperty("image_id", image.Image.Id),
                new JProperty("sub_id", User));

                image.FavouriteId = await CatRepository.AddFavouriteAsync(json);
            }
            else if (image.Icon == IconFont.Heart)
            {
                image.Icon = IconFont.HeartOutline;

                imageSource.Glyph = IconFont.HeartOutline;
                
                await CatRepository.DeleteFavouriteAsync(image.FavouriteId);
            }

           
        }

        //Onclick logoutbtn go back to loginpage
        private void Home_Tapped(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }

        //Onclick Favouritebtn go to favouritepage 
        private void Favourite_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FavouritePage(User, true));
        }
        //Onclick searchbtn go to searchpage
        private void Search_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}