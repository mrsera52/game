using UnityEngine;

public class ProjectileSelector : MonoBehaviour
{
    public static ProjectileSelector Instance;

    [Header("Доступные снаряды")]
    public ProjectileData[] available;

    [Header("Стартовый индекс")]
    public int currentIndex = 0;

    [Header("Клавиша переключения")]
    public KeyCode switchKey = KeyCode.F;

    public ProjectileData Current =>
        (available != null && available.Length > 0)
            ? available[Mathf.Clamp(currentIndex, 0, available.Length - 1)]
            : null;

    public System.Action<ProjectileData> OnChanged;

    void Awake() => Instance = this;

    void Start() => OnChanged?.Invoke(Current);

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            currentIndex = (currentIndex + 1) % available.Length;
            OnChanged?.Invoke(Current);
            Debug.Log($"Снаряд: {Current.displayName}");
        }
    }
}