using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float mouseSensitivity = 2.0f;

    private float verticalRotation = 0;
    private Camera playerCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        // Movement
        float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        float strafeSpeed = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        transform.Translate(strafeSpeed, 0, forwardSpeed);

        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation += mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);
        transform.Rotate(0, mouseX, 0);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
