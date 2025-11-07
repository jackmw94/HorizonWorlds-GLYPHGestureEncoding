public struct Bounds2D
{
    public float MinX;
    public float MaxX;
    public float MinY;
    public float MaxY;
    public float XRange => MaxX - MinX;
    public float YRange => MaxY - MinY;
}