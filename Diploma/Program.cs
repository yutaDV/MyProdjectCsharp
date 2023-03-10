
using static System.Net.Mime.MediaTypeNames;

namespace Diploma;

class Program
{

    //Дизайн-метод для виведення тексту по центру
    static void DesingCenterText(string text)
    {
        /* Визначаємо координати для виведення тексту по центру
            x - розраховуємо відступ
            y  -виводимо в поточному рядку консолі*/
        int x = (Console.WindowWidth - text.Length) / 2; 
        int y = Console.CursorTop; 
        // Переміщуємо курсор до потрібної позиції та виводимо текст
        Console.SetCursorPosition(x, y);
        Console.WriteLine(text);

    }

    //Дизайн-метод для виведення вказаної кількості порожніх рядків в консолі
    static void DesingEmptyLines(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine();
        }
    }

    //Дизайн-метод для виведення тексту в кольоровій гамі на кольоровому фоні
    static void DesingColorText(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        // Зберігаємо поточні кольори консолі
        ConsoleColor oldForegroundColor = Console.ForegroundColor;
        ConsoleColor oldBackgroundColor = Console.BackgroundColor;
        // Змінюємо кольори
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;
        // Виводимо текст
        Console.Write(text);
        // Повертаємо попередні кольори консолі
        Console.ForegroundColor = oldForegroundColor;
        Console.BackgroundColor = oldBackgroundColor;
    }

    // Дизайн-метод псевдо-очікування відповіді програми
    static void DesingWaiting()
    {
        Console.Write("\t\t\tДумаю...");
        Thread.Sleep(1000);
        Console.Write("генерую...");
        Thread.Sleep(1000);
        Console.WriteLine("комплектую...");
        Thread.Sleep(1000);
        DesingEmptyLines(2);
    }

    //Дизайн-метод для друку масуви з результатами примає масив який потрібно надрукувати
    static void DesingPrintFormulasTable(string[] formulas)
    {
        // Визначити ширину першого стовпчика для номерів формул
        int firstColWidth = formulas.Length.ToString().Length;
        // Форматування рядка заголовка таблиці
        string header = "{0,-" + firstColWidth + "} | {1,-30}";
        // Виведення заголовка таблиці
        Console.WriteLine(header, "\n\t\tNo.", "Formula");
        Console.Write(new string(' ', 15));
        Console.WriteLine(new string('-', 60));
        // Виведення формул в табличному вигляді
        for (int i = 0; i < formulas.Length; i++)
        {
            Console.WriteLine("\t\t{0,-" + firstColWidth + "} | {1,-30}", i + 1, formulas[i]);
        }
    }

    // Метод вітання користувача повертає ім'я користувача або присвоює імя анонім
    static string Welcome()
    {
        DesingEmptyLines(2);
        string welcomeText = "***** Ласкаво просимо до генератора логічних формул *****";
        DesingCenterText(welcomeText);
        DesingEmptyLines(2);
        Console.Write("\t\t Введіть ваше ім'я:  ");
        string userName =  Console.ReadLine();

        if (userName == null || userName == "")
        {
            userName = "анонім";
        }
        DesingEmptyLines(1);
        string welcomeUser = $"\t\t Привіт, {userName}! розпочнемо!";
        DesingColorText(welcomeUser, ConsoleColor.Blue, ConsoleColor.Yellow);
        DesingEmptyLines(2);
        return userName;
    }

    // метод для отримання відповіді користувача у форматі так чи ні
    static bool GetUserConfirmation(string message)
    {
        while (true)
        {
            Console.Write(message + " (Y/N): ");
            string input = Console.ReadLine().Trim().ToUpper(); //прибираємо пробіли та великі літери

            if (input == "Y")
            {
                return true;
            }
            else if (input == "N")
            {
                return false;
            }
            else
            {
                string textError = ("\t\tНевірний ввід. Будь ласка, введіть Y або N.");
                DesingColorText(textError, ConsoleColor.Black, ConsoleColor.Red);
            }
        }
    }

    /* додатковий метод для виконання методу GenerateFormulas приймає  рівень складносі 
     * та масив зі змінними і повертає  згенеровані підвормули, 
     * рівень складності використовується для визначення к-ті компонентів у формулі*/
    static string GenerateSubFormulas(int level, Random random, string[] variables)
    {
        //можливі оператори
        string[] operators = new string[] { "&&", "||", "!", "^", "<=>", "=>" };
        // якщо рівень складност або к-ть зміних 1 можливі лише два варіанти 
        if (level == 1 || variables.Length == 1)
        {
            int varIndex = random.Next(variables.Length);
            int randomNumber = random.Next(0, 2);
            string formula = (randomNumber == 0) ? "!" + variables[varIndex] : variables[varIndex];
            return formula;
        }
        /*інакше застосовується рекурсія для застосування попереднього рівня складності поки
         * рівень складності не буде дорівнювати 1*/
        else
        {
            // створюємо масив для поєднання 2-х підформул
            string[] subFormulas = new string[2];
            // формування підвормул застосовуючи рекурсію
            do
            {
                subFormulas[0] = GenerateSubFormulas(level - 1, random, variables);
                subFormulas[1] = GenerateSubFormulas(level - 1, random, variables);
            }
            //перевірка унікальності змінних
            while (!CheckUniqueVariables(subFormulas[0]) || !CheckUniqueVariables(subFormulas[1]));
            string oper = operators[random.Next(operators.Length)];
            //побудова підформули для заперечення
            if (oper == "!")
            {
                string subFormula = GenerateSubFormulas(level - 1, random, variables);
                return oper + subFormula;
            }
            // склейка Join підвормул в один рядок 
            string formula = "(" + string.Join(" " + oper + " ", subFormulas) + ")";
            return formula;
        }
    }

    /*метод для перевірки унікальності зміних використовується в методі
     * GenerateSubFormulas розбиває рядок formula на окремі слова 
     * (використовуючи пробіл як роздільник), видалити дубльовані слова та 
     * зберегти унікальні слова у вигляді масиву рядків і 
     * перевіряємо чи не змінилась довжина*/
    static bool CheckUniqueVariables(string formula)
    {
        string[] variables = formula.Split(' ').Distinct().ToArray();
        return variables.Length == formula.Split(' ').Length;
    }

    //метод для формування масиву формул, приймає к-ть зміних і перетворює їх в масив літер
    static string GenerateFormulas(int numVariables, Random random)
    {
        string[] variables = new string[numVariables];
        for (int i = 0; i < numVariables; i++)
        {
            variables[i] = ((char)('A' + i)).ToString();
        }
        int level = random.Next(1, 7);
        string formula = GenerateSubFormulas(level, random, variables);

        return formula;
     }

    // Main метод для генератора (меню) із запитом даних від користувача і повернення їх
    static void GenerateMain()
    {
        int numVariables = 0, numFormulas = 0;

        while (true)
        {
            Console.Write("\n\t\tВведіть максимальну кількість змінних (максимум 10): ");
            if (!int.TryParse(Console.ReadLine(), out numVariables) || numVariables < 1 || numVariables > 10)
            {
                DesingEmptyLines(1);
                string textError = ("\t\tПомилка введення! Максимальна кількість змінних - 10.");
                DesingColorText(textError, ConsoleColor.Black, ConsoleColor.Red);
                continue;
            }
            break;
        }

        while (true)
        {
            Console.Write("\n\t\tВведіть кількість формул, які бажаєте згенерувати (максимум 20): ");
            if (!int.TryParse(Console.ReadLine(), out numFormulas) || numFormulas < 1 || numFormulas > 20)
            {
                DesingEmptyLines(1);
                string textError = ("\t\t Помилка введення! Максимальна кількість формул - 20.");
                DesingColorText(textError, ConsoleColor.Black, ConsoleColor.Red);
                continue;
            }
            break;
        }

        string[] formulas = new string[numFormulas];

        Random random = new Random();
        for (int i = 0; i < numFormulas; i++)
        {
            string formula = GenerateFormulas(numVariables, random);
            formulas[i] = formula;
        }
        DesingEmptyLines(1);
        DesingWaiting();
        string textResult = ("\t\t Чудово!!! Тримай формули: ");
        DesingColorText(textResult, ConsoleColor.Black, ConsoleColor.Cyan);
        DesingEmptyLines(2);
        DesingPrintFormulasTable(formulas);
    }

    static void DesingChoiceLogicOperator(string textTitle, string textDescription)
    {

        string textChouse = $"\t\tВи обрали {textTitle} ";
        DesingColorText(textChouse, ConsoleColor.Black, ConsoleColor.Yellow);
        DesingEmptyLines(2);
        DesingWaiting();
        textChouse = $"\t\t!Довідка! {textDescription}";
        DesingColorText(textChouse, ConsoleColor.Black, ConsoleColor.DarkYellow);
        DesingEmptyLines(2);
        textChouse = "\t\tтаблиця істиності буде виглядати наступним чином: ";
        DesingColorText(textChouse, ConsoleColor.Black, ConsoleColor.Yellow);
        DesingEmptyLines(2);

    }

    static void LogicTheoryMenu()
    {
        bool[] a = { false, false, true, true };
        bool[] b = { false, true, false, true };
        while (true)
        {
            DesingEmptyLines(2);
            Console.WriteLine("\t\tОберіть операцію про яку бажаєте дізнатись або натисніть 7:");
            Console.WriteLine("\t1. Логічне І (&&)");
            Console.WriteLine("\t2. Логічне АБО (||)");
            Console.WriteLine("\t3. Виключе АБО XOR (^)");
            Console.WriteLine("\t4. Заперечення (!)");
            Console.WriteLine("\t5. Еквівалентність (<=>)");
            Console.WriteLine("\t6. Імплікація (=>)");
            Console.WriteLine("\t7. Вихід");
            Console.Write("Ваш вибір: ");

            string choiceString = Console.ReadLine();
            int choice;

            if (!int.TryParse(choiceString, out choice))// перевіряємо чи введено число 
            {
                string upsError = "\t\t Невірний ввід. Виберіть опцію від 1 до 7.";
                DesingColorText(upsError, ConsoleColor.Black, ConsoleColor.Red);
                continue;
            }

            switch (choice)
            {
                case 1:
                    LogicAnd(ref a, ref b);
                    break;
                case 2:
                    LogicOr(ref a, ref b);
                    break;
                case 3:
                    LogicXor(ref a, ref b);
                    break;
                case 4:
                    LogicNot();
                    break;
                case 5:
                    LogicEqual(ref a, ref b);
                    break;
                case 6:
                    LogicImplication(ref a, ref b);
                    break;
                case 7:
                    Console.WriteLine("\n\t\tДякую за цікавіть! Всього найкращого!");
                    return;
                default:
                    string upsError = "\t\t Невірний ввід. Виберіть опцію від 1 до 7.";
                    DesingColorText(upsError, ConsoleColor.Black, ConsoleColor.Red);
                    break;
            }
        }
    }

    static void DesingPrintTruthTable(string[,] truthTable, string oper)
    {
        int numRows = truthTable.GetLength(0);
        int numCols = truthTable.GetLength(1);
        // Виводимо заголовок таблиці
        Console.WriteLine("\t{0,-5} {1,-5} {2,-5}", "\tA", "\t\tB", $"\t     A {oper} B");
        Console.Write(new string(' ', 15));
        Console.WriteLine(new string('-', 45));
        // Виводимо значення масиву у вигляді таблиці
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                Console.Write("\t\t{0,-5} ", truthTable[i, j]);
            }
            Console.WriteLine();
        }
    }

    static void DesingPrintNotTable(string[,] truthTable)
    {
        int numRows = truthTable.GetLength(0);
        int numCols = truthTable.GetLength(1);
        // Виводимо заголовок таблиці
        Console.WriteLine("\t{0}\t{1}", "A", "!A");
        Console.Write(new string(' ', 5));
        Console.WriteLine(new string('-', 15));
        // Виводимо значення масиву у вигляді таблиці
        for (int i = 0; i < numRows; i++)
        {
            Console.WriteLine("\t{0}\t{1}", truthTable[i, 0], truthTable[i, 1]);
        }
    }

    static void LogicAnd(ref bool[]a, ref bool[]b)
    {
        string[,] truthTable = new string[4, 3];

        for (int i = 0; i < 4; i++)
        {
            bool result = a[i] && b[i];
            truthTable[i, 0] = Convert.ToInt32(a[i]).ToString();
            truthTable[i, 1] = Convert.ToInt32(b[i]).ToString();
            truthTable[i, 2] = Convert.ToInt32(result).ToString();
        }
        string textTitle= "логічне І (&&)";
        string  textDescription = "Логічне І поверне true, якщо обидва операнди є true. В інших випадках воно поверне false.";
        DesingChoiceLogicOperator(textTitle, textDescription);
        DesingPrintTruthTable(truthTable, "&&");
    }

    static void LogicOr(ref bool[] a, ref bool[] b)
    {
        string[,] truthTable = new string[4, 3];

        for (int i = 0; i < 4; i++)
        {
            bool result = a[i] || b[i];
            truthTable[i, 0] = Convert.ToInt32(a[i]).ToString();
            truthTable[i, 1] = Convert.ToInt32(b[i]).ToString();
            truthTable[i, 2] = Convert.ToInt32(result).ToString();
        }
        string textTitle = "логічне АБО (||)";
        string textDescription = "Логічне АБО поверне true, якщо хоча б один з операндів є true. Якщо обидва операнди є false, то воно поверне false.";
        DesingChoiceLogicOperator(textTitle, textDescription);
        DesingPrintTruthTable(truthTable, "||");
    }

    static void LogicXor(ref bool[] a, ref bool[] b)
    {
        string[,] truthTable = new string[4, 3];

        for (int i = 0; i < 4; i++)
        {
            bool result = a[i] ^ b[i];
            truthTable[i, 0] = Convert.ToInt32(a[i]).ToString();
            truthTable[i, 1] = Convert.ToInt32(b[i]).ToString();
            truthTable[i, 2] = Convert.ToInt32(result).ToString();
        }
        string textTitle = "ВИКЛЮЧНO-АБО (^)";
        string textDescription = "Логічне ВИКЛЮЧНО-АБО поверне true, якщо тільки один з операндів є true. Якщо обидва операнди є true або false, то воно поверне false.";
        DesingChoiceLogicOperator(textTitle, textDescription);
        DesingPrintTruthTable(truthTable, "^");
    }

    static void LogicNot()
    {
        bool[] a = { false, true };

        string[,] truthTable = new string[2, 2];

        for (int i = 0; i < 2; i++)
        {
            bool result = !a[i];

            truthTable[i, 0] = Convert.ToInt32(a[i]).ToString();
            truthTable[i, 1] = Convert.ToInt32(result).ToString();
        }
        string textTitle = "ЛОГІЧНЕ ЗАПЕРЕЧЕННЯ (!)";
        string textDescription = "Логічне ЗАПЕРЕЧЕННЯ поверне true, якщо операнд є false, і false, якщо операнд є true.";
        DesingChoiceLogicOperator(textTitle, textDescription);
        DesingPrintNotTable(truthTable);
    }

    static void LogicEqual(ref bool[] a, ref bool[] b)
    {
        string[,] truthTable = new string[4, 3];

        for (int i = 0; i < 4; i++)
        {
            bool result = a[i] == b[i];
            truthTable[i, 0] = Convert.ToInt32(a[i]).ToString();
            truthTable[i, 1] = Convert.ToInt32(b[i]).ToString();
            truthTable[i, 2] = Convert.ToInt32(result).ToString();
        }
        string textTitle = "ЛОГІЧНА ЕКВІВАЛЕНТНІСТЬ (<=>)";
        string textDescription = "Логічна ЕКВІВАЛЕНТНІСТЬ поверне true, якщо обидва операнди є true або обидва операнди є false. В іншому випадку, воно поверне false.";
        DesingChoiceLogicOperator(textTitle, textDescription);
        DesingPrintTruthTable(truthTable, "<=>");
    }

    static void LogicImplication(ref bool[] a, ref bool[] b)
    {
        string[,] truthTable = new string[4, 3];

        for (int i = 0; i < 4; i++)
        {
            bool result = !a[i] || b[i];
            truthTable[i, 0] = Convert.ToInt32(a[i]).ToString();
            truthTable[i, 1] = Convert.ToInt32(b[i]).ToString();
            truthTable[i, 2] = Convert.ToInt32(result).ToString();
        }
        string textTitle = "ІМПЛІКАЦІЯ (=>)";
        string textDescription = "Логічна ІМПЛІКАЦІЯ поверне false, якщо перший операнд (a) є true, а другий операнд (b) є false. В іншому випадку, воно поверне true.";
        DesingChoiceLogicOperator(textTitle, textDescription);
        DesingPrintTruthTable(truthTable, "=>");
    }

    static void GeneratePpredictions()
    {
        string[] predictions = {"Сьогодні буде хороший день.",
                                        "Ти зробиш велику справу сьогодні!",
                                        "Всі твої мрії здійсняться.",
                                        "Ти будеш дуже задоволений результатами своєї роботи.",
                                        "Щасливі зміни чекають на тебе.",
                                        "Твоя впевненість допоможе тобі досягти успіху.",
                                        "Ти знаходишся на правильному шляху до своєї мети.",
                                        "Твоя енергія та наполегливість допоможуть тобі досягти успіху.",
                                        "Успіх вже чекає на тебе, ти просто мусиш досягнути його.",
                                        "Твоя робота визнана і дуже цінується.",
                                        "Ти дуже талановитий, і цей талант принесе тобі успіх.",
                                        "Твоя наступна ідея буде дуже успішною.",
                                        "Твоя творчість надихає та мотивує багатьох людей.",
                                        "Ти маєш потенціал стати одним з найкращих в своїй сфері.",
                                        "Ти заслуговуєш на успіх, і він обов'язково прийде."};

        string positivePrediction = GeneratePrediction(predictions);
        DesingWaiting();
        DesingColorText("\t\t" + positivePrediction, ConsoleColor.Black, ConsoleColor.Cyan);
        DesingEmptyLines(2);
    }

    static string GeneratePrediction(string[] predictions)
    {
        Random random = new Random();
        int index = random.Next(predictions.Length);
        return predictions[index];
    }

    //метод який відображає основне меню та визначає наступний метод відповідно до вибору користувача
    static void Menu()
    {
        bool isGoing = true;
        while (isGoing)
        {

            DesingEmptyLines(1);
            Console.WriteLine("\t\t Виберіть опцію:");
            DesingEmptyLines(2);
            Console.WriteLine("\t 1. Згенерувати декілька логічних формул");
            Console.WriteLine("\t 2. Дізнатись більше про логічні операції");
            Console.WriteLine("\t 3. БОНУС Передбачення");
            Console.WriteLine("\t 4. Вийти");
            DesingEmptyLines(1);
            Console.Write("\t\tВведіть номер опції: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("\n\t\t1. Згенерувати формулу");
                    bool tryAgain = true;
                    do
                    {
                        GenerateMain();
                        tryAgain = GetUserConfirmation("\n\t\tБажаєш скористатись генератором ще раз? ");
                    }
                    while (tryAgain == true);
                    break;
                case "2":
                    Console.WriteLine("\n\t\t2. Тут ви можете дізнатись про логічні оператори та їх значення!");
                    LogicTheoryMenu();
                    break;
                case "3":
                    Console.WriteLine("\n\t\tТримай передбачення: ");
                    tryAgain = true;
                    do
                    {
                        GeneratePpredictions();
                        tryAgain = GetUserConfirmation("\n\t\tБажаєш отримати ще одне передбачення ");
                    }
                    while (tryAgain == true);
                    break;
                case "4":
                    DesingEmptyLines(2);
                    string exitText = "***** Бувай до зустрічі!!! *****";
                    DesingCenterText(exitText);
                    isGoing = false;
                    break;
                default:
                    DesingEmptyLines(2);
                    string upsError = "\t\t Невірний ввід. Виберіть опцію від 1 до 3.";
                    DesingColorText(upsError, ConsoleColor.Black, ConsoleColor.Red);
                    break;
            }
        }
    }
    
        static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        string userName = Welcome();
        Menu();
        Console.ReadKey();
    }
}
