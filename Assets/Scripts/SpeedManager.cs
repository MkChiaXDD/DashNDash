using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float dashPercentX = 0.1f; // % of screen width
    [Range(0f, 1f)]
    public float dashPercentY = 0.2f; // % of screen height

    public Vector2 GetDashDistance()
    {
        float worldW = ScreenSizeHelper.PercentWidthToWorld(dashPercentX);
        float worldH = ScreenSizeHelper.PercentHeightToWorld(dashPercentY);
        return new Vector2(worldW, worldH);
    }
}
