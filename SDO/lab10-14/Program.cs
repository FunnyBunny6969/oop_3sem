using System;
using System.Collections.Generic;
using System.Linq;

public class Employee
{
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Базовое задание (вариант 3): сотрудники ===");
        RunLabTask();

        Console.WriteLine();
        Console.WriteLine(new string('=', 80));
        Console.WriteLine("=== Дополнительное задание 1: Анализ продаж компании ===");
        Console.WriteLine(new string('=', 80));
        AdvancedTask1.Run();

        Console.WriteLine();
        Console.WriteLine(new string('=', 80));
        Console.WriteLine("=== Дополнительное задание 2: Многоуровневая группировка ===");
        Console.WriteLine(new string('=', 80));
        AdvancedTask2.Run();

        Console.WriteLine();
        Console.WriteLine(new string('=', 80));
        Console.WriteLine("=== Дополнительное задание 3: Гибкий поисковый фильтр ===");
        Console.WriteLine(new string('=', 80));
        AdvancedTask3.Run();
    }

    static void RunLabTask()
    {
        List<Employee> employees = new List<Employee>
        {
            new Employee { Name = "Иван Петров", Department = "IT", Salary = 95000, HireDate = new DateTime(2018, 3, 12) },
            new Employee { Name = "Анна Смирнова", Department = "IT", Salary = 88000, HireDate = new DateTime(2020, 7, 1) },
            new Employee { Name = "Петр Сидоров", Department = "IT", Salary = 120000, HireDate = new DateTime(2015, 11, 23) },
            new Employee { Name = "Мария Иванова", Department = "IT", Salary = 76000, HireDate = new DateTime(2021, 2, 15) },
            new Employee { Name = "Ольга Кузнецова", Department = "IT", Salary = 105000, HireDate = new DateTime(2017, 9, 5) },
            new Employee { Name = "Сергей Волков", Department = "Бухгалтерия", Salary = 60000, HireDate = new DateTime(2016, 4, 18) },
            new Employee { Name = "Елена Морозова", Department = "Бухгалтерия", Salary = 54000, HireDate = new DateTime(2019, 6, 30) },
            new Employee { Name = "Дмитрий Новиков", Department = "Отдел продаж", Salary = 70000, HireDate = new DateTime(2014, 1, 20) },
            new Employee { Name = "Татьяна Павлова", Department = "Отдел продаж", Salary = 48000, HireDate = new DateTime(2022, 8, 8) },
            new Employee { Name = "Алексей Соколов", Department = "Отдел продаж", Salary = 52000, HireDate = new DateTime(2020, 12, 3) },
            new Employee { Name = "Наталья Федорова", Department = "HR", Salary = 38000, HireDate = new DateTime(2019, 5, 14) },
            new Employee { Name = "Михаил Егоров", Department = "HR", Salary = 42000, HireDate = new DateTime(2017, 10, 27) }
        };

        string targetDepartment = "IT";

        var topEmployees = (from employee in employees
                            where employee.Department == targetDepartment
                            orderby employee.Salary descending
                            select employee).Take(3);

        Console.WriteLine("Топ-3 самых высокооплачиваемых сотрудников отдела IT:");
        foreach (var employee in topEmployees)
        {
            Console.WriteLine($"  {employee.Name} — {employee.Salary:F2} руб. (принят {employee.HireDate:dd.MM.yyyy})");
        }

        Console.WriteLine();

        var departmentStats = employees
            .GroupBy(e => e.Department)
            .Select(g => new
            {
                DepartmentName = g.Key,
                EmployeeCount = g.Count(),
                AverageSalary = g.Average(e => e.Salary)
            })
            .Where(d => d.AverageSalary > 50000)
            .OrderByDescending(d => d.AverageSalary);

        Console.WriteLine("Отделы со средней зарплатой выше 50000:");
        foreach (var department in departmentStats)
        {
            Console.WriteLine($"  {department.DepartmentName} — {department.AverageSalary:F2} руб. (сотрудников: {department.EmployeeCount})");
        }
    }
}
