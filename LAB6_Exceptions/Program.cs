using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

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

// ==================== ИСКЛЮЧЕНИЯ ====================
public class UIException : Exception
{
    public string ElementName { get; }
    public string Operation { get; }
    
    public UIException(string message, string elementName, string operation) : base(message)
    {
        ElementName = elementName;
        Operation = operation;
    }
    
    public UIException(string message, string elementName, string operation, Exception innerException) 
        : base(message, innerException)
    {
        ElementName = elementName;
        Operation = operation;
    }
    
    public override string ToString()
    {
        return $"UIException: {Message} | Элемент: {ElementName} | Операция: {Operation}";
    }
}

public class InvalidElementDataException : UIException
{
    public string PropertyName { get; }
    public object InvalidValue { get; }
    
    public InvalidElementDataException(string elementName, string propertyName, object invalidValue, string operation) 
        : base($"Недопустимое значение '{invalidValue}' для свойства '{propertyName}'", elementName, operation)
    {
        PropertyName = propertyName;
        InvalidValue = invalidValue;
    }
    
    public override string ToString()
    {
        return $"InvalidElementDataException: {Message} | Свойство: {PropertyName} | Значение: {InvalidValue}";
    }
}

public class ElementOperationException : UIException
{
    public ElementOperationException(string elementName, string operation, string reason) 
        : base($"Ошибка операции: {reason}", elementName, operation)
    {
    }
    
    public ElementOperationException(string elementName, string operation, string reason, Exception innerException) 
        : base($"Ошибка операции: {reason}", elementName, operation, innerException)
    {
    }
}

public class ContainerException : Exception
{
    public int Index { get; }
    public int ContainerSize { get; }
    
    public ContainerException(string message, int index, int containerSize) : base(message)
    {
        Index = index;
        ContainerSize = containerSize;
    }
    
    public override string ToString()
    {
        return $"ContainerException: {Message} | Индекс: {Index} | Размер контейнера: {ContainerSize}";
    }
}

public class MemoryAllocationException : Exception
{
    public long RequestedSize { get; }
    
    public MemoryAllocationException(string message, long requestedSize) : base(message)
    {
        RequestedSize = requestedSize;
    }
    
    public MemoryAllocationException(string message, long requestedSize, Exception innerException) 
        : base(message, innerException)
    {
        RequestedSize = requestedSize;
    }
    
    public override string ToString()
    {
        return $"MemoryAllocationException: {Message} | Запрошенный размер: {RequestedSize} байт";
    }
}
// ==================== КОНЕЦ ИСКЛЮЧЕНИЙ ====================

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
        if (factor <= 0)
            throw new InvalidElementDataException(Name, "factor", factor, "Resize");
            
        Size = new Size(Size.Width * factor, Size.Height * factor);
        Console.WriteLine($"Элемент '{Name}' масштабирован до размера {Size}");
    }
    
    public void MoveTo(double x, double y)
    {
        if (double.IsNaN(x) || double.IsNaN(y))
            throw new InvalidElementDataException(Name, "coordinates", $"({x}, {y})", "MoveTo");
            
        Position = new Position(x, y);
        Console.WriteLine($"Элемент '{Name}' перемещен в позицию {Position}");
    }
    
    public void ResizeUI(double width, double height)
    {
        if (width <= 0 || height <= 0)
            throw new InvalidElementDataException(Name, "size", $"{width}x{height}", "ResizeUI");
            
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
        if (index < 0 || index >= _elements.Count)
            throw new ContainerException($"Индекс {index} вне диапазона", index, _elements.Count);
            
        return _elements[index];
    }
    
    public void Set(int index, IControl element)
    {
        if (index < 0 || index >= _elements.Count)
            throw new ContainerException($"Индекс {index} вне диапазона", index, _elements.Count);
            
        if (element == null)
            throw new ArgumentNullException(nameof(element), "Элемент не может быть null");
            
        _elements[index] = element;
    }
    
    public void Add(IControl element)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element), "Нельзя добавить null элемент");
            
        _elements.Add(element);
    }
    
    public bool Remove(IControl element)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element), "Нельзя удалить null элемент");
            
        return _elements.Remove(element);
    }
    
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _elements.Count)
            throw new ContainerException($"Индекс {index} вне диапазона", index, _elements.Count);
            
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
        get
        {
            if (index < 0 || index >= _elements.Count)
                throw new ContainerException($"Индекс {index} вне диапазона", index, _elements.Count);
            return _elements[index];
        }
        set
        {
            if (index < 0 || index >= _elements.Count)
                throw new ContainerException($"Индекс {index} вне диапазона", index, _elements.Count);
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Элемент не может быть null");
            _elements[index] = value;
        }
    }
}
// ==================== КОНЕЦ КЛАССА-КОНТЕЙНЕРА ====================

