using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float distance = 12f;
    public float height = 5f;

    public float mouseSensitivity = 200f;
    public float smooth = 5f;

    private float yaw = 0f;
    private float pitch = 20f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (target == null) return;

        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, 5f, 60f);

        
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        Vector3 desiredPosition = target.position + offset + Vector3.up * height;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smooth);

        
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
