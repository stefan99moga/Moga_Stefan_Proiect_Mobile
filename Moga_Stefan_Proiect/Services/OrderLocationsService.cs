using Moga_Stefan_Proiect.Models;
using Moga_Stefan_Proiect.ViewModels;
using Moga_Stefan_Proiect.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace Moga_Stefan_Proiect.Services
{
    public static class OrderLocationsService
    {
        private static SQLiteAsyncConnection db;
        static async Task Init()
        {
            if (db != null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "Comenzi.db");
            db = new SQLiteAsyncConnection(dbPath);
            await db.CreateTableAsync<OrderLocations>();

        }
        public static async Task AddOrderLocationCoordonates() //converteste adresa in coordonate + uneste Order cu OrderLocations
        {
            IEnumerable<Order> orders  = await OrderService.GetOrder(); 
            
            Geocoder geoCoder = new Geocoder();
            for(int i = 0; i < orders.Count(); i++)
            {
                IEnumerable<Position> approximateLocations =
                        await geoCoder.GetPositionsForAddressAsync(orders.ElementAt(i).Adress);
                Position position = approximateLocations.FirstOrDefault();
                var coordonatesLat = Convert.ToDouble($"{ position.Latitude}");
                var coordonatesLong = Convert.ToDouble($"{ position.Longitude}");

                await AddOrderLocation(orders.ElementAt(i).ID, orders.ElementAt(i).OrderNumber, coordonatesLat, coordonatesLong); 
            }
        }
        public static async Task AddOrderLocation(int id, int ord, double lat, double longi)
        {
            await Init();

            var result = new OrderLocations
            {
                ID = id,
                OrderNumber = ord,
                Latitude = lat,
                Longitude = longi
            };

            await db.InsertAsync(result);
        }
        public static async Task<IEnumerable<OrderLocations>> GetOrderLocationsFromTable()
        {
            await Init();
            await AddOrderLocationCoordonates();
            var order = await db.Table<OrderLocations>().ToListAsync();

            return order;
        }
        public static async Task RefreshTable()
        {
            await db.DeleteAllAsync<OrderLocations>();
            ComenziViewModel comenzi = new ComenziViewModel();
            _ = comenzi.RefreshCommand;
            await AddOrderLocationCoordonates();
            HartaPage harta = new HartaPage();
            harta.GetOrderPinLocations();
            await Init();

        }
    }
}
