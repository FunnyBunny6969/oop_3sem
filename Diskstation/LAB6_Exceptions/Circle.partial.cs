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
        set
        {
            if (double.IsNaN(value.X) || double.IsNaN(value.Y))
                throw new InvalidElementDataException(Name, "position", value.ToString(), "Position setter");
            _position = value;
        }
    }
    
    public Size Size
    {
        get => _size;
        set
        {
            if (value.Width <= 0 || value.Height <= 0)
                throw new InvalidElementDataException(Name, "size", value.ToString(), "Size setter");
            _size = value;
        }
    }
    
    public double UIArea => _size.Width * _size.Height;
    
    public void MoveTo(double x, double y)
    {
        if (double.IsNaN(x) || double.IsNaN(y))
            throw new InvalidElementDataException(Name, "coordinates", $"({x}, {y})", "MoveTo");
            
        _position = new Position(x, y);
        Console.WriteLine($"Круг '{Name}' перемещен в позицию {_position}");
    }
    
    public void ResizeUI(double width, double height)
    {
        if (width <= 0 || height <= 0)
            throw new InvalidElementDataException(Name, "size", $"{width}x{height}", "ResizeUI");
            
        _size = new Size(width, height);
        Console.WriteLine($"Круг '{Name}' изменен до размера {_size}");
    }
    
    public string GetUIInfo()
    {
        return $"Круг '{Name}' | Позиция: {_position} | Размер: {_size} | Площадь UI: {UIArea:F2} | Статус: {_status}";
    }
}
// ==================== КОНЕЦ PARTIAL КЛАССА CIRCLE ====================