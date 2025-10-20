using System;
using System.Collections.Generic;
using System.Linq;

// Частичный класс - требование задания
public partial class Airline
{
    // ██████████████████████████████████████████████████████████████████████████████
    // ВОПРОС: Поля класса (должны быть закрытыми) + поле только для чтения + константа
    // ██████████████████████████████████████████████████████████████████████████████
    
    private string _destination;          // Пункт назначения
    private string _flightNumber;         // Номер рейса
    private string _aircraftType;         // Тип самолета
    private TimeSpan _departureTime;      // Время вылета
    private DayOfWeek[] _daysOfWeek;      // Дни недели
    
    // Поле только для чтения (уникальный ID на основе хэша) - требование задания
    public readonly int ID;
    
    // Поле-константа - требование задания
    private const string AIRLINE_COMPANY = "AeroFlot";
    
    // Статическое поле для подсчета созданных объектов - требование задания
    private static int _objectsCount = 0;
    
    // Статическое поле для хранения всех созданных авиалиний
    private static List<Airline> _allAirlines = new List<Airline>();

    // ██████████████████████████████████████████████████████████████████████████████
    // ВОПРОС: Статический конструктор (конструктор типа)
    // ██████████████████████████████████████████████████████████████████████████████
    
    static Airline()
    {
        Console.WriteLine("▓ Статический конструктор Airline вызван - класс инициализирован");
        Console.WriteLine($"▓ Авиакомпания: {AIRLINE_COMPANY}");
    }

    // ██████████████████████████████████████████████████████████████████████████████
    // ВОПРОС: Не менее трех конструкторов + закрытый конструктор
    // ██████████████████████████████████████████████████████████████████████████████
    
    // Конструктор по умолчанию - требование задания
    public Airline()
    {
        _destination = "Unknown";
        _flightNumber = "AF000";
        _aircraftType = "Boeing 737";
        _departureTime = new TimeSpan(12, 0, 0);
        _daysOfWeek = new DayOfWeek[] { DayOfWeek.Monday };
        
        // Вычисление уникального ID на основе хэша - требование задания
        ID = CalculateHashCode();
        _objectsCount++;
        _allAirlines.Add(this);
        
        Console.WriteLine($"✓ Создан рейс по умолчанию: {this}");
    }

    // Конструктор с параметрами - требование задания
    public Airline(string destination, string flightNumber, string aircraftType, 
                   TimeSpan departureTime, DayOfWeek[] daysOfWeek)
    {
        // Проверка корректности - требование варианта
        if (string.IsNullOrWhiteSpace(destination))
            throw new ArgumentException("Пункт назначения не может быть пустым");
        if (string.IsNullOrWhiteSpace(flightNumber))
            throw new ArgumentException("Номер рейса не может быть пустым");
        if (daysOfWeek == null || daysOfWeek.Length == 0)
            throw new ArgumentException("Должен быть указан хотя бы один день недели");

        _destination = destination;
        _flightNumber = flightNumber;
        _aircraftType = aircraftType;
        _departureTime = departureTime;
        _daysOfWeek = daysOfWeek;
        
        ID = CalculateHashCode();
        _objectsCount++;
        _allAirlines.Add(this);
        
        Console.WriteLine($"✓ Создан рейс с параметрами: {this}");
    }

