using UnityEngine;

namespace Box.UI
{
    public class MainSignal : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        [SerializeField] TMPro.TMP_Text field;
        System.Action OnDone;

        public void Init(string text, System.Action OnDone)
        {
            field.text = text;
            panel.SetActive(true);
            Invoke("Done", 2);
            this.OnDone = OnDone;
        }

        void Done()
        {
            if (OnDone != null)
                OnDone();

            panel.SetActive(false);
        }
    }
}