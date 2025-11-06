using _21_10_25;

internal class Program
{
    public static List<Table> tables = new List<Table>();
    public static List<Reservation> reservs = new List<Reservation>();
    
    private static void Main(string[] args)
    {
        //Не забыть везде прям реально ВЕЗДЕ добавить валидацию
        //Не забыть про комментарий в классе резервации
        //тут что-то должно было быть но я забыл что. Не забыть вспомнить!!!
        //Сделать проверку существования стола при бронировании
        //Добавить стандарт для ввода номера телефона(например через регулярное выражение)
        //Починить выход из системы
        //Везде добавить счётчик попыток ввода(как в удалении бронирования)
        //Сделать красивый вывод информации о бронировании
        //Сделать возможность изменения столика в брони, а не только времени. Посмотреть реализацию в методе редактирования столиков
        //Добавить изменение название окна в зависимости от текущего действия
        //Отредактировать вывод занятого времени в создании/изменении брони
        //В поиске брони по номеру сделать проверку на null
        Console.WriteLine("Добро пожаловать в систему бронирования!");
        string helpMessage = "Команды:\n\t1 - создание столиков\n\t2 - список столиков\n\t3 - редактирование столика\n\t4 - создание бронирования\n\t5 - список всех бронирований" +
            "\n\t6 - изменение бронирования\n\t7 - отмена бронирования\n\t8 - поиск брони по номеру\n\t0 - выход из системы бронирования";
        while (true)
        {
            Console.Write(helpMessage + "\nВведите команду: ");
            string command = Console.ReadLine();
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
                    ReservsCreator();
                    break;
                case ("5"):
                    ReservsInfoPrinter();
                    break;
                case ("6"):
                    ReservChanger();
                    break;
                case ("7"):
                    ReservDeleter();
                    break;
                case ("8"):
                    FindReservAtPhone();
                    break;
                case ("0"):
                    Environment.Exit(0);
                    break;
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }
    }
    public static void FindReservAtPhone()
    {
        Console.Write("Введите последние 4 цифры вашего телефона: ");
        string phone = Console.ReadLine();
        Reservation reserv = null;
        foreach(var reservation in reservs)
        {
            string clientPhone = new string(new char[] { reservation.clientPhone[^4], reservation.clientPhone[^3], reservation.clientPhone[^2], reservation.clientPhone[^1] });
            if(clientPhone == phone)
            {
                reserv = reservation;
                break;
            }
        }
        
        Console.WriteLine("Информация о вашем бронировании:");
        reserv.PrintInfo();
    }
    public static void TablesChanger()
    {
        Console.WriteLine("---Система редактирования столиков---");
        Console.Write("Введите номер столика для редактирования: ");
        int id = int.Parse(Console.ReadLine()) - 1;
        Table tableForChange = tables[id];
        tableForChange.ChangeInfo();
        tables[id] = tableForChange;
        Console.WriteLine("Информация о столике обновлена!");
    }
    public static void ReservChanger()
    {
        Console.WriteLine("---Система редактирования бронирований---");
        Reservation reservForChange;
        for (int i = 1; i <= 3; i++)
        {
            Console.Write("Введите ваш номер телефона для редактирования бронирования: ");
            string phone = Console.ReadLine();

            reservForChange = reservs.Where(x => x.clientPhone == phone).FirstOrDefault();
            if (reservForChange == null)
            {
                Console.WriteLine($"Проверьте корретность ввода номера телефона и повторите.(попытка {i}/3)");
                continue;
            }

            reservs.Remove(reservForChange);
            reservForChange.ChangeReserv();
            reservs.Add(reservForChange);
            Table tableReserved = reservForChange.tableReserv;
            for (int j = reservForChange.timeNumStart; j <= reservForChange.timeNumEnd; j++)
            {
                tableReserved.times[reservForChange.reservTime[j - 1]] = reservForChange;
            }
            tables[tableReserved.id] = tableReserved;
            Console.WriteLine("Бронирование успешно отредактировано!");
            return;
        }
        Console.WriteLine("Ошибка редактирования бронирования. Возврат в главное меню.");
    }
    public static void ReservDeleter()
    {
        Console.WriteLine("---Система отмены бронирований---");
        for (int i = 1; i <= 3; i++)
        {
            Console.Write("Введите ваш номер телефона для отмены бронирования: ");
            string phone = Console.ReadLine();

            Reservation reservForDelete = reservs.Where(x => x.clientPhone == phone).FirstOrDefault();
            if(reservForDelete == null)
            {
                Console.WriteLine($"Проверьте корретность ввода номера телефона и повторите.(попытка {i}/3)");
                continue;
            }

            reservForDelete.CancelReserv();
            reservs.Remove(reservForDelete);
            Console.WriteLine("Бронирование успешно отменено!");
            return;
        }
        Console.WriteLine("Ошибка отмены бронирования. Возврат в главное меню.");
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
        int tablesCount = int.Parse(Console.ReadLine());

        bool locationsPrinted = false;

        for (int i = 0; i < tablesCount; i++)
        {
            if (!locationsPrinted) Console.WriteLine("\t1 - у окна\n\t2 - у прохода\n\t3 - у выхода\n\t4 - в глубине\n\t5 - у туалета");
            Console.Write("Введите номер расположения столика: ");
            int location = int.Parse(Console.ReadLine());
            Console.Write("Введите количество сидячих мест: ");
            int seatsCount = int.Parse(Console.ReadLine());

            tables.Add(Table.CreateTable(tables.Count(), location - 1, seatsCount));
        }
        if (tablesCount == 1) Console.WriteLine("Столик успешно создан!");
        else Console.WriteLine($"Успешно создано {tablesCount} столиков!");
    }
    public static void ReservsCreator()
    {
        Console.WriteLine("---Система создания бронирований---");
        string[] reservTime = {
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

        Console.Write("Введите количество бронирований для создания: ");
        int reservsCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < reservsCount; i++)
        {
            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите ваш номер телефона: ");
            string phone = Console.ReadLine();

            Console.Write("Введите номер столика: ");
            int tableId = int.Parse(Console.ReadLine()) - 1;
            Table tableReserved = tables[tableId];

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
                        Console.Write("Указанное время уже занято!(");
                        foreach (var reservedTime in timeReserved)
                        {
                            Console.Write(reservedTime + " ");
                        }
                        Console.WriteLine(")");
                    }
                }
            }

            if(reservStart == reservEnd)
            {
                reserv = Reservation.CreateReserv(reservs.Count(), name, phone, reservStart, tableReserved, reservTime);
            }
            else
            {
                reserv = Reservation.CreateReserv(reservs.Count(), name, phone, reservStart, reservEnd, tableReserved, reservTime);
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