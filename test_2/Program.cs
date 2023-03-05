// See https://aka.ms/new-console-template for more information

/*Кон'юнкція (AND) - позначається символом '&', результатом операції буде 'true' тільки в тому випадку, якщо обидві операції є 'true'.
Диз'юнкція (OR) - позначається символом '|', результатом операції буде 'true', якщо принаймні одна з операцій є 'true'.
Від'ємне значення (NOT) - позначається символом '!', ця операція змінює результат операції на протилежне, наприклад, якщо результат операції дорівнює 'true', після застосування операції 'NOT' результат буде 'false'.
Еквіваленція (XOR) - позначається символом '^', результатом операції буде 'true', якщо різниця між операціями є парна (наприклад, true xor false = true).
Імплікація (IMP) - позначається символом '->', результатом операції буде 'false', якщо перша операція дорівнює 'true', а друга - 'false', в інших випадках результат буде 'true'.*/

//Дизайн-метод для виведення вказаної кількості порожніх рядків в консолі
using System;

static void DesingEmptyLines(int count)
{

    for (int i = 0; i < count; i++)
    {
        Console.WriteLine();
    }

}


// Метод вітання користувача повертає ім'я користувача
static string Welcome()
{
    DesingEmptyLines(3);
    string welcomeText = "\t" + "Ласкаво просимо до генератора логічних формул";
    Console.WriteLine(welcomeText);
    DesingEmptyLines(2);
    Console.Write($"Введіть ваше ім'я:  ");
    string userName = Console.ReadLine();
    if (userName == null || userName == "")
    {
        userName = "Анонім";
    }
    string welcomeUser = ($"Привіт, {userName}! розпочнемо!");
    DesingEmptyLines(2);
    Console.WriteLine($"Привіт, {userName}! розпочнемо!");
    return userName;
}

//метод який відображає основне меню та визначає наступний метод відповідно до вибору користувача
static void Menu()
{
    DesingEmptyLines(1);
    Console.WriteLine("\t\t\t" + "Виберіть опцію:");

    bool isGoing = true;

    while (isGoing)
    {

        DesingEmptyLines(1);
        Console.WriteLine("\t\t\t\t 1. Згенерувати формулу");
        Console.WriteLine("\t\t\t\t 2. Дізнатись більше");
        Console.WriteLine("\t\t\t\t 3. Вийти");
        DesingEmptyLines(1);
        Console.Write("\t\t" + "Введіть номер опції: ");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.WriteLine("1. Згенерувати формулу");
                //GenerateFormula();
                break;
            case "2":
                Console.WriteLine("2. Дізнатись більше");
                //ComputeFormula();
                break;
            case "3":
                isGoing = false;
                break;
            default:
                Console.WriteLine("Невірний ввід. Виберіть опцію від 1 до 3.");
                break;
        }
    }
}


string userName = Welcome();
Console.WriteLine(userName);
Menu();


// рандомно вибираємо один з двох варіантів і повертаємо його на цьому все
if (variables.Length == 1)
{
    Random randomTwo = new Random();
    int randomNumber = randomTwo.Next(0, 2);
    string randomFormul = (randomNumber == 0) ? "!" + variables[0] : variables[0];
    return randomFormul;
}

int operIndex = random.Next(operators.Length);//випадковий оператор
string oper = operators[operIndex];

if (oper == "!")
{
    string subFormul = GenerateSubFormulas(variables, operators, random);
    return oper + subFormul;
}
else
{
    //к-ть підформул від 1 до довжини масиву змінних
    int numSubFormuls = random.Next(1, variables.Length);
    string[] subFormuls = new string[numSubFormuls];
    for (int i = 0; i < numSubFormuls; i++)
    {
        int varIndex = random.Next(variables.Length);//випадковий індекс для змінної
        subFormuls[i] = variables[varIndex];
    }

    string formula = "(" + string.Join(" " + oper + " ", subFormuls) + ")";
    return formula;