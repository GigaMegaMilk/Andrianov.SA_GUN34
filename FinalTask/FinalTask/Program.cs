using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace HardcoreCasino
{
    public class PlayerProfile
    {
        public string Name { get; set; }
        public int Balance { get; set; } = 1000;
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
    }

    public class Card
    {
        public string Suit { get; }
        public string Rank { get; }
        public int Value { get; }
        public Card(string suit, string rank, int value) => (Suit, Rank, Value) = (suit, rank, value);
        public override string ToString() => $"{Rank} of {Suit}";
    }

    public abstract class CardCreator
    {
        public abstract Card CreateCard(string suit, string rank, int value);
    }

    public class StandardCardCreator : CardCreator
    {
        public override Card CreateCard(string suit, string rank, int value) => new Card(suit, rank, value);
    }

    public class BlackjackGame
    {
        private readonly int _numberOfCards;
        private List<Card> _cards;
        private Queue<Card> _deck;
        private readonly CardCreator _cardCreator;

        public event Action OnWin;
        public event Action OnLose;
        public event Action OnDraw;

        public BlackjackGame(int numberOfCards, CardCreator cardCreator)
        {
            _numberOfCards = numberOfCards;
            _cardCreator = cardCreator;
            InitializeCards();
            Shuffle();
        }

        private void InitializeCards()
        {
            _cards = new List<Card>();
            string[] suits = { "♥", "♦", "♣", "♠" };
            string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            int[] values = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11 };

            for (int i = 0; i < _numberOfCards / 52 + 1; i++)
            {
                foreach (var suit in suits)
                {
                    foreach (var rank in ranks)
                    {
                        if (_cards.Count >= _numberOfCards) return;
                        _cards.Add(_cardCreator.CreateCard(suit, rank, values[Array.IndexOf(ranks, rank)]));
                    }
                }
            }
        }

        private void Shuffle() => _deck = new Queue<Card>(_cards.OrderBy(x => new Random().Next()));

        public void Play()
        {
            var playerCards = new List<Card> { _deck.Dequeue(), _deck.Dequeue() };
            var dealerCards = new List<Card> { _deck.Dequeue(), _deck.Dequeue() };

            Console.WriteLine("\n=== BLACKJACK ===");
            Console.WriteLine("Твои карты:");
            playerCards.ForEach(Console.WriteLine);
            int playerScore = CalculateScore(playerCards);
            Console.WriteLine($"Очки: {playerScore}");

            Console.WriteLine("\nКарты дилера:");
            Console.WriteLine(dealerCards[0]);
            Console.WriteLine("[скрытая]");
            int dealerScore = CalculateScore(dealerCards);

            if (playerScore == 21 && dealerScore != 21)
            {
                Console.WriteLine("\nБЛЭКДЖЕК! Ты выиграл!");
                OnWin?.Invoke();
                return;
            }

            while (true)
            {
                if (playerScore >= 21 || dealerScore >= 21) break;

                Console.Write("\nЕще? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    var newCard = _deck.Dequeue();
                    playerCards.Add(newCard);
                    playerScore = CalculateScore(playerCards);
                    Console.WriteLine($"Ты получил: {newCard}");
                    Console.WriteLine($"Очки: {playerScore}");
                }
                else break;
            }

            Console.WriteLine("\nДилер открывает карты:");
            dealerCards.ForEach(Console.WriteLine);
            Console.WriteLine($"Очки дилера: {dealerScore}");

            while (dealerScore < 17 && playerScore <= 21)
            {
                var newCard = _deck.Dequeue();
                dealerCards.Add(newCard);
                dealerScore = CalculateScore(dealerCards);
                Console.WriteLine($"Дилер берет: {newCard}");
                Console.WriteLine($"Очки дилера: {dealerScore}");
            }

            DetermineWinner(playerScore, dealerScore);
        }

        private int CalculateScore(List<Card> cards)
        {
            int score = cards.Sum(c => c.Value);
            int aces = cards.Count(c => c.Rank == "A");

            while (score > 21 && aces > 0)
            {
                score -= 10;
                aces--;
            }
            return score;
        }

        private void DetermineWinner(int playerScore, int dealerScore)
        {
            if (playerScore > 21)
            {
                Console.WriteLine("\nПеребор! Ты на нуле, олух!");
                OnLose?.Invoke();
            }
            else if (dealerScore > 21)
            {
                Console.WriteLine("\nДилер сгорел! Ты выиграл!");
                OnWin?.Invoke();
            }
            else if (playerScore > dealerScore)
            {
                Console.WriteLine("\nТы победил!");
                OnWin?.Invoke();
            }
            else if (playerScore < dealerScore)
            {
                Console.WriteLine("\nДилер победил! Ты на нуле, олух!");
                OnLose?.Invoke();
            }
            else
            {
                Console.WriteLine("\nНичья!");
                OnDraw?.Invoke();
            }
        }
    }

    public class Dice
    {
        public int Min { get; }
        public int Max { get; }
        public Dice(int min, int max) => (Min, Max) = (min, max);
        public int Roll() => new Random().Next(Min, Max + 1);
    }

    public abstract class DiceCreator
    {
        public abstract Dice CreateDice(int min, int max);
    }

    public class StandardDiceCreator : DiceCreator
    {
        public override Dice CreateDice(int min, int max) => new Dice(min, max);
    }

    public class DiceGame
    {
        private readonly List<Dice> _dices;
        public event Action OnWin;
        public event Action OnLose;
        public event Action OnDraw;

        public DiceGame(int count, int min, int max, DiceCreator creator)
        {
            _dices = new List<Dice>();
            for (int i = 0; i < count; i++)
                _dices.Add(creator.CreateDice(min, max));
        }

        public void Play()
        {
            Console.WriteLine("\n=== КОСТИ ===");
            int playerScore = RollDices("Твой бросок");
            int computerScore = RollDices("Бросок казино");

            Console.WriteLine($"\nИтог: Ты - {playerScore}, Казино - {computerScore}");

            if (playerScore > computerScore)
            {
                Console.WriteLine("Ты выиграл!");
                OnWin?.Invoke();
            }
            else if (playerScore < computerScore)
            {
                Console.WriteLine("Ты на нуле, олух!");
                OnLose?.Invoke();
            }
            else
            {
                Console.WriteLine("Ничья!");
                OnDraw?.Invoke();
            }
        }

        private int RollDices(string player)
        {
            Console.WriteLine($"\n{player}:");
            int total = 0;
            foreach (var dice in _dices)
            {
                int roll = dice.Roll();
                Console.WriteLine($"Выпало: {roll} ({dice.Min}-{dice.Max})");
                total += roll;
            }
            Console.WriteLine($"Всего: {total}");
            return total;
        }
    }

    class Program
    {
        static PlayerProfile player;
        static string saveFile = "player.json";

        static void Main()
        {
            Console.WriteLine("╔════════════════════════════╗");
            Console.WriteLine("║   Кто прочитал тот ло...   ║");
            Console.WriteLine("╚════════════════════════════╝");
            LoadProfile();

            while (true)
            {
                Console.WriteLine($"\nБаланс: {player.Balance}$");
                Console.WriteLine("1. Блэкджек");
                Console.WriteLine("2. Кости");
                Console.WriteLine("3. Сохранить");
                Console.WriteLine("4. Выход");
                Console.Write("Выбор: ");

                switch (Console.ReadLine())
                {
                    case "1": PlayBlackjack(); break;
                    case "2": PlayDice(); break;
                    case "3": SaveProfile(); break;
                    case "4": return;
                    default: Console.WriteLine("Неверный ввод!"); break;
                }
            }
        }

        static void PlayBlackjack()
        {
            if (!PlaceBet()) return;

            var game = new BlackjackGame(52, new StandardCardCreator());

            game.OnWin += () => {
                player.Balance += player.Balance;
                player.Wins++;
                player.GamesPlayed++;
            };

            game.OnLose += () => player.GamesPlayed++;
            game.OnDraw += () => player.Balance += player.Balance / 2;

            game.Play();
        }

        static void PlayDice()
        {
            if (!PlaceBet()) return;

            var game = new DiceGame(2, 1, 6, new StandardDiceCreator());

            game.OnWin += () => {
                player.Balance += (int)(player.Balance * 1.5);
                player.Wins++;
                player.GamesPlayed++;
            };

            game.OnLose += () => player.GamesPlayed++;
            game.OnDraw += () => player.Balance += player.Balance / 3;

            game.Play();
        }

        static bool PlaceBet()
        {
            Console.Write($"\nСтавка: ");
            if (!int.TryParse(Console.ReadLine(), out int bet) || bet <= 0)
            {
                Console.WriteLine("Неверная ставка!");
                return false;
            }

            if (bet > player.Balance)
            {
                Console.WriteLine("Ты на нуле, олух!");
                return false;
            }

            player.Balance -= bet;
            return true;
        }

        static void LoadProfile()
        {
            if (File.Exists(saveFile))
            {
                player = JsonSerializer.Deserialize<PlayerProfile>(File.ReadAllText(saveFile));
                Console.WriteLine($"Загружен профиль: {player.Name}");
            }
            else
            {
                player = new PlayerProfile();
                Console.Write("Твое имя: ");
                player.Name = Console.ReadLine();
            }
        }

        static void SaveProfile()
        {
            File.WriteAllText(saveFile, JsonSerializer.Serialize(player));
            Console.WriteLine("Сохранено.");
        }
    }
}