using System;
public struct Point
{
    public double X { get; set; }
    public double Y { get; set; }
    public override string ToString() => $"({X:F2},{Y:F2})";
}

public class Triangle
{
    protected Point A;
    protected Point B;
    protected Point C;

    public Triangle(Point a, Point b, Point c)
    {
        A = a;
        B = b;
        C = c;
    }

    public virtual void SetVertices(params Point[] pts)
    {
        if (pts == null || pts.Length < 3)
            throw new ArgumentException("потрібно 3 точки!");
        A = pts[0];
        B = pts[1];
        C = pts[2];
        Console.WriteLine("\nКоординати {0}(A, B, C) встановлено", this.GetType().Name);
    }

    public virtual void DisplayVertices()
    {
        Console.WriteLine($"Вершина A: {A}");
        Console.WriteLine($"Вершина B: {B}");
        Console.WriteLine($"Вершина C: {C}");
    }

    public virtual double CalculateArea()
    {
        return 0.5 * Math.Abs(A.X * (B.Y - C.Y) + B.X * (C.Y - A.Y) + C.X * (A.Y - B.Y));
    }
}

public class ConvexQuadrilateral : Triangle
{
    protected Point D;

    public ConvexQuadrilateral(Point a, Point b, Point c, Point d) : base(a, b, c)
    {
        SetVertices(a, b, c, d);
    }

    public override void SetVertices(params Point[] pts)
    {
        if (pts == null || pts.Length < 4)
            throw new ArgumentException("потрібно 4 точки!");
        base.SetVertices(pts[0], pts[1], pts[2]);
        D = pts[3];
        Console.WriteLine("\nКоординати {0}(A, B, C, D) встановлено", this.GetType().Name);
    }

    public override void DisplayVertices()
    {
        base.DisplayVertices();
        Console.WriteLine($"Вершина D: {D}");
    }

    public override double CalculateArea()
    {
        double areaABC = base.CalculateArea();
        double areaACD = 0.5 * Math.Abs(A.X * (C.Y - D.Y) + C.X * (D.Y - A.Y) + D.X * (A.Y - C.Y));
        return areaABC + areaACD;
    }
}

public class Program
{
    public static Point InputPoint(string name)
    {
        Console.WriteLine($"Введіть координати точки {name}:");
        double x, y;
        while (true)
        {
            Console.Write($"  X {name}: ");
            if (double.TryParse(Console.ReadLine(), out x))
            break;
            Console.WriteLine("ПОМИЛКА! Введіть число");
        }
        while (true)
        {
            Console.Write($"  Y {name}: ");
            if (double.TryParse(Console.ReadLine(), out y))
            break;
            Console.WriteLine("ПОМИЛКА! Введіть число");
        }
        return new Point {X = x , Y = y};
    }
    public static void Main(string[] args)
    {
        //автовибір
        Point p1 = new Point { X = 1, Y = 1 };
        Point p2 = new Point { X = 5, Y = 1 };
        Point p3 = new Point { X = 3, Y = 4 };
        Point p4 = new Point { X = 7, Y = 3 };
        Console.WriteLine("Виберіть режим введення координат:");
        Console.WriteLine("1 - Автовибір;");
        Console.WriteLine("2 - Ручний вибір.");
        Console.Write("Ваш вибір: ");
        string modeInput = Console.ReadLine();
        if (int.TryParse(modeInput,out int mode) && mode == 2)
        {
            Console.WriteLine("\nРучний вибір: ");
            p1 = InputPoint("A: ");
            p2 = InputPoint("B: ");
            p3 = InputPoint("C: ");
            p4 = InputPoint("D: ");
        }
        else if (mode == 1)
        {
            Console.WriteLine("\nАвтовибір:");
            Console.WriteLine($"A: {p1}");
            Console.WriteLine($"B: {p2}");
            Console.WriteLine($"C: {p3}");
            Console.WriteLine($"D: {p4}");
        }
        else
        {
            Console.WriteLine("ПОМИЛКА! Використовується автовибір.");
        }
        Console.WriteLine("\nВибір фігури: ");
        Console.WriteLine("1 - трикутник;");
        Console.WriteLine("2 - опуклий чотирикутник.");
        Console.Write("Ваш вибір: ");
        string shapeInput = Console.ReadLine();
        if (int.TryParse(modeInput, out int choice))
        {
         if (choice == 1)   
        {
        Console.WriteLine("___ 1. Робота з трикутником ___");
        Triangle triangle = new Triangle(p1, p2, p3);
        triangle.DisplayVertices();
        double triangleArea = triangle.CalculateArea();
        Console.WriteLine($"-> Обчислена Площа Трикутника: {triangleArea:F2}");
        Console.WriteLine("___________________________________________________________");
        Console.WriteLine("Приклад роботи полімірфізму:");
        Triangle poly1 = new ConvexQuadrilateral(p1, p2, p3, p4);
        poly1.DisplayVertices();
        }
        else if (choice == 2)
        {
        Console.WriteLine("___ 2. Робота з Опуклим чотирикутником ___");
        ConvexQuadrilateral quadrilateral = new ConvexQuadrilateral(p1, p2, p3, p4);
        quadrilateral.DisplayVertices();
        double quadrilateralArea = quadrilateral.CalculateArea();
        Console.WriteLine($"-> Обчислена Площа Чотирикутника: {quadrilateralArea:F2}");
        Console.WriteLine("___________________________________________________________");
        Console.WriteLine("Приклад роботи полімірфізму:");
        Triangle poly2 = new ConvexQuadrilateral(p1, p2, p3, p4);
        poly2.DisplayVertices();
        }
        else
            {
                Console.WriteLine("ПОМИЛКА! Введіть одне з чисел: 1 або 2");
            }
        }
        else
            {
                Console.WriteLine("ПОМИЛКА! Введено не число");
            }
        Console.WriteLine("\nНатисніть будь яку клавішу для завершення...");
        Console.ReadKey();
    }
}
