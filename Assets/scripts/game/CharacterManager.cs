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
        public BodyPart CheckHit(Vector2 pos, float force)
        {
            if (Vector2.Distance(head.transform.position, pos) < head.hitAreaSize)
            {
                OnHit(head, pos, force);
                return head;
            }
            else
            {
                Hand hand1 = hands[0];
                Hand hand2 = hands[1];
                if (Vector2.Distance(hand1.transform.position, pos) < hand1.hitAreaSize)
                {
                    OnHit(hand1, pos, force);
                    return hand1;
                }
                else if (Vector2.Distance(hand2.transform.position, pos) < hand2.hitAreaSize)
                {
                    OnHit(hand2, pos, force);
                    return hand2;
                }
            }
            return null;
        }
        void OnHit(BodyPart bodyPart, Vector2 my, float force)
        {
            float _x =  bodyPart.transform.position.x - my.x;
            float _y =  bodyPart.transform.position.y - my.y;
            Vector2 v = new Vector2(_x, _y);

            if (bodyPart.type == BodyPart.types.HEAD)
            {
                Events.OnHit(bodyPart.characterID, force*500);
                bodyPart.Hit(my);
            }
        }
        public void SendHandsToOriginDefense()
        {
            GetPart("HAND1").GoToOriginDefense();
            GetPart("HAND2").GoToOriginDefense();
        }
        public Vector2 GetHandDefensePos(int handID)
        {
            return head.GetHandDefensePos(handID);
        }
    }
}