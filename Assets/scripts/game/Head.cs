using UnityEngine;

namespace Box
{    
    public class Head : BodyPart
    {
        [SerializeField] Transform lookAtAsset;
        [SerializeField] Transform headAsset;
        [SerializeField] Transform face;
        [SerializeField] Transform[] shoulders;
        [SerializeField] LineRenderer[] arms;

        float _y;
        float damping = 5;
        bool hitted;
        float hitForce;
        float hitAcceleration;
        bool hitAdd;
        Vector2 hitVector;
        float maxForce = 3;
        float maxAngleImpact = 90;
        float minAngleImpact = 20;
        float angleImpact;
        [SerializeField] float accel = 10;
        Quaternion q;

        void Start()
        {
            _y = transform.localEulerAngles.y;
            q = face.transform.rotation;
        }
        private void Update()
        {
            if (hitted)
                UpdateHit();
            else
                LookAtOther();
            UpdateArms();
        }
        void UpdateHit()
        {
            hitAcceleration -= Time.deltaTime * accel;
            face.transform.RotateAround(face.transform.position, hitVector, hitAcceleration);
            float AngleDiff = Quaternion.Angle(face.transform.rotation, q);
            if (AngleDiff < 4 || AngleDiff > maxAngleImpact)
                ResetHit();
        }
        public override void Hit(Vector3 pos)
        {
            face.transform.localEulerAngles = new Vector3(0, 0, -90);
            CancelInvoke();
            hitAcceleration = 0;
            hitted = true;
            Vector3 myPos = transform.position;
            myPos.z = 0;
            pos.z = 0;
            Vector3 distance = (pos - myPos).normalized;
            hitVector = Quaternion.AngleAxis(90, Vector3.forward) * distance;
            hitForce = Vector3.Distance(pos, myPos);
            if (hitForce > maxForce) hitForce = maxForce;
            angleImpact = Mathf.Lerp(minAngleImpact, maxAngleImpact, hitForce / maxForce);
            face.transform.RotateAround(face.transform.position, hitVector, angleImpact);
            Invoke("ResetHit", 0.5f);
        }
        void ResetHit()
        {
            CancelInvoke();
            face.transform.localEulerAngles = new Vector3(0, 0, -90);
            hitted = false;
        }
        void LookAtOther()
        {
            headAsset.transform.forward = lookAtAsset.position - transform.position;

            int eulerY = (int)headAsset.transform.localEulerAngles.y;
            if (eulerY < 0) eulerY += 360;
            if (characterID == 1 && (eulerY != 90 || headAsset.transform.localEulerAngles.z == -180))
            {
                Vector3 rot = headAsset.transform.localEulerAngles;
                rot.z = 180;
                rot.y = 90;
                headAsset.transform.localEulerAngles = rot;
            }
            else if (characterID == 2 && (eulerY != 270 || headAsset.transform.localEulerAngles.z == -180))
            {
                Vector3 rot = headAsset.transform.localEulerAngles;
                rot.z = 180;
                rot.y = -90;
                headAsset.transform.localEulerAngles = rot;
            }
        }
        void UpdateArms()
        {
            BodyPart[] all = GetAttachments();
            for (int a = 0; a < all.Length; a++)
            {
                arms[a].SetPositions(new Vector3[] { shoulders[a].transform.position, all[a].transform.position });
            }
        }
    }

}