using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private GameObject _finish;
    [SerializeField] private GameObject _rope;
    [SerializeField] private BoxController _box;
    [SerializeField] private float _dep;
    [SerializeField] private float distanceBetweenPoints;
    [SerializeField] private float _speed = 1f;

    private Camera _camera;
    private List<Vector3> _pointsLine;
    private LineRenderer _lineRenderer;
    private int _currentPointIndex = -1;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _camera = Camera.main;
        _pointsLine = new List<Vector3> { _lineRenderer.GetPosition(0) };
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _dep);
            var drawingPoint = _camera.ScreenToWorldPoint(mousePosition);

            // Добавляем новые точки, если расстояние до последней точки больше заданного
            if (Vector3.Distance(_pointsLine[^1], drawingPoint) >= distanceBetweenPoints)
            {
                // Получаем последнюю добавленную точку
                var lastPoint = _pointsLine.Count > 0 ? _pointsLine[^1] : drawingPoint;

                // Вычисляем вектор направления от последней точки к текущей позиции мыши
                var direction = (drawingPoint - lastPoint).normalized;

                // Вычисляем расстояние между последней точкой и текущей позицией мыши
                var totalDistance = Vector3.Distance(lastPoint, drawingPoint);

                // Добавляем промежуточные точки
                for (var d = distanceBetweenPoints; d < totalDistance; d += distanceBetweenPoints)
                {
                    _pointsLine.Add(lastPoint + direction * d);
                }

                // Добавляем конечную точку, если она не совпадает с последней добавленной
                if (totalDistance >= distanceBetweenPoints)
                {
                    _pointsLine.Add(drawingPoint);
                }

                UpdateLineRenderer();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _currentPointIndex = 0;
        }

        if (_currentPointIndex < _pointsLine.Count && _currentPointIndex >= 0)
        {
            // Двигаем объект к текущей точке
            _rope.transform.position = Vector3.MoveTowards(_rope.transform.position, _pointsLine[_currentPointIndex],
                _speed * Time.deltaTime);
            // Проверка достижения текущей точки
            if (Vector3.Distance(_rope.transform.position, _pointsLine[_currentPointIndex]) < 0.1f)
            {
                _currentPointIndex++;
                if (_pointsLine[_currentPointIndex].x <= _finish.transform.position.x)
                {
                    _box.DropDown(_finish.transform.position);
                    _currentPointIndex = -1;
                }
            }
        }
    }

    private void UpdateLineRenderer()
    {
        _lineRenderer.positionCount = _pointsLine.Count;
        for (var i = 0; i < _pointsLine.Count; i++)
        {
            _lineRenderer.SetPosition(i, _pointsLine[i]);
        }
    }
}