// ==================== УПРАВЛЯЮЩИЙ КЛАСС-КОНТРОЛЛЕР ====================
public class UIController
{
    private UIContainer _container;
    
    public UIController(UIContainer container)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
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
    
    // Метод с использованием Assert
    public void ValidateContainerState()
    {
        Debug.Assert(_container != null, "Контейнер не может быть null");
        Debug.Assert(_container.Count >= 0, "Количество элементов не может быть отрицательным");
        
        // Проверка на наличие null элементов
        for (int i = 0; i < _container.Count; i++)
        {
            Debug.Assert(_container[i] != null, $"Элемент с индексом {i} не может быть null");
        }
        
        Console.WriteLine("Состояние контейнера валидно");
    }
}
// ==================== КОНЕЦ УПРАВЛЯЮЩЕГО КЛАССА-КОНТРОЛЛЕРА ====================

// ==================== КЛАСС PRINTER ====================
public class Printer
{
    public void IAmPrinting(IControl control)
    {
        if (control == null)
            throw new ArgumentNullException(nameof(control), "Объект для печати не может быть null");
            
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
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidElementDataException("Unknown", "name", name, "Constructor");
            
        if (radius <= 0)
            throw new InvalidElementDataException(name, "radius", radius, "Constructor");
        
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
        else
        {
            throw new InvalidElementDataException(Name, "radius", "invalid input", "Input");
        }
    }
    
    public void Resize(double factor)
    {
        if (factor <= 0)
            throw new InvalidElementDataException(Name, "factor", factor, "Resize");
            
        Radius *= factor;
        Console.WriteLine($"Круг '{Name}' масштабирован в {factor} раз");
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
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidElementDataException("Unknown", "name", name, "Constructor");
            
        if (width <= 0 || height <= 0)
            throw new InvalidElementDataException(name, "dimensions", $"{width}x{height}", "Constructor");
        
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
        else
        {
            throw new InvalidElementDataException(Name, "width", "invalid input", "Input");
        }
        
        Console.Write($"Введите новую высоту для прямоугольника '{Name}': ");
        if (double.TryParse(Console.ReadLine(), out double newHeight) && newHeight > 0)
        {
            Height = newHeight;
        }
        else
        {
            throw new InvalidElementDataException(Name, "height", "invalid input", "Input");
        }
    }
    
    public void Resize(double factor)
    {
        if (factor <= 0)
            throw new InvalidElementDataException(Name, "factor", factor, "Resize");
            
        Width *= factor;
        Height *= factor;
        Console.WriteLine($"Прямоугольник '{Name}' масштабирован в {factor} раз");
    }
    
    public void MoveTo(double x, double y)
    {
        if (double.IsNaN(x) || double.IsNaN(y))
            throw new InvalidElementDataException(Name, "coordinates", $"({x}, {y})", "MoveTo");
            
        Position = new Position(x, y);
        Console.WriteLine($"Прямоугольник '{Name}' перемещен в позицию {Position}");
    }
    
