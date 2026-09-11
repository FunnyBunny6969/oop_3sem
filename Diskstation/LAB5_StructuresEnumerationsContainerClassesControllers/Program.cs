using System;
using System.Collections.Generic;
using System.Linq;

// ==================== ПЕРЕЧИСЛЕНИЯ И СТРУКТУРЫ ====================
public enum ElementStatus
{
    Active,
    Inactive,
    Hidden,
    Disabled
}

public struct Position
{
    public double X { get; set; }
    public double Y { get; set; }
    
    public Position(double x, double y)
    {
        X = x;
        Y = y;
    }
    
    public override string ToString()
    {
        return $"({X:F1}, {Y:F1})";
    }
}

public struct Size
{
    public double Width { get; set; }
    public double Height { get; set; }
    
    public Size(double width, double height)
    {
        Width = width;
        Height = height;
    }
    
    public override string ToString()
    {
        return $"{Width:F1}x{Height:F1}";
    }
}
// ==================== КОНЕЦ ПЕРЕЧИСЛЕНИЙ И СТРУКТУР ====================

// ==================== ИНТЕРФЕЙСЫ И АБСТРАКТНЫЕ КЛАССЫ ====================
public interface IControl
{
    void Show();
    void Input();
    void Resize(double factor);
    string GetInfo();
}

public abstract class GeometricFigure
{
    public string Name { get; protected set; } = "";
    public abstract double Area { get; }
    
    public abstract void Show();
    public abstract string GetInfo();
    
    public override string ToString()
    {
        return $"{GetType().Name}: {Name}, Площадь: {Area:F2}";
    }
}

public abstract class ControlElement : IControl
{
    public string Name { get; protected set; } = "";
    public bool IsEnabled { get; protected set; } = true;
    
    public Position Position { get; set; } = new Position(0, 0);
    public Size Size { get; set; } = new Size(0, 0);
    
    public double UIArea => Size.Width * Size.Height;
    
    public abstract void Show();
    public abstract void Input();
    
    public virtual void Resize(double factor)
    {
        Size = new Size(Size.Width * factor, Size.Height * factor);
        Console.WriteLine($"Элемент '{Name}' масштабирован до размера {Size}");
    }
    
    public void MoveTo(double x, double y)
    {
        Position = new Position(x, y);
        Console.WriteLine($"Элемент '{Name}' перемещен в позицию {Position}");
    }
    
    public void ResizeUI(double width, double height)
    {
        Size = new Size(width, height);
        Console.WriteLine($"Элемент '{Name}' изменен до размера {Size}");
    }
    
    public string GetUIInfo()
    {
        return $"{GetType().Name} '{Name}' | Позиция: {Position} | Размер: {Size} | Площадь UI: {UIArea:F2} | Активен: {IsEnabled}";
    }
    
    public abstract string GetInfo();
    
    public override string ToString()
    {
        return $"{GetType().Name}: {Name}, Активен: {IsEnabled}";
    }
}
// ==================== КОНЕЦ ИНТЕРФЕЙСОВ И АБСТРАКТНЫХ КЛАССОВ ====================

// ==================== КЛАСС-КОНТЕЙНЕР ====================
public class UIContainer
{
    private List<IControl> _elements;
    
    public UIContainer()
    {
        _elements = new List<IControl>();
    }
    
    public IControl? Get(int index)
    {
        if (index >= 0 && index < _elements.Count)
            return _elements[index];
        return null;
    }
    
    public void Set(int index, IControl element)
    {
        if (index >= 0 && index < _elements.Count)
            _elements[index] = element;
    }
    
    public void Add(IControl element)
    {
        _elements.Add(element);
    }
    
    public bool Remove(IControl element)
    {
        return _elements.Remove(element);
    }
    
    public void RemoveAt(int index)
    {
        if (index >= 0 && index < _elements.Count)
            _elements.RemoveAt(index);
    }
    
    public void PrintAll()
    {
        Console.WriteLine($"=== Элементы UI ({_elements.Count} шт.) ===");
        for (int i = 0; i < _elements.Count; i++)
        {
            Console.Write($"[{i}] ");
            _elements[i].Show();
        }
    }
    
    public int Count => _elements.Count;
    
    public IControl this[int index]
    {
        get => _elements[index];
        set => _elements[index] = value;
    }
}
// ==================== КОНЕЦ КЛАССА-КОНТЕЙНЕРА ====================

// ==================== УПРАВЛЯЮЩИЙ КЛАСС-КОНТРОЛЛЕР ====================
public class UIController
{
    private UIContainer _container;
    
    public UIController(UIContainer container)
    {
        _container = container;
    }
    
