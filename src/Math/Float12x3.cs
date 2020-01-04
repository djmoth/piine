using System.Runtime.InteropServices;

[StructLayout (LayoutKind.Explicit, Size = 144)]
public unsafe struct Float12x3
{
    private const int ROWS = 12;
    private const int COLUMNS = 3;

    [FieldOffset (0)]
    public fixed float data[ROWS * COLUMNS];

    public float this[int x, int y]
    {
        get => data[x * COLUMNS + y];
        set => data[x * COLUMNS + y] = value;
    }

    public static int CalculateIndex (int x, int y) => x * COLUMNS + y;
}