    public void ResizeUI(double width, double height)
    {
        if (width <= 0 || height <= 0)
            throw new InvalidElementDataException(Name, "size", $"{width}x{height}", "ResizeUI");
            
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
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidElementDataException("Unknown", "name", name, "Constructor");
            
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
        else
        {
            throw new ElementOperationException(Name, "Input", "Некорректный ввод пользователя");
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
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidElementDataException("Unknown", "name", name, "Constructor");
            
        if (string.IsNullOrWhiteSpace(group))
            throw new InvalidElementDataException(name, "group", group, "Constructor");
            
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
        else
        {
            throw new ElementOperationException(Name, "Input", "Некорректный ввод пользователя");
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
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidElementDataException("Unknown", "name", name, "Constructor");
            
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
        else
        {
            throw new ElementOperationException(Name, "Input", "Текст не может быть пустым");
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
    static void DemonstrateAssert()
    {
        Console.WriteLine("\n=== Демонстрация Assert ===");
        
        // Создаем тестовый контейнер
        UIContainer testContainer = new UIContainer();
        UIController testController = new UIController(testContainer);
        
        try
        {
            // Этот вызов должен пройти успешно
            testController.ValidateContainerState();
            
            // Добавим элемент и снова проверим
            testContainer.Add(new Button("TestButton"));
            testController.ValidateContainerState();
            
            Console.WriteLine("Assert проверки прошли успешно");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при демонстрации Assert: {ex.Message}");
        }
    }
    
    static void DemonstrateMultipleExceptionHandling()
    {
        Console.WriteLine("\n=== Демонстрация многоуровневой обработки исключений ===");
        
        try
        {
            Console.WriteLine("Попытка создать круг с отрицательным радиусом...");
            Circle invalidCircle = new Circle("Невалидный круг", -5.0);
        }
        catch (InvalidElementDataException ex)
        {
            Console.WriteLine($"Поймано InvalidElementDataException: {ex}");
            Console.WriteLine("Пробрасываем исключение выше...");
            throw; // Проброс исключения выше по стеку
        }
    }
    
    static void Main()
    {
        try
        {
            Console.WriteLine("=== СОЗДАНИЕ UI ИЗ ФИГУР И ЭЛЕМЕНТОВ УПРАВЛЕНИЯ ===\n");
            
            // ДЕМОНСТРАЦИЯ РАЗЛИЧНЫХ ИСКЛЮЧЕНИЙ
            
            // 1. Исключение при создании объекта с невалидными данными
            Console.WriteLine("1. Тест: Создание круга с отрицательным радиусом");
            try
            {
                Circle invalidCircle = new Circle("Невалидный круг", -5.0);
            }
            catch (InvalidElementDataException ex)
            {
                Console.WriteLine($"Поймано исключение: {ex}");
            }
            
            // 2. Исключение при работе с контейнером
            Console.WriteLine("\n2. Тест: Обращение к несуществующему индексу в контейнере");
            try
            {
                UIContainer container = new UIContainer();
                var element = container[10]; // Несуществующий индекс
            }
            catch (ContainerException ex)
            {
                Console.WriteLine($"Поймано исключение: {ex}");
            }
            
            // 3. Исключение при делении на ноль
            Console.WriteLine("\n3. Тест: Деление на ноль");
            try
            {
                int a = 10;
                int b = 0;
                int result = a / b;
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Поймано исключение: {ex.Message}");
            }
            
            // 4. Исключение при работе с null ссылкой
            Console.WriteLine("\n4. Тест: Работа с null ссылкой");
            try
            {
                Circle? nullCircle = null;
                nullCircle!.Show(); // Намеренное использование null
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"Поймано исключение: {ex.Message}");
            }
            
            // 5. Исключение при неверном преобразовании типа
            Console.WriteLine("\n5. Тест: Неверное преобразование типа");
            try
            {
                object obj = "не число";
                int number = (int)obj;
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"Поймано исключение: {ex.Message}");
            }
            
            // 6. Демонстрация многоуровневой обработки
            Console.WriteLine("\n6. Тест: Многоуровневая обработка исключений");
            try
            {
                DemonstrateMultipleExceptionHandling();
            }
            catch (InvalidElementDataException ex)
            {
                Console.WriteLine($"Исключение перехвачено на верхнем уровне: {ex}");
            }
            
            // 7. Демонстрация Assert
            DemonstrateAssert();
            
            // НОРМАЛЬНАЯ РАБОТА ПРОГРАММЫ
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("=== НОРМАЛЬНАЯ РАБОТА ПРОГРАММЫ ===");
            
            // Создание объектов различных классов
            Circle circle = new Circle("Маленький круг", 5.0);
            Rectangle rectangle = new Rectangle("Основной прямоугольник", 10.0, 8.0);
            Checkbox checkbox = new Checkbox("Согласен с условиями", true);
            Radiobutton radiobutton = new Radiobutton("Вариант А", "Группа1", false);
            Button button = new Button("Отправить", "Нажми меня!");
            
            // Создание контейнера и добавление элементов
            UIContainer uiContainer = new UIContainer();
            uiContainer.Add(circle);
            uiContainer.Add(rectangle);
            uiContainer.Add(checkbox);
            uiContainer.Add(radiobutton);
            uiContainer.Add(button);
            
            // Создание контроллера UI
            UIController uiController = new UIController(uiContainer);
            
            // ВЫПОЛНЕНИЕ ЗАПРОСОВ
            Console.WriteLine("\n" + new string('=', 50));
            uiController.PrintAllButtons();
            
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine($"Общее количество элементов на UI: {uiController.GetTotalElementCount()}");
            
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine($"Общая площадь занимаемая всеми элементами: {uiController.GetTotalUIArea():F2}");
            
            Console.WriteLine("\n" + new string('=', 50));
            uiController.PrintUIStatistics();
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n!!! УНИВЕРСАЛЬНЫЙ ОБРАБОТЧИК: Произошла непредвиденная ошибка !!!");
            Console.WriteLine($"Тип: {ex.GetType()}");
            Console.WriteLine($"Сообщение: {ex.Message}");
            Console.WriteLine($"Стек вызовов: {ex.StackTrace}");
        }
        finally
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("БЛОК FINALLY: Программа завершена. Ресурсы освобождены.");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
// ==================== КОНЕЦ ОСНОВНОЙ ПРОГРАММЫ ====================