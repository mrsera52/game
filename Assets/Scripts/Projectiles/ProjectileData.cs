using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Game/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    [Header("Идентификация")]
    public string displayName = "Снаряд";

    [Header("Префаб")]
    public GameObject prefab;

    [Header("Баллистика")]
    public float muzzleSpeed = 60f;
    public float overshootMultiplier = 1f;

    [Header("Урон")]
    public float damageMultiplier = 1f;

    [Header("UI")]
    public Color iconColor = Color.white;
}