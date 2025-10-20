using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("████████████████████████████████████████████████");
        Console.WriteLine("████          ТЕСТИРОВАНИЕ КЛАССА AIRLINE       ████");
        Console.WriteLine("████████████████████████████████████████████████");
        
        // ██████████████████████████████████████████████████████████████████████████████
        // ВОПРОС: Создание нескольких объектов + вызов конструкторов, свойств, методов
        // ██████████████████████████████████████████████████████████████████████████████
        
        Console.WriteLine("\n1. СОЗДАНИЕ ОБЪЕКТОВ:");
        Console.WriteLine("─────────────────────────────────────────");

        // Конструктор по умолчанию
        var flight1 = new Airline();
        
        // Конструктор с параметрами
        var flight2 = new Airline("London", "BA249", "Boeing 777", 
            new TimeSpan(14, 30, 0), new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday });
        
        // Конструктор с параметрами по умолчанию
        var flight3 = new Airline("Paris", "AF308");
        
        // Вызов закрытого конструктора через статический метод - требование задания
        var privateFlight = Airline.CreatePrivateFlight("Moscow", new TimeSpan(20, 15, 0));
        
        // ██████████████████████████████████████████████████████████████████████████████
        // ВОПРОС: Работа со свойствами
        // ██████████████████████████████████████████████████████████████████████████████
        
        Console.WriteLine("\n2. РАБОТА СО СВОЙСТВАМИ:");
        Console.WriteLine("─────────────────────────────────────────");
        
        flight1.Destination = "Berlin";  // Обычное свойство
        // flight1.FlightNumber = "NEW001"; // ❌ Ошибка: set ограничен!
        Console.WriteLine($"Рейс 1: {flight1.Destination}");
        Console.WriteLine($"Рейс 2: {flight2.AircraftType}");

        // ██████████████████████████████████████████████████████████████████████████████
        // ВОПРОС: Метод с ref и out параметрами
        // ██████████████████████████████████████████████████████████████████████████████
        
        Console.WriteLine("\n3. МЕТОД С REF И OUT ПАРАМЕТРАМИ:");
        Console.WriteLine("─────────────────────────────────────────");
        
        string flightNum = "";  // пустая строка
        string errorMsg;
        
        bool success = flight1.TryUpdateFlightInfo(ref flightNum, out errorMsg);
        Console.WriteLine($"Результат: {success}, Сообщение: {errorMsg}");
        Console.WriteLine($"Измененный ref параметр: '{flightNum}'");

        // ██████████████████████████████████████████████████████████████████████████████
        // ВОПРОС: Сравнение объектов + проверка типа
        // ██████████████████████████████████████████████████████████████████████████████
        
        Console.WriteLine("\n4. СРАВНЕНИЕ ОБЪЕКТОВ И ТИПЫ:");
        Console.WriteLine("─────────────────────────────────────────");
        
        var flight4 = new Airline("London", "BA249", "Boeing 777", 
            new TimeSpan(14, 30, 0), new DayOfWeek[] { DayOfWeek.Monday });
        
        Console.WriteLine($"flight2.Equals(flight4): {flight2.Equals(flight4)}");
        Console.WriteLine($"flight2 == flight4: {flight2 == flight4}");
        Console.WriteLine($"Тип flight1: {flight1.GetType()}");
        Console.WriteLine($"Хэш flight2: {flight2.GetHashCode()}");
        Console.WriteLine($"Хэш flight4: {flight4.GetHashCode()}");

        // ██████████████████████████████████████████████████████████████████████████████
        // ВОПРОС: Статический метод и поле
        // ██████████████████████████████████████████████████████████████████████████████
        
        Airline.PrintClassInfo();

        // ██████████████████████████████████████████████████████████████████████████████
        // ВОПРОС: Массив объектов + задания варианта
        // ██████████████████████████████████████████████████████████████████████████████
        
        Console.WriteLine("\n5. МАССИВ ОБЪЕКТОВ И ЗАДАНИЯ ВАРИАНТА:");
        Console.WriteLine("─────────────────────────────────────────");

        // Создаем еще несколько рейсов для демонстрации
        var flight5 = new Airline("London", "LH909", "Airbus A380", 
            new TimeSpan(9, 15, 0), new DayOfWeek[] { DayOfWeek.Tuesday, DayOfWeek.Thursday });
        
        var flight6 = new Airline("Madrid", "IB456", "Boeing 737", 
            new TimeSpan(16, 45, 0), new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday });
        
        var flight7 = new Airline("London", "BA123", "Airbus A320", 
            new TimeSpan(11, 30, 0), new DayOfWeek[] { DayOfWeek.Wednesday });

        // a) Список рейсов для заданного пункта назначения (London)
        Console.WriteLine("\na) РЕЙСЫ В LONDON:");
        var londonFlights = Airline.GetFlightsByDestination("London");
        foreach (var flight in londonFlights)
        {
            Console.WriteLine($"   📍 {flight}");
        }

        // b) Список рейсов для заданного дня недели (Monday)
        Console.WriteLine("\nb) РЕЙСЫ В ПОНЕДЕЛЬНИК:");
        var mondayFlights = Airline.GetFlightsByDay(DayOfWeek.Monday);
        foreach (var flight in mondayFlights)
        {
            Console.WriteLine($"   📅 {flight}");
        }

        // ██████████████████████████████████████████████████████████████████████████████
        // ВОПРОС: Анонимный тип по образцу класса
        // ██████████████████████████████████████████████████████████████████████████████
        
        Console.WriteLine("\n6. АНОНИМНЫЙ ТИП:");
        Console.WriteLine("─────────────────────────────────────────");
        
        // Создание анонимного типа с похожими свойствами
        var anonymousFlight = new
        {
            Destination = "Tokyo",
            FlightNumber = "JL045",
            AircraftType = "Boeing 787",
            DepartureTime = new TimeSpan(22, 10, 0),
            Days = new[] { DayOfWeek.Tuesday, DayOfWeek.Saturday }
        };
        
        Console.WriteLine($"Анонимный тип: {anonymousFlight}");
        Console.WriteLine($"Тип: {anonymousFlight.GetType()}");
        Console.WriteLine($"Рейс: {anonymousFlight.FlightNumber} → {anonymousFlight.Destination}");
        Console.WriteLine($"Самолет: {anonymousFlight.AircraftType}");

        // ██████████████████████████████████████████████████████████████████████████████
        // ВОПРОС: Вывод всех созданных рейсов
        // ██████████████████████████████████████████████████████████████████████████████
        
        Console.WriteLine("\n7. ВСЕ СОЗДАННЫЕ РЕЙСЫ:");
        Console.WriteLine("─────────────────────────────────────────");
        
        var allFlights = Airline.GetAllFlights();
        foreach (var flight in allFlights)
        {
            Console.WriteLine($"   ✈️  {flight}");
        }

        Console.WriteLine("\n████████████████████████████████████████████████");
        Console.WriteLine("████           ТЕСТИРОВАНИЕ ЗАВЕРШЕНО        ████");
        Console.WriteLine("████████████████████████████████████████████████");
    }
}
