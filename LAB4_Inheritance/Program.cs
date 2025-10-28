using System;

// Интерфейс управления
public interface IControl
{
    void Show();
    void Input();
    void Resize(double factor);
    string GetInfo();
}

// Абстрактный класс геометрической фигуры
public abstract class GeometricShape
{
    public string Name { get; set; }
    
    public abstract double GetArea();
    public abstract string GetInfo();
    
    public override string ToString()
    {
        return $"{GetType().Name}: {Name}, Площадь: {GetArea():F2}";
    }
}

// Класс Круг
public class Circle : GeometricShape, IControl
{
    public double Radius { get; set; }
    
    public Circle(string name, double radius)
    {
        Name = name;
        Radius = radius;
    }
    
    public override double GetArea()
    {
        return Math.PI * Radius * Radius;
    }
    
    public override string GetInfo()
    {
        return $"Круг '{Name}' с радиусом {Radius}";
    }
    
    public void Show()
    {
        Console.WriteLine($"Отображаем круг: {Name}");
    }
    
    public void Input()
    {
        Console.WriteLine($"Ввод данных для круга: {Name}");
    }
    
    public void Resize(double factor)
    {
        Radius *= factor;
        Console.WriteLine($"Круг масштабирован в {factor} раз. Новый радиус: {Radius}");
    }
    
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
public class Rectangle : GeometricShape, IControl
{
    public double Width { get; set; }
    public double Height { get; set; }
    
    public Rectangle(string name, double width, double height)
    {
        Name = name;
        Width = width;
        Height = height;
    }
    
    public override double GetArea()
    {
        return Width * Height;
    }
    
    public override string GetInfo()
    {
        return $"Прямоугольник '{Name}' размером {Width}x{Height}";
    }
    
    public void Show()
    {
        Console.WriteLine($"Отображаем прямоугольник: {Name}");
    }
    
    public void Input()
    {
        Console.WriteLine($"Ввод данных для прямоугольника: {Name}");
    }
    
    public void Resize(double factor)
    {
        Width *= factor;
        Height *= factor;
        Console.WriteLine($"Прямоугольник масштабирован в {factor} раз. Новый размер: {Width}x{Height}");
    }
    
    public override bool Equals(object obj)
    {
        return obj is Rectangle rect && Name == rect.Name && Width == rect.Width && Height == rect.Height;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Width, Height);
    }
}

// Абстрактный класс элемента управления
public abstract class ControlElement : IControl
{
    public string Name { get; set; }
    public bool IsEnabled { get; set; } = true;
    
    public abstract void Draw();
    
    public virtual void Show()
    {
        Console.WriteLine($"Отображаем элемент управления: {Name}");
    }
    
    public virtual void Input()
    {
        Console.WriteLine($"Ввод данных для элемента: {Name}");
    }
    
    public virtual void Resize(double factor)
    {
        Console.WriteLine($"Изменение размера элемента {Name} в {factor} раз");
    }
    
    public virtual string GetInfo()
    {
        return $"{GetType().Name} '{Name}' (Активен: {IsEnabled})";
    }
    
    public override string ToString()
    {
        return GetInfo();
    }
}

// Класс Checkbox
public class Checkbox : ControlElement
{
    public bool IsChecked { get; set; }
    
    public override void Draw()
    {
        Console.WriteLine($"Рисуем флажок '{Name}' - Отмечен: {IsChecked}");
    }
    
    public override string GetInfo()
    {
        return $"Флажок '{Name}' - Отмечен: {IsChecked}, Активен: {IsEnabled}";
    }
    
    public override bool Equals(object obj)
    {
        return obj is Checkbox cb && Name == cb.Name && IsEnabled == cb.IsEnabled && IsChecked == cb.IsChecked;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, IsEnabled, IsChecked);
    }
}

// Класс Radiobutton
public class Radiobutton : ControlElement
{
    public bool IsSelected { get; set; }
    public string GroupName { get; set; } = "default";
    
    public override void Draw()
    {
        Console.WriteLine($"Рисуем переключатель '{Name}' - Выбран: {IsSelected}, Группа: {GroupName}");
    }
    
    public override string GetInfo()
    {
        return $"Переключатель '{Name}' - Выбран: {IsSelected}, Группа: {GroupName}, Активен: {IsEnabled}";
    }
    
