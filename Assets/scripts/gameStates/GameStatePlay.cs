using System.Collections;
using UnityEngine;
namespace Box
{
    public class GameStatePlay : GameState
    {
        float timer = 0;
        float playSpeed = 0.5f;
        float movementLerp = 0.02f;
        public override void Init()
        {
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
            Debug.Log("Total duration : " + totalDuration + "    mov 1: " + gamesStatesManager.dbManager.ch1.movements.Count + "  2: " + gamesStatesManager.dbManager.ch2.movements.Count);
            totalDuration += 1;
        }
        MovementData.KeyFrameData ch1_k;
        MovementData.KeyFrameData ch2_k;
        float totalDuration;

        public override void OnUpdate()
        {
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
        void Move(CharacterManager ch, MovementData.Movement m)
        {
            int keyframeActive = m.keyframeActive;
            int keyFrame = 0;
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
                foreach(MovementData.KeyFrameData k in m.keyframes)
                {
                    if (keyFrame >= m.keyframeActive)
                    {
                        Vector2 newPos = k.pos - diffPos / 2;
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
            CharacterManager ch;
            if (bodyPart.characterID == 1)
                ch = gamesStatesManager.GetCharacter(2);
            else
                ch = gamesStatesManager.GetCharacter(1);

            return ch.CheckHit(dest);
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