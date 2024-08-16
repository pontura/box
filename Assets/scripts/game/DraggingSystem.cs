using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
namespace Box
{
    public class DraggingSystem
    {
        float distanceToEffort;
        float totalDistanceToEffort, effortByDistance;
        float offsetToDraw,  offsetTime, maxDistanceFromAnchor, maxDistanceAllowed;

        Camera cam;
        Vector2 lastPos;
        Vector2 offset;
        states state;
        enum states
        {
            NONE,
            DRAGGING,
            DONE
        }
        BodyPart bodyPart;
        DBManager dbManager;

        float timer;
        float lastMovementTimer;
        float timerToDraw;

        List<BodyPart.types> draggedPieces;

        public void Init(DBManager dbManager)
        {
            Debug.Log("Init DraggingSystem");
            this.dbManager = dbManager;

            this.totalDistanceToEffort = Settings.totalDistanceToEffort;
            this.offsetToDraw = Settings.offsetToDraw;
            this.offsetTime = Settings.offsetTime;
            this.maxDistanceFromAnchor = Settings.maxDistanceFromAnchor;
            this.maxDistanceAllowed = Settings.maxDistanceAllowed;
            this.effortByDistance = Settings.effortByDistance;
            maxDistanceFromAnchor = 0.25f;

            distanceToEffort = 0;
            timer = timerToDraw = lastMovementTimer = 0;
            draggedPieces = new List<BodyPart.types>();
        }
        System.Action OnDone;
        public void OnReady(System.Action OnDone)
        {
            this.OnDone = OnDone;
        }
        public void Reset()
        {
            bodyPart = null;
            state = states.NONE;
        }
        public void OnUpdate()
        {
            if (state == states.DONE) return;
            timer += Time.deltaTime;
            if (Input.GetMouseButtonDown(0) && CheckDrag())
                InitDrag();
            else if (bodyPart != null && Input.GetMouseButton(0))
                DragElement();
            else if (Input.GetMouseButtonUp(0))
                StopDrag();
        }
        bool CheckDrag()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                bodyPart = hit.collider.gameObject.GetComponent<BodyPart>();
              //  Debug.Log(bodyPart.characterID + "  Settings.characterActive " + Settings.characterActive);
                if (bodyPart.characterID != Settings.characterActive)
                {
                    bodyPart = null;
                    return false;
                }
                if (bodyPart != null)
                {
                    return true;
                }
            }
            return false;
        }
        void AddToDraggedPieces()
        {
          //  Debug.Log("AddToDraggedPieces " + draggedPieces.Count);
            foreach (BodyPart.types t in draggedPieces)
                if (bodyPart.type == t)
                    return;
            draggedPieces.Add(bodyPart.type);
        }
        bool NewPieceDragged()
        {
            foreach (BodyPart.types t in draggedPieces)
                if (bodyPart.type == t)
                    return false;
            return true;
        }
        void InitDrag()
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastPos = pos;
            state = states.DRAGGING;
            timerToDraw = 0;
            Debug.Log("InitDrag " + IsFirstMove() + " timer: " + timer);
            SetTimer();
            AddToDraggedPieces();
        }
        void SetTimer()
        {
            if (IsFirstMove() || bodyPart.type == BodyPart.types.HEAD)
                timer = 0;
            else
                timer = GetLatestMoveOfHand();
        }
        float GetLatestMoveOfHand()
        {
            return lastMovementTimer;
        }
        bool IsFirstMove()
        {
            return draggedPieces.Count == 0;
        }
        void DragElement()
        {
            timerToDraw += Time.deltaTime;
            if (timerToDraw > offsetTime)
            {
                Vector2 pos = GetPos();

                //bool isFarFromAnchor = IsFarFromAttached();
                //if (isFarFromAnchor) return;
                float distance = Vector2.Distance(pos, lastPos);
                if (distance < maxDistanceAllowed && distance > offsetToDraw)
                    Draw(distance * effortByDistance, pos);
            }
        }
        Vector2 GetPos()
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (pos.x < -Settings.limits.x) pos.x = -Settings.limits.x;
            if (pos.x > Settings.limits.x) pos.x = Settings.limits.x;
            if (pos.y < -Settings.limits.y) pos.y = -Settings.limits.y;
            if (pos.y > Settings.limits.y) pos.y = Settings.limits.y;
            return pos;
        }
        bool IsFarFromAttached() // Lmit of body
        {
            Debug.Log("IsFarFromAttached");
            Vector2 pos = bodyPart.transform.position;
            foreach (BodyPart go in bodyPart.attachedTo)
            {
                Debug.Log("distance is. " + Vector2.Distance(pos, go.transform.position));
                if (Vector2.Distance(pos, go.transform.position) > maxDistanceFromAnchor)
                    return true;
            }

            Debug.Log("IsFarFromAttached no");
            return false;
        }
        void AddReverse()
        {
            BodyPart bp = bodyPart.attachedTo[0];
            Vector2 pos = bp.transform.position;
            pos += (Vector2)bodyPart.transform.forward.normalized * 2f;
            SetDraggedElement(pos);
            Debug.Log("AddReverse " + pos);
        }
        void StopDrag()
        {
            
            if (bodyPart != null)
            {
                if (bodyPart.type != BodyPart.types.HEAD && IsFarFromAttached())
                {
                    AddReverse();
                }

                bodyPart.OnEndGrad();
                bodyPart = null;
            }
            state = states.NONE;
        }
        private void Draw(float distance, Vector2 pos)
        {
            distanceToEffort += distance;
            SetDraggedElement(pos);
            if (distanceToEffort > totalDistanceToEffort)
            {
                Debug.Log("ready");
                StopDrag();
                state = states.DONE;
                if (OnDone != null)
                    OnDone();
            }
            else
                Events.OnMovementMade(bodyPart.characterID, distance);
        }
        void SetDraggedElement(Vector2 pos)
        {
            lastPos = pos;
            bodyPart.AddGost(lastPos);
            dbManager.SaveMovement(bodyPart.characterID, bodyPart.type.ToString(), pos, timer);
            if (timer > lastMovementTimer && bodyPart.type != BodyPart.types.HEAD) lastMovementTimer = timer;
        }
    }

}