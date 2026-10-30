using System;

// 1. Создаем класс для передачи данных события (наследуемся от EventArgs)
class TemperatureChangedEventArgs : EventArgs
{
    // Свойство для хранения нового значения температуры
    public double NewTemperature { get; set; }

    public TemperatureChangedEventArgs(double temp)
    {
        NewTemperature = temp;
    }
}

// 2. Объявляем делегат для события по стандартной сигнатуре C#
delegate void TemperatureChangedHandler(object sender, TemperatureChangedEventArgs e);

// 3. Класс-издатель (генерирует событие при изменении температуры)
class TemperatureSensor
{
    private double currentTemperature;

    // Событие изменения температуры
    public event TemperatureChangedHandler? TemperatureChanged;

    public double CurrentTemperature
    {
        get { return currentTemperature; }
        set
        {
            // Если температура действительно изменилась, обновляем её и вызываем событие
            if (currentTemperature != value)
            {
                currentTemperature = value;
                OnTemperatureChanged(value);
            }
        }
    }

    // Метод для безопасного вызова события (OnИмяСобытия)
    protected virtual void OnTemperatureChanged(double temp)
    {
        // Проверяем, есть ли подписчики, с помощью оператора ?.
        TemperatureChanged?.Invoke(this, new TemperatureChangedEventArgs(temp));
    }
}

// 4. Класс-подписчик: Обогреватель (Heater)
class Heater
{
    private double targetTemperature = 20.0; // Целевая температура

    // Обработчик события
    public void Target_TemperatureChanged(object sender, TemperatureChangedEventArgs e)
    {
        Console.WriteLine($"  [Обогреватель]: Датчик зафиксировал {e.NewTemperature}°C.");
        if (e.NewTemperature < targetTemperature)
        {
            Console.WriteLine("    -> Слишком холодно! Обогреватель ВКЛЮЧЕН.");
        }
        else
        {
            Console.WriteLine("    -> Тепло достаточно. Обогреватель ВЫКЛЮЧЕН.");
        }
    }
}

// 5. Класс-подписчик: Кондиционер (Cooler)
class Cooler
{
    private double thresholdTemperature = 25.0; // Порог включения охлаждения

    // Обработчик события
    public void Target_TemperatureChanged(object sender, TemperatureChangedEventArgs e)
    {
        Console.WriteLine($"  [Кондиционер]: Датчик зафиксировал {e.NewTemperature}°C.");
        if (e.NewTemperature > thresholdTemperature)
        {
            Console.WriteLine("    -> Слишком жарко! Кондиционер ВКЛЮЧЕН на охлаждение.");
        }
        else
        {
            Console.WriteLine("    -> Температура в норме. Кондиционер ВЫКЛЮЧЕН.");
        }
    }
}

public class ProgramTask2
{
    public static void Run()
    {
        Console.WriteLine("=== Задание 2. Работа с событиями (Датчик температуры) ===\n");

        // Создаем объекты издателя и подписчиков
        TemperatureSensor sensor = new TemperatureSensor();
        Heater heater = new Heater();
        Cooler cooler = new Cooler();

        // Подписываем приборы на событие изменения температуры датчика (+=)
        Console.WriteLine("Подписка приборов на датчик температуры...");
        sensor.TemperatureChanged += heater.Target_TemperatureChanged;
        sensor.TemperatureChanged += cooler.Target_TemperatureChanged;

        // Имитируем изменение погоды / температуры в комнате
        Console.WriteLine("\n--- Ситуация 1: Похолодало до 15 градусов ---");
        sensor.CurrentTemperature = 15.0; // Изменение свойства спровоцирует событие

        Console.WriteLine("\n--- Ситуация 2: Наступила жара, потеплело до 28 градусов ---");
        sensor.CurrentTemperature = 28.0;

        // Демонстрируем отписку от события (-=)
        Console.WriteLine("\nОтписка кондиционера от датчика...");
        sensor.TemperatureChanged -= cooler.Target_TemperatureChanged;

        Console.WriteLine("\n--- Ситуация 3: Температура стала 29 градусов (кондиционер не среагирует) ---");
        sensor.CurrentTemperature = 29.0;
    }
}
