using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    internal class Class1
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Подождите");
            Random rnd = new Random();
            int x = 0;
            object locker = new object();  // объект-заглушка
            for (int i = 1; i <= 10; i++)
            {
                int t = rnd.Next(100, 5000);// Интервал задержки(от 0,1 до 5 секунд)
                await Task.Delay(TimeSpan.FromMilliseconds(t));// Ожидает задержку в интервале t
                Console.WriteLine($"Задержка потока {i} = {t} мс");
                /* Thread.Sleep(TimeSpan.FromMilliseconds(t));// Задержка для потока от 100 до 1000 мс
                 Console.WriteLine($"Время работы потока {i} = {t} мс");*/
                Thread myThread = new Thread(Print);// Объявляем поток
                myThread.Name = $"Поток {i}";
                myThread.Start();
                //Console.WriteLine(myThread.ThreadState);
            }
            
            void Print() //вывод на экран сообщения о номере потока и текста
            {
                lock (locker)
                {
                    x = 1;
                    for (int i = 1; i < 6; i++)
                    {
                        Console.WriteLine($"{Thread.CurrentThread.Name}: сообщение {x}");
                        x++;
                    }
                    Console.WriteLine($"Статус потока: {Thread.CurrentThread.ThreadState}");
                }
            }
            await PrintAsync(); // Ожидает вызова асинхронного метода 
            void PrintTask()// для работы метода
            {
                Console.WriteLine("sus");
            }
            async Task PrintAsync()// определение асинхронного метода
            {
                await Task.Delay(100);
                Console.WriteLine($"Вот {1 + 2}"); // выполняется синхронно
                await Task.Run(() => PrintTask());// выполняется асинхронно
                Console.WriteLine("Вроде концовочка");
            }
            Console.ReadKey();
        }
    }
}
