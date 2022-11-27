namespace SpaceBattle.Lib;
public class Vector
{
    public int Size { get; private set; }
    private int[] coordinates { get; set; }
    public int this[int index]
    {
        get { return coordinates[index]; }
    }
    public Vector(params int[] coordinates)
    {
        this.coordinates = coordinates;
        Size = coordinates.Length;
    }
    public override string ToString()
    {
        string result = "Vector(";
        for (int i = 0; i < coordinates.Length; i++)
        {
            result += i != coordinates.Length - 1 ? $"{coordinates[i]}, " : $"{coordinates[i]}";
        }
        result += ")";
        return result;
    }
    public static Vector operator *(int a, Vector v) => new Vector((v.coordinates.ToList().Select(i => a * i)).ToArray());
    public static Vector operator *(Vector v, int a) => new Vector((v.coordinates.ToList().Select(i => a * i)).ToArray());
    public static Vector operator +(Vector v1, Vector v2)
    {
        if (v1.Size != v2.Size) throw new Exception();
        else
        {
            List<int> result = new List<int>();
            for (int i = 0; i < v1.Size; i++)
            {
                result.Add(v1.coordinates[i] + v2.coordinates[i]);
            }
            return new Vector(result.ToArray());
        }
    }
    public static Vector operator -(Vector v1, Vector v2)
    {
        return (v1 + (-1) * v2);
    }
    public static bool operator ==(Vector v1, Vector v2)
    {
        if (System.Object.ReferenceEquals(v1, v2))
        {
            return true;
        }

        if (((object)v1 == null) || ((object)v2 == null))
        {
            return false;
        }
        return v1!.Size == v2!.Size && v1!.coordinates.SequenceEqual(v2!.coordinates);
    }
    public static bool operator !=(Vector v1, Vector v2)
    {
        return !(v1 == v2);
    }
    public static bool operator >(Vector v1, Vector v2)
    {
        int vsize = (v1.Size < v2.Size) ? v1.Size : v2.Size;
        bool result = true;
        for (int i = 0; i < vsize; i++) result = (result) && (v1.coordinates[i] >= v2.coordinates[i]);
        result = (result) && !(v1 == v2);
        result = (result) && !(v1.Size < v2.Size);
        return result;
    }
    public static bool operator <(Vector v1, Vector v2)
    {
        int vsize = (v1.Size < v2.Size) ? v1.Size : v2.Size;
        bool result = true;
        for (int i = 0; i < vsize; i++) result = result && v1.coordinates[i] <= v2.coordinates[i];
        result = result && !(v1 == v2);
        result = result && !(v1.Size > v2.Size);
        return result;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector;
    }
    public override int GetHashCode() => throw new Exception();
}