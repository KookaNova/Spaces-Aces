using UnityEngine;
[CreateAssetMenu]
public class FloatData : ScriptableObject
{
   public float value;

   public void SetValue(float input)
   {
      value = input;
   }

   public void UpdateValue(float input)
   {
      value = value + input;
   }

   public void IncrementValue()
   {
      value++;
   }

   public void LowerValue()
   {
      value--;
   }
   
}
