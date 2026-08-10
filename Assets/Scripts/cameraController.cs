using UnityEngine;
using UnityEngine.UIElements;

public class cameraController : MonoBehaviour
{
    public GameObject player;
    private playerController playerController;
    private float nextY;
    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        nextY = (playerController.jumpsRemaining != playerController.maxJumps || player.transform.position.y < playerController.lastGroundY?player.transform.position.y:transform.position.y);
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x + 3, nextY, transform.position.z);
    }
}
