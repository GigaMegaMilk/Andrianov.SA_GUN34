using System;
using System.Text;

// Использование операторов верхнего уровня 
var program = new Program();
program.RunTests();

partial class Program
{
    public void RunTests()
    {
        // Тест для задания 1
        string concatResult = ConcatenateStrings("Hello", " World");
        Console.WriteLine($"1. Конкатенация: {concatResult}");
        Console.WriteLine();

        // Тест для задания 2
        string greetResult = GreetUser("Alex", 30);
        Console.WriteLine($"2. Приветствие:\n{greetResult}");
        Console.WriteLine();

        // Тест для задания 3
        string infoResult = GetStringInfo("C# is Awesome!");
        Console.WriteLine($"3. Информация о строке:\n{infoResult}");
        Console.WriteLine();

        // Тест для задания 4
        string firstFive = GetFirstFiveChars("Programming");
        Console.WriteLine($"4. Первые 5 символов: {firstFive}");
        Console.WriteLine();

        // Тест для задания 5
        string[] words = { "This", "is", "a", "test" };
        StringBuilder sbResult = BuildSentence(words);
        Console.WriteLine($"5. Построение предложения:\n{sbResult}");
        Console.WriteLine();

        // Тест для задания 6
        string replaced = ReplaceWords("Hello world, world is beautiful", "world", "universe");
        Console.WriteLine($"6. Замена слов:\n{replaced}");
    }

    // Задание 1: Конкатенация строк
    public string ConcatenateStrings(string str1, string str2)
    {
        return str1 + str2;
    }

    // Задание 2: Форматированное приветствие
    public string GreetUser(string name, int age)
    {
        return $"Hello, {name}!{Environment.NewLine}You are {age} years old.";
    }

    // Задание 3: Информация о строке
    public string GetStringInfo(string input)
    {
        return $"Длина строки: {input.Length}\n" +
               $"Верхний регистр: {input.ToUpper()}\n" +
               $"Нижний регистр: {input.ToLower()}";
    }

    // Задание 4: Первые 5 символов
    public string GetFirstFiveChars(string input)
    {
        return input.Length <= 5 ? input : input.Substring(0, 5);
    }

    // Задание 5: Построение предложения из массива строк
    public StringBuilder BuildSentence(string[] words)
    {
        var sb = new StringBuilder();
        foreach (string word in words)
        {
            sb.Append(word).Append(' '); // Использование Append с пробелом
        }
        return sb;
    }

    // Задание 6: Замена слов в строке
    public string ReplaceWords(string inputString, string wordToReplace, string replacementWord)
    {
        return inputString.Replace(wordToReplace, replacementWord);
    }
}