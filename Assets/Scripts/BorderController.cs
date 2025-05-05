using UnityEngine;

[ExecuteAlways]
public class BorderController : MonoBehaviour
{
    [Header("Assign border GameObjects (must have BoxCollider2D)")]
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject floor;

    [Header("Playfield width % of screen width (0–1)")]
    [Range(0f, 1f)]
    [SerializeField] private float fieldWidthPercent = 0.75f;

    void OnValidate() => UpdateBorders();
    void Start() => UpdateBorders();
    void LateUpdate() { if (Application.isPlaying) UpdateBorders(); }

    private void UpdateBorders()
    {
        var cam = Camera.main;
        if (cam == null) return;

        // screen dims in world units
        float worldH = cam.orthographicSize * 2f;
        float worldW = worldH * cam.aspect;
        float fieldW = worldW * fieldWidthPercent;
        Vector3 camPos = cam.transform.position;

        // ?? FLOOR ??
        if (floor)
        {
            var bc = floor.GetComponent<BoxCollider2D>();
            if (bc)
            {
                // scale X so floor width = fieldW
                Vector3 ls = floor.transform.localScale;
                ls.x = fieldW / bc.size.x;
                floor.transform.localScale = ls;

                // position at bottom of camera view
                float halfH = (bc.size.y * ls.y) / 2f;
                float y = camPos.y - worldH / 2f + halfH;
                floor.transform.position = new Vector3(camPos.x, y, floor.transform.position.z);
            }
        }

        // ?? LEFT WALL ??
        if (leftWall)
        {
            var bc = leftWall.GetComponent<BoxCollider2D>();
            if (bc)
            {
                // scale Y to full screen height
                Vector3 ls = leftWall.transform.localScale;
                ls.y = worldH / bc.size.y;
                leftWall.transform.localScale = ls;

                // position at left edge of the playfield
                float halfW = (bc.size.x * ls.x) / 2f;
                float x = camPos.x - fieldW / 2f - halfW;
                leftWall.transform.position = new Vector3(x, camPos.y, leftWall.transform.position.z);
            }
        }

        // ?? RIGHT WALL ??
        if (rightWall)
        {
            var bc = rightWall.GetComponent<BoxCollider2D>();
            if (bc)
            {
                Vector3 ls = rightWall.transform.localScale;
                ls.y = worldH / bc.size.y;
                rightWall.transform.localScale = ls;

                float halfW = (bc.size.x * ls.x) / 2f;
                float x = camPos.x + fieldW / 2f + halfW;
                rightWall.transform.position = new Vector3(x, camPos.y, rightWall.transform.position.z);
            }
        }
    }
}
