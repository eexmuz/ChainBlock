[System.Serializable]
public struct Coords
{
    public int x;
    public int y;

    public int Index(int width) => y * width + x;
}