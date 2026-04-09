using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    private float _maxHealth;
    private float _currentHealth;
    [SerializeField] private Transform _maskTransform;
    public void Initialize(float maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
        //_maskTransform = maskTransform;
        _maskTransform.localScale = new Vector3(1, 1f, 1f);
    }
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        float fillAmount = _currentHealth / _maxHealth;
        _maskTransform.localScale = new Vector3(fillAmount, 1f, 1f);
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                GameEvents.BossDefeated_Boss?.Invoke();
            // Boss öldüðünde yapýlacak iþlemler
        }
    }
}
