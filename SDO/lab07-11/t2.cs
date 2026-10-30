using System;

class TemperatureChangedEventArgs : EventArgs
{
    public double NewTemperature { get; set; }

    public TemperatureChangedEventArgs(double temp)
    {
        NewTemperature = temp;
    }
}

delegate void TemperatureChangedHandler(object sender, TemperatureChangedEventArgs e);

class TemperatureSensor
{
    private double currentTemperature;

    public event TemperatureChangedHandler? TemperatureChanged;

    public double CurrentTemperature
    {
        get { return currentTemperature; }
        set
        {
            if (currentTemperature != value)
            {
                currentTemperature = value;
                OnTemperatureChanged(value);
            }
        }
    }

    protected virtual void OnTemperatureChanged(double temp)
    {
        TemperatureChanged?.Invoke(this, new TemperatureChangedEventArgs(temp));
    }
}

class Heater
{
    private double targetTemperature = 20.0;

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

class Cooler
{
    private double thresholdTemperature = 25.0;

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

        TemperatureSensor sensor = new TemperatureSensor();
        Heater heater = new Heater();
        Cooler cooler = new Cooler();

        Console.WriteLine("Подписка приборов на датчик температуры...");
        sensor.TemperatureChanged += heater.Target_TemperatureChanged;
        sensor.TemperatureChanged += cooler.Target_TemperatureChanged;

        Console.WriteLine("\n--- Ситуация 1: Похолодало до 15 градусов ---");
        sensor.CurrentTemperature = 15.0;

        Console.WriteLine("\n--- Ситуация 2: Наступила жара, потеплело до 28 градусов ---");
        sensor.CurrentTemperature = 28.0;

        Console.WriteLine("\nОтписка кондиционера от датчика...");
        sensor.TemperatureChanged -= cooler.Target_TemperatureChanged;

        Console.WriteLine("\n--- Ситуация 3: Температура стала 29 градусов (кондиционер не среагирует) ---");
        sensor.CurrentTemperature = 29.0;
    }
}
