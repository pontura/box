using UnityEngine;

namespace Box
{
    
    public class Head : BodyPart
    {
        [SerializeField] Transform lookAtAsset;
        [SerializeField] Transform headAsset;
        [SerializeField] LineRenderer[] arms;

        private void Update()
        {
            Quaternion rotation = Quaternion.LookRotation(
                lookAtAsset.transform.position - transform.position,
                transform.TransformDirection(Vector3.up)
            );
            transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);


            arms[0].SetPositions(new Vector3[] { transform.position, attachedTo[0].transform.position });
            arms[1].SetPositions(new Vector3[] { transform.position, attachedTo[1].transform.position });
        }
    }

}