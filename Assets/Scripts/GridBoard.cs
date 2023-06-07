using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBoard : MonoBehaviour
{
    [SerializeField] GameObject _cubePrefab;
    [SerializeField] int _size = 10;
    Coordinate[,] _grid;

    public Coordinate[,] Grid { get { return _grid; } }

    private void Start() => GenerateGrid();
    
    void GenerateGrid()
    {
        _grid = new Coordinate[_size, _size];
        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                _grid[x, y] = new Coordinate(x, y);
                var cube = Instantiate(_cubePrefab);
                cube.transform.position = new Vector3(x, 0, y) + Vector3.down;
                cube.transform.SetParent(transform);
                cube.GetComponent<Renderer>().material.color = (x + y) % 2 == 0 ? Color.black : Color.white;
            }
        }
    }

    public bool MoveAvailable(Coordinate pos)
    {
        if (pos.x >= 0 && pos.x < _size && pos.y >= 0 && pos.y < _size) return true;
        else return false;
    }
}
