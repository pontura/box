using UnityEngine;
using UnityEngine.UI;

namespace Box.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] float speed = 1;
        [SerializeField] Types type; 
        enum Types{
            FILL,
            ANIMATED_FILL,
            WIDTH
        }
        [SerializeField] states state;
        enum states
        {
            IDLE,
            UPDATING
        }
        [SerializeField] Image image;
        [SerializeField] TMPro.TMP_Text percentField;
        System.Action OnFilled;
        float v;
        float newValue;
        [SerializeField] float widthValue = 270;

        public void SetColor(Color color)
        {
            percentField.color = color;
            image.color = color;
        }
        public void Init(float value, System.Action OnFilled)
        {
            this.OnFilled = OnFilled;
            SetValue(value);
        }
        private void OnDestroy()
        {
            if (OnFilled != null)
                OnFilled = null;
        }
        public void Add(float value)
        {
            v += value;
            SetValue(v);
        }      
        private void Update()
        {
            if (type == Types.ANIMATED_FILL)
            {
                if (state == states.IDLE) return;
                if (state == states.UPDATING)
                {
                    if (Mathf.Abs(newValue - v) < 0.01f)
                    {
                        state = states.IDLE;
                        v = newValue;
                    }
                    else
                    {
                        if (v < newValue)
                            v += speed / 10 * Time.deltaTime;
                        else v -= speed / 10 * Time.deltaTime;

                        UpdateValue();
                    }
                }
            }
        }
        void SetPercentField(float v)
        {
            if (percentField == null) return;
            percentField.text = (int)Mathf.Round(v * 100) + "%";
        }
        void SetFill(float v) {
            image.fillAmount = v;
        }
        void SetWidth(float v) {
            float var = (1-v) * widthValue;
            RectTransform r = image.gameObject.GetComponent<RectTransform>();
            if (var < 0) var = 0;
            r.offsetMax = new Vector2(- var, r.offsetMax.y);
        }
        public void SetValue(float value)
        {
            if (newValue > 1) newValue = 1;
            if (newValue < 0) newValue = 0;
            newValue = value;
            if (type == Types.FILL)
                SetFill(value);
            else
                state = states.UPDATING;
        }
        void UpdateValue()
        { 
            if (v >= 1)
            {
                SetValueForBar(1);
                if (OnFilled != null)
                {
                    OnFilled();
                    OnFilled = null;
                }
            }
            else
            {
                SetValueForBar(v);
            }
            SetPercentField(v);
        }
        void SetValueForBar(float v)
        {
            switch (type)
            {
                case Types.FILL:
                    SetFill(v); break;
                case Types.WIDTH:
                    SetWidth(v); break;
            }
            SetPercentField(v);
        }
        public void Reset()
        {
            newValue = 0;
            SetWidth(newValue);
            v = newValue;
            UpdateValue();
            state = states.IDLE;
        }
    }
}