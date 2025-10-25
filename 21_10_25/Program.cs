internal class Program
{
    public static List<Table> tables = new List<Table>();
    public static List<Reservation> reservs = new List<Reservation>();
    public static List<Client> clients = new List<Client>();
    
    private static void Main(string[] args)
    {
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

        Console.Write("Введите количество столиков для создания: ");
        int tablesCount = int.Parse(Console.ReadLine());

        bool locationsPrinted = false;

        for (int i = 0; i < tablesCount; i++)
        {
            if(!locationsPrinted) Console.WriteLine("1 - у окна, 2 - у прохода, 3 - у выхода, 4 - в глубине, 5 - у туалета");
            Console.Write("Введите номер расположения столика: ");
            int location = int.Parse(Console.ReadLine());
            Console.Write("Введите количество сидячих мест: ");
            int seatsCount = int.Parse(Console.ReadLine());
            
            tables.Add(Table.CreateTable(tables.Count(), location-1, seatsCount));
        }

        //Добавить возможность резервации промежутка и не забыть про комментарий в классе резервации
        //!!!!!!!!!!!!!!!!!!!!!!!! Доделать класс Client
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

            // Сделать проверку на то что столик свободен в указанное время
            for (int j = 0; j < reservTime.Length; j++) Console.WriteLine($"{j} - {reservTime[j]}");

            Console.Write("Введите время брони по номеру из списка: ");
            int timeNum = int.Parse(Console.ReadLine());

            
            Reservation reserv = Reservation.CreateReserv(reservs.Count(), name, phone, timeNum, tableReserved, reservTime);
            reservs.Add(reserv);
            tableReserved.times[reservTime[reserv.timeNum]] = reserv;
        }

        foreach(var table in tables)
        {
            table.PrintInfo();
        }
    }
}
public class Client
{
    public readonly int id;
    public string name;
    public string phone;
}
public class Reservation
{
    public readonly int clientId;
    public string clientName;
    public string clientPhone;
    public int timeNum;
    public string comment;
    public Table tableReserv;
    public string[] reservTime;

    public Reservation(int id, string name, string phone, int timeNum, Table table, string[] reservTime)
    {
        clientId = id;
        clientName = name;
        clientPhone = phone;
        this.timeNum = timeNum;
        tableReserv = table;
        this.reservTime = reservTime;
    }

    public static Reservation CreateReserv(int id, string name, string phone, int timeNum, Table table, string[] reservTime)
    {
        return new Reservation(id, name, phone, timeNum, table, reservTime);
    }
    public static void ChangeReserv()
    {

    }
    public static void CancelReserv()
    {

    }
}
public class Table
{
    public readonly int id;
    public string locationName;
    public int seatsCount;
    public string[] reservTime = {
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
    public Dictionary<string, Reservation> times = new Dictionary<string, Reservation>();
    public static Dictionary<int, string> locations = new Dictionary<int, string>() {
            {0, "\"у окна\"" },
            {1, "\"у прохода\"" },
            {2, "\"у выхода\"" },
            {3, "\"в глубине\"" },
            {4, "\"у туалета\"" }
    };

    public Table(int id, int location, int seatsCount)
    {
        this.id = id;
        locationName = locations.GetValueOrDefault(location);
        this.seatsCount = seatsCount;

        for (int i = 0; i < 9; i++)
        {
            times.Add(reservTime[i], null);
        }
    }

    public static Table CreateTable(int id, int location, int seatsCount)
    {
        return new Table(id, location, seatsCount);
    }
    public void ChangeInfo()
    {
        
    }
    public void PrintInfo()
    {
        /*
        string info =
            new string('*', starsNum) + "\n"
            + new string(' ', 5) + "ID: " + new string('-', 31 - id.ToString().Length) + id + "\n"
            + new string(' ', 5) + "Расположение: " + new string('-', 21 - locationName.Length) + locationName + "\n"
            + new string(' ', 5) + "Количество мест: " + new string('-', 18 - seatsCount.ToString().Length) + seatsCount + "\n"
            + new string(' ', 5) + "Расписание:\n";

        int timeNum = 0;
        //!!!!!!!!!!!!Переделать! Не учитывается несколько резерваций одного столика на разное время
        foreach (var reserv in reservs)
        {
            string reservInfo = $"ID {reserv.clientId}, {reserv.clientName}, {reserv.clientPhone}";
            var line = new string(' ', 5 + "Расписание:".Length);
            info += line;
            info += reserv.reservTime[timeNum] + new string(' ', line.Length - reservInfo.Length - reserv.reservTime[timeNum].Length);
            info += reservInfo + "\n";
            timeNum++;
        }*/
        string info = "";
        int baseSpace = 5;
        int starsNum = 63;

        string idLine = new string(' ', baseSpace) + "ID: ";
        if (id < 10) idLine += new string('-', starsNum - idLine.Length - 3) + $"0{id}.\n";
        else idLine += new string('-', starsNum - idLine.Length - id.ToString().Length - 1) + id + ".\n";

        string locationLine = new string(' ', baseSpace) + "Расположение: ";
        locationLine += new string('-', starsNum - locationLine.Length - locationName.Length - 1) + locationName + ".\n";

        string seatsLine = new string(' ', baseSpace) + "Количество мест: ";
        seatsLine += new string('-', starsNum - seatsLine.Length - seatsCount.ToString().Length - 1) + seatsCount + ".\n";

        string reservsLine = new string(' ', baseSpace) + "Расписание:\n";
        int basicLength = reservsLine.Length;

    // Сделать вывод всего списка времени, а не только зарезервированного
        foreach(var time in times)
        {
            string reservLine;
            if (time.Value != null)
            {
                Reservation reserv = time.Value;
                string reservInfo = $"ID {reserv.clientId}, {reserv.clientName}, {reserv.clientPhone}";
                reservLine = new string(' ', basicLength - 1) + time.Key 
                    + new string('-', starsNum + 1 - basicLength - time.Key.Length - reservInfo.Length) + reservInfo + "\n";
                reservsLine += reservLine;
            }
            else
            {
                reservLine = new string(' ', basicLength - 1) + time.Key + new string('-', starsNum + 1 - basicLength - time.Key.Length) + "\n";
                reservsLine += reservLine;
            }
        }

        info += new string('*', starsNum) + "\n" + idLine + locationLine + seatsLine + reservsLine + new string('*', starsNum);
        Console.WriteLine(info);
    }
}