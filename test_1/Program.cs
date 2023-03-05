// See https://aka.ms/new-console-template for more information

// Методи до містять в імені desing домомагають зробити "красіво"


//Дизайн-метод для виведення вказаної кількості порожніх рядків в консолі

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

    DesingEmptyLines(2);
    string welcomeText = "\n\t\t" + "Ласкаво просимо до генератора логічних формул";
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


string userName = Welcome();
Console.WriteLine(userName);