    public override bool Equals(object obj)
    {
        return obj is Radiobutton rb && Name == rb.Name && IsEnabled == rb.IsEnabled && 
               IsSelected == rb.IsSelected && GroupName == rb.GroupName;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, IsEnabled, IsSelected, GroupName);
    }
}

// Sealed класс Button
public sealed class Button : ControlElement
{
    public string Text { get; set; } = "Кнопка";
    
    public override void Draw()
    {
        Console.WriteLine($"Рисуем кнопку '{Name}' - Текст: '{Text}'");
    }
    
    public override void Input()
    {
        Console.WriteLine($"Кнопка '{Name}' нажата!");
    }
    
    public override string GetInfo()
    {
        return $"Кнопка '{Name}' - Текст: '{Text}', Активна: {IsEnabled}";
    }
    
    public override bool Equals(object obj)
    {
        return obj is Button btn && Name == btn.Name && IsEnabled == btn.IsEnabled && Text == btn.Text;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, IsEnabled, Text);
    }
}

// Класс Printer с полиморфным методом
public class Printer
{
    public void IAmPrinting(IControl obj)
    {
        if (obj is null)
        {
            Console.WriteLine("Передан null объект");
            return;
        }
        
        var shape = obj as GeometricShape;
        if (shape != null)
        {
            Console.WriteLine($"Печать геометрической фигуры: {shape}");
        }
        else
        {
            if (obj is ControlElement control)
            {
                Console.WriteLine($"Печать элемента управления: {control}");
            }
            else
            {
                Console.WriteLine($"Печать неизвестного IControl: {obj}");
            }
        }
        
        Console.WriteLine($"ToString(): {obj.ToString()}");
        Console.WriteLine("---");
    }
}

// Демонстрационная программа
class Program
{
    static void Main()
    {
        Circle circle = new Circle("Маленький круг", 5.0);
        Rectangle rectangle = new Rectangle("Основной прямоугольник", 10.0, 5.0);
        Checkbox checkbox = new Checkbox { Name = "Согласие", IsChecked = true };
        Radiobutton radiobutton = new Radiobutton { Name = "Вариант А", IsSelected = true, GroupName = "опции" };
        Button button = new Button { Name = "Отправить", Text = "Нажми меня!" };
        
        GeometricShape[] shapes = { circle, rectangle };
        IControl[] controls = { circle, rectangle, checkbox, radiobutton, button };
        
        Console.WriteLine("=== Работа с геометрическими фигурами ===");
        foreach (var shape in shapes)
        {
            Console.WriteLine(shape.ToString());
            Console.WriteLine($"Площадь: {shape.GetArea():F2}");
            Console.WriteLine($"Информация: {shape.GetInfo()}");
            Console.WriteLine();
        }
        
        Console.WriteLine("=== Работа с элементами управления ===");
        foreach (var control in controls)
        {
            control.Show();
            control.Input();
            control.Resize(1.5);
            Console.WriteLine($"Информация: {control.GetInfo()}");
            Console.WriteLine();
        }
        
        Console.WriteLine("=== Использование класса Printer ===");
        Printer printer = new Printer();
        
        object[] objects = { circle, rectangle, checkbox, radiobutton, button, "Простая строка", 42 };
        
        foreach (var obj in objects)
        {
            var control = obj as IControl;
            if (control != null)
            {
                printer.IAmPrinting(control);
            }
            else
            {
                Console.WriteLine($"Объект {obj} не является IControl");
                Console.WriteLine("---");
            }
        }
        
        Console.WriteLine("=== Идентификация типов с помощью оператора 'is' ===");
        foreach (var control in controls)
        {
            if (control is Circle c)
            {
                Console.WriteLine($"Это круг с радиусом {c.Radius}");
            }
            else if (control is Rectangle r)
            {
                Console.WriteLine($"Это прямоугольник {r.Width}x{r.Height}");
            }
            else if (control is Checkbox cb)
            {
                Console.WriteLine($"Это флажок (отмечен: {cb.IsChecked})");
            }
            else if (control is Radiobutton rb)
            {
                Console.WriteLine($"Это переключатель в группе '{rb.GroupName}'");
            }
            else if (control is Button btn)
            {
                Console.WriteLine($"Это кнопка с текстом '{btn.Text}'");
            }
        }
    }
}