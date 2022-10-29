using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace player_database
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Database database = new Database();

            database.Work();
        }
    }

    class Database
    {
        private List<User> _users;
        private int _lastId;

        public Database()
        {
            _users = new List<User>();
            _lastId = 0;
        }

        public void Work()
        {
            const string CommandShowUsers = "1";
            const string commandAddUser = "2";
            const string commandDeleteUser = "3";
            const string commandBanUser = "4";
            const string commandUnbanUser = "5";
            const string commandExit = "6";
            bool isFinish = false;

            while (isFinish == false)
            {
                Console.WriteLine("Игроки сервера\n");
                Console.Write($"Выберите команду:\n{CommandShowUsers}. Вывести список пользователей\n{commandAddUser}. Добавить пользователя\n{commandDeleteUser}. Удалить пользователя\n{commandBanUser}. Забанить пользователя\n{commandUnbanUser}. Разбанить пользователя\n{commandExit}. Выйти\nВведите номер команды: ");

                switch (Console.ReadLine())
                {
                    case CommandShowUsers:
                        ShowUsers();
                        break;
                    case commandAddUser:
                        AddUser();
                        break;
                    case commandDeleteUser:
                        DeleteUser();
                        break;
                    case commandBanUser:
                        BanUser();
                        break;
                    case commandUnbanUser:
                        UnbanUser();
                        break;
                    case commandExit:
                        isFinish = true;
                        Console.WriteLine("Программа завершена");
                        break;
                    default:
                        Console.WriteLine("Команда не найдена");
                        break;
                }

                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ShowUsers()
        {
            Console.Clear();

            for(int i = 0; i < _users.Count; i++)
            {
                _users[i].ShowInfo(i);
            }
        }

        private void AddUser()
        {
            Console.Clear();
            Console.Write($"Введите имя пользователя: ");

            string userName = Console.ReadLine();
            int userLevel = ReadInt("Введите уровень пользователя");

            _lastId++;

            _users.Add(new User(userName, userLevel, _lastId));
        }

        private void DeleteUser()
        {
            Console.Clear();

            _users.Remove(TryGetUser("Введите ID пользователя, которого хотите удалить"));
        }

        private void BanUser()
        {
            Console.Clear();

            User user = TryGetUser("Введите ID пользователя, которого хотите забанить");

            if (user == null)
                Console.WriteLine("Пользователь не найден");
            else if (user.IsBanned == false)
                user.BanUser();
            else
                Console.WriteLine("Пользователь уже забанен");
        }

        private void UnbanUser()
        {
            Console.Clear();

            User user = TryGetUser("Введите ID пользователя, которого хотите разбанить");

            if (user == null)
                Console.WriteLine("Пользователь не найден");
            else if (user.IsBanned == true)
                user.UnbanUser();
            else
                Console.WriteLine("Пользователь не забанен");
        }

        private int ReadInt(string textContinuation)
        {
            int result = 0;
            bool isNumber = false;

            while (isNumber == false)
            {
                Console.Write($"{textContinuation}: ");

                string input = Console.ReadLine();

                if (int.TryParse(input, out result))
                    isNumber = true;
                else
                    Console.WriteLine("Некоректная команда");
            }

            return result;
        }

        private User TryGetUser(string textContinuation)
        {
            int desiredId = ReadInt(textContinuation);
            User desiredUser = null;

            foreach (var user in _users)
            {
                if (user.Id == desiredId)
                    desiredUser = user;
            }

            return desiredUser;
        }
    }

    class User
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Level { get; private set; }
        public bool IsBanned { get; private set; }

        public User(string name, int level, int id)
        {
            Id = id;
            Name = name;
            Level = level;
            IsBanned = false;
        }

        public void BanUser()
        {
            IsBanned = true;
        }

        public void UnbanUser()
        {
            IsBanned = false;
        }

        public void ShowInfo(int index)
        {
            Console.Write($"{index + 1}. {Name}: уровень {Level}, ID {Id}");

            if (IsBanned)
                Console.Write(", забанен");

            Console.WriteLine();
        }
    }
}