    // Конструктор с параметрами по умолчанию - требование задания
    public Airline(string destination, string flightNumber) 
        : this(destination, flightNumber, "Airbus A320", 
              new TimeSpan(10, 30, 0), 
              new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday })
    {
        Console.WriteLine($"✓ Создан рейс с параметрами по умолчанию: {this}");
    }

    // Закрытый конструктор - требование задания
    private Airline(string destination, TimeSpan departureTime)
    {
        _destination = destination;
        _flightNumber = "PRIVATE001";
        _aircraftType = "Private Jet";
        _departureTime = departureTime;
        _daysOfWeek = new DayOfWeek[] { DayOfWeek.Sunday };
        
        ID = CalculateHashCode();
        _objectsCount++;
        _allAirlines.Add(this);
        
        Console.WriteLine($"🔒 Создан рейс через закрытый конструктор: {this}");
    }

    // ██████████████████████████████████████████████████████████████████████████████
    // ВОПРОС: Свойства (get, set) с проверкой корректности + ограничение set
    // ██████████████████████████████████████████████████████████████████████████████
    
    // Свойство с полным доступом
    public string Destination
    {
        get => _destination;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Пункт назначения не может быть пустым");
            _destination = value;
        }
    }

    // Свойство с ограничением на set (только для чтения извне) - требование задания
    public string FlightNumber
    {
        get => _flightNumber;
        private set   // Ограниченный доступ по set - требование задания
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Номер рейса не может быть пустым");
            _flightNumber = value;
        }
    }

    public string AircraftType
    {
        get => _aircraftType;
        set => _aircraftType = value ?? "Unknown Aircraft";
    }

    public TimeSpan DepartureTime
    {
        get => _departureTime;
        set => _departureTime = value;
    }

    public DayOfWeek[] DaysOfWeek
    {
        get => _daysOfWeek;
        set
        {
            if (value == null || value.Length == 0)
                throw new ArgumentException("Должен быть указан хотя бы один день недели");
            _daysOfWeek = value;
        }
    }

    // ██████████████████████████████████████████████████████████████████████████████
    // ВОПРОС: Статический метод вывода информации о классе
    // ██████████████████████████████████████████████████████████████████████████████
    
    public static void PrintClassInfo()
    {
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║         ИНФОРМАЦИЯ О КЛАССЕ         ║");
        Console.WriteLine("╠══════════════════════════════════════╣");
        Console.WriteLine($"║ Авиакомпания: {AIRLINE_COMPANY,-20} ║");
        Console.WriteLine($"║ Создано объектов: {_objectsCount,-16} ║");
        Console.WriteLine($"║ Всего рейсов в системе: {_allAirlines.Count,-9} ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
    }

    // ██████████████████████████████████████████████████████████████████████████████
    // ВОПРОС: Метод с ref и out параметрами
    // ██████████████████████████████████████████████████████████████████████████████
    
    public bool TryUpdateFlightInfo(ref string newFlightNumber, out string errorMessage)
    {
        errorMessage = null;
        
        // ref параметр - можем изменить оригинальную переменную
        if (string.IsNullOrWhiteSpace(newFlightNumber))
        {
            newFlightNumber = "DEFAULT001"; // изменяем ref параметр
            errorMessage = "Номер рейса был пустым, установлено значение по умолчанию";
            return false;
        }

        // out параметр - должен быть обязательно установлен
        if (newFlightNumber.Length < 3)
        {
            errorMessage = "Номер рейса слишком короткий";
            return false;
        }

        this.FlightNumber = newFlightNumber;
        errorMessage = "Рейс успешно обновлен";
        return true;
    }

    // ██████████████████████████████████████████████████████████████████████████████
    // ВОПРОС: Переопределение методов класса Object
    // ██████████████████████████████████████████████████████████████████████████████
    
    // Переопределение Equals для сравнения объектов - требование задания
    public override bool Equals(object obj)
    {
        if (obj is Airline other)
        {
            return _flightNumber == other._flightNumber && 
                   _destination == other._destination &&
                   _departureTime == other._departureTime;
        }
        return false;
    }

    // Переопределение GetHashCode - требование задания
    public override int GetHashCode()
    {
        return CalculateHashCode();
    }

    // Вспомогательный метод для вычисления хэша
    private int CalculateHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + (_destination?.GetHashCode() ?? 0);
            hash = hash * 23 + (_flightNumber?.GetHashCode() ?? 0);
            hash = hash * 23 + _departureTime.GetHashCode();
            return hash;
        }
    }

    // Переопределение ToString - вывод информации об объекте - требование задания
    public override string ToString()
    {
        string days = string.Join(", ", _daysOfWeek.Select(d => d.ToString()));
        return $"Рейс {_flightNumber} → {_destination} | {_aircraftType} | {_departureTime:hh\\:mm} | Дни: {days} | ID: {ID}";
    }

    // ██████████████████████████████████████████████████████████████████████████████
    // ВОПРОС: Варианты вызова закрытого конструктора
    // ██████████████████████████████████████████████████████████████████████████████
    
    // Статический метод для создания объекта через закрытый конструктор
    public static Airline CreatePrivateFlight(string destination, TimeSpan time)
    {
        return new Airline(destination, time);
    }

    // Фабричный метод
    public static Airline CreateCharterFlight(string destination)
    {
        return new Airline(destination, new TimeSpan(18, 0, 0));
    }
}

// ██████████████████████████████████████████████████████████████████████████████
// ВОПРОС: Partial класс (вторая часть)
// ██████████████████████████████████████████████████████████████████████████████

public partial class Airline
{
    // Методы для работы с массивом объектов - требование варианта
    
    // a) Список рейсов для заданного пункта назначения
    public static List<Airline> GetFlightsByDestination(string destination)
    {
        return _allAirlines
            .Where(airline => airline.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    // b) Список рейсов для заданного дня недели
    public static List<Airline> GetFlightsByDay(DayOfWeek day)
    {
        return _allAirlines
            .Where(airline => airline.DaysOfWeek.Contains(day))
            .ToList();
    }

    // Метод для получения всех рейсов
    public static List<Airline> GetAllFlights()
    {
        return new List<Airline>(_allAirlines);
    }
}
