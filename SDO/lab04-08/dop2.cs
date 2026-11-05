using System;
using System.Collections.Generic;
using System.Linq;

public static class Dop2
{
    public enum DeviceType { Light, Thermostat, Socket, Sensor }
    public enum TriggerType { Time, Temperature, Motion }

    public struct Device
    {
        public string Name { get; set; }
        public DeviceType Type { get; set; }
        public bool IsOn { get; set; }
        public double PowerConsumption { get; set; }

        public Device(string name, DeviceType type, double powerConsumption)
        {
            Name = name;
            Type = type;
            PowerConsumption = powerConsumption;
            IsOn = false;
        }
    }

    public struct Room
    {
        public string Name { get; set; }
        public double Temperature { get; set; }
        public List<Device> Devices { get; set; }
        public List<Scenario> Scenarios { get; set; }

        public Room(string name, double temperature)
        {
            Name = name;
            Temperature = temperature;
            Devices = new List<Device>();
            Scenarios = new List<Scenario>();
        }
    }

    public struct Scenario
    {
        public string Name { get; set; }
        public TriggerType Trigger { get; set; }
        public string TriggerValue { get; set; }
        public string Action { get; set; }

        public Scenario(string name, TriggerType trigger, string triggerValue, string action)
        {
            Name = name;
            Trigger = trigger;
            TriggerValue = triggerValue;
            Action = action;
        }
    }

    public static void Run()
    {
        Room livingRoom = new Room("Гостиная", 21.5);
        livingRoom.Devices.Add(new Device("Лампа", DeviceType.Light, 0.06));
        livingRoom.Devices.Add(new Device("Термостат", DeviceType.Thermostat, 0.01));
        livingRoom.Devices.Add(new Device("Розетка", DeviceType.Socket, 0.0));

        livingRoom.Scenarios.Add(new Scenario("Утро", TriggerType.Time, "07:00", "Включить свет"));
        livingRoom.Scenarios.Add(new Scenario("Жарко", TriggerType.Temperature, "25", "Включить кондиционер"));
        livingRoom.Scenarios.Add(new Scenario("Движение", TriggerType.Motion, "Обнаружено", "Включить свет"));

        Console.WriteLine($"Комната: {livingRoom.Name}, температура: {livingRoom.Temperature}°C");
        Console.WriteLine("Устройства:");
        foreach (Device device in livingRoom.Devices)
        {
            Console.WriteLine($"  {device.Name} ({device.Type}), включено: {device.IsOn}, мощность: {device.PowerConsumption} кВт");
        }

        Console.WriteLine("\nСценарии:");
        foreach (Scenario scenario in livingRoom.Scenarios)
        {
            Console.WriteLine($"  {scenario.Name}: триггер {scenario.Trigger} = {scenario.TriggerValue}, действие: {scenario.Action}");
        }

        Console.WriteLine("\nПроверка сценария \"Жарко\"...");
        Scenario hot = livingRoom.Scenarios.First(s => s.Name == "Жарко");
        if (livingRoom.Temperature > double.Parse(hot.TriggerValue))
        {
            Console.WriteLine($"  Температура выше {hot.TriggerValue}°C. Выполняется действие: {hot.Action}");
        }

        double totalPower = livingRoom.Devices.Where(d => d.IsOn).Sum(d => d.PowerConsumption);
        Console.WriteLine($"\nСуммарная мощность включённых устройств: {totalPower:F2} кВт");
    }
}
