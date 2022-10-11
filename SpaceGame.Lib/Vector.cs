namespace Vector;

public class Vector
{
    private readonly int[] _elements;
    public Vector(params int[] elements) => this._elements = elements;

    public static bool EqualSize(Vector A, Vector B) => A._elements.Length == B._elements.Length;
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(obj, null) || this.GetType() != obj.GetType()) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj is Vector vector &&
               _elements.SequenceEqual(vector._elements);
    }

    public static Vector operator +(Vector A, Vector B)
    {
        if (!EqualSize(A, B)) throw new System.ArgumentException("Dimensions do not match");
        return new Vector(A._elements.Zip(B._elements, (a, b) => a + b).ToArray());
    }
}