using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Header("Ссылки")]
    public Transform barrel;

    [Header("Движение")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 80f;

    [Header("Мышь (дополнительно)")]
    public float mouseSensitivity = 2f;
    public bool useMouseRotation = true;

    [Header("Наведение")]
    public float aimSpeed = 90f;

    [Header("Ограничения подъёма")]
    public float elevationMin = -5f;
    public float elevationMax = 75f;

    [Header("Режим")]
    public bool autoAim = true;

    private float currentElevation = 30f;
    private float targetYaw;
    private float targetPitch = 30f;

    void Start()
    {
        targetYaw = transform.eulerAngles.y;
    }

    void Update()
    {
        HandleMovement();

        if (autoAim)
            ApplyAimSmoothly();
        else
            HandleManualAim();

        // наклон ствола
        if (barrel != null)
            barrel.localRotation = Quaternion.Euler(-currentElevation, 0, 0);

    }

 
    void HandleMovement()
    {
        float move = 0f;

        if (Input.GetKey(KeyCode.W)) move = 1f;
        if (Input.GetKey(KeyCode.S)) move = -1f;

        transform.Translate(0f, 0f, move * moveSpeed * Time.deltaTime, Space.Self);

        float turn = 0f;
        if (Input.GetKey(KeyCode.A)) turn = -1f;
        if (Input.GetKey(KeyCode.D)) turn = 1f;

        float yawDelta = turn * rotateSpeed * Time.deltaTime;

        if (autoAim)
        {
            if (Mathf.Abs(turn) > 0.001f)
                targetYaw += yawDelta;
        }
        else
        {
            transform.Rotate(0f, yawDelta, 0f);
        }
    }

    void HandleManualAim()
    {
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0);

        if (Input.GetKey(KeyCode.E))
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

        if (Input.GetKey(KeyCode.R))
            currentElevation = Mathf.Clamp(currentElevation + aimSpeed * Time.deltaTime, elevationMin, elevationMax);

        if (Input.GetKey(KeyCode.F))
            currentElevation = Mathf.Clamp(currentElevation - aimSpeed * Time.deltaTime, elevationMin, elevationMax);
    }

    void ApplyAimSmoothly()
    {
        float yaw = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetYaw, aimSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, yaw, 0);

        currentElevation = Mathf.MoveTowards(
            currentElevation,
            Mathf.Clamp(targetPitch, elevationMin, elevationMax),
            aimSpeed * Time.deltaTime
        );
    }

    public void SetAimTarget(float yaw, float pitch)
    {
        targetYaw = yaw;
        targetPitch = pitch;
    }

    public bool IsAimedAt(float yaw, float pitch, float tolerance = 1.5f)
    {
        float dy = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, yaw));
        float dp = Mathf.Abs(currentElevation - Mathf.Clamp(pitch, elevationMin, elevationMax));
        return dy < tolerance && dp < tolerance;
    }
}