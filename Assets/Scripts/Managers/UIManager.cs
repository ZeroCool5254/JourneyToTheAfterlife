using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private HealthManagerSO _healthManager;
        [SerializeField] private ManaManagerSO _manaManager;

        [SerializeField, Header("Health Bars")] private Image[] _hearts;

        [SerializeField, Header("Mana Slider")] private Slider _manaSlider;

        private void OnEnable()
        {
            _healthManager.HealthChangedEvent.AddListener(UpdateHealth);
            _manaManager.ManaChangedEvent.AddListener(UpdateMana);
        }

        private void OnDisable()
        {
            _healthManager.HealthChangedEvent.RemoveListener(UpdateHealth);
            _manaManager.ManaChangedEvent.RemoveListener(UpdateMana);
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
