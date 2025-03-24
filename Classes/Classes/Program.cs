using System;

public class Unit
{
    // Свойства
    public string Name { get; } // Имя юнита (только для чтения)
    private float health;       // Приватное поле для здоровья
    public float Health         // Свойство здоровья (только для чтения)
    {
        get { return health; }
    }
    public int Damage { get; } = 5; // Урон (только для чтения)
    public float Armor { get; } = 0.6f; // Броня (только для чтения)

    // Конструкторы
    public Unit() : this("Unknown Unit") { } // Конструктор по умолчанию

    public Unit(string name) // Конструктор с именем
    {
        Name = name;
        health = 100f; // Начальное здоровье
    }

    // Методы
    public float GetRealHealth() // Фактическое здоровье
    {
        return Health * (1f + Armor);
    }

    public bool SetDamage(float value) // Получить урон
    {
        health -= value * Armor;
        return health <= 0f;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Создаем юнита с именем "PIVOSOS"
        Unit PIVOSOS = new Unit("PIVOSOS");

        // Выводим информацию о юните
        Console.WriteLine($"Name: {PIVOSOS.Name}");
        Console.WriteLine($"Health: {PIVOSOS.Health}");
        Console.WriteLine($"Real Health: {PIVOSOS.GetRealHealth()}");
        Console.WriteLine($"Damage: {PIVOSOS.Damage}");
        Console.WriteLine($"Armor: {PIVOSOS.Armor}");

        // Наносим урон
        bool isDead = PIVOSOS.SetDamage(50);
        Console.WriteLine($"Is Dead: {isDead}");
        Console.WriteLine($"Health after damage: {PIVOSOS.Health}");
    }
}