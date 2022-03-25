using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity;
    public float zoomSensitivity;
    private Quaternion defaultCameraRotation;
    private Vector3 oldMousePosition;
    private Vector3 newMousePosition;
    private Vector3 lockedPosition;
    private bool isLocked;

    private float defaultFOV;
    // Start is called before the first frame update
    void Start()
    {
        defaultCameraRotation = transform.rotation;
        defaultFOV = gameObject.GetComponent<Camera>().fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            transform.rotation = defaultCameraRotation;
            gameObject.GetComponent<Camera>().fieldOfView = defaultFOV;
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            gameObject.GetComponent<Camera>().fieldOfView -= Input.mouseScrollDelta.y * zoomSensitivity;
        }
        
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            float xRotation = Input.GetAxis("Mouse X");
            float yRotation = -Input.GetAxis("Mouse Y");
            Quaternion newRotation =
                new Quaternion(transform.rotation.x + yRotation * sensitivity * Time.deltaTime,
                               transform.rotation.y + xRotation * sensitivity * Time.deltaTime,
                               transform.rotation.z * Time.deltaTime, transform.rotation.w);

            transform.rotation = newRotation;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
