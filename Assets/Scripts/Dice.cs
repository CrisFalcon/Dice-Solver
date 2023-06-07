using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Dice : MonoBehaviour
{
    public Face currentFace = Face.TWO;
    public Face desiredFace = Face.ONE;

    [SerializeField] DiceMovement _movement;

    Dictionary<Normals, Face> _faceStatus;

    [SerializeField] GridBoard _board;

    [SerializeField] Coordinate currentPos = new Coordinate(0, 0);

    Dictionary<Directions, Coordinate> _directions;

    bool _canMove = true;
    public bool IsMoving { get { return _path.Count > 0; } }

    Queue<Directions> _path = new Queue<Directions>();
    DiceSolver _solver = new DiceSolver();

    #region Initialize
    void Start()
    {
        _faceStatus = new Dictionary<Normals, Face>();
        _faceStatus.Add(Normals.RIGHT, Face.ONE);
        _faceStatus.Add(Normals.UP, Face.TWO);
        _faceStatus.Add(Normals.BACK, Face.THREE);
        _faceStatus.Add(Normals.LEFT, Face.SIX);
        _faceStatus.Add(Normals.FORWARD, Face.FOUR);
        _faceStatus.Add(Normals.DOWN, Face.FIVE);

        _directions = new Dictionary<Directions, Coordinate>(Tools.CoorDir);
        _movement.SetTransform(transform);
    }
    #endregion

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentFace == desiredFace) return;

            Solve();
        }
    }

    public void Solve(int desired)
    {
        if (IsMoving) return;
        desiredFace = (Face)desired;
        Solve();
    }

    void Solve()
    {
        var start = new WorldState(_faceStatus, currentPos);
        Face desired = desiredFace;

        Func<WorldState, bool> satisfies = x => x.faceStatus[Normals.UP] == desired;

        Func<WorldState, IEnumerable<WorldState>> neighbors = x => AvailableDirections(x.currentPosition).Select(y => x.CloneOnDir(y));

        Func<WorldState, float> heuristic = x =>
        {
            Vector3 startPos = new Vector3(start.currentPosition.x, start.currentPosition.y);
            Vector3 endPos = new Vector3(x.currentPosition.x, x.currentPosition.y);
            float total = (endPos - startPos).sqrMagnitude;

            if (x.faceStatus[Normals.UP] != desiredFace) total += 3;
            return total;
        };

        var path = _solver.AStar(start, satisfies, neighbors, heuristic);

        _path = new Queue<Directions>(path.Select(x => x.dirToGetHere).ToList());

        if (_path.Count > 0) StartCoroutine(FeedDirections(_path));
    }


    void ApplyNewCoordinates(Directions dir)
    {
        currentPos += _directions[dir];
    }

    void SwitchDirections(Directions dir)
    {
        _faceStatus = _faceStatus.SwitchFace(dir);
        currentFace = _faceStatus[Normals.UP];
    }

    IEnumerable<Directions> AvailableDirections(Coordinate pos)
    {
        return _directions.Where(x => _board.MoveAvailable(pos + x.Value)).Select(x => x.Key);
    }

    void RollDie(Directions dir)
    {
        if (!_canMove) return;

        StartCoroutine(_movement.Roll(dir, x => _canMove = x));
        SwitchDirections(dir);
        ApplyNewCoordinates(dir);
    }

    IEnumerator FeedDirections(Queue<Directions> directions)
    {
        while (directions.Count > 0)
        {
            while (!_canMove) yield return null;

            yield return new WaitForSeconds(0.15f);
            RollDie(directions.Dequeue());
            AudioManager.instance.PlayStepSound();
        }

    }
}

[System.Serializable]
public class DiceMovement
{
    Transform transform;
    [SerializeField] float _rollSpeed = 5;

    Dictionary<Directions, Vector3> _directions = new Dictionary<Directions, Vector3>();

    public DiceMovement()
    {
        _directions.Add(Directions.North, Vector3.forward);
        _directions.Add(Directions.South, Vector3.back);
        _directions.Add(Directions.East, Vector3.right);
        _directions.Add(Directions.West, Vector3.left);
    }

    public void SetTransform(Transform t)
    {
        transform = t;
    }

    public IEnumerator Roll(Directions dir, Action<bool> callBack)
    {
        callBack(false);
        var anchor = transform.position + (Vector3.down + _directions[dir]) * 0.5f;
        var axis = Vector3.Cross(Vector3.up, _directions[dir]);
        for (int i = 0; i < (90 / _rollSpeed); i++)
        {
            transform.RotateAround(anchor, axis, _rollSpeed);
            yield return new WaitForSeconds(0.01f);
        }
        callBack(true);
    }
}

