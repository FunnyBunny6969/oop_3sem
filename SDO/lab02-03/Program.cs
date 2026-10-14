using System;

class Employee
{
    private static int TotalEmployees = 0;
    private string name;
    private string position;
    private double salary;

    public Employee()
    {
        TotalEmployees++;

        name = "None";
        position = "None";
        salary = 9999999.99;  
    }

    public Employee(string name, string position, double salary)
    {
        TotalEmployees++;

        this.name = name;
        this.position = position;
        this.salary = salary;
    }

    public static int GetTotalEmployees()
    {
        return TotalEmployees;
    }

    public void PrintInfo()
    {
        Console.WriteLine("----- EMPLOYEE -----");
        Console.WriteLine($"Name \t\t{name}");
        Console.WriteLine($"Position \t{position}");
        Console.WriteLine($"Salary \t\t${salary}");
        Console.WriteLine("--------------------");
    }

    public void GiveRaise(double percent)
    {
        this.salary *= percent / 100 + 1;
    }
}


class Program
{
    static void Main()
    {
        Employee emp1 = new Employee("Daniel", "Python Dev", 14.89);
        emp1.PrintInfo();
        emp1.GiveRaise(354.511535728677);
        emp1.PrintInfo();

        Console.WriteLine($"EmpNum\t\t{Employee.GetTotalEmployees()}");
        var emp2 = new Employee();
        var emp3 = new Employee();
        var emp4 = new Employee();
        Console.WriteLine($"EmpNum\t\t{Employee.GetTotalEmployees()}");
    }
}