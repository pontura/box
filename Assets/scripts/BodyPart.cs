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

        public GameObject[] attachedTo; // A que objeto está attached:
        [SerializeField] DragElement dragElement;
        List<DragElement> draggedElements;
        public int characterID;
        Material mat;
        Color color;
        [SerializeField] Vector2 initialPos;
        public float hitAreaSize;

        private void Awake()
        {
            //mat = GetComponent<Material>();
            //color = mat.color;
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
            Reset();
        }
        void Reset()
        {
            MadeHit(false);
        }
        void SetAlpha(float alpha)
        {
            color.a = alpha;
           // sr.color = color;
        }

        public void Initializa(CharacterManager characterManager, int characterID)
        {
            this.characterID = characterID;
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
        public virtual void Hit(Vector3 pos)   {   }


        bool canDamage;
        public void MadeDamageToEnemy()
        {
            canDamage = false;
        }
        public bool CanDamage()
        {
            return canDamage;
        }

        [SerializeField] bool hitted;
        public bool HasHitted() { return hitted; }
        public void MadeHit(bool hitted) { this.hitted = hitted; }
    }
}