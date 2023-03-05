using System;

namespace test;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Console.Write($"Введіть ваше ім'я:  ");
        string userName = Console.ReadLine();
        Console.WriteLine(userName);
    }
}

