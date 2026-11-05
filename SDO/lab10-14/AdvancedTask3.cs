using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public enum Operation
{
    Equal,
    NotEqual,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    Contains
}

public sealed class FilterCondition
{
    public FilterCondition(string propertyName, Operation operation, object? value)
    {
        PropertyName = propertyName;
        Operation = operation;
        Value = value;
    }

    public string PropertyName { get; }
    public Operation Operation { get; }
    public object? Value { get; }
}

public sealed class SmartFilter<T>
{
    private readonly List<FilterCondition> _conditions;

    public SmartFilter()
    {
        _conditions = new List<FilterCondition>();
    }

    public SmartFilter(IEnumerable<FilterCondition> conditions)
    {
        _conditions = new List<FilterCondition>(conditions);
    }

    public SmartFilter<T> AddCondition(string propertyName, Operation operation, object? value)
    {
        _conditions.Add(new FilterCondition(propertyName, operation, value));
        return this;
    }

    public IEnumerable<T> Apply(IEnumerable<T> source)
    {
        if (_conditions.Count == 0)
        {
            return source;
        }

        ParameterExpression parameter = Expression.Parameter(typeof(T), "item");
        Expression? body = null;
        PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (FilterCondition condition in _conditions)
        {
            PropertyInfo? property = properties.FirstOrDefault(candidate =>
                string.Equals(candidate.Name, condition.PropertyName, StringComparison.OrdinalIgnoreCase));

            if (property == null)
            {
                throw new ArgumentException($"Свойство \"{condition.PropertyName}\" не найдено у типа {typeof(T).Name}.");
            }

            MemberExpression propertyAccess = Expression.Property(parameter, property);
            Expression constant = BuildConstant(condition.Value, property.PropertyType);
            Expression comparison = BuildComparison(propertyAccess, constant, property.PropertyType, condition.Operation);
            body = body == null ? comparison : Expression.AndAlso(body, comparison);
        }

        if (body == null)
        {
            return source;
        }

        Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
        Func<T, bool> predicate = lambda.Compile();
        return source.Where(predicate);
    }

    private static Expression BuildConstant(object? value, Type propertyType)
    {
        Type targetType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (value == null)
        {
            if (propertyType.IsValueType && Nullable.GetUnderlyingType(propertyType) == null)
            {
                throw new ArgumentException($"Свойство типа {propertyType.Name} не может сравниваться с null.");
            }

            return Expression.Constant(null, propertyType);
        }

        object convertedValue = ConvertValue(value, targetType);
        return Expression.Constant(convertedValue, targetType);
    }

    private static object ConvertValue(object value, Type targetType)
    {
        if (targetType == typeof(string))
        {
            return value.ToString() ?? string.Empty;
        }

        if (targetType.IsEnum)
        {
            return Enum.Parse(targetType, value.ToString()!, true);
        }

        if (targetType == typeof(Guid))
        {
            return Guid.Parse(value.ToString()!);
        }

        if (targetType == typeof(DateTime))
        {
            return value is DateTime dateTime
                ? dateTime
                : DateTime.Parse(value.ToString()!, CultureInfo.InvariantCulture);
        }

        return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
    }

    private static Expression BuildComparison(Expression property, Expression constant, Type propertyType, Operation operation)
    {
        return operation switch
        {
            Operation.Equal => Expression.Equal(property, constant),
            Operation.NotEqual => Expression.NotEqual(property, constant),
            Operation.GreaterThan => Expression.GreaterThan(property, constant),
            Operation.GreaterThanOrEqual => Expression.GreaterThanOrEqual(property, constant),
            Operation.LessThan => Expression.LessThan(property, constant),
            Operation.LessThanOrEqual => Expression.LessThanOrEqual(property, constant),
            Operation.Contains => BuildContains(property, constant, propertyType),
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, "Неподдерживаемая операция.")
        };
    }

    private static Expression BuildContains(Expression property, Expression constant, Type propertyType)
    {
        if (propertyType != typeof(string))
        {
            throw new ArgumentException("Операция Contains поддерживается только для строковых свойств.");
        }

        MethodInfo containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
        return Expression.Call(property, containsMethod, constant);
    }
}

public static class AdvancedTask3
{
    public class Car
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
    }

    public static void Run()
    {
        List<Car> cars = new List<Car>
        {
            new Car { Brand = "Toyota", Model = "Camry", Year = 2020, Price = 2500000m, InStock = true },
            new Car { Brand = "Toyota", Model = "Corolla", Year = 2019, Price = 1800000m, InStock = false },
            new Car { Brand = "Toyota", Model = "RAV4", Year = 2017, Price = 2200000m, InStock = true },
            new Car { Brand = "Toyota", Model = "Land Cruiser", Year = 2022, Price = 5500000m, InStock = false },
            new Car { Brand = "BMW", Model = "X5", Year = 2021, Price = 6000000m, InStock = true },
            new Car { Brand = "Kia", Model = "Rio", Year = 2018, Price = 1200000m, InStock = true },
            new Car { Brand = "Hyundai", Model = "Solaris", Year = 2016, Price = 900000m, InStock = false },
            new Car { Brand = "Skoda", Model = "Octavia", Year = 2019, Price = 1600000m, InStock = true },
            new Car { Brand = "Volkswagen", Model = "Polo", Year = 2018, Price = 1100000m, InStock = false },
            new Car { Brand = "Toyota", Model = "Corolla", Year = 2021, Price = 2100000m, InStock = true }
        };

        List<FilterCondition> conditions = new List<FilterCondition>
        {
            new FilterCondition(nameof(Car.Brand), Operation.Equal, "Toyota"),
            new FilterCondition(nameof(Car.Year), Operation.GreaterThanOrEqual, 2018),
            new FilterCondition(nameof(Car.Price), Operation.LessThan, 3000000m)
        };

        SmartFilter<Car> filter = new SmartFilter<Car>(conditions);
        List<Car> filteredCars = filter.Apply(cars).ToList();

        Console.WriteLine("Условия: Brand = Toyota, Year >= 2018, Price < 3 000 000");
        Console.WriteLine($"Найдено автомобилей: {filteredCars.Count}");

        foreach (Car car in filteredCars)
        {
            Console.WriteLine($"   {car.Brand} {car.Model}, {car.Year}, {car.Price:F2} руб.");
        }

        Console.WriteLine();
        Console.WriteLine("Дополнительный фильтр: Model содержит \"Corolla\"");

        SmartFilter<Car> corollaFilter = new SmartFilter<Car>()
            .AddCondition(nameof(Car.Model), Operation.Contains, "Corolla");

        foreach (Car car in corollaFilter.Apply(cars))
        {
            Console.WriteLine($"   {car.Brand} {car.Model}, {car.Year}, {car.Price:F2} руб.");
        }
    }
}
