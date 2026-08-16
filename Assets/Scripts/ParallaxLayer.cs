using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves a background sprite relative to the camera for depth, and tiles copies
/// left/right so the layer never runs out as the player travels.
/// Runs after the camera to avoid one-frame lag jitter.
/// </summary>
[DefaultExecutionOrder(100)]
public class ParallaxLayer : MonoBehaviour
{
    [Tooltip("0 = fixed in world (scrolls fast on screen). 1 = locked to camera (stays still on screen).")]
    [Range(0f, 1f)]
    public float parallaxFactor = 0.5f;

    [Tooltip("How much vertical camera movement this layer should follow.")]
    [Range(0f, 1f)]
    public float verticalParallaxFactor = 1f;

    public bool affectVertical = false;
    public bool infiniteHorizontal = true;

    [Tooltip("Extra copies on each side of the original sprite.")]
    [Min(1)]
    public int tilesEachSide = 1;

    private Transform followTarget;
    private float cameraStartX;
    private float cameraStartY;
    private float layerStartX;
    private float layerStartY;
    private float tileWidth;
    private float wrapDistance;
    private float totalWidth;
    private bool ready;
    private readonly List<Transform> tiles = new List<Transform>();
    private readonly List<float> tileOffsetsX = new List<float>();
    private readonly List<float> tileZ = new List<float>();

    public void Initialize(Transform target, float factor, bool infinite, bool vertical, float verticalFactor)
    {
        followTarget = target;
        parallaxFactor = factor;
        infiniteHorizontal = infinite;
        affectVertical = vertical;
        verticalParallaxFactor = verticalFactor;
    }

    private void Start()
    {
        if (followTarget == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
                followTarget = mainCam.transform;
        }

        if (followTarget == null)
        {
            enabled = false;
            return;
        }

        layerStartX = transform.position.x;
        layerStartY = transform.position.y;
        cameraStartX = followTarget.position.x;
        cameraStartY = followTarget.position.y;

        SetupTiles();
        ready = true;
    }

    private void SetupTiles()
    {
        SpriteRenderer sourceRenderer = GetComponent<SpriteRenderer>();
        if (sourceRenderer == null || sourceRenderer.sprite == null)
        {
            tiles.Add(transform);
            tileOffsetsX.Add(0f);
            tileZ.Add(transform.position.z);
            return;
        }

        // World width including parent scale — keep exact so tiles don't overlap/gap.
        tileWidth = sourceRenderer.bounds.size.x;
        totalWidth = tileWidth * (1 + tilesEachSide * 2);
        wrapDistance = totalWidth * 0.5f;

        tiles.Add(transform);
        tileOffsetsX.Add(0f);
        tileZ.Add(transform.position.z);

        if (!infiniteHorizontal || tileWidth <= 0.001f)
            return;

        for (int i = 1; i <= tilesEachSide; i++)
        {
            float right = tileWidth * i;
            float left = -tileWidth * i;

            tiles.Add(CreateTileCopy(sourceRenderer, transform.position + Vector3.right * right));
            tileOffsetsX.Add(right);
            tileZ.Add(transform.position.z);

            tiles.Add(CreateTileCopy(sourceRenderer, transform.position + Vector3.right * left));
            tileOffsetsX.Add(left);
            tileZ.Add(transform.position.z);
        }
    }

    private Transform CreateTileCopy(SpriteRenderer source, Vector3 worldPosition)
    {
        GameObject copy = new GameObject(name + "_tile");
        copy.transform.SetParent(transform.parent, false);
        copy.transform.SetPositionAndRotation(worldPosition, transform.rotation);
        copy.transform.localScale = transform.localScale;

        SpriteRenderer rendererCopy = copy.AddComponent<SpriteRenderer>();
        rendererCopy.sprite = source.sprite;
        rendererCopy.color = source.color;
        rendererCopy.flipX = source.flipX;
        rendererCopy.flipY = source.flipY;
        rendererCopy.sortingLayerID = source.sortingLayerID;
        rendererCopy.sortingOrder = source.sortingOrder;
        rendererCopy.sharedMaterial = source.sharedMaterial;
        rendererCopy.drawMode = source.drawMode;
        rendererCopy.size = source.size;

        return copy.transform;
    }

    private void LateUpdate()
    {
        if (!ready || followTarget == null)
            return;

        float cameraX = followTarget.position.x;
        float cameraY = followTarget.position.y;

        // Absolute position from camera — no per-frame delta accumulation (avoids drift/jitter).
        float originX = layerStartX + (cameraX - cameraStartX) * parallaxFactor;
        float originY = affectVertical
            ? layerStartY + (cameraY - cameraStartY) * verticalParallaxFactor
            : layerStartY;

        if (infiniteHorizontal && tileWidth > 0.001f)
        {
            for (int i = 0; i < tileOffsetsX.Count; i++)
            {
                float tileX = originX + tileOffsetsX[i];
                float offsetFromCamera = cameraX - tileX;

                // Use half the strip width so tiles never wrap back-and-forth at rest.
                if (offsetFromCamera > wrapDistance)
                    tileOffsetsX[i] += totalWidth;
                else if (offsetFromCamera < -wrapDistance)
                    tileOffsetsX[i] -= totalWidth;
            }
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].position = new Vector3(originX + tileOffsetsX[i], originY, tileZ[i]);
        }
    }
}
