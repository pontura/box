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
                hand.Initializa(this);
            head.Initializa(this);
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
        public bool CheckHit(Vector2 pos)
        {
            if (Vector2.Distance(head.transform.position, pos) < 1)
                return true;
            else if (Vector2.Distance(hands[0].transform.position, pos) < 1)
                return true;
            else if (Vector2.Distance(hands[1].transform.position, pos) < 1)
                return true;
            return false;
        }
    }
}