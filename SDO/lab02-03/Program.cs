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


        Console.WriteLine("=== DOP1 ===");
        Console.WriteLine($"LoggerNum\t\t{Logger.GetInstanceCount()}");
        var log1 = new Logger();
        var log2 = new Logger();
        var kig3 = new Logger();
        Console.WriteLine($"LoggerNum\t\t{Logger.GetInstanceCount()}");


        Console.WriteLine("=== DOP2 ===");
        Order ord1 = new Order();
        Order ord2 = new Order("Вячеслав");
        Order ord3 = new Order("Дмитрий", 15750.50);
        ord1.DisplayOrderInfo();
        ord2.DisplayOrderInfo();
        ord3.DisplayOrderInfo();


        Console.WriteLine("\n=== DOP3 ===");
        Document doc1 = new Document("Useless list");
        Document doc2 = new Document("Unreadable nonsense №12");
        Document doc3 = new Document($"Report {string.Concat(Enumerable.Repeat("on the report ", 7))}");
        doc1.PrintDocument();
        doc2.PrintDocument();
        doc3.PrintDocument();

        Document.ResetIdCounter();
        Document docNew1 = new Document("Empty form approved");
        Document docNew2 = new Document("Permit to allow permit");
        docNew1.PrintDocument();
        docNew2.PrintDocument();
    }
}

class Logger
{
    private static int InstanceCount = 0;

    public static int GetInstanceCount()
    {
        return InstanceCount;
    }

    public Logger()
    {
        InstanceCount++;
        Console.WriteLine("Created new Logger");
    }
}

class Order
{
    private string orderID;
    private string customerName;
    private double amount;

    public Order()
    {
        this.orderID = "ID-" + Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
        this.customerName = "Неизвестно";
        this.amount = 0;
        Console.WriteLine("[Конструктор 1]: Инициализация по умолчанию.");
    }

    public Order(string customerName) : this()
    {
        this.customerName = customerName;
        Console.WriteLine("[Конструктор 2]: Установлено имя клиента.");
    }

    public Order(string customerName, double amount) : this(customerName)
    {
        this.amount = amount;
        Console.WriteLine("[Конструктор 3]: Установлена сумма заказа.");
    }

    public void DisplayOrderInfo()
    {
        Console.WriteLine($"Заказ [{orderID}] | Клиент: {customerName} | Сумма: {amount}");
        Console.WriteLine(new string('-', 45));
    }
}


class Document
{
    private static int nextId = 1;

    private int id;
    private string title;

    public Document(string title)
    {
        this.title = title;
        this.id = nextId;
        nextId++;
    }

    public static void ResetIdCounter()
    {
        nextId = 1;
    }

    public void PrintDocument()
    {
        Console.WriteLine($"Document\t{id} | Title\t{title}");
    }
}
