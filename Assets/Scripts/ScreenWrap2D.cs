using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ScreenWrap2D : MonoBehaviour
{
    [Tooltip("Pequeño margen en unidades world antes de envolver")]
    public float padding = 0.05f;

    Camera mainCam;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Vector2 halfExtents; 

    float left, right, top, bottom;

    void Awake()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            halfExtents = spriteRenderer.bounds.extents;
        else
            halfExtents = Vector2.one * 0.5f;

        UpdateCameraBounds();
    }

    void OnValidate()
    {
        if (Application.isPlaying) return;
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        UpdateCameraBounds();
    }

    void FixedUpdate()
    {
        TryWrap();
    }

    void UpdateCameraBounds()
    {
        if (mainCam == null) return;

        if (!mainCam.orthographic)
        {
            Vector3 leftBottom = mainCam.ViewportToWorldPoint(new Vector3(0, 0, mainCam.nearClipPlane));
            Vector3 rightTop = mainCam.ViewportToWorldPoint(new Vector3(1, 1, mainCam.nearClipPlane));
            left = leftBottom.x;
            bottom = leftBottom.y;
            right = rightTop.x;
            top = rightTop.y;
            return;
        }

        float camHeight = mainCam.orthographicSize * 2f;
        float camWidth = camHeight * mainCam.aspect;

        Vector3 camPos = mainCam.transform.position;
        left = camPos.x - camWidth / 2f;
        right = camPos.x + camWidth / 2f;
        bottom = camPos.y - camHeight / 2f;
        top = camPos.y + camHeight / 2f;
    }

    void TryWrap()
    {
        if (mainCam == null || rb == null) return;

        Vector2 pos = rb.position;
        Vector2 oldVel = rb.velocity;
        bool wrapped = false;

        float rightThreshold = right + halfExtents.x + padding;
        float leftThreshold = left - halfExtents.x - padding;
        if (pos.x > rightThreshold)
        {
            pos.x = left - halfExtents.x - padding;
            wrapped = true;
        }
        else if (pos.x < leftThreshold)
        {
            pos.x = right + halfExtents.x + padding;
            wrapped = true;
        }

        float topThreshold = top + halfExtents.y + padding;
        float bottomThreshold = bottom - halfExtents.y - padding;
        if (pos.y > topThreshold)
        {
            pos.y = bottom - halfExtents.y - padding;
            wrapped = true;
        }
        else if (pos.y < bottomThreshold)
        {
            pos.y = top + halfExtents.y + padding;
            wrapped = true;
        }

        if (wrapped)
        {
            rb.position = pos;
            rb.velocity = oldVel;
        }
    }
}
