using UnityEngine;
using UnityEngine.UI; // Required for Sliders

public class ManaBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxMana(float mana)
    {
        slider.maxValue = mana;
        slider.value = mana;
    }

    public void SetMana(float mana)
    {
        slider.value = mana;
    }
}