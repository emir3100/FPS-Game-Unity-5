using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuryMeter : MonoBehaviour
{
    public Slider slider;
    public int maxFury = 100;
    public int currentFury = 0;

    public Gradient gradient;
    public Image filler;

    void Start()
    {
        slider.value = 0;
    }

    void Update()
    {
        slider.maxValue = maxFury;
        slider.value = currentFury;
        filler.color = gradient.Evaluate(slider.normalizedValue);
    }
}
