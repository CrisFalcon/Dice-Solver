using System.Collections.Generic;

public struct WorldState
{
    public Dictionary<Normals, Face> faceStatus;
    public Coordinate currentPosition;
    public Face currentFace { get { return faceStatus[Normals.UP]; } }

    private Dictionary<Directions, Coordinate> _dirCoords;

    public Directions dirToGetHere;

    public WorldState(Dictionary<Normals, Face> fs, Coordinate pos)
    {
        faceStatus = fs;
        currentPosition = pos;
        dirToGetHere = Directions.North;
        _dirCoords = new Dictionary<Directions, Coordinate>(Tools.CoorDir);
    }

    public WorldState CloneOnDir(Directions dir)
    {
        var newState = new Dictionary<Normals, Face>(faceStatus).SwitchFace(dir);
        Coordinate newCoords = currentPosition + _dirCoords[dir];
        var ws = new WorldState(newState, newCoords);
        ws.dirToGetHere = dir;
        return ws;
    }

}