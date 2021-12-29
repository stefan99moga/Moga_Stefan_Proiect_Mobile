using Moga_Stefan_Proiect.Models;
using Moga_Stefan_Proiect.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Moga_Stefan_Proiect.ViewModels
{
    public class ComenziViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Order> Order { get; set; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand<Order> RemoveCommand { get; }
        public AsyncCommand<Order> EditCommand { get; }
        public ICommand refreshCmmd
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
        async Task Add()
        {
            var orderNumber = await App.Current.MainPage.DisplayPromptAsync("NUMAR COMANDA", "Introduceti numarul comenzii aflat pe bon", maxLength: 3, keyboard: Keyboard.Numeric);
            if (orderNumber == null)
                return;

            var adress = await App.Current.MainPage.DisplayPromptAsync("ADRESA COMANDA", "oras strada numar", "Urmator", "Renunta");
            if (adress == null)
                return;

            var paymentMethod = await App.Current.MainPage.DisplayActionSheet("TIP PLATA", "Renunta", null, "Cash","Card","Online");
            if (paymentMethod == "Renunta")
                return;

            await OrderService.AddOrder(Convert.ToInt16(orderNumber), adress, paymentMethod);
            await Refresh();
        }
        async Task Remove(Order order)
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
        async Task Refresh()
        {
            Order.Clear();
            var orders = await OrderService.GetOrder();
            Order.AddRange(orders);
        }
        async Task Edit(Order order)
        {
            order.OrderNumber = Convert.ToInt16(await App.Current.MainPage.DisplayPromptAsync("NUMAR COMANDA", "Introduceti numarul comenzii aflat pe bon", maxLength: 3, keyboard: Keyboard.Numeric, initialValue: order.OrderNumber.ToString()));
            if (order.OrderNumber == 0)
                return;

            order.Adress = await App.Current.MainPage.DisplayPromptAsync("ADRESA COMANDA", null, initialValue: order.Adress);
            if (order.Adress == null)
                return;

            order.PaymentMethod = await App.Current.MainPage.DisplayActionSheet("TIP PLATA", "Renunta", null, "Cash", "Card", "Online");
            if (order.PaymentMethod == "Renunta")
                return;

            await OrderService.EditOrder(order);
            await Refresh();
        }
    }
}
