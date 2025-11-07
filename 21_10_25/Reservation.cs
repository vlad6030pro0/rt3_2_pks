using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21_10_25
{
    public class Reservation
    {
        public readonly int clientId;
        public string clientName;
        public string clientPhone;
        public int timeNumStart;
        public int timeNumEnd;
        public string comment;
        public Table tableReserv;
        public string[] reservTime;

        public Reservation(int id, string name, string phone, int timeNumStart, int timeNumEnd, Table table, string[] reservTime)
        {
            clientId = id;
            clientName = name;
            clientPhone = phone;
            this.timeNumStart = timeNumStart;
            this.timeNumEnd = timeNumEnd;
            tableReserv = table;
            this.reservTime = reservTime;
        }

        public static Reservation CreateReserv(int id, string name, string phone, int timeNumStart, Table table, string[] reservTime)
        {
            return new Reservation(id, name, phone, timeNumStart, timeNumStart, table, reservTime);
        }
        public static Reservation CreateReserv(int id, string name, string phone, int timeNumStart, int timeNumEnd, Table table, string[] reservTime)
        {
            return new Reservation(id, name, phone, timeNumStart, timeNumEnd, table, reservTime);
        }

        public void ChangeReserv()
        {
            bool correctTime = false;
            string time;
            int reservStart = 7632;
            int reservEnd = 7632;
            List<string> timeReserved;

            for (int j = 0; j < reservTime.Length; j++) Console.WriteLine($"{j + 1} - {reservTime[j]}");

            while (!correctTime)
            {
                timeReserved = new List<string>();
                Console.Write("Введите новое время брони по номеру из списка. Если хотите забронировать промежуток, то пишите в формате a-b: ");
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
                    int niceCount = 0;
                    for (int j = reservStart; j <= reservEnd; j++)
                    {
                        if (tableReserv.times[reservTime[j - 1]] == null || tableReserv.times[reservTime[j - 1]].clientPhone == clientPhone)
                        {
                            niceCount++;
                        }
                        else
                        {
                            timeReserved.Add(reservTime[j - 1]);
                        }
                    }
                    if (niceCount == (1 + reservEnd - reservStart))
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

            for (int i = timeNumStart; i <= timeNumEnd; i++)
            {
                tableReserv.times[reservTime[i - 1]] = null;
            }
            timeNumStart = reservStart;
            timeNumEnd = reservEnd;
            for (int i = timeNumStart; i <= timeNumEnd; i++)
            {
                tableReserv.times[reservTime[i - 1]] = null;
            }
            Console.WriteLine("Бронирование успешно отредактировано!");
        }
        public void CancelReserv()
        {
            for(int i = timeNumStart; i <= timeNumEnd; i++)
            {
                tableReserv.times[reservTime[i - 1]] = null;
            }
            Console.WriteLine("Бронирование успешно отменено!");
        }

        public void PrintInfo()
        {
            //Debug.WriteLine($"TIME ID: {timeNumStart}-{timeNumEnd}");
            Console.WriteLine($"ID {clientId} NAME {clientName} PHONE {clientPhone} TABLE {tableReserv.id} {reservTime[timeNumStart - 1].Split('-')[0]}-{reservTime[timeNumEnd - 1].Split('-')[1]}");
        }
    }
}
