using System.ComponentModel;
using AggregateException = System.AggregateException;

namespace Lab01Task01;
//Wawrzyniec Ciuła 15560
public enum WeightUnits
{
    G = (int)1,
    DAG = (int)10,
    KG = (int)1000,
    T = (int)1000000,  
    LB = (int)453.59237, 
    OZ = (int)(453.59237/16)
}
public class Weight : IEquatable<Weight>, IComparable<Weight>
{
    public double Value { get; init; }
    public WeightUnits Unit { get; init; }
    public Weight(double value, WeightUnits unit)
    {
        Value = value;
        Unit = unit;
    }
    private Weight() { }
    public static Weight Of(double value, WeightUnits unit)
    {

        if (value < 0)
        {
            throw new ArgumentException();
        }

        return new Weight(value, unit);
    }
    public static Weight Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException();

        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
            throw new ArgumentException();

        if (!double.TryParse(parts[0], out double value) || value < 0)
            throw new ArgumentException();

        if (!Enum.TryParse<WeightUnits>(parts[1], true, out WeightUnits unit))
            throw new ArgumentException();

        return new Weight(value, unit);
    }

    private double ToGram()
    {
        switch (Unit)
        {
            case WeightUnits.G:
                return Value;
            case WeightUnits.DAG:
                return Value * 10;
            case WeightUnits.KG:
                return Value * 1000;
            case WeightUnits.T:
                return Value * 1_000_000;
            case WeightUnits.LB:
                return Value * 453.59237;
            case WeightUnits.OZ:
                return Value * (453.59237 / 16);
            default:
                throw new ArgumentException();
        }
    }

    public int CompareTo(Weight? other)
    {
        double thisInGrams = this.ToGram();
        double otherInGrams = other.ToGram();
        return thisInGrams.CompareTo(otherInGrams);
    }

    public bool Equals(Weight? other)
    {
        if (other == null)
            return false;

        double thisG = this.ToGram();
        double otherG = other.ToGram();
        double tolerance = 0.0001;

        return (thisG - otherG) < tolerance;
    }

    public static bool operator >(Weight left, Weight right)
    {
        return left.CompareTo(right) > 0;
    }
    public static bool operator <(Weight left, Weight right)
    {
        return right.CompareTo(left) > 0;
    }

    public static bool operator ==(Weight left, Weight right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        return left.Equals(right);
    }

    public static bool operator !=(Weight left, Weight right)
    {
        return !(left == right);
    }
}
