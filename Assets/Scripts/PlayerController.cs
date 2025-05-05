using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private SpeedManager speedManager;
    [SerializeField] private float dashDuration = 0.2f;

    private Rigidbody2D _rb;
    private Vector2 aimDir = Vector2.right;
    private bool isDashing;
    private float normalGrav;

    private Vector2 DashDist => speedManager != null
        ? speedManager.GetDashDistance()
        : Vector2.zero;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        normalGrav = _rb.gravityScale;
    }

    void Update()
    {
        if (isDashing) return;

        // aim at cursor/tap
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = wp - transform.position;
        if (dir.sqrMagnitude > 0.001f) aimDir = dir.normalized;

        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        isDashing = true;
        _rb.gravityScale = 0;
        _rb.linearVelocity = Vector2.zero;

        Vector2 start = _rb.position;
        Vector2 dashVec = new Vector2(aimDir.x * DashDist.x, aimDir.y * DashDist.y);
        Vector2 end = start + dashVec;
        float t = 0f;

        while (t < dashDuration)
        {
            _rb.MovePosition(Vector2.Lerp(start, end, t / dashDuration));
            t += Time.deltaTime;
            yield return null;
        }
        _rb.MovePosition(end);

        _rb.gravityScale = normalGrav;
        _rb.linearVelocity = Vector2.zero;
        isDashing = false;
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.cyan;
        Vector3 from = transform.position;
        Vector2 dashVec = new Vector2(aimDir.x * DashDist.x, aimDir.y * DashDist.y);
        Vector3 to = from + (Vector3)dashVec;
        Gizmos.DrawLine(from, to);
        Gizmos.DrawSphere(to, 0.1f);
    }
}

