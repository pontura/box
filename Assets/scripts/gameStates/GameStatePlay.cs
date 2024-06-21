using System.Collections;
using UnityEngine;
namespace Box
{
    public class GameStatePlay : GameState
    {
        float timer = 0;
        float playSpeed = 1f;
        float movementLerp = 0.25f;
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
            foreach (MovementData.KeyFrameData k in m.keyframes)
            {
                if (timer < k.time)
                {
                    Animate(ch.GetPart(m.part), k.pos);
                    return;
                }
            }

        }
        public override void End()
        {
            base.End();
        }
        void Animate(BodyPart bodyPart, Vector2 dest)
        {
            bool hitting = CheckHit(bodyPart, dest);

            //if (hitting)
            //{
            //    Debug.Log("hittttt");
            //    dest = Vector3.Lerp(bodyPart.transform.position, dest, 0.05f);
            //}

            bodyPart.transform.position = Vector3.Lerp(bodyPart.transform.position, new Vector3(dest.x, dest.y, bodyPart.transform.position.z), movementLerp);
        }
        bool CheckHit(BodyPart bodyPart, Vector2 dest)
        {
            CharacterManager ch;
            if (bodyPart.characterID == 1)
                ch = gamesStatesManager.GetCharacter(2);
            else
                ch = gamesStatesManager.GetCharacter(1);
            return ch.CheckHit(dest);
        }

    }

}