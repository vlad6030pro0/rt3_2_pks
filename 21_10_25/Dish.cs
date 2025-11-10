using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21_10_25
{
    public class Dish
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Composition { get; set; }
        public string Weight { get; set; }
        public float Cost { get; set; }
        public Categories Category { get; set; }
        public int CookingTime { get; set; }
        public string[] Types { get; set; }

        public Dish(int id, string title, string composition, string weight, float cost, Categories category, int cookingTime, string[] types)
        {
            Id = id;
            Title = title;
            Composition = composition;
            Weight = weight;
            Cost = cost;
            Category = category;
            CookingTime = cookingTime;
            Types = types;
        }
        public static Dish CreateDish(int id)
        {
            Console.WriteLine("---Создание нового блюда---");
            Console.Write("Введите название блюда: ");
            string title = Console.ReadLine();
            Console.Write("Введите состав блюда: ");
            string composition = Console.ReadLine();
            Console.Write("Введите вес блюда(в формате 100/20/50): ");
            string weight = Console.ReadLine();
            Console.Write("Введите стоимость блюда: ");
            float cost = float.Parse(Console.ReadLine());
            Console.WriteLine("Список категорий блюда:\n\t1 - Напиток\n\t2 - Салат\n\t3 - Холодная закуска\n\t4 - Горячая закуска\n\t5 - Суп\n\t6 - Горячее блюдо\n\t7 - Десерт");
            Console.Write("Введите категорию блюда: ");
            Categories category = (Categories)(int.Parse(Console.ReadLine()) - 1);
            Console.Write("Введите время приготовления блюда(в минутах): ");
            int cookingTime = int.Parse(Console.ReadLine());
            string[] types;
            Console.Write("Введите количество типов блюда: ");
            int count = int.Parse(Console.ReadLine());
            types = new string[count];
            Console.WriteLine("Введите типы блюда:");
            for (int i = 0; i < count; i++)
            {
                types[i] = Console.ReadLine();
            }

            return new Dish(id, title, composition, weight, cost, category, cookingTime, types);
        }
        public void DishChange()
        {
            Console.WriteLine("---Изменение блюда---");
            Console.WriteLine("Чтобы оставить прежнее значение нажмите на Enter.");
            Console.Write("Введите название блюда: ");
            string title = Console.ReadLine();
            Title = title != "" ? title : Title;
            Console.Write("Введите состав блюда: ");
            string composition = Console.ReadLine();
            Composition = composition != "" ? composition : Composition;
            Console.Write("Введите вес блюда(в формате 100/20/50): ");
            string weight = Console.ReadLine();
            Weight = weight != "" ? weight : Weight;
            Console.Write("Введите стоимость блюда: ");
            string cost = Console.ReadLine();
            Cost = cost != "" ? float.Parse(cost) : Cost;
            Console.WriteLine("Список категорий блюда:\n\t1 - Напиток\n\t2 - Салат\n\t3 - Холодная закуска\n\t4 - Горячая закуска\n\t5 - Суп\n\t6 - Горячее блюдо\n\t7 - Десерт");
            Console.Write("Введите категорию блюда: ");
            string category = Console.ReadLine();
            Category = category != "" ? (Categories)(int.Parse(category)- 1) : Category;
            Console.Write("Введите время приготовления блюда(в минутах): ");
            string cookingTime = Console.ReadLine();
            CookingTime = cookingTime != "" ? int.Parse(cookingTime) : CookingTime;
            string[] types;
            Console.Write("Введите количество типов блюда: ");
            string count = Console.ReadLine();
            if (count == "") return;
            types = new string[int.Parse(count)];
            Console.WriteLine("Введите типы блюда:");
            for (int i = 0; i < int.Parse(count); i++)
            {
                types[i] = Console.ReadLine();
            }
        }
        public void PrintInfo()
        {
            //string info = $"{Title}{new string(' ', 15)}{Cost} рублей за порцию\n";
            int starsNum = Program.StarsNum - Title.Length - Cost.ToString().Length - "рублей за порцию".Length;
            string info = $"{Title}{new string(' ', starsNum)}{Cost} рублей за порцию";
            Console.WriteLine(info);
        }
        public void PrintInfo(int a)
        {
            //string info = $"{Title}{new string(' ', 15)}{Cost} рублей за порцию\n";
            int starsNum = Program.StarsNum - Title.Length - Cost.ToString().Length - "рублей за порцию".Length - 1;
            string info = $"{Title}{new string(' ', starsNum)}{Cost} рублей за порцию.\n";
            info += "Типы блюда:\n";
            foreach (var type in Types)
            {
                info += $"\t{type}\n";
            }
            Console.Write(info);
        }
        public void DishDelete()
        {
            Program.dishes.Remove(Program.dishes.Where(x => x.Id == Id).FirstOrDefault());
            Console.WriteLine("Блюдо удалено из меню!");
        }
    }
    public enum Categories
    {
        Drinks, Salads, ColdSnacks, HotSnacks, Soups, HotDishes, Desserts
    }
}
