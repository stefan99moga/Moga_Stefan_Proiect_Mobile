using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Moga_Stefan_Proiect.Services;

namespace Moga_Stefan_Proiect.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HartaPage : ContentPage
    {
        public HartaPage()
        {
            InitializeComponent();
            GetCurrentLocation();
            GetOrderPinLocations();
        }
        public async void GetCurrentLocation()
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location != null)
            {
                Position position = new Position(location.Latitude, location.Longitude);
                MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(10));
                harta.MoveToRegion(mapSpan);
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
            }
        }
        public async void GetOrderPinLocations()
        {
            var order = await OrderService.GetOrder();
            
            if(order != null)
            {
                foreach(var item in order)
                {
                    Pin OrderPins = new Pin()
                    {
                        Label = item.OrderNumber.ToString() + " - " + item.PaymentMethod.ToString(),
                        Position = new Position(item.CoordonateLat, item.CoordonateLogi)
                    };
                    harta.Pins.Add(OrderPins);
                }
            }
        }
        private void ToolbarRefresh_Clicked(object sender, EventArgs e)
        {
            harta.Pins.Clear();
            GetOrderPinLocations();
        }
    }
}