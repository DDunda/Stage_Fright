using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public static class Extension
{
    public static Rect FixSize(this Rect rect)
    {
        if (rect.width < 0)
        {
            rect.width = -rect.width;
            rect.x -= rect.width;
        }
        if (rect.height < 0)
        {
            rect.height = -rect.height;
            rect.y -= rect.height;
        }
        return rect;
    }
}

public class CameraPanner : MonoBehaviour
{
    [SerializeField]
    private Rect _visibleArea;
    public Camera targetCamera = null;

    public Rect localVisibleArea
    {
        get => _visibleArea;
        set
        {
            _visibleArea = value.FixSize();
            CalculateCameraArea();
        }
    }

    public Rect globalVisibleArea
    {
        get
        {
            Rect area = localVisibleArea;
            area.position += (Vector2)transform.position;
            return area;
        }
        set
        {
            value.position -= (Vector2)transform.position;
            localVisibleArea = value;
        }
    }

    private Rect _cameraArea;
    public Rect localCameraArea
    {
        get => _cameraArea;
        private set => _cameraArea = value.FixSize();
    }
    public Rect globalCameraArea
    {
        get {
            Rect area = _cameraArea;
            area.position += (Vector2)transform.position;
            return area;
        }
    }

    private void CalculateCameraArea()
    {
        localCameraArea = localVisibleArea;
        if (targetCamera == null) return;

        Vector2 halfsize = new Vector2(targetCamera.aspect, 1) * Mathf.Abs(targetCamera.orthographicSize);
        halfsize = Vector2.Min(halfsize, _cameraArea.size / 2);

        _cameraArea.position += halfsize;
        _cameraArea.size -= halfsize * 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        localVisibleArea = _visibleArea; // Will fix negative size and set camera area
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DrawRect(Rect r)
    {
        Vector2 min = r.min;
        Vector2 max = r.max;
        Gizmos.DrawLine(min, new(max.x, min.y));
        Gizmos.DrawLine(new(max.x, min.y), max);
        Gizmos.DrawLine(max, new(min.x, max.y));
        Gizmos.DrawLine(new(min.x, max.y), min);
    }

    void OnDrawGizmos()
    {
        CalculateCameraArea();

        Gizmos.color = UnityEngine.Color.green;
        DrawRect(globalVisibleArea);

        Gizmos.color = UnityEngine.Color.red;
        DrawRect(globalCameraArea);
    }
}
