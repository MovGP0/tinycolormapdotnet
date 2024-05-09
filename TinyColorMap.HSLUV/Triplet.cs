namespace TinyColorMap;

internal struct Triplet
{
    public double a { get; set; }
    public double b { get; set; }
    public double c { get; set; }

    public Triplet(double a, double b, double c)
    {
        this.a = a;
        this.b = b;
        this.c = c;
    }
}