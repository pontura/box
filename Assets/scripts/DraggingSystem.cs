using UnityEngine;
namespace Box
{
    public class DraggingSystem
    {
        int draws = 0;
        int totalDraws = 50;
        float offsetToDraw = 0.2f;
        float offsetTime = 0.05f;
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
        float timerToDraw;

        public void Init(DBManager dbManager)
        {
            this.dbManager = dbManager;
            draws = 0;
            timer = timerToDraw = 0;
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

                Debug.Log(bodyPart.characterID + "  Settings.characterActive " + Settings.characterActive);

                if (bodyPart.characterID != Settings.characterActive)
                {
                    bodyPart = null;
                    return false;
                }
                if (bodyPart != null)
                    return true;
            }
            return false;
        }
        void InitDrag()
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastPos = pos;
            state = states.DRAGGING;
             timerToDraw = 0;
        }
        void DragElement()
        {
            timerToDraw += Time.deltaTime;
            if (timerToDraw > offsetTime)
            {
                timerToDraw = 0;
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float dist = Vector2.Distance(pos, lastPos);
                if (dist < offsetToDraw) return;
                Draw(pos);
            }
        }
        void StopDrag()
        {
            if (bodyPart != null)
            {
                bodyPart.OnEndGrad();
                bodyPart = null;
            }
            state = states.NONE;
        }
        private void Draw(Vector2 pos)
        {
            draws++;
            SetDraggedElement(pos);
            if (draws > totalDraws)
            {
                Debug.Log("ready");
                StopDrag();
                state = states.DONE;
                if (OnDone != null)
                    OnDone();
            }
            else
                Events.OnMovementMade(draws, totalDraws);
        }
        void SetDraggedElement(Vector2 pos)
        {
            lastPos = pos;
            bodyPart.AddGost(lastPos);
            dbManager.SaveMovement(bodyPart.characterID, bodyPart.type.ToString(), pos, timer);
        }
    }

}