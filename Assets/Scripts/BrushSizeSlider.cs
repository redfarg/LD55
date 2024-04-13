using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushSizeSlider : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Slider slider; // Reference to the Slider UI element

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
        slider.onValueChanged.AddListener(UpdateBrushSize);
    }

    // Update the brush size in the paint script when the slider value changes
    void UpdateBrushSize(float value)
    {
        player.GetComponent<IPlayerPainter>().changeBrushSize(value);
    }
}
