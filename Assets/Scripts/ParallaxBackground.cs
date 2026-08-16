using UnityEngine;

/// <summary>
/// Attach to the backGround parent. Configures every child sprite as an endless
/// parallax layer that tracks the main camera (which follows the player).
/// </summary>
public class ParallaxBackground : MonoBehaviour
{
    [Tooltip("Usually the Main Camera. Leave empty to use Camera.main.")]
    public Transform followTarget;

    [Tooltip("Camera-follow amount for the farthest layer (high = stays on screen longer).")]
    [Range(0f, 1f)]
    public float farthestFactor = 0.9f;

    [Tooltip("Camera-follow amount for the closest layer (low = scrolls past faster).")]
    [Range(0f, 1f)]
    public float closestFactor = 0.2f;

    public bool infiniteHorizontal = true;
    public bool affectVertical = false;

    [Range(0f, 1f)]
    public float verticalParallaxFactor = 1f;

    [Min(1)]
    public int tilesEachSide = 1;

    private void Awake()
    {
        if (followTarget == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
                followTarget = mainCam.transform;
        }

        SetupLayers();
    }

    private void SetupLayers()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>(true);
        if (renderers.Length == 0)
            return;

        int minOrder = int.MaxValue;
        int maxOrder = int.MinValue;
        for (int i = 0; i < renderers.Length; i++)
        {
            int order = renderers[i].sortingOrder;
            if (order < minOrder) minOrder = order;
            if (order > maxOrder) maxOrder = order;
        }

        int orderRange = Mathf.Max(1, maxOrder - minOrder);

        for (int i = 0; i < renderers.Length; i++)
        {
            SpriteRenderer renderer = renderers[i];
            ParallaxLayer layer = renderer.GetComponent<ParallaxLayer>();
            if (layer == null)
                layer = renderer.gameObject.AddComponent<ParallaxLayer>();

            // Lower sorting order = farther back = sticks more to the camera.
            float t = (renderer.sortingOrder - minOrder) / (float)orderRange;
            float factor = Mathf.Lerp(farthestFactor, closestFactor, t);

            layer.tilesEachSide = tilesEachSide;
            layer.Initialize(followTarget, factor, infiniteHorizontal, affectVertical, verticalParallaxFactor);
        }
    }
}
