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
            const string CommandShowUsers = "1";
            const string commandAddUser = "2";
            const string commandDeleteUser = "3";
            const string commandBanUser = "4";
            const string commandUnbanUser = "5";
            const string commandExit = "6";
            bool isFinish = false;
            Database database = new Database();

            while(isFinish == false)
            {
                Console.WriteLine("Игроки сервера\n");
                Console.Write($"Выберите команду:\n{CommandShowUsers}. Вывести список пользователей\n{commandAddUser}. Добавить пользователя\n{commandDeleteUser}. Удалить пользователя\n{commandBanUser}. Забанить пользователя\n{commandUnbanUser}. Разбанить пользователя\n{commandExit}. Выйти\nВведите номер команды: ");

                switch (Console.ReadLine())
                {
                    case CommandShowUsers:
                        database.ShowUsers();
                        break;
                    case commandAddUser:
                        AddUser(database);
                        break;
                    case commandDeleteUser:
                        DeleteUser(database);
                        break;
                    case commandBanUser:
                        BanUser(database);
                        break;
                    case commandUnbanUser:
                        UnbanUser(database);
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

        static void AddUser(Database database)
        {
            const string CommandCancel = "exit";
            string userName;
            bool isExit = false;

            Console.Clear();
            Console.Write($"Введите имя пользователя (или {CommandCancel} для отмены): ");
            userName = Console.ReadLine();

            if (userName == CommandCancel)
                return;

            int userLevel = CheckNumber("Введите уровень пользователя", ref isExit);

            if (isExit == false)
                database.CreateUser(userName, userLevel);
        }

        static void DeleteUser(Database database)
        {
            bool isExit = false;

            Console.Clear();
            int userId = CheckNumber("Введите ID пользователя, которого хотите удалить", ref isExit);

            if (isExit == false)
                database.DeleteUser(userId);
        }

        static void BanUser (Database database)
        {
            bool isExit = false;

            Console.Clear();
            int userId = CheckNumber("Введите ID пользователя, которого хотите забанить", ref isExit);

            if(isExit == false)
                database.BanUser(userId);
        }

        static void UnbanUser(Database database)
        {
            bool isExit = false;

            Console.Clear();
            int userId = CheckNumber("Введите ID пользователя, которого хотите забанить", ref isExit);

            if (isExit == false)
                database.UnbanUser(userId);
        }

        static int CheckNumber(string textContinuation, ref bool isExit)
        {
            const string CommandCancel = "exit";
            const string textError = "Некоректная команда";
            int result = 0;
            bool isNumber = false;

            while (isNumber == false)
            {
                Console.Write($"{textContinuation} (или {CommandCancel} для отмены): ");
                string input = Console.ReadLine();

                if (input == CommandCancel)
                {
                    isExit = true;
                    break;
                }
                else if (int.TryParse(input, out result))
                    isNumber = true;
                else
                    Console.WriteLine(textError);
            }

            return result;
        }
    }

    class Database
    {
        private List<User> users = new List<User>();

        public void ShowUsers ()
        {
            foreach(var user in users)
            {
                Console.Write($"{users.IndexOf(user) + 1}. {user.Name}: уровень {user.Level}, ID {user.Id}");

                if (user.IsBanned)
                    Console.Write(", забанен");

                Console.WriteLine();
            }
        }

        private User FindUser(int desiredId)
        {
            User desiredUser = null;

            foreach(var user in users)
            {
                if(user.Id == desiredId)
                    desiredUser = user;
            }

            return desiredUser;
        }

        public void CreateUser(string name, int level)
        {
            users.Add(new User(name, level));
        }

        public void DeleteUser(int deleteId)
        {
            users.Remove(FindUser(deleteId));
        }

        public void BanUser(int banId)
        {
            User user = FindUser(banId);
            
            if(user == null)
                Console.WriteLine("Пользователь не найден");
            else if (user.IsBanned == false)
                user.ChangeBanStatus();
            else 
                Console.WriteLine("Пользователь уже забанен");
        }

        public void UnbanUser(int unbanId)
        {
            User user = FindUser(unbanId);

            if (user == null)
                Console.WriteLine("Пользователь не найден");
            else if (user.IsBanned)
                user.ChangeBanStatus();
            else
                Console.WriteLine("Пользователь не забанен");
        }
    }

    class User
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Level { get; private set; }
        public bool IsBanned { get; private set; }

        static int id;

        public User(string name, int level)
        {
            Id = ++id;
            Name = name;
            Level = level;
            IsBanned = false;
        }

        public void ChangeBanStatus()
        {
            if (IsBanned)
                IsBanned = false;
            else
                IsBanned = true;
        }
    }
}
