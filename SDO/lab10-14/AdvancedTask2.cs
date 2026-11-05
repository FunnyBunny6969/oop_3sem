using System;
using System.Collections.Generic;
using System.Linq;

public static class AdvancedTask2
{
    public class StaffMember
    {
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int Age { get; set; }
        public decimal Salary { get; set; }
    }

    public static void Run()
    {
        List<StaffMember> staff = new List<StaffMember>
        {
            new StaffMember { Name = "Иван Петров", Department = "IT", Gender = "М", Age = 28, Salary = 95000m },
            new StaffMember { Name = "Анна Смирнова", Department = "IT", Gender = "Ж", Age = 31, Salary = 88000m },
            new StaffMember { Name = "Петр Сидоров", Department = "IT", Gender = "М", Age = 42, Salary = 120000m },
            new StaffMember { Name = "Мария Иванова", Department = "IT", Gender = "Ж", Age = 25, Salary = 76000m },
            new StaffMember { Name = "Сергей Волков", Department = "Бухгалтерия", Gender = "М", Age = 45, Salary = 60000m },
            new StaffMember { Name = "Елена Морозова", Department = "Бухгалтерия", Gender = "Ж", Age = 38, Salary = 54000m },
            new StaffMember { Name = "Татьяна Павлова", Department = "Бухгалтерия", Gender = "Ж", Age = 29, Salary = 48000m },
            new StaffMember { Name = "Дмитрий Новиков", Department = "Отдел продаж", Gender = "М", Age = 35, Salary = 70000m },
            new StaffMember { Name = "Алексей Соколов", Department = "Отдел продаж", Gender = "М", Age = 27, Salary = 52000m },
            new StaffMember { Name = "Ольга Кузнецова", Department = "Отдел продаж", Gender = "Ж", Age = 33, Salary = 65000m },
            new StaffMember { Name = "Наталья Федорова", Department = "HR", Gender = "Ж", Age = 40, Salary = 38000m },
            new StaffMember { Name = "Михаил Егоров", Department = "HR", Gender = "М", Age = 36, Salary = 42000m }
        };

        var groupedStaff = staff
            .GroupBy(employee => employee.Department)
            .Select(departmentGroup => new
            {
                Department = departmentGroup.Key,
                GenderGroups = departmentGroup
                    .GroupBy(employee => employee.Gender)
                    .Select(genderGroup => new
                    {
                        Gender = genderGroup.Key,
                        EmployeeCount = genderGroup.Count(),
                        AverageAge = genderGroup.Average(employee => employee.Age),
                        AverageSalary = genderGroup.Average(employee => employee.Salary)
                    })
                    .OrderBy(genderGroup => genderGroup.Gender)
                    .ToList()
            })
            .OrderBy(departmentGroup => departmentGroup.Department)
            .ToList();

        Console.WriteLine("{0,-18}| {1,-6}| {2,8}| {3,14}| {4,18}",
            "Отдел", "Пол", "Кол-во", "Ср. возраст", "Ср. зарплата, руб.");
        Console.WriteLine(new string('-', 74));

        foreach (var department in groupedStaff)
        {
            foreach (var genderGroup in department.GenderGroups)
            {
                Console.WriteLine("{0,-18}| {1,-6}| {2,8}| {3,14:F1}| {4,18:F2}",
                    department.Department,
                    genderGroup.Gender,
                    genderGroup.EmployeeCount,
                    genderGroup.AverageAge,
                    genderGroup.AverageSalary);
            }
        }
    }
}
