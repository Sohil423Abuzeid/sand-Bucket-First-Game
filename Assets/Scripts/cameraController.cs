using System;
using Unity.VisualScripting;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject player;
    public float horizontalShift = 3;
    public float verticalShift = 3;
    public float cameraHorizontalSpeedIdel = 0.03f;
    public float cameraHorizontalSpeedRunning = 0.1f;

    private playerController2 playerController;
    private Rigidbody2D rigidbody;
    private float nextY;
    private float nextx;
    private float targetx;
    private float xdiff;
    private float cameraCurrentSpeed;
    void Start()
    {
        playerController = player.GetComponent<playerController2>();
        rigidbody = player.GetComponent<Rigidbody2D>();
        targetx = player.transform.position.x + (horizontalShift * (playerController.isFacingRight ? 1 : -1));


        nextx = targetx;
        cameraCurrentSpeed = cameraHorizontalSpeedRunning;
    }

    void Update()
    {
        nextY = player.transform.position.y+verticalShift;

        if (playerController.moveInput.x != 0 || playerController.isDashing)
        {
            targetx = player.transform.position.x;
            cameraCurrentSpeed = MathF.Abs(rigidbody.velocity.x) *cameraHorizontalSpeedRunning ;
        }
        else
        {
            targetx = player.transform.position.x + (horizontalShift * (playerController.isFacingRight ? 1 : -1));
            cameraCurrentSpeed = cameraHorizontalSpeedRunning;
        }
        xdiff = targetx - nextx;
        if (xdiff != 0)
        {
            if (Mathf.Abs(xdiff) < cameraCurrentSpeed)
                nextx += xdiff;
            else
                nextx += cameraCurrentSpeed * (xdiff / Mathf.Abs(xdiff));
        }

    }

    private void LateUpdate()
    {
        transform.position = new Vector3(nextx, nextY, transform.position.z);
    }
}
