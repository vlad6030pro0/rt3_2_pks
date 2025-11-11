using _21_10_25;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Channels;

public class Program
{
    //Списки
    public static List<Table> tables = new List<Table>();
    public static List<Reservation> reservs = new List<Reservation>();
    public static string[] filters = new string[] { "none", "none", "none" }; // [0] - seatsCount, [1] - location, [2] - time
    public static List<Table> tablesForReserv = new List<Table>();
    public static List<Dish> dishes = new List<Dish>();
    public static List<Order> orders = new List<Order>();
    public static Dictionary<string, int> Waiters { get; set; } = new Dictionary<string, int>()
    {
        { "Вася", 0 },
        { "Петя", 0 },
        { "Егор", 0 }
    };

    //Необходимая информация
    public static int StarsNum { get; } = 63;
    public static string Stars { get; } = new string('*', 63);
    public readonly static string[] reservTime = {
        "9:00-10:00",
        "10:00-11:00",
        "11:00-12:00",
        "12:00-13:00",
        "13:00-14:00",
        "14:00-15:00",
        "15:00-16:00",
        "16:00-17:00",
        "17:00-18:00"
    };

    //Статистика
    public static float TotalCost { get; set; } = 0;
    private static void Main(string[] args)
    {
        //Не забыть везде прям реально ВЕЗДЕ добавить валидацию. Можно весь ввод данных вынести в отдельный метод где будет производиться проверка
        //Сделать проверку существования блюда при добавлении в заказ
        //Не забыть про комментарий в классе резервации
        //Добавить стандарт для ввода номера телефона и времени для бронирования(например через регулярное выражение)
        //Везде добавить счётчик попыток ввода(как в удалении бронирования)
        //Сделать красивый вывод информации о бронировании
        //Сделать возможность изменения столика в брони, а не только времени. Посмотреть реализацию в методе редактирования столиков
        //Добавить изменение название окна в зависимости от текущего действия
        //Вынести вывод информации и ввод данных в методы классов
        //При удалении столика/бронирования из списка может возникнуть ошибка при дальнейшей работе из-за нарушения упорядочивания(поиск по id например). Переделать(ВАЩЕ ПОФИГ НИКТО НЕ УЗНАЕТ)
        //Вынести проверку свободен ли столик в указанное время в отдельный метод
        //Добавить возможность ввода промежутка для количества мест, локации, нескольких промежутков времени(необязательно но хочу очень)
        //Везде добавить фразы по типу "Нет активных бронирований", "Нет фильтров" и тд
        //Переделать список блюд в список списков блюд(список содержит списки блюд по категориям)
        //Поправить вывод меню так, чтобы ценники были в ровный столбик
        //Перенести удаление бронирования в метод внутри класса(через Program.reservs)
        //Нет учёта количества одинаковых блюд в Order.cs PrintReceipt()
        string helpMessage1 = "Команды:" +
                                     "\n\t1 - создание столиков" +
                                     "\n\t2 - список столиков" +
                                     "\n\t3 - редактирование столика" +
                                     "\n\t4 - поиск столика по фильтру" +
                                     "\n\t5 - поиск столика по номеру" +
                                     "\n\t6 - создание бронирования" +
                                     "\n\t7 - список всех бронирований" +
                                     "\n\t8 - изменение бронирования" +
                                     "\n\t9 - поиск бронирования по номеру" +
                                     "\n\t0 - отмена бронирования" +
                                     "\n\tq - выход в меню" +
                                     "\n" + new string('-', 40);
        string helpMessage2 = "Команды:" +
                                     "\n\t1 - создание блюд" +
                                     "\n\t2 - вывод меню" +
                                     "\n\t3 - изменить блюдо" +
                                     "\n\t4 - удалить блюдо" +
                                     "\n\t5 - создание заказа" +
                                     "\n\t6 - изменение заказа" +
                                     "\n\t7 - вывод информации о заказе" +
                                     "\n\t8 - закрытие заказа(вывод чека)" +
                                     "\n\tq - выход в меню" +
                                     "\n" + new string('-', 40);
        while (true)
        {
            Console.WriteLine("Выберите то, что вас интересует:\n\t1 - Бронирование\n\t2 - Заказ\n\tq - Выход");
            Console.Write("Введите команду: ");
            string selected = Console.ReadLine();
            Console.Clear();
            switch (selected)
            {
                case ("1"):
                    string command = "";
                    while (command != "q")
                    {
                        Console.WriteLine("Добро пожаловать в систему бронирования!");
                        Console.Write(helpMessage1 + "\nВведите команду: ");
                        command = Console.ReadLine();
                        Console.Clear();
                        switch (command)
                        {
                            case ("1"):
                                TablesCreator();
                                break;
                            case ("2"):
                                TablesInfoPrinter();
                                break;
                            case ("3"):
                                TablesChanger();
                                break;
                            case ("4"):
                                FindTableAtFilter();
                                break;
                            case ("5"):
                                FindTableAtId();
                                break;
                            case ("6"):
                                ReservsCreator();
                                break;
                            case ("7"):
                                ReservsInfoPrinter();
                                break;
                            case ("8"):
                                ReservChanger();
                                break;
                            case ("9"):
                                FindReservAtPhone();
                                break;
                            case ("0"):
                                ReservDeleter();
                                break;
                            case ("q"):
                                break;
                        }
                        Console.WriteLine("Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case ("2"):
                    command = "";
                    while (command != "q")
                    {
                        Console.WriteLine("Добро пожаловать в систему заказов!");
                        Console.Write(helpMessage2 + "\nВведите команду: ");
                        command = Console.ReadLine();
                        Console.Clear();
                        switch (command)
                        {
                            case ("1"):
                                DishesCreator();
                                break;
                            case ("2"):
                                MenuPrinter();
                                break;
                            case ("3"):
                                DishesChanger();
                                break;
                            case ("4"):
                                DishesDeleter();
                                break;
                            case ("5"):
                                OrderCreator();
                                break;
                            case ("6"):
                                OrderChanger();
                                break;
                            case ("7"):
                                OrderInfoPrinter();
                                break;
                            case ("8"):
                                OrderCloser();
                                break;
                            case ("q"):
                                break;
                        }
                        Console.WriteLine("Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case ("q"):
                    Environment.Exit(0);
                    break;
            }
        }
    }
    public static void OrderCloser()
    {
        Console.Write("Введите номер заказа: ");
        int id = 7632;
        for (int i = 0; i < 3; i++)
        {
            if (int.TryParse(Console.ReadLine(), out id) == false)
            {
                Console.WriteLine($"Некорректный ввод номера заказа! Попробуйте ещё раз. (попытка {i + 1}/3)");
                continue;
            }
            break;
        }
        if (id == 7632)
        {
            Console.WriteLine("Некорректный ввод номера заказа! Возврат в главное меню.");
            return;
        }
        Order order = orders.FirstOrDefault(x => x.Id == id);
        if (order == null)
        {
            Console.WriteLine("Заказ с таким номером не найден! Возврат в главное меню.");
            return;
        }
        order.OrderClose();
    }
    public static void OrderInfoPrinter()
    {
        Console.Write("Введите номер заказа: ");
        int id = 7632;
        for (int i = 0; i < 3; i++)
        {
            if (int.TryParse(Console.ReadLine(), out id) == false)
            {
                Console.WriteLine($"Некорректный ввод номера заказа! Попробуйте ещё раз. (попытка {i + 1}/3)");
                continue;
            }
            break;
        }
        if (id == 7632)
        {
            Console.WriteLine("Некорректный ввод номера заказа! Возврат в главное меню.");
            return;
        }
        Order order = orders.FirstOrDefault(x => x.Id == id);
        if(order == null)
        {
            Console.WriteLine("Заказ с таким номером не найден! Возврат в главное меню.");
            return;
        }
        Order.PrintInfo(order);
    }
    public static void OrderChanger()
    {
        Console.Write("Введите номер заказа: ");
        int id = 7632;
        for (int i = 0; i < 3; i++)
        {
            if (int.TryParse(Console.ReadLine(), out id) == false)
            {
                Console.WriteLine($"Некорректный ввод номера заказа! Попробуйте ещё раз. (попытка {i + 1}/3)");
                continue;
            }
            break;
        }
        if (id == 7632)
        {
            Console.WriteLine("Некорректный ввод номера заказа! Возврат в главное меню.");
            return;
        }
        Order order = orders.FirstOrDefault(x => x.Id == id);
        if (order == null)
        {
            Console.WriteLine("Заказ с таким номером не найден! Возврат в главное меню.");
            return;
        }
        Order.OrderChange(ref order);
    }
    public static void OrderCreator()
    {
        Console.Write("Введите количество создаваемых заказов: ");
        int ordersCount = 7632;
        for (int i = 0; i < 3; i++)
        {
            if (int.TryParse(Console.ReadLine(), out ordersCount) == false)
            {
                Console.WriteLine($"Некорректный ввод количества заказов! Попробуйте ещё раз. (попытка {i + 1}/3)");
                continue;
            }
            break;
        }
        if(ordersCount == 7632)
        {
            Console.WriteLine("Некорректный ввод количества заказов! Возврат в главное меню.");
            return;
        }
        Order order;
        for (int i = 0; i < ordersCount; i++)
        {
            Order.CreateOrder(orders.Count(), out order);
            orders.Add(order);
        }
        if (ordersCount == 1) Console.WriteLine("Заказ создан!");
        else Console.WriteLine($"Создано {ordersCount} заказов!");
    }
    public static void DishesChanger()
    {
        Console.Write("Введите название блюда: ");
        Dish dish = null;
        for (int i = 0; i < 3; i++)
        {
            string title = Console.ReadLine();
            dish = dishes.FirstOrDefault(x => x.Title == title);
            if (dish == null)
            {
                Console.WriteLine($"Блюдо не найдено! Проверьте корректность ввода и попробуйте ещё раз. (попытка {i + 1}/3)");
                continue;
            }
            break;
        }
        if (dish == null)
        {
            Console.WriteLine("Блюдо не найдено! Возврат в главное меню.");
            return;
        }
        Dish.DishChange(ref dish);
    }
    public static void DishesDeleter()
    {
        Console.Write("Введите название блюда, которое хотите удалить: ");
        Dish dish = null;
        for (int i = 0; i < 3; i++)
        {
            string title = Console.ReadLine();
            dish = dishes.FirstOrDefault(x => x.Title == title);
            if (dish == null)
            {
                Console.WriteLine($"Блюдо не найдено! Проверьте корректность ввода и попробуйте ещё раз. (попытка {i + 1}/3)");
                continue;
            }
            break;
        }
        if (dish == null)
        {
            Console.WriteLine("Блюдо не найдено! Возврат в главное меню.");
            return;
        }
        dish.DishDelete();
    }
    public static void DishesCreator()
    {
        Console.WriteLine("---Система создания блюд---");
        Console.Write("Введите количество создаваемых блюд: ");
        int dishesCount = 7632;
        for (int i = 0; i < 3; i++)
        {
            if (int.TryParse(Console.ReadLine(), out dishesCount) == false)
            {
                Console.WriteLine($"Некорректный ввод количества блюд! Попробуйте ещё раз. (попытка {i + 1}/3)");
                continue;
            }
            break;
        }
        if (dishesCount == 7632)
        {
            Console.WriteLine("Некорректный ввод количества блюд! Возврат в главное меню.");
            return;
        }
        Dish dish;
        for (int i = 0; i < dishesCount; i++)
        {
            Dish.CreateDish(dishes.Count(), out dish);
            dishes.Add(dish);
        }
        if (dishesCount == 1) Console.WriteLine("Блюдо успешно создано!");
        else if (dishesCount > 1) Console.WriteLine($"Успешно создано {dishesCount} блюд");
    }
    public static void MenuPrinter()
    {
        Dictionary<string, List<Dish>> dishesCategories = new Dictionary<string, List<Dish>>()
        {
            {"Напитки", dishes.Where(x => x.Category == Categories.Drinks).ToList()},
            {"Салаты", dishes.Where(x => x.Category == Categories.Salads).ToList() },
            {"Холодные закуски", dishes.Where(x => x.Category == Categories.ColdSnacks).ToList() },
            {"Горячие закуски", dishes.Where(x => x.Category == Categories.HotSnacks).ToList()},
            {"Супы", dishes.Where(x => x.Category == Categories.Soups).ToList()},
            {"Горячие блюда", dishes.Where(x => x.Category == Categories.HotDishes).ToList()},
            {"Десерты", dishes.Where(x => x.Category == Categories.Desserts).ToList()}
        };

        Console.WriteLine("Меню\n" + Stars);
        foreach (var category in dishesCategories)
        {
            Console.WriteLine($"{category.Key}:");
            foreach(var dish in category.Value)
            {
                dish.PrintInfo();
            }
            Console.WriteLine();
        }
        Console.WriteLine(Stars);
    }
    public static void FindTableAtFilter()
    {
        string info = "Список возможных фильтров:\n\t1 - Добавить фильтр на количество сидячих мест\n\t2 - Добавить фильтр на расположение" +
            "\n\t3 - Добавить фильтр на время/промежуток времени для брони" +
            "\n\t4 - Посмотреть подходящие столики\n\t5 - Список ваших фильтров\n\t6 - Сброс фильтров\n\tq - Выход из системы фильтров\n" + new string('-', 40);
        while (true)
        {
            Console.WriteLine("---Система поиска столиков по фильтру---");
            Console.WriteLine(info);
            Console.Write("Введите нужное действие: ");
            string command = Console.ReadLine();
            switch (command)
            {
                case ("1"):
                    Console.Write("Введите интересующее Вас количество мест: ");
                    int seatsCount = 7632;
                    for (int i = 0; i < 3; i++)
                    {
                        if (int.TryParse(Console.ReadLine(), out seatsCount) == false)
                        {
                            Console.WriteLine($"Некорректный ввод количества заказов! Попробуйте ещё раз. (попытка {i + 1}/3)");
                            continue;
                        }
                        break;
                    }
                    if (seatsCount == 7632)
                    {
                        Console.WriteLine("Некорректный ввод количества заказов! Возврат в главное меню.");
                        return;
                    }

                    filters[0] = seatsCount.ToString();
                    break;
                case ("2"):
                    Console.WriteLine("Список доступных расположений столика:");
                    for (int i = 0; i < Table.locations.Length; i++)
                    {
                        Console.WriteLine($"\t{i + 1} - {Table.locations[i]}");
                    }
                    Console.WriteLine("Введите номер интересующего Вас расположения столика: ");
                    int location = 7632;
                    for (int i = 0; i < 3; i++)
                    {
                        if (int.TryParse(Console.ReadLine(), out location) == false)
                        {
                            Console.WriteLine($"Некорректный ввод номера локации! Попробуйте ещё раз. (попытка {i + 1}/3)");
                            continue;
                        }
                        break;
                    }
                    if (location == 7632)
                    {
                        Console.WriteLine("Некорректный ввод номера локации! Возврат в главное меню.");
                        return;
                    }
                    filters[1] = Table.locations[location - 1];
                    break;
                case ("3"):
                    for (int j = 0; j < reservTime.Length; j++) Console.WriteLine($"{j + 1} - {reservTime[j]}");

                    bool correctTime = false;
                    string time;
                    int reservStart = 7632;
                    int reservEnd = 7632;
                    Reservation reserv;
                    List<string> timeReserved;
                    while (!correctTime)
                    {
                        timeReserved = new List<string>();
                        Console.Write("Введите время брони по номеру из списка. Если хотите забронировать промежуток, то пишите в формате a-b: ");
                        time = Console.ReadLine();

                        if (time.Contains('-'))
                        {
                            reservStart = int.Parse(time.Split('-')[0]);
                            reservEnd = int.Parse(time.Split('-')[1]);
                        }
                        else
                        {
                            reservStart = int.Parse(time);
                            reservEnd = reservStart;
                        }

                        if (reservStart < 1 || reservStart > 9 || reservEnd < 1 || reservEnd > 9 || reservEnd < reservStart)
                        {
                            Console.WriteLine("Введённый промежуток неверен! Вот вам правила указания времени бронирования:\n Время конца бронирования не может быть раньше" +
                                " времени начала бронирования.\n Указывайте только написанные значения, а не выдумывайте какие-то большие/маленькие цифры/числа.");
                        }
                        else
                        {
                            correctTime = true;
                        }

                        if (!correctTime)
                        {
                            reservStart = 7632;
                            reservEnd = 7632;
                        }
                    }
                    filters[2] = $"{reservStart}-{reservEnd}";
                    break;
                case ("4"):
                    List<Table> correctTables = tables;
                    List<Table> newCorrectTables = tables;
                    if (filters[0] != "none")
                    {
                        correctTables = correctTables.Where(x => x.seatsCount == int.Parse(filters[0])).ToList();
                    }
                    if(filters[1] != "none")
                    {
                        correctTables = correctTables.Where(x => x.locationName ==  filters[1]).ToList();
                    }
                    if (filters[2] != "none")
                    {
                        reservStart = 7632;
                        if (int.TryParse(filters[2].Split('-')[0], out reservStart) == false)
                        {
                            Console.WriteLine($"Ошибка при обработке времени! Возврат в главное меню.");
                            return;
                        }
                        reservEnd = 7632;
                        if (int.TryParse(filters[2].Split('-')[1], out reservEnd) == false)
                        {
                            Console.WriteLine($"Ошибка при обработке времени! Возврат в главное меню.");
                            return;
                        }

                        correctTables = correctTables.Where(x => 
                        {
                            int freeTimes = 0;
                            for (int i = reservStart; i <= reservEnd; i++)
                            {
                                if (x.times[reservTime[i - 1]] == null)
                                {
                                    freeTimes++;
                                }
                            }
                            return freeTimes == (1 + reservEnd - reservStart);
                        }).ToList();
                        //correctTables.AddRange(newCorrectTables);
                    }
                    foreach(var table in correctTables)
                    {
                        table.PrintInfo();
                    }
                    break;
                case ("5"):
                    if(filters.Where(x => x == "none").Count() < 3)
                    {
                        Console.WriteLine("Список ваших фильтров:");
                        if (filters[0] != "none")
                        {
                            Console.WriteLine($"\tКоличество сидячих мест: {filters[0]}");
                        }
                        if (filters[1] != "none")
                        {
                            Console.WriteLine($"\tРасположение столика: {filters[1]}");
                        }
                        if (filters[2] != "none")
                        {
                            Console.WriteLine($"\tВремя: {reservTime[int.Parse(filters[2].Split('-')[0])].Split('-')[0]}-{reservTime[int.Parse(filters[2].Split('-')[1])].Split('-')[1]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("У вас нет фильтров!");
                    }
                    break;
                case ("6"):
                    for (int i = 0; i < filters.Length; i++) filters[i] = "none";
                    Console.WriteLine("Фильтры успешно сброшены!");
                    break;
                case ("q"):
                    return;
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }
    }
    public static Table FindTableAtId()
    {
        Console.Write("Введите номер столика: ");
        int id = 7632;
        for (int i = 0; i < 3; i++)
        {
            if (int.TryParse(Console.ReadLine(), out id) == false)
            {
                Console.WriteLine($"Некорректный ввод номера столика! Попробуйте ещё раз. (попытка {i + 1}/3)");
                continue;
            }
            break;
        }
        if (id == 7632)
        {
            Console.WriteLine("Некорректный ввод номера столика! Возврат в главное меню.");
            return null;
        }
        Table table = tables.FirstOrDefault(x => x.id == id);
        return table;
    }
    public static Reservation FindReservAtPhone()
    {
        Reservation reserv = null;
        for (int i = 1; i <= 3; i++)
        {
            Console.Write("Введите последние 4 цифры вашего телефона: ");
            string phone = Console.ReadLine();

            foreach (var reservation in reservs)
            {
                string clientPhone = new string(new char[] { reservation.clientPhone[^4], reservation.clientPhone[^3], reservation.clientPhone[^2], reservation.clientPhone[^1] });
                if (clientPhone == phone)
                {
                    reserv = reservation;
                    break;
                }
            }
            if (reserv == null)
            {
                Console.WriteLine($"Проверьте корретность ввода номера телефона и повторите.(попытка {i}/3)");
                continue;
            }
        }
        if(reserv == null) Console.WriteLine("Бронирование по указанному номеру не найдено!\nВозврат в главное меню.");
        return reserv;
    }
    public static void TablesChanger()
    {
        Console.WriteLine("---Система редактирования столиков---");
        Table table = FindTableAtId();
        int index = tables.IndexOf(table);
        Table.TableChange(ref table);
        tables[index] = table;
    }
    public static void ReservChanger()
    {
        Console.WriteLine("---Система редактирования бронирований---");
        Reservation reservForChange = FindReservAtPhone();
        if (reservForChange == null) return;

        int index = reservs.IndexOf(reservForChange);
        reservForChange.ChangeReserv();
        reservs[index] = reservForChange;
        Table tableReserved = reservForChange.tableReserv;
        for (int j = reservForChange.timeNumStart; j <= reservForChange.timeNumEnd; j++)
        {
            tableReserved.times[reservForChange.reservTime[j - 1]] = reservForChange;
        }
        tables[tableReserved.id] = tableReserved;

        Console.WriteLine("Ошибка редактирования бронирования.\nВозврат в главное меню.");
    }
    public static void ReservDeleter()
    {
        Console.WriteLine("---Система отмены бронирований---");
        Reservation reservForDelete;
        for (int i = 1; i <= 3; i++)
        {
            Console.Write("Введите последние 4 цифры вашего номера телефона для отмены бронирования: ");
            string phone = Console.ReadLine();

            reservForDelete = reservs.Where(x => new string(new char[] { x.clientPhone[^4], x.clientPhone[^3], x.clientPhone[^2], x.clientPhone[^1] }) == phone).FirstOrDefault();
            if(reservForDelete == null)
            {
                Console.WriteLine($"Проверьте корретность ввода номера телефона и повторите.(попытка {i}/3)");
                continue;
            }

            reservForDelete.CancelReserv();
            return;
        }

        Console.WriteLine("Ошибка отмены бронирования.\nВозврат в главное меню.");
    }
    public static void ReservsInfoPrinter()
    {
        Console.WriteLine("Список всех бронирований:");
        foreach (var reserv in reservs)
        {
            reserv.PrintInfo();
        }
    }
    public static void TablesInfoPrinter()
    {
        Console.WriteLine("Список всех столиков:");
        foreach (var table in tables)
        {
            table.PrintInfo();
        }
    }
    public static void TablesCreator()
    {
        Console.WriteLine("---Система создания столиков---");
        Console.Write("Введите количество столиков для создания: ");
        int tablesCount = 7632;
        for (int i = 0; i < 3; i++)
        {
            if (int.TryParse(Console.ReadLine(), out tablesCount) == false)
            {
                Console.WriteLine($"Некорректный ввод количества столиков! Попробуйте ещё раз. (попытка {i + 1}/3)");
                continue;
            }
            break;
        }
        if (tablesCount == 7632)
        {
            Console.WriteLine("Некорректный ввод количества столиков! Возврат в главное меню.");
            return;
        }

        bool locationsPrinted = false;
        Table table;
        for (int i = 0; i < tablesCount; i++)
        {
            if (!locationsPrinted) Console.WriteLine("\t1 - у окна\n\t2 - у прохода\n\t3 - у выхода\n\t4 - в глубине\n\t5 - у туалета");
            Console.Write("Введите номер расположения столика: ");
            int location = 7632;
            for (int j = 0; j < 3; j++)
            {
                if (int.TryParse(Console.ReadLine(), out location) == false)
                {
                    Console.WriteLine($"Некорректный ввод номера расположения! Попробуйте ещё раз. (попытка {j + 1}/3)");
                    location = 7632;
                    continue;
                }
                break;
            }
            if (location == 7632)
            {
                Console.WriteLine("Некорректный ввод номера расположения! Возврат в главное меню.");
                return;
            }
            Console.Write("Введите количество сидячих мест: ");
            int seatsCount = 7632;
            for (int j = 0; j < 3; j++)
            {
                if (int.TryParse(Console.ReadLine(), out seatsCount) == false)
                {
                    Console.WriteLine($"Некорректный ввод количества сидячих мест! Попробуйте ещё раз. (попытка {j + 1}/3)");
                    continue;
                }
                break;
            }
            if (seatsCount == 7632)
            {
                Console.WriteLine("Некорректный ввод количества сидячих мест! Возврат в главное меню.");
                return;
            }
            Table.CreateTable(tables.Count(), location - 1, seatsCount, out table);
            tables.Add(table);
        }
        if (tablesCount == 1) Console.WriteLine("Столик успешно создан!");
        else Console.WriteLine($"Успешно создано {tablesCount} столиков!");
    }
    public static void ReservsCreator()
    {
        Console.WriteLine("---Система создания бронирований---");

        Console.Write("Введите количество бронирований для создания: ");
        int reservsCount = 7632;
        for (int i = 0; i < 3; i++)
        {
            if (int.TryParse(Console.ReadLine(), out reservsCount) == false)
            {
                Console.WriteLine($"Некорректный ввод количества заказов! Попробуйте ещё раз. (попытка {i + 1}/3)");
                continue;
            }
            break;
        }
        if (reservsCount == 7632)
        {
            Console.WriteLine("Некорректный ввод количества заказов! Возврат в главное меню.");
            return;
        }

        for (int i = 0; i < reservsCount; i++)
        {
            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите ваш номер телефона: ");
            string phone = Console.ReadLine();

            Table tableReserved = FindTableAtId();
            /*
            Console.Write("Введите номер столика: ");
            int tableId;
            Table tableReserved = null;
            for (int j = 0; j < 3; j++)
            {
                tableId = int.Parse(Console.ReadLine()) - 1;
                tableReserved = tables[tableId];
                if(tableReserved == null)
                {
                    Console.WriteLine($"Столик не найден! Проверьте корретность введённого номера и попробуйте ещё раз. (попытка {i + 1}/3)");
                    continue;
                }
                break;
            }

            if(tableReserved == null)
            {
                Console.WriteLine("Столик не найден! Возврат в главное меню.");
                return;
            }*/

            for (int j = 0; j < reservTime.Length; j++) Console.WriteLine($"{j + 1} - {reservTime[j]}");

            bool correctTime = false;
            string time;
            int reservStart = 7632;
            int reservEnd = 7632;
            Reservation reserv;
            List<string> timeReserved;
            while (!correctTime)
            {
                timeReserved = new List<string>();
                Console.Write("Введите время брони по номеру из списка. Если хотите забронировать промежуток, то пишите в формате a-b: ");
                time = Console.ReadLine();
                
                if (time.Contains('-'))
                {
                    reservStart = int.Parse(time.Split('-')[0]);
                    reservEnd = int.Parse(time.Split('-')[1]);
                }
                else
                {
                    reservStart = int.Parse(time);
                    reservEnd = reservStart;
                }

                if(reservStart < 1 || reservStart > 9 || reservEnd < 1 || reservEnd > 9 || reservEnd < reservStart)
                {
                    Console.WriteLine("Введённый промежуток неверен! Вот вам правила указания времени бронирования:\n Время конца бронирования не может быть раньше" +
                        " времени начала бронирования.\n Указывайте только написанные значения, а не выдумывайте какие-то большие/маленькие цифры/числа.");
                }
                else
                {
                    int niceCount = 0;
                    for (int j = reservStart; j <= reservEnd; j++)
                    {
                        if (tableReserved.times[reservTime[j - 1]] == null)
                        {
                            niceCount++;
                        }
                        else
                        {
                            timeReserved.Add(reservTime[j - 1]);
                        }
                    }
                    if(niceCount == (1 + reservEnd - reservStart))
                    {
                        correctTime = true;
                    }
                }

                if (!correctTime)
                {
                    reservStart = 7632;
                    reservEnd = 7632;
                    if (timeReserved.Count() > 0)
                    {
                        int count = 0;
                        Console.Write("Указанное время уже занято!(");
                        foreach (var reservedTime in timeReserved)
                        {
                            count++;
                            Console.Write("\n\t");
                            if (count == timeReserved.Count())
                            {
                                Console.Write(reservedTime);
                                break;
                            }
                            Console.Write(reservedTime + " ");
                        }
                        Console.WriteLine(")");
                    }
                }
            }

            if(reservStart == reservEnd)
            {
                Reservation.CreateReserv(reservs.Count(), name, phone, reservStart, tableReserved, reservTime, out reserv);
            }
            else
            {
                Reservation.CreateReserv(reservs.Count(), name, phone, reservStart, reservEnd, tableReserved, reservTime, out reserv);
            }
            reservs.Add(reserv);
            for (int j = reserv.timeNumStart; j <= reserv.timeNumEnd; j++)
            {
                tableReserved.times[reservTime[j - 1]] = reserv;
            }
            tables[tableReserved.id] = tableReserved;
        }

        if(reservsCount == 1) Console.WriteLine("Бронь успешно создана!");
        else Console.WriteLine($"Успешно создано {reservsCount} бронирований!");
    }
}