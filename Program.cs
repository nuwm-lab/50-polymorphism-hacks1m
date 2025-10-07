using System;

namespace LabWork
{
    // Даний проект є шаблоном для виконання лабораторних робіт
    // з курсу "Об'єктно-орієнтоване програмування та патерни проектування"
    // Необхідно змінювати і дописувати код лише в цьому проекті
    // Відео-інструкції щодо роботи з github можна переглянути 
    // за посиланням https://www.youtube.com/@ViktorZhukovskyy/videos 

class Sphere
{
    protected double b1, b2, b3, R;

    public virtual void SetCoefficients(double b1, double b2, double b3, double R)
    {
        this.b1 = b1;
        this.b2 = b2;
        this.b3 = b3;
        this.R = R;
    }

    public virtual void PrintCoefficients()
    {
        Console.WriteLine($"b1 = {b1}, b2 = {b2}, b3 = {b3}, R = {R}");
    }

    public virtual double Volume()
    {
        return (4.0 / 3.0) * Math.PI * Math.Pow(R, 3);
    }
}

class Ellipsoid : Sphere
{
    protected double a1, a2, a3;

    public void SetCoefficients(double b1, double b2, double b3, double a1, double a2, double a3)
    {
        base.SetCoefficients(b1, b2, b3, 0); // R не використовується
        this.a1 = a1;
        this.a2 = a2;
        this.a3 = a3;
    }

    public override void PrintCoefficients()
    {
        Console.WriteLine($"b1 = {b1}, b2 = {b2}, b3 = {b3}, a1 = {a1}, a2 = {a2}, a3 = {a3}");
    }

    public override double Volume()
    {
        return (4.0 / 3.0) * Math.PI * a1 * a2 * a3;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Куля
        Sphere sphere = new Sphere();
        sphere.SetCoefficients(1, 2, 3, 5); // b1, b2, b3, R
        Console.WriteLine("Коефіцієнти кулі:");
        sphere.PrintCoefficients();
        Console.WriteLine($"Об'єм кулі: {sphere.Volume():F2}");

        // Еліпсоїд
        Ellipsoid ellipsoid = new Ellipsoid();
        ellipsoid.SetCoefficients(1, 2, 3, 4, 5, 6); // b1, b2, b3, a1, a2, a3
        Console.WriteLine("\nКоефіцієнти еліпсоїда:");
        ellipsoid.PrintCoefficients();
        Console.WriteLine($"Об'єм еліпсоїда: {ellipsoid.Volume():F2}");
    }
}
}
