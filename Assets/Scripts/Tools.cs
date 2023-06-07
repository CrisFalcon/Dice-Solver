using System.Collections.Generic;

public static class Tools
{
    //could be better
    public static Dictionary<Normals, Face> SwitchFace(this Dictionary<Normals, Face> current, Directions dir)
    {
        var prev = new Dictionary<Normals, Face>(current);

        if (dir == Directions.North || dir == Directions.South)
        {
            current[Normals.UP] = dir == Directions.North ? prev[Normals.BACK] : prev[Normals.FORWARD];
            current[Normals.DOWN] = dir == Directions.North ? prev[Normals.FORWARD] : prev[Normals.BACK];
            current[Normals.FORWARD] = dir == Directions.North ? prev[Normals.UP] : prev[Normals.DOWN];
            current[Normals.BACK] = dir == Directions.North ? prev[Normals.DOWN] : prev[Normals.UP];
        }
        else
        {
            current[Normals.UP] = dir == Directions.East ? prev[Normals.LEFT] : prev[Normals.RIGHT];
            current[Normals.LEFT] = dir == Directions.East ? prev[Normals.DOWN] : prev[Normals.UP];
            current[Normals.DOWN] = dir == Directions.East ? prev[Normals.RIGHT] : prev[Normals.LEFT];
            current[Normals.RIGHT] = dir == Directions.East ? prev[Normals.UP] : prev[Normals.DOWN];
        }

        return current;
    }

    private static Dictionary<Directions, Coordinate> _dirCoords;

    /// <summary>
    /// Dictionary used to know how to change local coordinates based on a given direction.
    /// </summary>
    public static Dictionary<Directions, Coordinate> CoorDir
    {
        get
        {
            if(_dirCoords == null || _dirCoords.Count == 0)
            {
                _dirCoords = new Dictionary<Directions, Coordinate>();
                _dirCoords.Add(Directions.North, new Coordinate(0, 1));
                _dirCoords.Add(Directions.South, new Coordinate(0, -1));
                _dirCoords.Add(Directions.East, new Coordinate(1, 0));
                _dirCoords.Add(Directions.West, new Coordinate(-1, 0));
            }

            return _dirCoords;
        }
    }

}
