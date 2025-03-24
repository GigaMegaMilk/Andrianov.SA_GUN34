using System;
using System.Collections.Generic;

namespace HomeWork
{
    internal class Program
    {
        private class ListTask
        {
            private readonly List<string> _listOfStrings;

            public ListTask()
            {
                _listOfStrings = new List<string> { "Яблоко", "Банан", "Вишня" };
            }

            public void TaskLoop()
            {
                while (true)
                {
                    Console.WriteLine("Текущий список:");
                    foreach (var item in _listOfStrings)
                    {
                        Console.WriteLine(item);
                    }

                    Console.WriteLine("Введите новую строку для добавления в список (или '–exit' для выхода):");
                    string input = Console.ReadLine();

                    if (input == "–exit")
                        break;

                    _listOfStrings.Add(input);

                    Console.WriteLine("Введите ещё одну строку для добавления в середину списка:");
                    input = Console.ReadLine();

                    if (input == "–exit")
                        break;

                    _listOfStrings.Insert(_listOfStrings.Count / 2, input);
                }
            }
        }

        private class DictionaryTask
        {
            private readonly Dictionary<string, int> _studentGrades;

            public DictionaryTask()
            {
                _studentGrades = new Dictionary<string, int>();
            }

            public void TaskLoop()
            {
                while (true)
                {
                    Console.WriteLine("Введите имя студента (или '–exit' для выхода):");
                    string name = Console.ReadLine();

                    if (name == "–exit")
                        break;

                    Console.WriteLine("Введите оценку студента (от 2 до 5):");
                    if (int.TryParse(Console.ReadLine(), out int grade) && grade >= 2 && grade <= 5)
                    {
                        _studentGrades[name] = grade;
                    }
                    else
                    {
                        Console.WriteLine("Некорректная оценка. Оценка должна быть от 2 до 5.");
                        continue;
                    }

                    Console.WriteLine("Введите имя студента для поиска его оценки:");
                    name = Console.ReadLine();

                    if (_studentGrades.TryGetValue(name, out int foundGrade))
                    {
                        Console.WriteLine($"Оценка студента {name}: {foundGrade}");
                    }
                    else
                    {
                        Console.WriteLine($"Студент с именем {name} не найден.");
                    }
                }
            }
        }

        private class LinkedListTask
        {
            private class Node
            {
                public string Data;
                public Node Next;
                public Node Previous;

                public Node(string data)
                {
                    Data = data;
                    Next = null;
                    Previous = null;
                }
            }

            private Node _head;
            private Node _tail;

            public LinkedListTask()
            {
                _head = null;
                _tail = null;
            }

            public void Add(string data)
            {
                Node newNode = new Node(data);
                if (_head == null)
                {
                    _head = newNode;
                    _tail = newNode;
                }
                else
                {
                    _tail.Next = newNode;
                    newNode.Previous = _tail;
                    _tail = newNode;
                }
            }

            public void DisplayForward()
            {
                Node current = _head;
                while (current != null)
                {
                    Console.WriteLine(current.Data);
                    current = current.Next;
                }
            }

            public void DisplayBackward()
            {
                Node current = _tail;
                while (current != null)
                {
                    Console.WriteLine(current.Data);
                    current = current.Previous;
                }
            }

            public void TaskLoop()
            {
                while (true)
                {
                    Console.WriteLine("Введите от 3 до 6 элементов для создания списка (или '–exit' для выхода):");
                    for (int i = 0; i < 6; i++)
                    {
                        string input = Console.ReadLine();
                        if (input == "–exit")
                            return;

                        Add(input);

                        if (i >= 2)
                        {
                            Console.WriteLine("Продолжить ввод? (y/n)");
                            if (Console.ReadLine().ToLower() != "y")
                                break;
                        }
                    }

                    Console.WriteLine("Список в прямом порядке:");
                    DisplayForward();

                    Console.WriteLine("Список в обратном порядке:");
                    DisplayBackward();

                    break;
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Введите 1, 2 или 3 для выбора задачи 1, 2 или 3:");
            if (int.TryParse(Console.ReadLine(), out int task))
            {
                switch (task)
                {
                    case 1:
                        CheckTaskFirst();
                        break;
                    case 2:
                        CheckTaskSecond();
                        break;
                    case 3:
                        CheckTaskThird();
                        break;
                    default:
                        Console.WriteLine("Неверный номер задачи.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Некорректный ввод.");
            }
        }

        private static void CheckTaskFirst()
        {
            var listTask = new ListTask();
            listTask.TaskLoop();
        }

        private static void CheckTaskSecond()
        {
            var dictionaryTask = new DictionaryTask();
            dictionaryTask.TaskLoop();
        }

        private static void CheckTaskThird()
        {
            var linkedListTask = new LinkedListTask();
            linkedListTask.TaskLoop();
        }
    }
}