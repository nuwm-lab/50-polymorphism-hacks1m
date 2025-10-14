using System;

namespace LabWork
{
    // Заміна предметної області: додаємо абстракцію Shape та конкретні фігури

    public readonly record struct Point(double X, double Y)
    {
        public override string ToString() => $"({X}, {Y})";
    }

    public abstract class Shape
    {
        protected Point[] vertices;

        protected Shape(Point[] vertices)
        {
            this.vertices = vertices ?? Array.Empty<Point>();
        }

        // Обчислити площу фігури
        public abstract double Area();

        // Вивести вершини фігури
        public virtual void PrintVertices()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Console.WriteLine($"V{i + 1}: {vertices[i]}");
            }
        }
    }

    public class Triangle : Shape
    {
        public Triangle(Point a, Point b, Point c) : base(new[] { a, b, c }) { }

        public override double Area()
        {
            // Площа через формулу шнурка (shoelace) для трьох вершин
            var v = vertices;
            double area = Math.Abs((v[0].X * (v[1].Y - v[2].Y)
                                  + v[1].X * (v[2].Y - v[0].Y)
                                  + v[2].X * (v[0].Y - v[1].Y)) / 2.0);
            return area;
        }

        public override void PrintVertices()
        {
            Console.WriteLine("Triangle vertices:");
            base.PrintVertices();
        }
    }

    public class ConvexQuadrilateral : Shape
    {
        public ConvexQuadrilateral(Point a, Point b, Point c, Point d) : base(new[] { a, b, c, d }) { }

        public override double Area()
        {
            // Загальна формула шнурка (shoelace) для довільного опуклого чотирикутника
            var v = vertices;
            double sum = 0;
            for (int i = 0; i < v.Length; i++)
            {
                int j = (i + 1) % v.Length;
                sum += v[i].X * v[j].Y - v[j].X * v[i].Y;
            }
            return Math.Abs(sum) / 2.0;
        }

        public override void PrintVertices()
        {
            Console.WriteLine("Convex quadrilateral vertices:");
            base.PrintVertices();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Демонстрація Triangle
            var triangle = new Triangle(new Point(0, 0), new Point(4, 0), new Point(0, 3));
            triangle.PrintVertices();
            Console.WriteLine($"Area: {triangle.Area():F2}\n");

            // Демонстрація ConvexQuadrilateral
            var quad = new ConvexQuadrilateral(
                new Point(0, 0),
                new Point(4, 0),
                new Point(5, 3),
                new Point(0, 4)
            );
            quad.PrintVertices();
            Console.WriteLine($"Area: {quad.Area():F2}");
        }
    }
}