using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21_10_25
{
    public class Table
    {
        public readonly int id;
        public string locationName;
        public int seatsCount;
        public int reservCount;
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
        public static string[] locations = new string[]{
            "\"у окна\"" ,
            "\"у прохода\"",
            "\"у выхода\"",
            "\"в глубине\"",
            "\"у туалета\""
        };
        public Table(int id, int location, int seatsCount)
        {
            this.id = id;
            locationName = locations[location];
            this.seatsCount = seatsCount;
            reservCount = 0;

            for (int i = 0; i < 9; i++)
            {
                times.Add(reservTime[i], null);
            }
        }

        public static void CreateTable(int id, int location, int seatsCount, out Table table)
        {
            table = new Table(id, location, seatsCount);
        }
        public static void TableChange(ref Table table)
        {
            table.UpdateReservCount();
            if(table.reservCount > 0)
            {
                Console.WriteLine("Невозможно изменить информацию о столе на который есть бронирования.\nВозврат в главное меню.");
                return;
            }
            Console.WriteLine("Список расположений для столика:");
            for (int i = 0; i < locations.Length; i++)
            {
                Console.WriteLine($"\t{i + 1} - {locations[i]}");
            }
            Console.WriteLine("Вводите новую информацию о столике(Нажмите Enter чтобы оставить прежнее значение):");
            Console.Write("Расположение столика: ");
            string newLocation = Console.ReadLine();
            if(newLocation != "")
            {
                table.locationName = locations[int.Parse(newLocation) - 1];
            }
            Console.Write("Количество мест: ");
            string newSeatsCount = Console.ReadLine();
            if (newSeatsCount != "")
            {
                table.seatsCount = int.Parse(newSeatsCount);
            }
            Console.WriteLine("Информация о столике обновлена!");
        }
        public void PrintInfo()
        {
            string info = "";
            int baseSpace = Program.StarsNum;
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

            foreach (var time in times)
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
        public void UpdateReservCount()
        {
            reservCount = times.Where(x => x.Value != null).Count();
        }
    }
}
