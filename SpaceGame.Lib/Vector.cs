namespace Vector;

public class Vector
{
    private readonly int[] _elements;
    public int Size => this._elements.Length;
    public Vector(params int[] elements) => this._elements = elements;
    public override string ToString() => $"Vector({string.Join(", ", this._elements)})";

    public static bool operator ==(Vector A, Vector B)
    {
        if (A.Size != B.Size) return false;
        for (int i = 0; i < A.Size; i++) if (A._elements[i] != B._elements[i]) return false;
        return true;
    }
    public static bool operator !=(Vector A, Vector B) => !(A == B);
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(obj, null) || this.GetType() != obj.GetType()) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj is Vector vector &&
               _elements.SequenceEqual(vector._elements);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static Vector operator +(Vector A, Vector B)
    {
        if (A.Size != B.Size) throw new System.ArgumentException("Dimensions do not match");
        return new Vector(A._elements.Zip(B._elements, (a, b) => a + b).ToArray());
    }
}