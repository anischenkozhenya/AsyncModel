using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;
//Создайте консольное приложение, в котором организуйте асинхронный вызов метода.
//Используя конструкцию BeginInvoke передайте в поток некоторую информацию (возможно, в
//формате строки). Организуйте обработку переданных данных в callback методе.

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            var deleg = new Func<int, int>(Method);
            var callback = new AsyncCallback(callbackMethod);
            string str = "Дополнительная информация в виде строки";
            //Передоваемый аргумент в Method, делегат AsyncCallback, Дополнительный обьект
            deleg.BeginInvoke(25, callback, str);
            Console.WriteLine("Для выхода нажмите любую кнопку...");
            Console.ReadKey();
        }
        /// <summary>
        /// Метод вызывающийся после окончания работы основного метода
        /// </summary>
        /// <param name="ar">Значение типа IAsyncResult</param>
        private static void callbackMethod(IAsyncResult ar)
        {
            Console.WriteLine("Колбек метод начался..");
            Console.WriteLine("Id Потока Колбек метода: " + Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Фоновый поток :"+Thread.CurrentThread.IsBackground);
            //IAsyncResult приводим  к AsyncResult
            var async = ar as AsyncResult;
            //Получаем делегат
            var del = (Func<int, int>)async.AsyncDelegate;
            //Получаем результат
            int result = del.EndInvoke(ar);
            Console.WriteLine("Результат работы основного метода: "+result);
            Console.WriteLine("Доролнительная инфа: "+ar.AsyncState);
            Console.WriteLine("Колбек метод закончился");
        }
        /// <summary>
        /// Принимает число и возвращает квадрат числа
        /// </summary>
        /// <param name="arg">Целочисленный аргумент</param>
        /// <returns>Квадрат числа</returns>
        private static int Method(int arg)
        {
            Console.WriteLine("Метод начало");
            Console.WriteLine("Id Потока метода: "+Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Фоновый поток :"+Thread.CurrentThread.IsBackground);
            int a = arg * arg;
            Console.WriteLine("Метод окончен");
            return a;//число 625
        }
    }
}

