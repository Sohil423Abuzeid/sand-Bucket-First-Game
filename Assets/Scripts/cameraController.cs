using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject player;
    private playerController playerController;
    private float nextY;
    public float horizontalShift = 3;
    public float verticalShift = 3;

    void Start()
    {
        playerController = player.GetComponent<playerController>();
    }

    void Update()
    {

        nextY = player.transform.position.y+verticalShift;
        //nextY = (playerController.jumpsRemaining != playerController.maxJumps || player.transform.position.y < playerController.lastGroundY?player.transform.position.y + verticalShift : transform.position.y);
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x + horizontalShift, nextY, transform.position.z);
    }
}
