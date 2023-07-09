using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 10f;
    public float jumpHeight = 5f;
    public Vector3 customGravity;
    public float Gravity;

    public Transform cameraTransform;


    public bool isGrounded;
    public Transform GroundCheck;
    public float groundDistance;
    public LayerMask groundMask;

    public CharacterController CC;


    float horizontal;
    float vertical;

    public Animator playerAnim;
    float AnimSwitchVelo;
    int AnimHash;
    public float AnimSwitchAccel;

    public GameObject Hologram;

    public enum playerstate
    {
        Up,
        Down,
        Forward,
        Backward,
        Right,
        Left,
        None
    }
    public playerstate GravityState;
    public playerstate HoloGravityState;
    public Vector3 direction = Vector3.zero;

    public Quaternion H_Rotation;

    public Transform Body;
    private void Start()
    {
        AnimHash = Animator.StringToHash("Controller");

        //ChangeGravity(playerstate.Backward, -transform.forward) ;

        Hologram.SetActive(false);
    }

    Vector3 HologramRightAngle(float playerYRot)
    {
        Vector3 rot = Vector3.zero;

        if(playerYRot >=0 && playerYRot <= 90)
        {
            rot = Vector3.right;
            return rot;
        }
        else if (playerYRot > 90 && playerYRot <= 180)
        {
            rot = Vector3.back;
            return rot;
        }
        else if (playerYRot > 180 && playerYRot <= 270)
        {
            rot = Vector3.left;
            return rot;
        }
        else if (playerYRot > 270 && playerYRot <= 360)
        {
            rot = Vector3.forward;
            return rot;
        }

        return rot;
    }
    Vector3 HologramForwardAngle(float playerYRot)
    {
        Vector3 rot = Vector3.zero;

        if ((playerYRot >= 315f && playerYRot <= 360f) || (playerYRot >= 0f && playerYRot <= 45f))
        {
            rot = Vector3.forward;
        }
        else if (playerYRot > 45f && playerYRot <= 135f)
        {
            rot = Vector3.right;
        }
        else if (playerYRot > 135f && playerYRot <= 225f)
        {
            rot = Vector3.back;
        }
        else if (playerYRot > 225f && playerYRot <= 315f)
        {
            rot = Vector3.left;
        }

        return rot;
    }

    void ShowDirections(playerstate state)
    {
        GravityState = playerstate.None;
        Hologram.SetActive(true);
         direction = Vector3.zero;

        Quaternion playerRot = cameraTransform.rotation;

        if (state == playerstate.Right)
        {
            direction = HologramRightAngle(playerRot.eulerAngles.y);
        }
        else if (state == playerstate.Left)
        {
            direction = -HologramRightAngle(playerRot.eulerAngles.y);
        }
        else if (state == playerstate.Forward)
        {
            direction = HologramForwardAngle(playerRot.eulerAngles.y);
        }
        else if (state == playerstate.Backward)
        {
            direction = -HologramForwardAngle(playerRot.eulerAngles.y);
        }
        else if (state == playerstate.Up)
        {
            // direction = transform.up;
            direction = ((transform.position.normalized) + (Vector3.up.normalized));
        }
        else if (state == playerstate.Down)
        {
            //direction = -transform.up;
            direction = ((transform.position.normalized) - (Vector3.up.normalized));
        }


        Hologram.transform.up = -direction;

        H_Rotation = Hologram.transform.rotation;

    }
    void ChangeGravity(playerstate state,Vector3 dir)
    {
        Hologram.SetActive(false);

        GravityState = state;

        customGravity = new Vector3(0, 0, 0);
        customGravity = dir;
        UpdateRotation(customGravity);

        /* if (state == playerstate.Right)
         {

         }
         else if (state == playerstate.Left)
         {
             GravityState = playerstate.Left;

             customGravity = new Vector3(0, 0, 0);
             customGravity = -dir;
             UpdateRotation(customGravity);
         }
         else if (state == playerstate.Forward)
         {
             GravityState = playerstate.Forward;

             customGravity = new Vector3(0, 0, 0);
             customGravity = -dir;
             UpdateRotation(customGravity);
         }
         else if (state == playerstate.Backward)
         {
             GravityState = playerstate.Backward;

             customGravity = new Vector3(0, 0, 0);
             customGravity = -dir;
             UpdateRotation(customGravity);
         }*/

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HoloGravityState = playerstate.Right;
            ShowDirections(HoloGravityState);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            HoloGravityState = playerstate.Left;
            ShowDirections(HoloGravityState);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            HoloGravityState = playerstate.Forward;
            ShowDirections(HoloGravityState);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            HoloGravityState = playerstate.Backward ;
            ShowDirections(HoloGravityState);
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ChangeGravity(HoloGravityState, direction);
        }

        //jump
        //jumpVec = mathf.sqrt(jumpheight * -2 * gravity);

        if (isGrounded)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }



        isGrounded = Physics.CheckSphere(GroundCheck.position, groundDistance, groundMask);


        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            /* if (GravityState == playerstate.Forward)
             {
                 customGravity.z = Mathf.Sqrt(jumpHeight * -2 * Gravity);

                 //torque = new Vector3(-movementRelative.x, 0, movementRelative.z);
             }
             else if (GravityState == playerstate.Backward)
             {
                 customGravity.z = Mathf.Sqrt(jumpHeight * 2 * Gravity);

                 //torque = new Vector3(movementRelative.x, 0, movementRelative.z);
             }
             else if (GravityState == playerstate.Right)
             {
                 customGravity.x = Mathf.Sqrt(jumpHeight * -2 * Gravity);

                 //torque = new Vector3(0, movementRelative.x, movementRelative.z);
             }
             else if (GravityState == playerstate.Left)
             {
                 customGravity.x = Mathf.Sqrt(jumpHeight * 2 * Gravity);

                 //torque = new Vector3(0, -movementRelative.x, movementRelative.z);
             }*/


            //customGravity = direction.normalized * (Mathf.Sqrt(jumpHeight * -2 * Gravity));


            isGrounded = false;
        }
    }
    private void LateUpdate()
    {
        Hologram.transform.rotation = H_Rotation;
    }
    private void UpdateRotation(Vector3 rot)
    {
        transform.up = -rot.normalized;
        cameraTransform.up = -rot.normalized;
    }
    Vector3 torque;
    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(horizontal, 0f, vertical) * moveSpeed;

        if(movement.magnitude != 0 && AnimSwitchVelo <1)
        {
            AnimSwitchVelo += AnimSwitchAccel * Time.deltaTime;
        }
        else if(movement.magnitude == 0 && (AnimSwitchVelo > 0))
        {
            AnimSwitchVelo -= AnimSwitchAccel * Time.deltaTime;
        }

        if(AnimSwitchVelo < 0)
        {
            AnimSwitchVelo = 0;
        }

        playerAnim.SetFloat(AnimHash, AnimSwitchVelo);



        Quaternion camRotation = Quaternion.Euler(0f, cameraTransform.localEulerAngles.y, 0f);
        Vector3 movementRelative = camRotation * movement;


        /*if (movementRelative != Vector3.zero)
        {
            Quaternion desiredRot = Quaternion.LookRotation(movementRelative, transform.up);

           Body.transform.localRotation = Quaternion.Slerp(Body.transform.localRotation, desiredRot, rotateSpeed * Time.deltaTime);
        }*/
        

        torque = cameraTransform.forward * movement.z + cameraTransform.right * movement.x;

       /* if (GravityState == playerstate.Forward)
        {
             torque = new Vector3(-movementRelative.x, movementRelative.z, 0);
        }
        else if (GravityState == playerstate.Backward)
        {
             torque = new Vector3(movementRelative.x, -movementRelative.z, 0);
        }
        else if (GravityState == playerstate.Right)
        {
             torque = new Vector3(0, movementRelative.x,movementRelative.z);
        }
        else if (GravityState == playerstate.Left)
        {
             torque = new Vector3(0, -movementRelative.x, movementRelative.z);
        }*/

        CC.Move(torque * Time.deltaTime);

        //Body.transform.forward = movement;




        if (isGrounded)
        {
            if (GravityState == playerstate.Forward)
            {
                if (customGravity.z > 0)
                {
                    customGravity.z = 2;
                }
            }
            else if (GravityState == playerstate.Backward)
            {
                if (customGravity.z < 0)
                {
                    customGravity.z = -2;
                }
                
            }
            else if (GravityState == playerstate.Right)
            {
                if (customGravity.x > 0)
                {
                    customGravity.x = 2;
                }
                
            }
            else if (GravityState == playerstate.Left)
            {
                if (customGravity.x < 0)
                {
                    customGravity.x = -2;
                }
                
            }
        }

        if(GravityState != playerstate.None)
        {
            customGravity += ((direction) * Gravity) * Time.deltaTime;
        }


        CC.Move(customGravity * Time.deltaTime );
        
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(GroundCheck.position, groundDistance);
    }
}
