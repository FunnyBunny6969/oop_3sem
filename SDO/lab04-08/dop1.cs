using System;
using System.Collections.Generic;
using System.Linq;

public static class Dop1
{
    public enum CrewRole { Captain, Pilot, Engineer, Medic, Scientist }
    public enum HealthStatus { Healthy, Injured, Critical }
    public enum ShipStatus { Normal, Warning, Critical }

    public struct CrewMember
    {
        public string Name { get; set; }
        public CrewRole Role { get; set; }
        public int HealthLevel { get; set; }
        public HealthStatus HealthStatus { get; set; }

        public CrewMember(string name, CrewRole role, int healthLevel)
        {
            Name = name;
            Role = role;
            HealthLevel = Math.Clamp(healthLevel, 1, 10);
            HealthStatus = healthLevel >= 7 ? HealthStatus.Healthy
                : healthLevel >= 4 ? HealthStatus.Injured
                : HealthStatus.Critical;
        }

        public void UpdateStatus()
        {
            HealthStatus = HealthLevel >= 7 ? HealthStatus.Healthy
                : HealthLevel >= 4 ? HealthStatus.Injured
                : HealthStatus.Critical;
        }
    }

    public struct Spaceship
    {
        public string Name { get; set; }
        public double Speed { get; set; }
        public string FuelType { get; set; }
        public CrewMember[] Crew { get; set; }
        public ShipStatus Status { get; set; }

        public Spaceship(string name, double speed, string fuelType, CrewMember[] crew)
        {
            Name = name;
            Speed = speed;
            FuelType = fuelType;
            Crew = crew;
            Status = ShipStatus.Normal;
        }

        public int TotalCrew => Crew.Length;
        public bool IsReady => Crew.All(c => c.HealthStatus != HealthStatus.Critical) && Status != ShipStatus.Critical;
        public double AverageHealth => Crew.Average(c => c.HealthLevel);

        public void SimulateDamage(int damage)
        {
            for (int i = 0; i < Crew.Length; i++)
            {
                Crew[i].HealthLevel = Math.Max(1, Crew[i].HealthLevel - damage);
                Crew[i].UpdateStatus();
            }
            UpdateStatus();
        }

        public void Repair()
        {
            for (int i = 0; i < Crew.Length; i++)
            {
                Crew[i].HealthLevel = Math.Min(10, Crew[i].HealthLevel + 2);
                Crew[i].UpdateStatus();
            }
            UpdateStatus();
        }

        public void UpdateStatus()
        {
            Status = Crew.Any(c => c.HealthStatus == HealthStatus.Critical) ? ShipStatus.Critical
                : Crew.Any(c => c.HealthStatus == HealthStatus.Injured) ? ShipStatus.Warning
                : ShipStatus.Normal;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Корабль: {Name}, скорость: {Speed}, топливо: {FuelType}");
            Console.WriteLine($"Экипаж: {TotalCrew}, среднее здоровье: {AverageHealth:F1}, статус: {Status}, готовность: {IsReady}");
            foreach (CrewMember member in Crew)
            {
                Console.WriteLine($"  {member.Name} — {member.Role}, здоровье {member.HealthLevel}/10 ({member.HealthStatus})");
            }
        }
    }

    public static void Run()
    {
        CrewMember[] crew =
        {
            new CrewMember("Иван Петров", CrewRole.Captain, 9),
            new CrewMember("Анна Смирнова", CrewRole.Pilot, 8),
            new CrewMember("Петр Сидоров", CrewRole.Engineer, 6)
        };

        Spaceship ship = new Spaceship("Аврора", 1500, "Ионное", crew);
        ship.PrintInfo();

        Console.WriteLine("\nКорабль получает повреждения (урон 4):");
        ship.SimulateDamage(4);
        ship.PrintInfo();

        Console.WriteLine("\nРемонт и восстановление экипажа:");
        ship.Repair();
        ship.PrintInfo();
    }
}