    // ЗАПРОС 1: Вывести список всех кнопок
    public void PrintAllButtons()
    {
        Console.WriteLine("=== Список всех кнопок ===");
        bool found = false;
        
        for (int i = 0; i < _container.Count; i++)
        {
            if (_container[i] is Button button)
            {
                button.Show();
                Console.WriteLine($"   UI информация: {button.GetUIInfo()}");
                found = true;
            }
        }
        
        if (!found)
            Console.WriteLine("Кнопки не найдены");
    }
    
    // ЗАПРОС 2: Подсчитать общее количество элементов на UI
    public int GetTotalElementCount()
    {
        return _container.Count;
    }
    
    // ЗАПРОС 3: Найти площадь занимаемую всеми элементами
    public double GetTotalUIArea()
    {
        double totalArea = 0;
        
        for (int i = 0; i < _container.Count; i++)
        {
            var element = _container[i];
            switch (element)
            {
                case Circle circle:
                    totalArea += circle.UIArea;
                    break;
                case Rectangle rectangle:
                    totalArea += rectangle.UIArea;
                    break;
                case ControlElement controlElement:
                    totalArea += controlElement.UIArea;
                    break;
            }
        }
        
        return totalArea;
    }
    
    public void PrintUIStatistics()
    {
        int circles = 0, rectangles = 0, buttons = 0, checkboxes = 0, radiobuttons = 0;
        double circlesArea = 0, rectanglesArea = 0, buttonsArea = 0, checkboxesArea = 0, radiobuttonsArea = 0;
        
        for (int i = 0; i < _container.Count; i++)
        {
            var element = _container[i];
            switch (element)
            {
                case Circle circle:
                    circles++;
                    circlesArea += circle.UIArea;
                    break;
                case Rectangle rectangle:
                    rectangles++;
                    rectanglesArea += rectangle.UIArea;
                    break;
                case Button button:
                    buttons++;
                    buttonsArea += button.UIArea;
                    break;
                case Checkbox checkbox:
                    checkboxes++;
                    checkboxesArea += checkbox.UIArea;
                    break;
                case Radiobutton radiobutton:
                    radiobuttons++;
                    radiobuttonsArea += radiobutton.UIArea;
                    break;
            }
        }
        
        Console.WriteLine("=== Статистика UI ===");
        Console.WriteLine($"Круги: {circles} шт., занимаемая площадь: {circlesArea:F2}");
        Console.WriteLine($"Прямоугольники: {rectangles} шт., занимаемая площадь: {rectanglesArea:F2}");
        Console.WriteLine($"Кнопки: {buttons} шт., занимаемая площадь: {buttonsArea:F2}");
        Console.WriteLine($"Флажки: {checkboxes} шт., занимаемая площадь: {checkboxesArea:F2}");
        Console.WriteLine($"Радиокнопки: {radiobuttons} шт., занимаемая площадь: {radiobuttonsArea:F2}");
        Console.WriteLine($"ВСЕГО: {_container.Count} элементов, общая площадь: {GetTotalUIArea():F2}");
    }
    
    public void PrintAllWithUIInfo()
    {
        Console.WriteLine("=== Все элементы UI с информацией ===");
        for (int i = 0; i < _container.Count; i++)
        {
            var element = _container[i];
            string uiInfo = element switch
            {
                Circle circle => circle.GetUIInfo(),
                Rectangle rectangle => rectangle.GetUIInfo(),
                ControlElement controlElement => controlElement.GetUIInfo(),
                _ => "Неизвестный элемент"
            };
            Console.WriteLine($"[{i}] {uiInfo}");
        }
    }
}
// ==================== КОНЕЦ УПРАВЛЯЮЩЕГО КЛАССА-КОНТРОЛЛЕРА ====================

// ==================== КЛАСС PRINTER ====================
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
// ==================== КОНЕЦ КЛАССА PRINTER ====================

// ==================== КОНКРЕТНЫЕ КЛАССЫ ====================
public partial class Circle : GeometricFigure, IControl
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

    string IControl.GetInfo()
    {
        return $"[ЭЛЕМЕТ УПРАВЛЕНИЯ]: Круг '{Name}', Размер: {Radius}";
    }
    
    public override bool Equals(object? obj)
    {
        return obj is Circle circle && Name == circle.Name && Radius == circle.Radius;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Radius);
    }
}

public class Rectangle : GeometricFigure, IControl
{
    public double Width { get; private set; }
    public double Height { get; private set; }
    
    public ElementStatus Status { get; set; } = ElementStatus.Active;
    public Position Position { get; set; } = new Position(0, 0);
    public Size Size { get; set; } = new Size(0, 0);
    
    public double UIArea => Size.Width * Size.Height;
    
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
    
    public void MoveTo(double x, double y)
    {
        Position = new Position(x, y);
        Console.WriteLine($"Прямоугольник '{Name}' перемещен в позицию {Position}");
    }
    
