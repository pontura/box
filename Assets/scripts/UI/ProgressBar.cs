using UnityEngine;
using UnityEngine.UI;

namespace Box.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] TMPro.TMP_Text percentField;
        public float value;
        System.Action OnReady;

        public void SetColor(Color color)
        {
            percentField.color = color;
            image.color = color;
        }
        public void Init(float _value, System.Action OnReady)
        {
            this.OnReady = OnReady;
            value = _value;
            SetValue(value);
        }
        public void Add(float _value)
        {
            value += _value;
            if (value < 0) value = 0; else if (value > 1) value = 1;
            SetValue(value);           
        }      
        public void SetValue(float _value)
        {
            this.value = _value;
            image.fillAmount = _value;
            if (value <= 0 && OnReady != null)
            {
                OnReady();
                OnReady = null;
            }
        }
    }
}