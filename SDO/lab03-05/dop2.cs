using System;

public static class Dop2
{
    public abstract class Shape
    {
        public string Color { get; }

        protected Shape(string color) => Color = color;

        public abstract double GetArea();
        public abstract double GetPerimeter();

        public virtual bool CanContain(Shape other) => GetArea() >= other.GetArea();

        public virtual string GetInfo() =>
            $"{GetType().Name} ({Color}), площадь {GetArea():F2}, периметр {GetPerimeter():F2}";
    }

    public class Circle : Shape
    {
        public double Radius { get; }

        public Circle(string color, double radius) : base(color)
        {
            if (radius <= 0) throw new ArgumentException("Радиус должен быть положительным.");
            Radius = radius;
        }

        public override double GetArea() => Math.PI * Radius * Radius;
        public override double GetPerimeter() => 2 * Math.PI * Radius;

        public override bool CanContain(Shape other) => other is Circle circle && Radius > circle.Radius;

        public override string GetInfo() => base.GetInfo() + $", радиус {Radius:F2}";
    }

    public class Rectangle : Shape
    {
        public double Width { get; }
        public double Height { get; }

        public Rectangle(string color, double width, double height) : base(color)
        {
            if (width <= 0 || height <= 0) throw new ArgumentException("Стороны должны быть положительными.");
            Width = width;
            Height = height;
        }

        public override double GetArea() => Width * Height;
        public override double GetPerimeter() => 2 * (Width + Height);

        public override bool CanContain(Shape other) =>
            other is Rectangle rectangle && Width >= rectangle.Width && Height >= rectangle.Height;

        public override string GetInfo() => base.GetInfo() + $", {Width:F2}x{Height:F2}";
    }

    public class Triangle : Shape
    {
        public double A { get; }
        public double B { get; }
        public double C { get; }

        public Triangle(string color, double a, double b, double c) : base(color)
        {
            if (a <= 0 || b <= 0 || c <= 0) throw new ArgumentException("Стороны должны быть положительными.");
            if (a + b <= c || a + c <= b || b + c <= a)
                throw new ArgumentException("Треугольник с такими сторонами не существует.");

            A = a;
            B = b;
            C = c;
        }

        public override double GetArea()
        {
            double semiPerimeter = GetPerimeter() / 2;
            return Math.Sqrt(semiPerimeter * (semiPerimeter - A) * (semiPerimeter - B) * (semiPerimeter - C));
        }

        public override double GetPerimeter() => A + B + C;

        public override string GetInfo() => base.GetInfo() + $", стороны {A:F2}, {B:F2}, {C:F2}";
    }

    public static void Run()
    {
        Shape[] shapes =
        {
            new Circle("Красный", 5),
            new Circle("Синий", 3),
            new Rectangle("Зелёный", 10, 6),
            new Rectangle("Жёлтый", 8, 4),
            new Triangle("Оранжевый", 3, 4, 5)
        };

        foreach (Shape shape in shapes) Console.WriteLine($"  {shape.GetInfo()}");

        Console.WriteLine("\nПроверка вложенности:");
        Console.WriteLine($"  Круг R=5 содержит круг R=3: {shapes[0].CanContain(shapes[1])}");
        Console.WriteLine($"  Прямоугольник 10x6 содержит 8x4: {shapes[2].CanContain(shapes[3])}");
        Console.WriteLine($"  Прямоугольник 8x4 содержит 10x6: {shapes[3].CanContain(shapes[2])}");
        Console.WriteLine($"  Круг R=5 содержит прямоугольник 10x6: {shapes[0].CanContain(shapes[2])}");
        Console.WriteLine($"  Треугольник 3-4-5 содержит круг R=3: {shapes[4].CanContain(shapes[1])}");
    }
}
