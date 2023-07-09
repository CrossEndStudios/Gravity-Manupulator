using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private bool isGrounded = false;

    private Vector3 movement = Vector3.zero;

    public MovementDirection dir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Apply movement force
        rb.AddForce(movement * moveSpeed);

        // Check if the character is grounded
        isGrounded = Physics.Raycast(transform.position, -transform.up, 1f);

        // Handle jump input
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // Apply jump force based on current gravity
            Vector3 jumpDirection = -Physics.gravity.normalized;
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
        }
    }

    // Change the gravity based on input
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            RotateGravityClockwise();
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            RotateGravityCounterClockwise();
        }

        // Get input for movement based on current gravity
        switch (GetMovementDirection())
        {
            case MovementDirection.Forward:
                movement = new Vector3(moveSpeed, 0f, 0f);
                dir = MovementDirection.Right;
                break;
                case MovementDirection.Backward:
                dir = MovementDirection.Left;
                movement = new Vector3(-moveSpeed, 0f, 0f);
                break;
            case MovementDirection.Left:
                dir = MovementDirection.Forward;
                movement = new Vector3(0f, 0f, moveSpeed);
                break;
            case MovementDirection.Right:
                dir = MovementDirection.Backward;
                movement = new Vector3(0f, 0f, -moveSpeed);
                break;
            default:
                movement = Vector3.zero;
                break;
        }
    }

    // Rotate the player and gravity 90 degrees clockwise
    void RotateGravityClockwise()
    {
        transform.Rotate(Vector3.up, 90f);
        Physics.gravity = Quaternion.Euler(0f, 90f, 0f) * Physics.gravity;
    }

    // Rotate the player and gravity 90 degrees counter-clockwise
    void RotateGravityCounterClockwise()
    {
        transform.Rotate(Vector3.up, -90f);
        Physics.gravity = Quaternion.Euler(0f, -90f, 0f) * Physics.gravity;
    }

    // Get the movement direction based on the current player rotation
    MovementDirection GetMovementDirection()
    {
        Vector3 forwardDir = transform.forward;
        Vector3 rightDir = transform.right;

        float forwardDot = Vector3.Dot(movement, forwardDir);
        float rightDot = Vector3.Dot(movement, rightDir);

        if (forwardDot > 0.5f)
        {
            return MovementDirection.Forward;
        }
        else if (forwardDot < -0.5f)
        {
            return MovementDirection.Backward;
        }
        else if (rightDot > 0.5f)
        {
            return MovementDirection.Right;
        }
        else if (rightDot < -0.5f)
        {
            return MovementDirection.Left;
        }
        else
        {
            return MovementDirection.None;
        }
    }

    // Enumeration to represent movement direction
    public enum MovementDirection
    {
        None,
        Forward,
        Backward,
        Left,
        Right
    }
}
