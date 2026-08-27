using System;
using Unity.VisualScripting;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject player;
    public float horizontalShift = 3;
    public float verticalShift = 3;
    public float cameraHorizontalSpeedIdel = 0.03f;
    public float cameraHorizontalSpeedDashingPrecent = 0.9f;
    public float cameraHorizontalSpeedReverceRunning = 0.1f;
    public float cameraHorizontalSpeedRunningMultible = 2;

    private playerController2 playerController;
    private Rigidbody2D rigidbody;
    private float nextY;
    private float nextX;
    private float targetx;
    private float xdiff;
    private float cameraCurrentSpeed;
    
    void Start()
    {
        playerController = player.GetComponent<playerController2>();
        rigidbody = player.GetComponent<Rigidbody2D>();
        targetx = player.transform.position.x + (horizontalShift * (playerController.isFacingRight ? 1 : -1));


        nextX = targetx;
        cameraCurrentSpeed = cameraHorizontalSpeedReverceRunning;
    }

    void Update()
    {
        nextY = player.transform.position.y+verticalShift;

        if (playerController.moveInput.x != 0 || playerController.isDashing)
        {
            Debug.Log(rigidbody.velocity.x);
            targetx = player.transform.position.x;
            xdiff = targetx - nextX;
            if ((xdiff / Mathf.Abs(xdiff)) == playerController.moveInput.x ||((xdiff / Mathf.Abs(xdiff)) == (playerController.isFacingRight ? 1 : -1) && playerController.isDashing))
            {
                cameraCurrentSpeed = MathF.Abs(rigidbody.velocity.x)*Time.deltaTime* cameraHorizontalSpeedRunningMultible;
            }
            else
                cameraCurrentSpeed =-1* cameraHorizontalSpeedReverceRunning;
        }
        else
        {
            targetx = player.transform.position.x + (horizontalShift * (playerController.isFacingRight ? 1 : -1));
            cameraCurrentSpeed = cameraHorizontalSpeedIdel;
        }

        xdiff = targetx - nextX;

        if (xdiff != 0&&cameraCurrentSpeed!= 0)
        {
            if (Mathf.Abs(xdiff) < cameraCurrentSpeed)
                nextX += xdiff;
            else
                nextX += cameraCurrentSpeed * (xdiff / Mathf.Abs(xdiff));
        }

    }

    private void LateUpdate()
    {
        transform.position = new Vector3(nextX, nextY, transform.position.z);
    }

    public void ResetPostion()
    {
        nextX = player.transform.position.x;
        nextY = player.transform.position.y;
    }
}
