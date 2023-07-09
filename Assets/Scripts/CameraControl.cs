using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;

    public PlayerMovement _P;

    public float MouseSensitivity;

    GameObject PlayerPivot;


     float X;
    float Y;


    [SerializeField]
    private Vector2 _rotationXMinMax ;

    public GameObject Cam;

    float xRot;
    private void Start()
    {
        transform.SetParent(null);

        SetupPoints();
    }
    void SetupPoints()
    {

        PlayerPivot = new GameObject("PlayerPivot");
        PlayerPivot.transform.position = transform.position;
        PlayerPivot.transform.SetParent(player.transform);



        Cursor.lockState = CursorLockMode.Locked;

    }
    private void FixedUpdate()
    {
        FollowPlayer();
        CameraRotations();
    }
    float Y_rot;
    void CameraRotations()
    {
        
        X = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        Y = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        

        xRot -= Y;

        Y_rot += X;

        xRot = Mathf.Clamp(xRot, _rotationXMinMax.x, _rotationXMinMax.y);

        Cam.transform.localRotation = Quaternion.Euler(xRot, 0, 0);


        transform.Rotate((transform.up * X));


    }
    void FollowPlayer()
    {
        transform.position = PlayerPivot.transform.position;
    }
}
