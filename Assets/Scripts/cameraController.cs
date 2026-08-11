using UnityEngine;
using UnityEngine.UIElements;

public class cameraController : MonoBehaviour
{
    public GameObject player;
    private playerController playerController;
    private float nextY;
    public float horizontalShift = 3;
    public float verticalShift = 3;
    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        nextY = (playerController.jumpsRemaining != playerController.maxJumps || player.transform.position.y < playerController.lastGroundY?player.transform.position.y + verticalShift : transform.position.y);
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x +horizontalShift, nextY , transform.position.z);
    }
}
