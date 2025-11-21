using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class HealthComponent : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField, Min(0f)] private float _maxHealth = 100f;
    private float _currentHealth = 100f;
    public float _regeneration = 0.0f;
    public float regenTimer_;


    [Header("Events")]
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent OnDeath;

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;
    public bool IsDead => _currentHealth <= 0f;

    private void FixedUpdate()
    {
        regenTimer_ += Time.deltaTime;

        if (regenTimer_ > 1)
        {
            regenTimer_ = 0;
            if (_regeneration > 0)
                Heal(_regeneration);
        }
    }

    private void Awake()
    {
        _currentHealth = _maxHealth
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0f)
        {
            Debug.LogWarning($"{name}: Negative damage ignored.");
            return;
        }

        if (IsDead)
            return;

        _currentHealth = Mathf.Max(_currentHealth - damage, 0f);
        OnHealthChanged?.Invoke(_currentHealth);

        if (_currentHealth <= 0f)
            Die();
    }

    public void Heal(float amount)
    {
        if (amount < 0f)
        {
            Debug.LogWarning($"{name}: Negative heal ignored.");
            return;
        }

        if (IsDead)
            return;

        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        //OnHealthChanged?.Invoke(_currentHealth);
    }

    public void SetMaxHealth(float newMax)
    {
        if (newMax <= 0f)
        {
            Debug.LogWarning($"{name}: Max health must be positive.");
            return;
        }

        _currentHealth = Mathf.Clamp(_currentHealth, 0f, newMax);
        OnHealthChanged?.Invoke(_currentHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _maxHealth = Mathf.Max(0f, _maxHealth);
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);
    }
#endif
}
