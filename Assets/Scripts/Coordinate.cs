using UnityEngine;

[System.Serializable]
public struct Coordinate
{
    [SerializeField] public int x;
    [SerializeField] public int y;

    public Coordinate(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public static Coordinate operator +(Coordinate a, Coordinate b)
    {
        return new Coordinate(a.x + b.x, a.y + b.y);
    }
}
