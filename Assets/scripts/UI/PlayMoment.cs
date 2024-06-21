using UnityEngine;

namespace Box.UI
{
    public class PlayMoment : MonoBehaviour
    {
        [SerializeField] GameObject panel;

        public void Init()
        {
            panel.SetActive(true);
        }
        public void Reset()
        {
            panel.SetActive(false);
        }
    }
}