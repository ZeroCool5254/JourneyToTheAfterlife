using Core;
using ScriptableObjects;
using ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField, Header("Events")] private UpdateHealthEvent _healthChangedEvent;
        [SerializeField] private UpdateManaEvent _manaChangedEvent;

        [SerializeField, Header("Health Bars")] private Image[] _hearts;

        [SerializeField, Header("Mana Slider")] private Slider _manaSlider;

        private void OnEnable()
        {
            _healthChangedEvent.HealthChangedEvent.AddListener(UpdateHealth);
            _manaChangedEvent.ManaChangedEvent.AddListener(UpdateMana);
        }

        private void OnDisable()
        {
            _healthChangedEvent.HealthChangedEvent.RemoveListener(UpdateHealth);
            _manaChangedEvent.ManaChangedEvent.RemoveListener(UpdateMana);
        }

        private void UpdateHealth(int livesRemaining)
        {
            for (int i = 0; i <= livesRemaining; i++)
            {
                if (i < livesRemaining)
                {
                    _hearts[i].enabled = false;
                }
                else if (i < _hearts.Length)
                {
                    _hearts[i].enabled = true;
                }
            }
        }

        private void UpdateMana(float mana)
        {
            _manaSlider.value = mana;
        }
    }
}
