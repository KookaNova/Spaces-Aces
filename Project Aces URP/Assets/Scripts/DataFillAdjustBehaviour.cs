using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataFillAdjustBehaviour : MonoBehaviour
{
    public FloatData fillData;
    public Image fillImage;

    public float maxValue;
    private float _currentValue;
    
    void Start()
    {
        fillImage = GetComponent<Image>();
    }

    void Update()
    {
        _currentValue = fillData.value / maxValue;
        fillImage.fillAmount = _currentValue;
    }
}
