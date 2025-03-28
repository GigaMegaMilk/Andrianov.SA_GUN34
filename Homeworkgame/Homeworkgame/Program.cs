using System;
using System.Collections.Generic;

namespace SimpleTextroguliterpg
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }

    class Game
    {
        private Player player;
        private List<Room> rooms;
        private Random random;
        private int currentRoomIndex;

        public Game()
        {
            random = new Random();
            InitializeRooms();
        }

        public void Start()
        {
            Console.WriteLine("Добро пожаловать в простую текстовую роглайт RPG!");
            CreatePlayer();
            currentRoomIndex = 0;
            GameLoop();
        }

        private void CreatePlayer()
        {
            Console.Write("Введите имя вашего персонажа: ");
            string name = Console.ReadLine();

            Console.WriteLine("\nВыберите стартовую экипировку:");
            Console.WriteLine("1. Меч и кожаная броня (атака: 10, защита: 5, прочность: 100)");
            Console.WriteLine("2. Топор и кольчуга (атака: 12, защита: 8, прочность: 80)");
            Console.WriteLine("3. Посох и мантия (атака: 15, защита: 3, прочность: 60)");
            Console.Write("Ваш выбор: ");

            int choice = int.Parse(Console.ReadLine());
            Equipment equipment = choice switch
            {
                1 => new Equipment("Меч", "Кожаная броня", 10, 5, 100),
                2 => new Equipment("Топор", "Кольчуга", 12, 8, 80),
                3 => new Equipment("Посох", "Мантия", 15, 3, 60),
                _ => new Equipment("Деревянный меч", "Тряпки", 5, 1, 50)
            };

            player = new Player(name, 100, equipment);
            Console.WriteLine($"\nПерсонаж {player.Name} создан!");
            player.ShowStatus();
        }

        private void InitializeRooms()
        {
            rooms = new List<Room>
            {
                new Room("Каменная вонючая пещера", "Вы в темной сырой пещере. Капли воды падают с потолка."),
                new Room("Лесная не вонючая поляна", "Вы выходите на солнечную поляну, окруженную высокими деревьями."),
                new Room("Заброшенный почти вонючий замок", "Разрушенный замок с разбитыми стенами и скрипучими дверями."),
                new Room("Подземелье явно не вонючее", "Мрачное подземелье с цепями на стенах и странными символами."),
                new Room("Тронный зал самый вонючий", "Огромный зал с разрушенным троном. Кажется, это финальная комната.")
            };

            // Добавляем врагов и лут в комнаты
            rooms[0].Enemy = new Enemy("Гоблин-гнида", 30, 5, 20);
            rooms[0].Loot = new Item("Зелье здоровья", "Восстанавливает 25 HP", ItemType.Consumable);

            rooms[1].Enemy = new Enemy("Волк-урод", 25, 7, 15);
            rooms[1].Loot = new Item("Кожаный доспех", "Защита +3", ItemType.Armor);

            rooms[2].Enemy = new Enemy("Скелет-пердед", 40, 8, 30);
            rooms[2].Loot = new Item("Стальной меч", "Атака +5", ItemType.Weapon);

            rooms[3].Enemy = new Enemy("Орк-кро", 50, 10, 40);
            rooms[3].Loot = new Item("Кольцо силы", "Атака +3, Защита +3", ItemType.Accessory);

            rooms[4].Enemy = new Enemy("Дракон-ган...", 100, 15, 100);
            rooms[4].Loot = new Item("Легендарный артефакт", "Победа в игре!", ItemType.Quest);
        }

        private void GameLoop()
        {
            while (player.Health > 0 && currentRoomIndex < rooms.Count)
            {
                EnterRoom(rooms[currentRoomIndex]);

                if (player.Health <= 0) break;

                Console.WriteLine("\nЧто вы хотите сделать?");
                Console.WriteLine("1. Перейти в следующую комнату");
                Console.WriteLine("2. Проверить статус");
                Console.WriteLine("3. Выйти из игры");
                Console.Write("Ваш выбор: ");

                int choice = int.Parse(Console.ReadLine());
                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        currentRoomIndex++;
                        break;
                    case 2:
                        player.ShowStatus();
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор, попробуйте снова.");
                        break;
                }
            }

            if (player.Health <= 0)
            {
                Console.WriteLine("Игра окончена. Вы погибли!");
            }
            else
            {
                Console.WriteLine("Поздравляем! Вы прошли игру и нашли легендарный артефакт!");
            }
        }

        private void EnterRoom(Room room)
        {
            Console.WriteLine($"\n=== {room.Name} ===");
            Console.WriteLine(room.Description);

            if (room.Enemy != null)
            {
                Console.WriteLine($"\nВ комнате находится {room.Enemy.Name} (HP: {room.Enemy.Health}, ATK: {room.Enemy.Attack})!");
                Combat(room.Enemy);
            }

            if (room.Loot != null && player.Health > 0)
            {
                Console.WriteLine($"\nВы нашли {room.Loot.Name} - {room.Loot.Description}");
                Console.WriteLine("Предмет добавлен в инвентарь.");
                player.AddItemToInventory(room.Loot);
                room.Loot = null; // Убираем лут из комнаты после подбора
            }
        }

        private void Combat(Enemy enemy)
        {
            while (enemy.Health > 0 && player.Health > 0)
            {
                Console.WriteLine("\n1. Атаковать");
                Console.WriteLine("2. Использовать предмет");
                Console.WriteLine("3. Попытаться убежать");
                Console.Write("Ваш выбор: ");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        PlayerAttack(enemy);
                        if (enemy.Health > 0) EnemyAttack(enemy);
                        break;
                    case 2:
                        UseItem();
                        EnemyAttack(enemy);
                        break;
                    case 3:
                        if (random.Next(0, 2) == 0)
                        {
                            Console.WriteLine("Вам удалось сбежать!");
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Попытка побега не удалась!");
                            EnemyAttack(enemy);
                        }
                        break;
                    default:
                        Console.WriteLine("Неверный выбор, враг атакует!");
                        EnemyAttack(enemy);
                        break;
                }

                player.ShowStatus();
                Console.WriteLine($"{enemy.Name}: HP {enemy.Health}");
            }

            if (enemy.Health <= 0)
            {
                Console.WriteLine($"\nВы победили {enemy.Name} и получили {enemy.ExpReward} опыта!");
                player.GainExp(enemy.ExpReward);
            }
        }

        private void PlayerAttack(Enemy enemy)
        {
            int damage = player.Equipment.Attack + random.Next(1, 5);
            enemy.Health -= damage;
            Console.WriteLine($"Вы атакуете {enemy.Name} и наносите {damage} урона!");

            // Уменьшаем прочность оружия
            player.Equipment.Durability -= random.Next(1, 5);
            if (player.Equipment.Durability <= 0)
            {
                Console.WriteLine("Ваше оружие сломалось!");
                player.Equipment.Attack = 1; // Минимальный урон
            }
        }

        private void EnemyAttack(Enemy enemy)
        {
            int damage = enemy.Attack + random.Next(1, 3) - player.Equipment.Defense / 2;
            if (damage < 1) damage = 1;
            player.Health -= damage;
            Console.WriteLine($"{enemy.Name} атакует вас и наносит {damage} урона!");

            // Уменьшаем прочность брони
            player.Equipment.Durability -= random.Next(1, 3);
            if (player.Equipment.Durability <= 0)
            {
                Console.WriteLine("Ваша броня разрушилась!");
                player.Equipment.Defense = 1; // Минимальная защита
            }
        }

        private void UseItem()
        {
            Console.WriteLine("\nВаш инвентарь:");
            for (int i = 0; i < player.Inventory.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {player.Inventory[i].Name} - {player.Inventory[i].Description}");
            }

            Console.Write("Выберите предмет для использования (0 для отмены): ");
            int choice = int.Parse(Console.ReadLine()) - 1;

            if (choice >= 0 && choice < player.Inventory.Count)
            {
                Item item = player.Inventory[choice];
                if (item.Type == ItemType.Consumable)
                {
                    player.Health += 25;
                    if (player.Health > 100) player.Health = 100;
                    Console.WriteLine("Вы использовали зелье здоровья и восстановили 25 HP!");
                    player.Inventory.RemoveAt(choice);
                }
                else
                {
                    Console.WriteLine("Этот предмет нельзя использовать напрямую.");
                }
            }
        }
    }

    class Player
    {
        public string Name { get; }
        public int Health { get; set; }
        public Equipment Equipment { get; }
        public List<Item> Inventory { get; }
        public int Exp { get; private set; }
        public int Level { get; private set; }

        public Player(string name, int health, Equipment equipment)
        {
            Name = name;
            Health = health;
            Equipment = equipment;
            Inventory = new List<Item>();
            Exp = 0;
            Level = 1;
        }

        public void ShowStatus()
        {
            Console.WriteLine($"\n=== Статус {Name} ===");
            Console.WriteLine($"HP: {Health}/100");
            Console.WriteLine($"Уровень: {Level} (Опыт: {Exp}/{Level * 100})");
            Console.WriteLine($"Оружие: {Equipment.WeaponName} (АТК: {Equipment.Attack})");
            Console.WriteLine($"Броня: {Equipment.ArmorName} (ЗАЩ: {Equipment.Defense})");
            Console.WriteLine($"Прочность: {Equipment.Durability}%");
            Console.WriteLine("Инвентарь:");
            foreach (var item in Inventory)
            {
                Console.WriteLine($"- {item.Name}: {item.Description}");
            }
        }

        public void AddItemToInventory(Item item)
        {
            Inventory.Add(item);
        }

        public void GainExp(int amount)
        {
            Exp += amount;
            if (Exp >= Level * 100)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            Level++;
            Exp = 0;
            Console.WriteLine($"\nПоздравляем! Вы достигли уровня {Level}!");
            Equipment.Attack += 2;
            Equipment.Defense += 1;
            Equipment.Durability += 20;
            if (Equipment.Durability > 100) Equipment.Durability = 100;
            Health = 100;
            Console.WriteLine("Ваши характеристики улучшены!");
        }
    }

    class Equipment
    {
        public string WeaponName { get; }
        public string ArmorName { get; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Durability { get; set; }

        public Equipment(string weaponName, string armorName, int attack, int defense, int durability)
        {
            WeaponName = weaponName;
            ArmorName = armorName;
            Attack = attack;
            Defense = defense;
            Durability = durability;
        }
    }

    class Room
    {
        public string Name { get; }
        public string Description { get; }
        public Enemy Enemy { get; set; }
        public Item Loot { get; set; }

        public Room(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }

    class Enemy
    {
        public string Name { get; }
        public int Health { get; set; }
        public int Attack { get; }
        public int ExpReward { get; }

        public Enemy(string name, int health, int attack, int expReward)
        {
            Name = name;
            Health = health;
            Attack = attack;
            ExpReward = expReward;
        }
    }

    class Item
    {
        public string Name { get; }
        public string Description { get; }
        public ItemType Type { get; }

        public Item(string name, string description, ItemType type)
        {
            Name = name;
            Description = description;
            Type = type;
        }
    }

    enum ItemType
    {
        Weapon,
        Armor,
        Consumable,
        Accessory,
        Quest
    }
}