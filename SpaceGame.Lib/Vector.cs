namespace Vector;

public class Vector
{
    private readonly int[] _elements;
    public int Size => this._elements.Length;
    public Vector(params int[] elements) => this._elements = elements;
    public override string ToString() => $"Vector({string.Join(", ", this._elements)})";
    public static Vector operator +(Vector A, Vector B)
    {
        if (A.Size != B.Size) throw new System.ArgumentException("Dimensions do not match");
        return new Vector(A._elements.Zip(B._elements, (a, b) => a + b).ToArray());
    }
}