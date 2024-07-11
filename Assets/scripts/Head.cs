using UnityEngine;

namespace Box
{
    
    public class Head : BodyPart
    {
        [SerializeField] Transform lookAtAsset;
        [SerializeField] Transform headAsset;
        [SerializeField] Transform[] shoulders;
        [SerializeField] LineRenderer[] arms;
        float _y;
        float damping = 5;
        void Start()
        {
            _y = transform.localEulerAngles.y;
        }
        private void Update()
        {
            headAsset.transform.forward = lookAtAsset.position - transform.position;

            int eulerY = (int)headAsset.transform.localEulerAngles.y;
            if (eulerY < 0) eulerY += 360;
            if (characterID == 1 && eulerY != 90)
            {
                Vector3 rot = headAsset.transform.localEulerAngles;
                rot.z = 180;
                headAsset.transform.localEulerAngles = rot;
            }
            else if (characterID == 2 && eulerY != 270)
            {
                Vector3 rot = headAsset.transform.localEulerAngles;
                rot.z = 180;
                headAsset.transform.localEulerAngles = rot;
            }

            arms[0].SetPositions(new Vector3[] { shoulders[0].transform.position, attachedTo[0].transform.position });
            arms[1].SetPositions(new Vector3[] { shoulders[1].transform.position, attachedTo[1].transform.position });
        }
    }

}