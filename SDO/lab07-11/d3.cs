using System;

public class Task5_Chain
{
    // Делегат-обработчик: принимает строку-запрос, возвращает bool (прошел проверку или нет)
    public delegate bool RequestHandler(string request);

    // Проверка 1: Логирование
    public static bool LogStep(string req)
    {
        Console.WriteLine($"  [Шаг 1. Логгер]: Фиксируем входящий запрос: '{req}'");
        return true; // Логгер всегда пропускает дальше
    }

    // Проверка 2: Валидация токена
    public static bool AuthStep(string req)
    {
        Console.WriteLine("  [Шаг 2. Админка]: Проверяем права доступа...");
        if (req.Contains("ADMIN_TOKEN"))
        {
            Console.WriteLine("    -> Успешно! Токен администратора валиден.");
            return true;
        }
        Console.WriteLine("    -> ОТКАЗ! Нет прав доступа (Требуется ADMIN_TOKEN).");
        return false; // Блокируем запрос
    }

    // Проверка 3: Безопасность (проверка на спам/вирусы)
    public static bool SecurityStep(string req)
    {
        Console.WriteLine("  [Шаг 3. Защита]: Проверка на опасный контент...");
        if (req.Contains("DROP DATABASE"))
        {
            Console.WriteLine("    -> АЛАРМ! Обнаружена SQL-инъекция!");
            return false; // Блокируем
        }
        Console.WriteLine("    -> Чисто. Запрос безопасен.");
        return true;
    }

    public static void Run()
    {
        Console.WriteLine("=== ДОП 3. Цепочка обработчиков (Chain of Responsibility) ===\n");

        // Собираем массив делегатов (нашу цепочку проверок)
        RequestHandler[] chain = { LogStep, AuthStep, SecurityStep };

        // Функция, которая гонит запрос по всей цепочке
        Action<string> processRequest = (req) =>
        {
            Console.WriteLine($"\n--- Обработка запроса ---");
            bool isSuccess = true;

            foreach (var handler in chain)
            {
                // Если какой-то шаг вернул false, прерываем цепочку
                if (!handler(req))
                {
                    isSuccess = false;
                    break; 
                }
            }

            if (isSuccess) Console.WriteLine("ИТОГ: Запрос успешно выполнен!");
            else Console.WriteLine("ИТОГ: Запрос заблокирован цепочкой безопасности.");
        };

        // Тест 1: Хороший запрос от админа
        processRequest("ADMIN_TOKEN: Показать список пользователей");

        // Тест 2: Запрос от хакера (нет токена)
        processRequest("USER_TOKEN: Слить все данные");

        // Тест 3: Опасный запрос от админа
        processRequest("ADMIN_TOKEN: DROP DATABASE users;");
    }
}
