using System;
using System.Collections.Generic;
using System.Linq;

public static class Dop3
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException(List<string> errors)
            : base("Пользователь не прошёл валидацию.")
        {
            Errors = errors;
        }
    }

    public class User
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class UserValidator
    {
        public List<string> Validate(User user)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(user.Name))
                errors.Add("Имя не может быть пустым.");
            else if (user.Name.Length < 2)
                errors.Add("Имя должно содержать минимум 2 символа.");

            if (!user.Email.Contains('@') || !user.Email.Contains('.'))
                errors.Add("Email должен содержать символы '@' и '.'.");

            if (user.Password.Length < 6)
                errors.Add("Пароль должен содержать минимум 6 символов.");

            if (!user.Phone.All(char.IsDigit) || user.Phone.Length < 10)
                errors.Add("Телефон должен состоять минимум из 10 цифр.");

            return errors;
        }

        public void ValidateAndThrow(User user)
        {
            List<string> errors = Validate(user);
            if (errors.Count > 0) throw new ValidationException(errors);
            Console.WriteLine($"Пользователь {user.Name} успешно прошёл валидацию.");
        }
    }

    public static void Run()
    {
        UserValidator validator = new UserValidator();

        User[] users =
        {
            new User { Name = "Иван Петров", Email = "ivan@mail.ru", Password = "secret123", Phone = "89991234567" },
            new User { Name = "А", Email = "bad-email", Password = "123", Phone = "abc" }
        };

        foreach (User user in users)
        {
            try
            {
                validator.ValidateAndThrow(user);
            }
            catch (ValidationException ex)
            {
                Console.WriteLine(ex.Message);
                foreach (string error in ex.Errors)
                {
                    Console.WriteLine($"  - {error}");
                }
            }
        }

        Console.WriteLine();
        List<string> allErrors = validator.Validate(users[1]);
        Console.WriteLine($"Всего ошибок во втором пользователе: {allErrors.Count}");
    }
}
