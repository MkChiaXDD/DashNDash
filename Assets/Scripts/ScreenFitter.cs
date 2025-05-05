using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class ScreenFitter : MonoBehaviour
{
    [Header("Enable Features")]
    public bool enableScaling = true;
    public bool enablePositioning = true;
    public bool followCamera = false;

    [Header("Scaling Settings")]
    [Range(0f, 1f)] public float widthPercent = 1f;
    [Range(0f, 1f)] public float heightPercent = 1f;
    public bool matchWidth = true;
    public bool matchHeight = false;

    [Header("Positioning Settings")]
    [Range(0f, 1f)] public float posXPercent = 0.5f;
    [Range(0f, 1f)] public float posYPercent = 0.5f;
    public Vector2 positionOffset = Vector2.zero;

    private Vector3 originalScale;
    private SpriteRenderer sr;

    void OnValidate()
    {
        // ensure refs are up-to-date in the editor
        sr = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        ApplyAll();
    }

    void Start()
    {
        // also apply once when play begins
        sr = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        ApplyAll();
    }

    void LateUpdate()
    {
        // if you want it to keep following the camera at runtime
        if (Application.isPlaying && followCamera && enablePositioning)
            ApplyPositioning();
    }

    private void ApplyAll()
    {
        if (sr == null) return;                   // no SpriteRenderer? bail
        if (enableScaling) ApplyScaling();
        if (enablePositioning) ApplyPositioning();
    }

    private void ApplyScaling()
    {
        var cam = Camera.main;
        if (cam == null) return;                  // no main camera? bail

        Vector3 newScale = originalScale;
        float worldH = cam.orthographicSize * 2f;
        float worldW = worldH * cam.aspect;

        if (matchWidth && sr.bounds.size.x > 0f)
        {
            float targetW = worldW * widthPercent;
            newScale.x = originalScale.x * (targetW / sr.bounds.size.x);
        }

        if (matchHeight && sr.bounds.size.y > 0f)
        {
            float targetH = worldH * heightPercent;
            newScale.y = originalScale.y * (targetH / sr.bounds.size.y);
        }

        transform.localScale = newScale;
    }

    private void ApplyPositioning()
    {
        var cam = Camera.main;
        if (cam == null) return;

        float worldH = cam.orthographicSize * 2f;
        float worldW = worldH * cam.aspect;

        float x = (posXPercent - 0.5f) * worldW + positionOffset.x;
        float y = (posYPercent - 0.5f) * worldH + positionOffset.y;

        Vector3 basePos = followCamera
            ? cam.transform.position
            : Vector3.zero;

        transform.position = new Vector3(
            basePos.x + x,
            basePos.y + y,
            transform.position.z
        );
    }
}
