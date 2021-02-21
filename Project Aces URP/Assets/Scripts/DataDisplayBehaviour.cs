using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DataDisplayBehaviour : MonoBehaviour
{
    [Header("Display")]
    public DecimalBehaviour decimalBehaviour;
    public FloatData floatData;
    public TextMeshProUGUI dataText;
    
    [Header("Measurement Type")]
    public MeasurementType measurementType;
    
    [Header("Preferred Units of Measurement")]
    public LengthTypes lengthTypes;
    public MassTypes massTypes;
    public TemperatureTypes temperatureTypes;
    public SpeedTypes speedTypes;
    public CurrencyTypes currencyTypes;

    private void Update()
    {
        dataText.text = floatData.value < 1f ? "0" : floatData.value.ToString("####.##");

        switch (decimalBehaviour)
        {
            case DecimalBehaviour.Integer:
                dataText.text = floatData.value.ToString("###");
                break;
            
            case DecimalBehaviour.DecimalTwo:
                dataText.text = floatData.value.ToString("###.##");
                break;
            
            case DecimalBehaviour.DecimalFour:
                dataText.text = floatData.value.ToString("###.####");
                break;
        }
        
        switch (measurementType)
        {
            case MeasurementType.None:
                break;
            case MeasurementType.Length:
                LengthDisplay();
                break;
            case MeasurementType.Mass:
                MassDisplay();
                break;
            case MeasurementType.Temperature:
                TemperatureDisplay();
                break;
            case MeasurementType.Speed:
                SpeedDisplay();
                break;
            case MeasurementType.Currency:
                CurrencyDisplay();
                break;
        }
    }

    private void LengthDisplay()
    {
        switch (lengthTypes)
        {
            case LengthTypes.Inches:
                dataText.text = dataText.text + "in";
                break;
            case LengthTypes.Feet:
                dataText.text = dataText.text + "ft";
                break;
            case LengthTypes.Yards:
                dataText.text = dataText.text + "yd";
                break;
            case LengthTypes.Miles:
                dataText.text = dataText.text + "mi";
                break;
            case LengthTypes.Centimeters:
                dataText.text = dataText.text + "cm";
                break;
            case LengthTypes.Meters:
                dataText.text = dataText.text + "m";
                break;
            case LengthTypes.Kilometers:
                dataText.text = dataText.text + "km";
                break;
        }
    }
    private void MassDisplay()
    {
        switch (massTypes)
        {
            case MassTypes.Ounces:
                dataText.text = dataText.text + "oz";
                break;
            case MassTypes.Pounds:
                dataText.text = dataText.text + "lbs";
                break;
            case MassTypes.Tons:
                dataText.text = dataText.text + "t";
                break;
            case MassTypes.Grams:
                dataText.text = dataText.text + "g";
                break;
            case MassTypes.Kilograms:
                dataText.text = dataText.text + "kg";
                break;
        }
    }
    private void TemperatureDisplay()
    {
        switch (temperatureTypes)
        {
            case TemperatureTypes.Fahrenheit:
                dataText.text = dataText.text + "°F";
                break;
            case TemperatureTypes.Celsius:
                dataText.text = dataText.text + "°C";
                break;
        }
    }
    private void SpeedDisplay()
    {
        switch (speedTypes)
        {
            case SpeedTypes.FeetPerSecond:
                dataText.text = dataText.text + "ft/s";
                break;
            case SpeedTypes.MilesPerHour:
                dataText.text = dataText.text + "mph";
                break;
            case SpeedTypes.Knots:
                dataText.text = dataText.text + "kt";
                break;
            case SpeedTypes.MetersPerSecond:
                dataText.text = dataText.text + "m/s";
                break;
            case SpeedTypes.KilometersPerHour:
                dataText.text = dataText.text + "km/h";
                break;
        }
    }
    private void CurrencyDisplay()
    {
        switch (currencyTypes)
        {
            case CurrencyTypes.USD:
                dataText.text = "$" + dataText.text;
                break;
            case CurrencyTypes.EUR:
                dataText.text = "€" + dataText.text;
                break;
            case CurrencyTypes.JPY:
                dataText.text = "¥" + dataText.text;
                break;
            case CurrencyTypes.GBP:
                dataText.text = "£" + dataText.text;
                break;
        }
    }
}

public enum DecimalBehaviour
{
    Integer, DecimalTwo, DecimalFour
};

public enum MeasurementType
{
    None, Length, Mass, Temperature, Speed, Currency
};

public enum LengthTypes
{
    Inches, Feet, Yards, Miles, Centimeters, Meters, Kilometers
};

public enum MassTypes
{
    Ounces, Pounds, Tons, Grams, Kilograms
};

public enum TemperatureTypes
{
    Fahrenheit, Celsius
};

public enum SpeedTypes
{
    FeetPerSecond, MilesPerHour, Knots, MetersPerSecond, KilometersPerHour
};

public enum CurrencyTypes
{
    USD, EUR, JPY, GBP
};