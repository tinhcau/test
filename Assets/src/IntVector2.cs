public struct IntVector2
{
    public IntVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return "x = " + x.ToString() + ", y = " + y.ToString();
    }

    public bool isEqual(IntVector2 vector)
    {
        return x == vector.x && y == vector.y;
    }

    public int x;
    public int y;
}