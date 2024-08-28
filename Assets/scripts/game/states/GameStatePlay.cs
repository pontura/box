using System.Collections;
using UnityEngine;

namespace Box
{
    public class GameStatePlay : GameState
    {
        float timer, playSpeed, movementLerp;
        MovementData.KeyFrameData ch1_k;
        MovementData.KeyFrameData ch2_k;
        float totalDuration;
        float delay;
        bool movementsReseting;
        public override void Init()
        {
            movementsReseting = false;
            playSpeed = Settings.playSpeed;
            movementLerp = Settings.movementLerp;

            base.Init();
            timer = 0;
            totalDuration = 0;
            foreach (MovementData.Movement m in gamesStatesManager.dbManager.ch1.movements)
            {
                foreach (MovementData.KeyFrameData k in m.keyframes)
                    if (k.time > totalDuration)
                        totalDuration = k.time;
            }
            foreach (MovementData.Movement m in gamesStatesManager.dbManager.ch2.movements)
            {
                foreach (MovementData.KeyFrameData k in m.keyframes)
                    if (k.time > totalDuration)
                        totalDuration = k.time;
            }
          //  Debug.Log("Total duration : " + totalDuration + "    mov 1: " + gamesStatesManager.dbManager.ch1.movements.Count + "  2: " + gamesStatesManager.dbManager.ch2.movements.Count);
           // totalDuration += 1;
            delay = 0;
        }
        public override void OnUpdate()
        {
            if (delay < 1)
            {
                delay += Time.deltaTime;
                return;
            }
            base.OnUpdate();
            timer += Time.deltaTime * playSpeed;
            foreach (MovementData.Movement m in gamesStatesManager.dbManager.ch1.movements)
                InitMoves(gamesStatesManager.ch1, m);
            foreach (MovementData.Movement m in gamesStatesManager.dbManager.ch2.movements)
                InitMoves(gamesStatesManager.ch2, m);
            if (timer > totalDuration && !movementsReseting)
            {
                gamesStatesManager.ch1.SendHandsToOriginDefense();
                gamesStatesManager.ch2.SendHandsToOriginDefense();
                movementsReseting = true;
            }
            if (timer > totalDuration+1)
                Finish();
        }
        void UpdateHand(BodyPart bp, Vector2 pos)
        {
            bp.transform.position = Vector2.Lerp(bp.transform.position, pos, 0.2f);
        }
        void Finish()
        {
            gamesStatesManager.PlayModeDone();
        }
        void InitMoves(CharacterManager ch, MovementData.Movement m)
        {
            int keyFrame = 0;
            Move(keyFrame, ch, m);
        }
        void Move(int keyFrame, CharacterManager ch, MovementData.Movement m)
        {
            if (m.keyframes.Count == 0) return;
            int keyframeActive = m.keyframeActive;
            
            foreach (MovementData.KeyFrameData k in m.keyframes)
            {
                if (timer < k.time)
                {
                    BodyPart bodyPart = ch.GetPart(m.part);
                    if (keyframeActive < keyFrame)
                    {
                        ProcessKeyframe(k, m, keyFrame, bodyPart, ch);
                        if (keyFrame == m.keyframes.Count - 1)
                        {
                            LastFrameFor(ch.GetPart(m.part));
                            m.keyframes.Clear();
                            return;
                        }
                    }
                    else
                        Animate(m, bodyPart, k.pos);
                    return;
                }
                keyFrame++;
            }
            ForcePosition(ch.GetPart(m.part), m.keyframes[m.keyframes.Count-1].pos);
        }
        void LastFrameFor(BodyPart bp)
        {
            bp.GoToOriginDefense();
        }
        void ProcessKeyframe(MovementData.KeyFrameData k, MovementData.Movement m, int keyFrame, BodyPart bodyPart, CharacterManager ch)
        {
            m.keyframeActive = keyFrame;
            ForcePosition(bodyPart, k.pos);
            float force = 0;
            if (keyFrame > 0)
            {
                if (keyFrame - 1 < m.keyframes.Count)
                {
                    Vector2 lastPos = m.keyframes[keyFrame - 1].pos;
                    Vector2 pos = k.pos;
                    force = Vector2.Distance(lastPos, pos);
                }
            }
            if (!bodyPart.HasHitted())
            {// si ya golpeo la cabeza del otro no cheqeua nada:
                BodyPart hittedTo = CheckHitTo(bodyPart, k.pos, force);
                if (hittedTo != null)
                {
                    if (hittedTo.type == BodyPart.types.HEAD)
                    {
                        bodyPart.MadeHit(true);
                        ReverseMovements(ch, m, bodyPart);
                        return;
                    }
                    else if (hittedTo.type == BodyPart.types.HAND1 || hittedTo.type == BodyPart.types.HAND2)
                    {
                        ChangeHandForwards(ch, m, bodyPart, hittedTo);
                    }
                }
            }
        }
        void ReverseMovements(CharacterManager ch, MovementData.Movement m, BodyPart bodyPart)
        {
            //Reset all keyframes forward:
            m.keyframes.RemoveRange(m.keyframeActive + 1, m.keyframes.Count - 1 - m.keyframeActive);

            MovementData.KeyFrameData newKeyFrame = new MovementData.KeyFrameData();
            if (bodyPart.type == BodyPart.types.HEAD) return;
            Vector2 pos = Vector2.zero;
            if (bodyPart.type == BodyPart.types.HAND1)
                pos = ch.GetHandDefensePos(1);
            else
                pos = ch.GetHandDefensePos(2);
            newKeyFrame.pos = pos;
            newKeyFrame.time = 1;
            m.keyframes.Add(newKeyFrame);
            //int totalRewindKeyframes = 12;
            
            //float time = m.keyframes[m.keyframeActive].time;
            //for (int a = 0; a < totalRewindKeyframes; a++)
            //{
            //    int k1 = m.keyframeActive - a;
            //    if (k1 > 0)
            //    {
            //        MovementData.KeyFrameData k = m.keyframes[k1];
            //        MovementData.KeyFrameData newKeyFrame = new MovementData.KeyFrameData();
            //        newKeyFrame.pos = k.pos;
            //        time += 0.025f*a;
            //        newKeyFrame.time = time;
            //        m.keyframes.Add(newKeyFrame);
            //    }
            //}
            Move(m.keyframeActive, ch, m);
        }
        void ChangeHandForwards(CharacterManager ch, MovementData.Movement m, BodyPart bodyPart, BodyPart bodyPartHitTo)
        {
            Vector2 diffVector = bodyPart.transform.position - bodyPartHitTo.transform.position;
            for (int a = m.keyframeActive; a < m.keyframes.Count; a++)
            {
                MovementData.KeyFrameData k = m.keyframes[a];
                k.pos += diffVector;
            }
        }
        public override void End()
        {
            base.End();
        }
        void Animate(MovementData.Movement m, BodyPart bodyPart, Vector2 dest)
        {
            dest = GetPos(dest);
            bodyPart.transform.position = Vector3.Lerp(bodyPart.transform.position, new Vector3(dest.x, dest.y, bodyPart.transform.position.z), movementLerp);
        }
        void ForcePosition(BodyPart bodyPart, Vector2 dest)
        {
            dest = GetPos(dest);
            bodyPart.transform.position = dest;
        }
        BodyPart CheckHitTo(BodyPart bodyPart, Vector2 dest, float force)
        {
            CharacterManager chDamaged;
            if (bodyPart.characterID == 1)
                chDamaged = gamesStatesManager.GetCharacter(2);
            else
                chDamaged = gamesStatesManager.GetCharacter(1);
            
            return chDamaged.CheckHit(dest, force);
        }
        Vector2 GetPos(Vector2 pos)
        {
            if (pos.x < -Settings.limits.x) pos.x = -Settings.limits.x;
            if (pos.x > Settings.limits.x) pos.x = Settings.limits.x;
            if (pos.y < -Settings.limits.y) pos.y = -Settings.limits.y;
            if (pos.y > Settings.limits.y) pos.y = Settings.limits.y;
            return pos;
        }
    }

}