using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Movement Stats")]
public class PlayerMovementStats : ScriptableObject
{
    [Header("Walk & Run")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float groundAcceleration = 10f;
    public float groundDeceleration = 10f;
    public float airAcceleration = 5f;
    public float airDeceleration = 5f;

    [Header("Jump Heights & Timings")]
    public float jumpHeight = 6.5f;
    public float timeToJumpApex = 0.4f;
    public int allowedJumps = 1; // Set to 2 for double jump

    [Header("Jump Assists")]
    public float jumpBufferTime = 0.1f;
    public float coyoteTime = 0.1f;

    [Header("Apex Modifiers")]
    public float apexHangTime = 0.1f;
    public float apexThreshold = 1f; // Velocity range to apply hang time

    [Header("Falling & Jump Cut")]
    public float maxFallSpeed = 25f;
    public float fastFallTime = 0.1f;
    public float gravityMultiplierOnRelease = 2f;
    public float jumpCutCompensationFactor = 1.1f; // The adjustment factor mentioned in the video

    [Header("Collision Checks")]
    public LayerMask groundLayer;
    public float groundDetectionRayLength = 0.05f;
    public float headDetectionRayLength = 0.05f;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashTime = 0.15f;
    public float dashCooldown = 1.0f; 
    public int allowedDashes = 1;
    public float dashDiagonalBias = 0.4f; // The tweak mentioned at 23:53
    public float dashGravityOnReleaseMultiplier = 2f;
    public float timeForUpwardsCancel = 0.1f;
    public bool resetAirDashesOnWallSlide = true;


}
