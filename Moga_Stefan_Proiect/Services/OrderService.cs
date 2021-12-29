using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using SQLite;
using Moga_Stefan_Proiect.Models;
using System.Collections.Generic;
using System;

namespace Moga_Stefan_Proiect.Services
{
    public static class OrderService
    {
        private static SQLiteAsyncConnection db;
        public static async Task Init()
        {
            if (db != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "Comenzi.db");
            db = new SQLiteAsyncConnection(databasePath);
            Console.WriteLine(databasePath);

            await db.CreateTableAsync<Order>();
        }
        public static async Task AddOrder(int orderNumber, string adress, string paymentMethod, double coordonatesLat, double coordonatesLogi)
        {
            await Init();

            var order = new Order
            {
                OrderNumber = orderNumber,
                Adress = adress,
                PaymentMethod = paymentMethod,
                CoordonateLat = coordonatesLat,
                CoordonateLogi = coordonatesLogi
            };

            await db.InsertAsync(order);
        }
        public static async Task RemoveOrder(int id)
        {
            await Init();

            await db.DeleteAsync<Order>(id);
        }
        public static async Task<IEnumerable<Order>> GetOrder()
        {
            await Init();

            var order = await db.Table<Order>().ToListAsync();

            return order;
        }
        public static Task<int> SaveOrderAsync(Order order)
        {
            _ = Init(); // e necesar??
            if (order.ID != 0)
            {
                return db.UpdateAsync(order);
            }
            else
            {
                return db.InsertAsync(order);
            }
        }
        internal static async Task EditOrder(Order order)
        {
            await Init();

            await SaveOrderAsync(order);

        }
    }
}
