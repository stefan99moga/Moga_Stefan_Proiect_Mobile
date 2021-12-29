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
            //_ = OrderLocationsService.RefreshTable();
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
            var position = await OrderLocationsService.GetOrderLocationsFromTable();

            if(position != null)
            {
                foreach(var item in position)
                {
                    Pin OrderPins = new Pin()
                    {
                        Label = item.OrderNumber.ToString(),
                        Position = new Position(item.Latitude, item.Longitude)
                    };
                    harta.Pins.Add(OrderPins);
                }
            }
        }
    }
}