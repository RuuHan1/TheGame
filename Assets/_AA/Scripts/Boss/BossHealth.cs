using UnityEngine;
using UnityEngine.UI;

public class BossHealth 
{
    private float _maxHealth;
    private float _currentHealth;
    [SerializeField] private Image _healthBarFill;
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        float fillAmount = _currentHealth / _maxHealth;
        _healthBarFill.fillAmount = fillAmount;
    }
}
