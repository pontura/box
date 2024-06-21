using UnityEngine;

namespace Box
{
    
    public class Head : BodyPart
    {
        [SerializeField] Transform lookAtAsset;
        [SerializeField] Transform headAsset;

        private void Update()
        {
            Quaternion rotation = Quaternion.LookRotation(
                lookAtAsset.transform.position - transform.position,
                transform.TransformDirection(Vector3.up)
            );
            transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

        }
    }

}