using EO_JS.Models;
using EO_JS.Repositories;
using EO_JS.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EO_JS
{
    public partial class MainPage : ContentPage
    {
        //Global Properties
        public String Origin { get; set; }
        public String User { get; set; }
        public String Password { get; set; }
        public List<Breed> Breeds { get; set; }
        public List<String> Origins { get; set; }

        public MainPage()
        {
            InitializeComponent();
            
            LoadBreeds();
            btnLogin.Clicked += BtnLogin_Clicked;
            btnRegister.Clicked += BtnRegister_Clicked;
        }

        //OnClick btnRegister
        private async void BtnRegister_Clicked(object sender, EventArgs e)
        {
            try
            {
                User = entUsername.Text;
                Password = entPassword.Text;
                if (User != null || User == "")
                {
                    if (Password != null || Password == "")
                    {
                        String hash = ComputeSha256Hash(Password);
                        UserInfo userInfo = new UserInfo
                        {
                            Name = User,
                            Password = hash
                        };
                        UserInfo user = await CatRepository.GetUserInformation(User);
                        if (user is null)
                        {
                            await CatRepository.AddUser(userInfo);
                            await DisplayAlert("Alert", "Account created,\nYou can now login ", "Ok");
                        }
                        else
                        {
                            await DisplayAlert("Error", "User already exists,\nPlease choose a different name ", "Ok");
                        }
                           
                    }
                    else
                    {
                        await DisplayAlert("Error", "Please enter a password,\nPlease try again", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Please enter a username,\nPlease try again", "Ok");
                }

                

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);    
            }
        }

        //OnClick btnLogin
        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                //Set global user and password
                User = entUsername.Text;
                Password = entPassword.Text;

                //Get User information from api
                UserInfo userInfo = await CatRepository.GetUserInformation(User);

                //If use does not exist
                if (userInfo is null)
                {
                    await DisplayAlert("Error","Incorrect Username,\nPlease register your account ", "Ok");
                }
                else
                {
                    //Check password for null values
                    if (Password != null || Password == "")
                    {
                        //Hash entered password and check with password from api
                        String hash = ComputeSha256Hash(Password);
                        if (hash == userInfo.Password)
                        {
                            //Check if picker has been selected
                            if (pickOrigin.SelectedIndex == -1)
                            {
                                await DisplayAlert("Error", "Please select an origin", "Ok");

                            }
                            else
                            {
                                //Succesfully login and go to next page
                                Origin = pickOrigin.Items[pickOrigin.SelectedIndex];
                                await Navigation.PushAsync(new OverviewPage(Origin, User, Breeds, Origins));
                            }

                        }
                        else
                        {
                            await DisplayAlert("Error", "Password incorrect,\nPlease try again", "Ok");

                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Please enter your password", "Ok");

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex); 
            }
            
        }

        //Get data from api and put it in global property Breeds
        private async void LoadBreeds()
        {
            Breeds = await CatRepository.GetBreedsAsync();
            ShowPicker();
            btnLogin.IsEnabled = true;
            btnRegister.IsEnabled = true;
        }

        //Get data for picker
        private void ShowPicker()
        {
            Origins = CatRepository.GetOriginsAsync(Breeds);
            Origins.Sort();
            pickOrigin.ItemsSource = Origins;
        }

        //Hashing method
        public static string ComputeSha256Hash(string value)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                Encoding enc = Encoding.UTF8;
                byte[] bytes = sha256Hash.ComputeHash(enc.GetBytes(value));

                // Convert byte array to a string   
                StringBuilder sb = new StringBuilder();
                foreach (Byte b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
