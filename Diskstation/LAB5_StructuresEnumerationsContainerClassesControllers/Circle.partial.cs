using System;

// ==================== PARTIAL КЛАСС CIRCLE ====================
public partial class Circle
{
    private ElementStatus _status = ElementStatus.Active;
    private Position _position = new Position(0, 0);
    private Size _size = new Size(0, 0);
    
    public ElementStatus Status 
    { 
        get => _status;
        set
        {
            _status = value;
            Console.WriteLine($"Статус круга '{Name}' изменен на: {value}");
        }
    }
    
    public Position Position
    {
        get => _position;
        set => _position = value;
    }
    
    public Size Size
    {
        get => _size;
        set => _size = value;
    }
    
    public double UIArea => _size.Width * _size.Height;
    
    public void MoveTo(double x, double y)
    {
        _position = new Position(x, y);
        Console.WriteLine($"Круг '{Name}' перемещен в позицию {_position}");
    }
    
    public void ResizeUI(double width, double height)
    {
        _size = new Size(width, height);
        Console.WriteLine($"Круг '{Name}' изменен до размера {_size}");
    }
    
    public string GetUIInfo()
    {
        return $"Круг '{Name}' | Позиция: {_position} | Размер: {_size} | Площадь UI: {UIArea:F2} | Статус: {_status}";
    }
}
// ==================== КОНЕЦ PARTIAL КЛАССА CIRCLE ====================