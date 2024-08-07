using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Box
{
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField] int id;
        [SerializeField] Hand[] hands;
        [SerializeField] Head head;

        void Awake()
        {
            Events.OnChangeState += OnChangeState;
        }
        void Start()
        {
            foreach (Hand hand in hands)
                hand.Initializa(this, id);
            head.Initializa(this, id);
        }
        private void OnDestroy()
        {
            Events.OnChangeState -= OnChangeState;
        }

        private void OnChangeState(GamesStatesManager.states state)
        {
            switch (state)
            {
                case GamesStatesManager.states.MOVE_1:
                    if (id == 1)
                        Init(); break;
                case GamesStatesManager.states.MOVE_2:
                    if (id == 1)
                        ResetPositions();
                    else if (id == 2)
                        Init(); break;
                case GamesStatesManager.states.PLAY:
                    if (id == 2)
                        ResetPositions();
                    break;
            }
        }

        public void Init()
        {
            foreach (Hand hand in hands)
                hand.Init();
            head.Init();
        }
        public void ResetPositions()
        {
            foreach (Hand hand in hands)
                hand.ResetPosition();
            head.ResetPosition();
        }
        public void RecalculatePositions()
        {
            foreach (Hand hand in hands)
                hand.RecalculatePosition();
            head.RecalculatePosition();
        }
        public BodyPart GetPart(string typeName)
        {
            switch (typeName)
            {
                case "HAND1": return hands[0];
                case "HAND2": return hands[1];
                default: return head;
            }
        }
        bool canDamage;
        public Vector2 CheckHit(Vector2 pos, bool canDamage)
        {
            this.canDamage = canDamage;
            if (Vector2.Distance(head.transform.position, pos) < head.hitAreaSize)
                return GetVectorBetween(head, pos);
            else if (Vector2.Distance(hands[0].transform.position, pos) < hands[0].hitAreaSize)
                return GetVectorBetween(hands[0], pos);
            else if (Vector2.Distance(hands[1].transform.position, pos) < hands[0].hitAreaSize)
                return GetVectorBetween(hands[1],  pos);
            return Vector2.zero;
        }
        Vector2 GetVectorBetween(BodyPart bodyPart, Vector2 my)
        {
            float _x =  bodyPart.transform.position.x - my.x;
            float _y =  bodyPart.transform.position.y - my.y;
            Vector2 v = new Vector2(_x, _y);

            if (bodyPart.type == BodyPart.types.HEAD)
            {
                if(canDamage)
                {
                    float force = (int)Mathf.Abs((v.x + v.y)*100);
                    Events.OnHit(bodyPart.characterID, force);
                }
                bodyPart.Hit(my);
               // v *= 3;
            }

            return v;
        }
    }
}