using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21_10_25
{
    public class Order
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public Dish[] Dishes { get; set; }
        public string Comment { get; set; }
        public string StartTime { get; set; } // Время ПРИНЯТИЯ заказа
        public int WaiterId { get; set; }
        public string CloseTime { get; set; } // Время ЗАКРЫТИЯ заказа
        public float TotalCost { get; set; } = 0;
        public bool IsOpen { get; set; } = true;

        public Order(int id, int tableId, Dish[] dishes, string comment, string startTime, int waiterId)
        {
            Id = id;
            TableId = tableId;
            Dishes = dishes;
            Comment = comment;
            StartTime = startTime;
            WaiterId = waiterId;

        }

        public static Order CreateOrder(int id)
        {
            Console.WriteLine("---Открытие заказа---");
            Console.Write("Введите номер столика: ");
            int tableId = int.Parse(Console.ReadLine());
            Console.Write("Введите комментарий к заказу: ");
            string comment = Console.ReadLine();
            string startTime = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}";
            int waiterId = new Random().Next(0, Program.Waiters.Count());
            Console.Write("Введите количество заказываемых блюд: ");
            int dishCount = int.Parse(Console.ReadLine());
            Dish[] dishes = new Dish[dishCount];
            Console.WriteLine("Введите названия блюд:");
            for (int i = 0; i < dishCount; i++)
            {
                string title = Console.ReadLine();
                dishes[i] = Program.dishes.FirstOrDefault(x => x.Title == title);
            }

            return new Order(id, tableId, dishes, comment, startTime, waiterId);
        } 
        public void OrderChange()
        {
            Console.WriteLine("Желаете поменять официанта?(Enter, если нет. Введите имя другого официанта, если да)");
            string newWaiter = Console.ReadLine();
            if(newWaiter != "") WaiterId = Program.Waiters[newWaiter];
            Console.Write("Количество блюд для добавления: ");
            int dishCount = int.Parse(Console.ReadLine());
            Dish[] dishes = new Dish[dishCount];
            Console.WriteLine("Введите названия блюд:");
            for (int i = 0; i < dishCount; i++)
            {
                string title = Console.ReadLine();
                dishes[i] = Program.dishes.FirstOrDefault(x => x.Title == title);
            }
            Dishes = Dishes.Concat(dishes).ToArray();
            Console.WriteLine("Заказ обновлён!");
        }
        public void PrintInfo()
        {
            Console.WriteLine($"Номер заказа: {Id} Номер столика: {TableId} Текущая стоимость: {TotalCost} Имя официанта: {Program.Waiters.ElementAt(WaiterId).Key}");
            // Номер заказа: 00 Номер столика: 00 Текущая стоимость: 00 Имя официанта: Петя
        }
        public void OrderClose()
        {
            CloseTime = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}";
            Program.TotalCost += TotalCost;
            string waiter = Program.Waiters.ElementAt(WaiterId).Key;
            Program.Waiters[waiter]++;
            Program.orders.Remove(Program.orders.FirstOrDefault(x => x.Id == Id));
        }
        public void PrintReceipt()
        {
            if(IsOpen == false)
            {
                Dictionary<string, List<Dish>> dishesCategories = new Dictionary<string, List<Dish>>()
                {
                    {"Напитки", Dishes.Where(x => x.Category == Categories.Drinks).ToList()},
                    {"Салаты", Dishes.Where(x => x.Category == Categories.Salads).ToList() },
                    {"Холодные закуски", Dishes.Where(x => x.Category == Categories.ColdSnacks).ToList() },
                    {"Горячие закуски", Dishes.Where(x => x.Category == Categories.HotSnacks).ToList()},
                    {"Супы", Dishes.Where(x => x.Category == Categories.Soups).ToList()},
                    {"Горячие блюда", Dishes.Where(x => x.Category == Categories.HotDishes).ToList()},
                    {"Десерты", Dishes.Where(x => x.Category == Categories.Desserts).ToList()}
                };
                Console.WriteLine(Program.Stars + $"\nСтолик: {TableId + 1}\nОфициант: {Program.Waiters.ElementAt(WaiterId).Key}\nПериод обслуживания: с {StartTime} по {CloseTime}\n\n");
                foreach (var category in dishesCategories)
                {
                    Console.WriteLine($"{category.Key}:");
                    float categoryTotalCost = 0;
                    foreach (var dish in category.Value)
                    {
                        dish.PrintInfo();
                        categoryTotalCost += dish.Cost;
                    }
                    TotalCost += categoryTotalCost;
                    Console.WriteLine(new string('*', Program.StarsNum - $"Итог категории: {categoryTotalCost} рублей".Length) + $"Итог категории: {categoryTotalCost} рублей");
                }
                Console.WriteLine(new string('*', Program.StarsNum - $"Итог счёта: {TotalCost} рублей".Length) + $"Итог счёта: {TotalCost} рублей");
                Console.WriteLine(Program.Stars);
            }
            else
            {
                Console.WriteLine("Нельзя вывести чек открытого заказа!");
            }
        }
    }
}
