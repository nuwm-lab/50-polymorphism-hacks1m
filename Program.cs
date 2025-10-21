using System;
using System.Collections.Generic;
using System.Linq;

namespace LabWork
{
    // Point struct with small vector helpers
    public readonly record struct Point(double X, double Y)
    {
        public override string ToString() => $"({X}, {Y})";

        public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);

        // 2D cross product (as scalar) of vectors a x b
        public static double Cross(Point a, Point b) => a.X * b.Y - a.Y * b.X;
    }

    // Abstract base for shapes
    public abstract class Shape
    {
        private readonly Point[] _vertices;

        protected Shape(Point[] vertices)
        {
            if (vertices == null) throw new ArgumentNullException(nameof(vertices));
            _vertices = (Point[])vertices.Clone();
        }

        // Expose vertices as read-only collection
        public IReadOnlyList<Point> Vertices => Array.AsReadOnly(_vertices);

        // Compute area
        public abstract double Area();

        // Print vertices (virtual so derived can add heading)
        public virtual void PrintVertices()
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                Console.WriteLine($"V{i + 1}: {Vertices[i]}");
            }
        }
    }

    // Triangle: area via shoelace; validate non-degenerate
    public class Triangle : Shape
    {
        public Triangle(Point a, Point b, Point c) : base(new[] { a, b, c })
        {
            if (Area() <= 1e-12)
                throw new ArgumentException("Triangle vertices are collinear or degenerate.");
        }

        public override double Area()
        {
            var v = Vertices;
            // Shoelace formula for 3 points
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

    // Convex quadrilateral: validate convexity and compute area via shoelace
    public class ConvexQuadrilateral : Shape
    {
        public ConvexQuadrilateral(Point a, Point b, Point c, Point d) : base(new[] { a, b, c, d })
        {
            if (!IsConvex())
                throw new ArgumentException("Quadrilateral must be convex and non-degenerate.");
        }

        private bool IsConvex()
        {
            var v = Vertices;
            if (v.Count != 4) return false;

            // Check cross product signs for consecutive edges
            double sign = 0;
            for (int i = 0; i < 4; i++)
            {
                var p0 = v[i];
                var p1 = v[(i + 1) % 4];
                var p2 = v[(i + 2) % 4];

                var a = p1 - p0;
                var b = p2 - p1;
                double cross = Point.Cross(a, b);

                // if any three consecutive points are collinear, consider invalid
                if (Math.Abs(cross) < 1e-12) return false;

                if (sign == 0) sign = Math.Sign(cross);
                else if (Math.Sign(cross) != sign) return false;
            }

            return true;
        }

        public override double Area()
        {
            var v = Vertices;
            double sum = 0;
            for (int i = 0; i < v.Count; i++)
            {
                int j = (i + 1) % v.Count;
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
            // Create several shapes with known areas to test correctness
            var t1 = new Triangle(new Point(0, 0), new Point(4, 0), new Point(0, 3)); // area 6
            var t2 = new Triangle(new Point(0, 0), new Point(2, 0), new Point(0, 2)); // area 2

            // Rectangle (convex quad) 4x3 => area 12
            var rect = new ConvexQuadrilateral(new Point(0, 0), new Point(4, 0), new Point(4, 3), new Point(0, 3));

            // Example of another convex quad (trapezoid)
            var trap = new ConvexQuadrilateral(new Point(0, 0), new Point(5, 0), new Point(4, 3), new Point(0, 3));

            // Demonstrate polymorphism: store different shapes in a list of base type
            var shapes = new List<Shape> { t1, rect, t2, trap };

            Console.WriteLine("--- Individual shapes (polymorphic calls via base reference) ---\n");
            foreach (Shape s in shapes)
            {
                // Method calls are polymorphic: runtime chooses the override
                Console.WriteLine($"Type: {s.GetType().Name}");
                s.PrintVertices();
                Console.WriteLine($"Area: {s.Area():F6}\n");
            }

            // Compute total area by iterating and calling Area() on the base type
            double total = shapes.Sum(s => s.Area());
            Console.WriteLine($"Total area of all shapes: {total:F6}");

            // Find shape with maximum area
            var max = FindMaxArea(shapes);
            Console.WriteLine($"Shape with max area: {max.GetType().Name}, Area = {max.Area():F6}\n");

            // Small tests: compare with expected values
            Console.WriteLine("--- Quick checks ---");
            CheckExpectedArea(t1, 6.0);
            CheckExpectedArea(rect, 12.0);
            CheckExpectedArea(t2, 2.0);
        }

        static Shape FindMaxArea(IEnumerable<Shape> shapes)
        {
            if (shapes == null) throw new ArgumentNullException(nameof(shapes));
            return shapes.OrderByDescending(s => s.Area()).FirstOrDefault();
        }

        static void CheckExpectedArea(Shape s, double expected)
        {
            double actual = s.Area();
            const double eps = 1e-9;
            Console.WriteLine($"{s.GetType().Name}: expected={expected}, actual={actual:F9}, OK={(Math.Abs(actual - expected) <= eps)}");
        }
    }
}