using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color=gradient.Evaluate(1f);
    }
    public void SetCurrentHealth(float health)
    {
        slider.value = health;
        fill.color=gradient.Evaluate(slider.normalizedValue);
    }

}
