using System;

// Интерфейс управления
public interface IControl
{
    void Show();
    void Input();
    void Resize(double factor);
    string GetInfo(); // Одноименный метод с абстрактным классом
}

// Абстрактный класс геометрической фигуры
public abstract class GeometricFigure
{
    public string Name { get; protected set; }
    public abstract double Area { get; }
    
    public abstract void Show();
    public abstract string GetInfo(); // Одноименный метод с интерфейсом
    
    public override string ToString()
    {
        return $"{GetType().Name}: {Name}, Площадь: {Area:F2}";
    }
}

// Класс Круг
public class Circle : GeometricFigure, IControl
{
    public double Radius { get; private set; }
    
    public Circle(string name, double radius)
    {
        Name = name;
        Radius = radius;
    }
    
    public override double Area => Math.PI * Radius * Radius;
    
    public override void Show()
    {
        Console.WriteLine($"Круг: {Name}, Радиус: {Radius}, Площадь: {Area:F2}");
    }
    
    public void Input()
    {
        Console.Write($"Введите новый радиус для круга '{Name}': ");
        if (double.TryParse(Console.ReadLine(), out double newRadius) && newRadius > 0)
        {
            Radius = newRadius;
        }
    }
    
    public void Resize(double factor)
    {
        if (factor > 0)
        {
            Radius *= factor;
            Console.WriteLine($"Круг '{Name}' масштабирован в {factor} раз");
        }
    }
    
    public override string GetInfo()
    {
        return $"Круг '{Name}' - Радиус: {Radius}, Площадь: {Area:F2}";
    }

    // ЯВНАЯ реализация для интерфейса IControl
    string IControl.GetInfo()
    {
        return $"[ЭЛЕМЕТ УПРАВЛЕНИЯ]: Круг '{Name}', Размер: {Radius}";
    }
    
    // Переопределение методов Object
    public override bool Equals(object obj)
    {
        return obj is Circle circle && Name == circle.Name && Radius == circle.Radius;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Radius);
    }
}

// Класс Прямоугольник
public class Rectangle : GeometricFigure, IControl
{
    public double Width { get; private set; }
    public double Height { get; private set; }
    
    public Rectangle(string name, double width, double height)
    {
        Name = name;
        Width = width;
        Height = height;
    }
    
    public override double Area => Width * Height;
    
    public override void Show()
    {
        Console.WriteLine($"Прямоугольник: {Name}, Ширина: {Width}, Высота: {Height}, Площадь: {Area:F2}");
    }
    
    public void Input()
    {
        Console.Write($"Введите новую ширину для прямоугольника '{Name}': ");
        if (double.TryParse(Console.ReadLine(), out double newWidth) && newWidth > 0)
        {
            Width = newWidth;
        }
        
        Console.Write($"Введите новую высоту для прямоугольника '{Name}': ");
        if (double.TryParse(Console.ReadLine(), out double newHeight) && newHeight > 0)
        {
            Height = newHeight;
        }
    }
    
    public void Resize(double factor)
    {
        if (factor > 0)
        {
            Width *= factor;
            Height *= factor;
            Console.WriteLine($"Прямоугольник '{Name}' масштабирован в {factor} раз");
        }
    }
    
    public override string GetInfo()
    {
        return $"Прямоугольник '{Name}' - Ширина: {Width}, Высота: {Height}, Площадь: {Area:F2}";
    }
}

// Абстрактный класс элемента управления
public abstract class ControlElement : IControl
{
    public string Name { get; protected set; }
    public bool IsEnabled { get; protected set; } = true;
    
    public abstract void Show();
    public abstract void Input();
    
    public virtual void Resize(double factor)
    {
        Console.WriteLine($"Изменение размера элемента '{Name}' в {factor} раз");
    }
    
    public abstract string GetInfo();
    
    public override string ToString()
    {
        return $"{GetType().Name}: {Name}, Активен: {IsEnabled}";
    }
}

// Sealed класс Checkbox
public sealed class Checkbox : ControlElement
{
    public bool IsChecked { get; private set; }
    
    public Checkbox(string name, bool isChecked = false)
    {
        Name = name;
        IsChecked = isChecked;
    }
    
    public override void Show()
    {
        Console.WriteLine($"Флажок: {Name}, Отмечен: {IsChecked}, Активен: {IsEnabled}");
    }
    
    public override void Input()
    {
        Console.Write($"Переключить флажок '{Name}'? (y/n): ");
        string input = Console.ReadLine();
        if (input?.ToLower() == "y" || input?.ToLower() == "n")
        {
            IsChecked = !IsChecked;
            Console.WriteLine($"Флажок '{Name}' теперь {(IsChecked ? "отмечен" : "не отмечен")}");
        }
    }
    
    public override string GetInfo()
    {
        return $"Флажок '{Name}' - Отмечен: {IsChecked}, Активен: {IsEnabled}";
    }
    
    // Переопределение всех методов Object
    public override bool Equals(object obj)
    {
        return obj is Checkbox checkbox && Name == checkbox.Name && IsChecked == checkbox.IsChecked && IsEnabled == checkbox.IsEnabled;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, IsChecked, IsEnabled);
    }
}

// Класс Radiobutton
public class Radiobutton : ControlElement
{
    public string Group { get; private set; }
    public bool IsSelected { get; private set; }
    
    public Radiobutton(string name, string group, bool isSelected = false)
    {
        Name = name;
        Group = group;
        IsSelected = isSelected;
    }
    
