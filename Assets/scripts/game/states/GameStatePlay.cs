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
        public override void Init()
        {
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
            totalDuration += 1;
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
                Move(gamesStatesManager.ch1, m);
            foreach (MovementData.Movement m in gamesStatesManager.dbManager.ch2.movements)
                Move(gamesStatesManager.ch2, m);

            if (timer > totalDuration)
                Finish();
        }
        void Finish()
        {
            gamesStatesManager.PlayModeDone();
        }
        int keyFrame = 0;
        void Move(CharacterManager ch, MovementData.Movement m)
        {
            int keyframeActive = m.keyframeActive;
            keyFrame = 0;
            foreach (MovementData.KeyFrameData k in m.keyframes)
            {
                if (timer < k.time)
                {
                    if(keyframeActive < keyFrame)
                    {
                        m.keyframeActive = keyFrame;
                        ForcePosition(ch.GetPart(m.part), k.pos);
                        CheckHitOnKeyframe(m, ch.GetPart(m.part), k.pos);
                    } else
                        Animate(m, ch.GetPart(m.part), k.pos);
                    return;
                }
                keyFrame++;
            }
            ForcePosition(ch.GetPart(m.part), m.keyframes[m.keyframes.Count-1].pos);
        }
        void CheckHitOnKeyframe(MovementData.Movement m, BodyPart bodyPart, Vector2 dest)
        {
            Vector2 diffPos = CheckHit(bodyPart, dest);
            if (diffPos != Vector2.zero)
            {
                int keyFrame = 0;
                float recalculations = 1;
                foreach(MovementData.KeyFrameData k in m.keyframes)
                {
                    if (keyFrame >= m.keyframeActive)
                    {
                        recalculations -= 0.15f;
                        if (recalculations < 0) recalculations = 0;
                         Vector2 newPos = k.pos - (diffPos * recalculations);
                        k.pos = GetPos(newPos);
                    }
                    keyFrame++;
                }
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
        Vector2 CheckHit(BodyPart bodyPart, Vector2 dest)
        {
            CharacterManager chDamaged;
            if (bodyPart.characterID == 1)
                chDamaged = gamesStatesManager.GetCharacter(2);
            else
                chDamaged = gamesStatesManager.GetCharacter(1);

            bool canDamage = bodyPart.CanDamage();
            Vector2 hitPos = chDamaged.CheckHit(dest, canDamage);
            return hitPos;
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