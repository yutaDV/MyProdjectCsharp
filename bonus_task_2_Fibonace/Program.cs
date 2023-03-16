// See https://aka.ms/new-console-template for more information
// Скласти програму яка має поверати n-не число Фібоначчі за допомогою
//рекурсії (рекурсія в методі має викликатись один раз)

static double Fibonacci(double n, double preprevious = 1, double previous = 1)
{
    if (n < 3)
        return previous;

    return Fibonacci(n - 1, previous, preprevious + previous);
}

Console.Write($"Введіть число n: ");
double n = Convert.ToInt32(Console.ReadLine());
Console.WriteLine($" {n} число фібоначі =  {Fibonacci(n)}");