    public override void Show()
    {
        Console.WriteLine($"Радиокнопка: {Name}, Группа: {Group}, Выбрана: {IsSelected}, Активна: {IsEnabled}");
    }
    
    public override void Input()
    {
        Console.Write($"Выбрать радиокнопку '{Name}'? (y/н): ");
        string input = Console.ReadLine();
        if (input?.ToLower() == "y" || input?.ToLower() == "д")
        {
            IsSelected = true;
            Console.WriteLine($"Радиокнопка '{Name}' теперь выбрана");
        }
    }
    
    public override string GetInfo()
    {
        return $"Радиокнопка '{Name}' - Группа: {Group}, Выбрана: {IsSelected}, Активна: {IsEnabled}";
    }
}

// Класс Button
public class Button : ControlElement
{
    public string Text { get; private set; }
    
    public Button(string name, string text = "")
    {
        Name = name;
        Text = text;
    }
    
    public override void Show()
    {
        Console.WriteLine($"Кнопка: {Name}, Текст: '{Text}', Активна: {IsEnabled}");
    }
    
    public override void Input()
    {
        Console.Write($"Введите новый текст для кнопки '{Name}': ");
        string newText = Console.ReadLine();
        if (!string.IsNullOrEmpty(newText))
        {
            Text = newText;
            Console.WriteLine($"Текст кнопки '{Name}' обновлен");
        }
    }
    
    public override void Resize(double factor)
    {
        base.Resize(factor);
        Console.WriteLine($"Визуальный размер кнопки '{Name}' изменен");
    }
    
    public override string GetInfo()
    {
        return $"Кнопка '{Name}' - Текст: '{Text}', Активна: {IsEnabled}";
    }
}

// Класс Printer с полиморфным методом
public class Printer
{
    public void IAmPrinting(IControl control)
    {
        if (control is GeometricFigure figure)
        {
            Console.WriteLine($"Печать геометрической фигуры: {figure.ToString()}");
        }
        else if (control is ControlElement element)
        {
            Console.WriteLine($"Печать элемента управления: {element.ToString()}");
        }
        else
        {
            Console.WriteLine($"Печать неизвестного элемента: {control.ToString()}");
        }
    }
}

// Демонстрационная программа
class Program
{
    static void Main()
    {
        // Создание объектов различных классов
        Circle circle = new Circle("Маленький круг", 5.0);
        Rectangle rectangle = new Rectangle("Основной прямоугольник", 10.0, 8.0);
        Checkbox checkbox = new Checkbox("Согласен с условиями", true);
        Radiobutton radiobutton = new Radiobutton("Вариант А", "Группа1", false);
        Button button = new Button("Отправить", "Нажми меня!");
        
        // Работа с объектами через ссылки на абстрактные классы и интерфейсы
        GeometricFigure[] figures = { circle, rectangle };
        IControl[] controls = { circle, rectangle, checkbox, radiobutton, button };
        
        Console.WriteLine("=== Работа с геометрическими фигурами ===");
        foreach (var figure in figures)
        {
            figure.Show();
            Console.WriteLine($"Площадь: {figure.Area:F2}");
            Console.WriteLine($"GetInfo: {figure.GetInfo()}");
            Console.WriteLine();
        }
        
        Console.WriteLine("=== Работа с элементами управления ===");
        foreach (var control in controls)
        {
            control.Show();
            
            // Использование операторов is и as
            if (control is GeometricFigure geomFigure)
            {
                Console.WriteLine($"Это геометрическая фигура с площадью: {geomFigure.Area:F2}");
            }
            
            ControlElement controlElement = control as ControlElement;
            if (controlElement != null)
            {
                Console.WriteLine($"Это элемент управления: {controlElement.Name}");
            }
            
            Console.WriteLine();
        }
        
        // Демонстрация методов Input и Resize
        Console.WriteLine("=== Тестирование Input и Resize ===");
        circle.Input();
        circle.Resize(1.5);
        checkbox.Input();
        button.Input();
        
        Console.WriteLine("\n=== После изменений ===");
        circle.Show();
        checkbox.Show();
        button.Show();
        
        // Использование класса Printer
        Console.WriteLine("\n=== Демонстрация Printer ===");
        Printer printer = new Printer();
        
        // Создание массива с разнотипными объектами
        object[] objects = { circle, rectangle, checkbox, radiobutton, button };
        
        foreach (var obj in objects)
        {
            if (obj is IControl control)
            {
                printer.IAmPrinting(control);
            }
        }
        
        // Демонстрация переопределенных методов ToString()
        Console.WriteLine("\n=== Демонстрация ToString() ===");
        foreach (var obj in objects)
        {
            Console.WriteLine(obj.ToString());
        }
        
        // Демонстрация работы с одноименными методами
        Console.WriteLine("\n=== Демонстрация одноименных методов ===");
        Console.WriteLine($"GetInfo круга: {circle.GetInfo()}");
        Console.WriteLine($"GetInfo флажка: {checkbox.GetInfo()}");
        
        // Демонстрация sealed класса
        Console.WriteLine("\n=== Sealed класс Checkbox ===");
        Checkbox sealedCheckbox = new Checkbox("Тестовый флажок");
        Console.WriteLine(sealedCheckbox.ToString());

        // Демонстрация раличий с явной реализацией
        Console.WriteLine("\n=== Использование оператора as ===");
        var circleAsControl = circle as IControl;
        if (circleAsControl != null)
        {
            Console.WriteLine(circleAsControl.GetInfo());
            circleAsControl.Show();
        }
        
        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}