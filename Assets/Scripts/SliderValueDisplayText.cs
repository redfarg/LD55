using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueDisplayText : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sizeText;

    void Start()
    {
        slider.onValueChanged.AddListener(UpdateBrushSizeText);

        UpdateBrushSizeText(slider.value);
    }

    void UpdateBrushSizeText(float value)
    {
        sizeText.text = "Chalk size: " + (value + 1).ToString();
    }

}
