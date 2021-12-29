
using Moga_Stefan_Proiect.Models;
using Moga_Stefan_Proiect.Services;
using Moga_Stefan_Proiect.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Moga_Stefan_Proiect.ViewModels
{
    public class HartaViewModel : BaseViewModel
    {
        public ObservableRangeCollection<OrderLocations> OrderLocations { get; set; }
        public AsyncCommand RefreshAsyncCommand { get; }
        public ICommand refreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    await PerformRefreshCMD();
                    IsRefreshing = false;

                });
            }
        }
        public HartaViewModel()
        {
            Title = "Harta";

            //_ = PerformRefreshCMD();

            RefreshAsyncCommand = new AsyncCommand(PerformRefreshCMD);
        }

        //private Command refreshCMD;

        //public ICommand RefreshCMD
        //{
        //    get
        //    {
        //        if (refreshCMD == null)
        //        {
        //            refreshCMD = new Command(PerformRefreshCMD);
        //        }

        //        return refreshCMD;
        //    }
        //}

        private async Task PerformRefreshCMD()
        {
            //OrderLocations.Clear();
            //var orderLocations = await OrderLocationsService.GetOrderLocationsFromTable();
            //OrderLocations.AddRange(orderLocations);
            HartaPage hartaPage =new HartaPage();
            hartaPage.GetOrderPinLocations();
            await OrderLocationsService.RefreshTable();
            await OrderLocationsService.AddOrderLocationCoordonates();
            hartaPage.GetOrderPinLocations();
            await App.Current.MainPage.DisplayAlert("Refresh Success", null, "OK");
        }
    }
}