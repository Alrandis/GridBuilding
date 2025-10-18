using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private int _range = 10;
    [SerializeField] private GameObject _linePrefab; // пустой объект с LineRenderer

    private void Start()
    {
        DrawGrid();
    }

    private void DrawGrid()
    {
        float size = _gridManager.CellSize;

        // Вертикальные линии
        for (int x = -_range; x <= _range; x++)
        {
            GameObject line = Instantiate(_linePrefab, transform);
            LineRenderer lr = line.GetComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, new Vector3(x * size, -_range * size, 0));
            lr.SetPosition(1, new Vector3(x * size, _range * size, 0));
        }

        // Горизонтальные линии
        for (int y = -_range; y <= _range; y++)
        {
            GameObject line = Instantiate(_linePrefab, transform);
            LineRenderer lr = line.GetComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, new Vector3(-_range * size, y * size, 0));
            lr.SetPosition(1, new Vector3(_range * size, y * size, 0));
        }
    }
}
