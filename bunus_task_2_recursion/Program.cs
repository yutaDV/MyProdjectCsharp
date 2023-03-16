// See https://aka.ms/new-console-template for more information
// виконати завдання 7(для циклів) з використанням рекурсії

static double CalculateResult(int n, double x, int i, int j)
{
    if (i > n)
    {
        return 0;
    }

    if (Math.Sin(i * x) == 0 || Math.Cos((i + 1) * x) == 0)
    {
        Console.Write(" Не коректно введені дані змініть х");
        return 0;
    }
    else
    {
        double currentResult = (((n - i) * (x - j)) / Math.Sin(i * x)) - ((((n - (i + 1)) * (x - (j + 2))) / Math.Cos((i + 1) * x)));
        return currentResult + CalculateResult(n, x, i + 2, j + 1);
    }
}


Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.Write("Введіть число n: ");
int n = Convert.ToInt32(Console.ReadLine());

Console.Write("Введіть число x:");
double x = Convert.ToInt32(Console.ReadLine());

double result = CalculateResult(n, x, 1, 1);

Console.Write(result);

