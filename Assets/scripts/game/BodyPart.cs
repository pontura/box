using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace Box
{
    public class BodyPart : MonoBehaviour
    {
        public types type;
        public enum types
        {
            HAND1,
            HAND2,
            HEAD
        }
        Collider2D collider;
        CharacterManager characterManager;

        public BodyPart[] attachedTo; // A que objeto está attached:
        public BodyPart[] GetAttachments() { return attachedTo; }
        public int characterID;
        Material mat;
        Color color;
        public Color Color { get { return color;  } }
        [SerializeField] Vector2 initialPos;
        public float hitAreaSize;
        Vector3 originalPos;
        private void Awake()
        {
            Events.OnChangeState += OnChangeState;
            collider = GetComponent<Collider2D>();
            originalPos = transform.position;
        }
        private void OnDestroy()
        {
            Events.OnChangeState -= OnChangeState;
        }
        private void OnChangeState(GamesStatesManager.states state)
        {
            if (state == GamesStatesManager.states.PLAY)
                SetDrag(false);
            else if (Settings.characterActive == characterID)
                SetDrag(true);
            Reset();
        }
        public void ResetToInit()
        {
            transform.position = originalPos;
        }
        public void Reset()
        {
            MadeHit(false);
        }
        void SetDrag(bool isOn)
        {
            collider.enabled = isOn;
            Events.SetMovementSignal(transform.position, this);
        }
        public void Initializa(CharacterManager characterManager, int characterID)
        {
            this.characterID = characterID;
            this.characterManager = characterManager;
        }
        public void Init()
        {
            initialPos = transform.position;
        }
        public void ResetPosition()
        {
            if (initialPos != Vector2.zero)
                transform.position = initialPos;
        }
        public void RecalculatePosition()
        {
            print("RecalculatePosition");
        }
        public void OnEndGrad(Vector2 pos)
        {
            characterManager.RecalculatePositions();
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }
        Vector2 defensePos = Vector2.zero;
        public void GoToOriginDefense()
        {
            if (type == types.HEAD) return;

            Vector2 gotoPos = Vector2.zero;
            if (type == types.HAND1) gotoPos = characterManager.GetHandDefensePos(1);
            if (type == types.HAND2) gotoPos = characterManager.GetHandDefensePos(2);
           
            Vector2 opponentPos = GetOpponentPosition();

            float distanceToHead = Vector2.Distance(characterManager.GetPart("HEAD").transform.position, opponentPos);
            float distanceToHand = Vector2.Distance(transform.position, opponentPos);

            if (distanceToHead < distanceToHand)
            {
                if (type == types.HAND1) defensePos = characterManager.GetHandDefensePos(1);
                if (type == types.HAND2) defensePos = characterManager.GetHandDefensePos(2);
            }
            else if (Vector2.Distance(gotoPos, transform.position) > 2)
                defensePos = gotoPos;
            
        }
        void Update()
        {
            if (defensePos == Vector2.zero) return;
            Vector2 newPos = Vector2.Lerp(transform.position, defensePos, 0.15f);
            transform.position = newPos;
            if (Vector2.Distance(transform.position, defensePos) < 0.01f)
                defensePos = Vector2.zero;
        }
        public virtual void Hit(Vector3 pos) { }


        [SerializeField] bool hitted;
        public bool HasHitted() { return hitted; }
        public void MadeHit(bool hitted) { this.hitted = hitted; }

        Vector2 GetOpponentPosition()
        {
            BodyPart headB = characterManager.GetPart("HEAD");
            return headB.GetComponent<Head>().GetOpponentPos();
        }
    }
}