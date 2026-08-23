using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject player;
    private float nextY;
    public float horizontalShift = 3;
    public float verticalShift = 3;

    void Start()
    {
    }

    void Update()
    {

        nextY = player.transform.position.y+verticalShift;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x + horizontalShift, nextY, transform.position.z);
    }
}
