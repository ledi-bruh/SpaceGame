namespace Vector;

public class Vector
{
    private readonly int[] _elements;
    public int Size => _elements.Length;
    public Vector(params int[] elements) => this._elements = elements;

    public static bool IsEqualSize(Vector A, Vector B) => A.Size == B.Size;

    public override bool Equals(object? obj)
    {
        return obj is Vector vector && this._elements.SequenceEqual(vector._elements);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (int)2166136261;
            _elements.ToList().ForEach(n => hash = (hash * 16777619) ^ n.GetHashCode());
            return hash;
        }
    }

    public static Vector operator +(Vector A, Vector B)
    {
        if (!IsEqualSize(A, B)) throw new System.ArgumentException("Dimensions do not match");
        return new Vector(A._elements.Zip(B._elements, (a, b) => a + b).ToArray());
    }
}