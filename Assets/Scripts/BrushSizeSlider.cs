using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushSizeSlider : MonoBehaviour
{
    [SerializeField] private GameObject player;

    void Start()
    {
        Application.targetFrameRate = 120;
        var slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(UpdateBrushSize);
    }

    void UpdateBrushSize(float value)
    {
        player.GetComponent<IPlayerPainter>().changeBrushSize(value);
    }
}
