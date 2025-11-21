using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HealthComponent HealthComponent { get; private set; }

    [Header("Stats")]
    private float _damage;
    private float _attackSpeed;
    private float _speed;

}
