using UnityEngine;

namespace Box
{
    public class DragElement : MonoBehaviour
    {
        [SerializeField] GameObject asset;
        [SerializeField] TrailRenderer trail;
        private void Start()
        {
            Reset();
        }
        public void Init(Vector2 pos)
        {
            print("Init");
            asset.SetActive(true);
            SetPos(pos);
            trail.enabled = true;
            trail.time = 0.2f;
        }
        public void Reset()
        {
            trail.time = 0f;
            print("Reset");
            asset.SetActive(false);
            trail.enabled = false;
        }
        public void SetPos(Vector2 pos)
        {
            transform.position = pos;
        }
        public void SetColor(Color c)
        {
            //asset.GetComponent<MeshRenderer>().color = c;            
        }

    }

}