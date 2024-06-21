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
        CharacterManager characterManager;
        [SerializeField] Transform ghosts;
        [SerializeField] DragElement dragElement;
        List<DragElement> draggedElements;
        public int characterID;
        SpriteRenderer sr;
        Color color;
        [SerializeField] Vector2 initialPos;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            color = sr.color;
            Events.OnChangeState += OnChangeState;
        }
        private void OnDestroy()
        {
            Events.OnChangeState -= OnChangeState;
        }
        private void OnChangeState(GamesStatesManager.states state)
        {
            if (state == GamesStatesManager.states.PLAY)
                SetAlpha(1);
            else if (Settings.characterActive != characterID)
                SetAlpha(0.2f);
            else
                SetAlpha(1);
        }
        void SetAlpha(float alpha)
        {
            color.a = alpha;
            sr.color = color;
        }

        public void Initializa(CharacterManager characterManager)
        {
            draggedElements = new List<DragElement>();
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

            ResetGhosts();
        }
        public void AddGost(Vector2 pos)
        {
            DragElement de = Instantiate(dragElement, ghosts);
            de.transform.position = pos;
            de.SetColor();
            draggedElements.Add(de);
        }
        public void RecalculatePosition()
        {
            if (draggedElements.Count > 0)
            {
                Vector3 dest = draggedElements[draggedElements.Count - 1].transform.position;
                transform.position = new Vector3(dest.x, dest.y, transform.position.z);
            }
            ResetGhosts();
        }
        private void ResetGhosts()
        {
            Utils.RemoveAllChildsIn(ghosts);
            draggedElements.Clear();
        }
        public void OnEndGrad()
        {
            characterManager.RecalculatePositions();
        }
    }
}