using Moga_Stefan_Proiect.Models;
using Moga_Stefan_Proiect.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Moga_Stefan_Proiect.ViewModels
{
    public class ComenziViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Order> Order { get; set; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand<Order> RemoveCommand { get; }
        public AsyncCommand<Order> EditCommand { get; }
        public ICommand RefreshCmmd
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    await Refresh();
                    IsRefreshing = false;
                });
            }
        }

        public ComenziViewModel()
        {
            Title = "Comenzi";
            
            Order = new ObservableRangeCollection<Order>();

            _ = Refresh();

            RefreshCommand = new AsyncCommand(Refresh);
            AddCommand = new AsyncCommand(Add);
            RemoveCommand = new AsyncCommand<Order>(Remove);
            EditCommand = new AsyncCommand<Order>(Edit);
        }
        private async Task Add()
        {
           

            var orderNumber = await App.Current.MainPage.DisplayPromptAsync("NUMAR COMANDA", "Introduceti numarul comenzii aflat pe bon", "Urmator", "Renunta", maxLength: 3, keyboard: Keyboard.Numeric);
            if (orderNumber == null)
                return;

            var adress = await App.Current.MainPage.DisplayPromptAsync("ADRESA COMANDA", "oras strada numar", "Urmator", "Renunta");
            if (adress == null)
                return;

            var paymentMethod = await App.Current.MainPage.DisplayActionSheet("TIP PLATA", "Renunta", null, "Cash","Card","Online");
            if (paymentMethod == "Renunta")
                return;

            //converteste adresa in coordonate
            Geocoder geoCoder = new Geocoder();
            IEnumerable<Position> approximateLocations =
                        await geoCoder.GetPositionsForAddressAsync(adress);
            Position position = approximateLocations.FirstOrDefault();
            var coordonatesLat = Convert.ToDouble($"{ position.Latitude}");
            var coordonatesLong = Convert.ToDouble($"{ position.Longitude}");

            //adauga date in tabela
            await OrderService.AddOrder(Convert.ToInt16(orderNumber), adress, paymentMethod, coordonatesLat, coordonatesLong);
            await Refresh();
        }

        private async Task Remove(Order order)
        {
            bool result = await App.Current.MainPage.DisplayAlert("Sigur doriti sa stergeti comanda?", null, "DA", "NU");
            if(result == true)
            {
                await OrderService.RemoveOrder(order.ID);
                await App.Current.MainPage.DisplayAlert("Succes!", "Comanda Stearsa", "OK");
                await Refresh();
            }
            else
            {
                return;
            }
        }
        private async Task Refresh()
        {
            Order.Clear();
            var orders = await OrderService.GetOrder();
            Order.AddRange(orders);
        }
        private async Task Edit(Order order)
        {
            order.OrderNumber = Convert.ToInt16(await App.Current.MainPage.DisplayPromptAsync("NUMAR COMANDA", "Introduceti numarul comenzii aflat pe bon", "Urmator", "Renunta", maxLength: 3, keyboard: Keyboard.Numeric, initialValue: order.OrderNumber.ToString()));
            if (order.OrderNumber == 0)
                return;

            order.Adress = await App.Current.MainPage.DisplayPromptAsync("ADRESA COMANDA", "oras strada numar", "Urmator", "Renunta", initialValue: order.Adress);
            if (order.Adress == null)
                return;

            order.PaymentMethod = await App.Current.MainPage.DisplayActionSheet("TIP PLATA", "Renunta", null, "Cash", "Card", "Online");
            if (order.PaymentMethod == "Renunta")
                return;

            //converteste adresa in coordonate
            Geocoder geoCoder = new Geocoder();
            IEnumerable<Position> approximateLocations =
                        await geoCoder.GetPositionsForAddressAsync(order.Adress);
            Position position = approximateLocations.FirstOrDefault();
            order.CoordonateLat = Convert.ToDouble($"{ position.Latitude}");
            order.CoordonateLogi = Convert.ToDouble($"{ position.Longitude}");

            await OrderService.EditOrder(order);
            await Refresh();
        }
    }
}