    public void ResizeUI(double width, double height)
    {
        Size = new Size(width, height);
        Console.WriteLine($"Прямоугольник '{Name}' изменен до размера {Size}");
    }
    
    public string GetUIInfo()
    {
        return $"Прямоугольник '{Name}' | Позиция: {Position} | Размер: {Size} | Площадь UI: {UIArea:F2} | Статус: {Status}";
    }
    
    public override string GetInfo()
    {
        return $"Прямоугольник '{Name}' - Ширина: {Width}, Высота: {Height}, Площадь: {Area:F2}";
    }
}

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
        string? input = Console.ReadLine();
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
    
    public override bool Equals(object? obj)
    {
        return obj is Checkbox checkbox && Name == checkbox.Name && IsChecked == checkbox.IsChecked && IsEnabled == checkbox.IsEnabled;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, IsChecked, IsEnabled);
    }
}

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
        string? input = Console.ReadLine();
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
        string? newText = Console.ReadLine();
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
// ==================== КОНЕЦ КОНКРЕТНЫХ КЛАССОВ ====================

// ==================== ОСНОВНАЯ ПРОГРАММА ====================
class Program
{
    static void Main()
    {
        Console.WriteLine("=== СОЗДАНИЕ UI ИЗ ФИГУР И ЭЛЕМЕНТОВ УПРАВЛЕНИЯ ===\n");
        
        // Создание объектов различных классов (оригинальная часть)
        Circle circle = new Circle("Маленький круг", 5.0);
        Rectangle rectangle = new Rectangle("Основной прямоугольник", 10.0, 8.0);
        Checkbox checkbox = new Checkbox("Согласен с условиями", true);
        Radiobutton radiobutton = new Radiobutton("Вариант А", "Группа1", false);
        Button button = new Button("Отправить", "Нажми меня!");
        
        // ОРИГИНАЛЬНАЯ ДЕМОНСТРАЦИЯ ИЗ ЛАБЫ 4
        Console.WriteLine("=== Оригинальная демонстрация из лабы 4 ===");
        
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
            
            ControlElement? controlElement = control as ControlElement;
            if (controlElement != null)
            {
                Console.WriteLine($"Это элемент управления: {controlElement.Name}");
            }
            
            Console.WriteLine();
        }
        
        // Использование класса Printer
        Console.WriteLine("=== Демонстрация Printer ===");
        Printer printer = new Printer();
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

        // Демонстрация различий с явной реализацией
        Console.WriteLine("\n=== Использование оператора as ===");
        var circleAsControl = circle as IControl;
        if (circleAsControl != null)
        {
            Console.WriteLine(circleAsControl.GetInfo());
            circleAsControl.Show();
        }
        
        // НОВАЯ ФУНКЦИОНАЛЬНОСТЬ ЛАБЫ 5
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("=== НОВАЯ ФУНКЦИОНАЛЬНОСТЬ ЛАБЫ 5 (UI СИСТЕМА) ===");
        
        // Настройка UI свойств для элементов
        circle.MoveTo(10, 10);
        circle.ResizeUI(100, 100);
        
        rectangle.MoveTo(50, 50);
        rectangle.ResizeUI(200, 150);
        
        checkbox.MoveTo(20, 180);
        checkbox.ResizeUI(150, 25);
        
        radiobutton.MoveTo(20, 210);
        radiobutton.ResizeUI(120, 25);
        
        button.MoveTo(180, 180);
        button.ResizeUI(80, 30);
        
        Button button2 = new Button("Отмена", "Отмена");
        button2.MoveTo(180, 220);
        button2.ResizeUI(80, 30);
        
        Button button3 = new Button("Справка", "Помощь");
        button3.MoveTo(180, 260);
        button3.ResizeUI(80, 30);
        
        // Создание контейнера и добавление элементов
        UIContainer uiContainer = new UIContainer();
        uiContainer.Add(circle);
        uiContainer.Add(rectangle);
        uiContainer.Add(checkbox);
        uiContainer.Add(radiobutton);
        uiContainer.Add(button);
        uiContainer.Add(button2);
        uiContainer.Add(button3);
        
        // Создание контроллера UI
        UIController uiController = new UIController(uiContainer);
        
        // ВЫПОЛНЕНИЕ ЗАПРОСОВ ПО ВАРИАНТУ 2:
        Console.WriteLine("\n" + new string('=', 50));
        uiController.PrintAllButtons();
        
        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine($"Общее количество элементов на UI: {uiController.GetTotalElementCount()}");
        
        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine($"Общая площадь занимаемая всеми элементами: {uiController.GetTotalUIArea():F2}");
        
        Console.WriteLine("\n" + new string('=', 50));
        uiController.PrintUIStatistics();
        
        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}
// ==================== КОНЕЦ ОСНОВНОЙ ПРОГРАММЫ ====================