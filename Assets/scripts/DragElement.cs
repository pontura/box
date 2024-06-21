using UnityEngine;

namespace Box
{
    public class DragElement : MonoBehaviour
    {
        void Start()
        {

        }
        public void SetColor()
        {
            Color c;
            if (Settings.characterActive == 1)
                c = Color.red;
            else c = Color.blue;
            c.a = 0.5f;
            GetComponent<SpriteRenderer>().color = c;
        }

    }

}