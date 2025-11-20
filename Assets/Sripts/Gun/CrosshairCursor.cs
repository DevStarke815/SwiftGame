using UnityEngine;

public class CrosshairCursor : MonoBehaviour
{
    public Texture2D crosshairTexture;
    public Vector2 hotspot = new Vector2(16f, 16f); // center of a 32x32 texture

    void Start()
    {
        if (crosshairTexture != null)
        {
            Cursor.SetCursor(crosshairTexture, hotspot, CursorMode.Auto);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

