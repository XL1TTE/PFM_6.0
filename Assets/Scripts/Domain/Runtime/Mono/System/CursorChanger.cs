using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public Texture2D customCursor;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        if (hotspot == Vector2.zero && customCursor != null)
        {
            hotspot = new Vector2(customCursor.width * 0.1f, customCursor.height * 0.1f);
        }

        Cursor.SetCursor(customCursor, hotspot, cursorMode);
    }

    // Метод для ручной настройки в редакторе
#if UNITY_EDITOR
    void OnValidate()
    {
        if (customCursor != null && hotspot == Vector2.zero)
        {
            hotspot = new Vector2(customCursor.width * 0.1f, customCursor.height * 0.1f);
        }
    }
#endif
}