using System;
using System.Collections.Generic;
using UnityEngine;
namespace Box
{
    public class MovementData
    {

        [Serializable]
        public class CharacterData
        {
            public List<Movement> movements;
        }
        [Serializable]
        public class Movement
        {
            public string part;
            public List<KeyFrameData> keyframes;
        }
        [Serializable]
        public class KeyFrameData
        {
            public Vector2 pos;
            public float time;
        }
        public CharacterData SetDataToCharacter(string data)
        {
            CharacterData ch = new CharacterData();
            ch.movements = new List<Movement>();

            if (data != "empty")
            {
                string[] partsData = data.Split("_");
                foreach (string d in partsData)
                {
                    if (d.Length > 1)
                    {
                        string[] pData = d.Split("|");
                        Movement movement = null;
                        if (pData.Length > 0)
                        {
                            string part = pData[0];
                            movement = GetMovementForPart(ch, part);
                        }
                        if (movement == null)
                        {
                            movement = new Movement();
                            movement.keyframes = new List<KeyFrameData>();
                            ch.movements.Add(movement);
                            movement.part = pData[0];
                        }
                        pData[0] = "";
                        SetDataToCharacterID(movement, pData);
                    }
                }
            }
            return ch;
        }
        void SetDataToCharacterID(Movement movement, string[] partsData)
        {
            Debug.Log(movement);
            Debug.Log(partsData);
            Debug.Log(partsData.Length);

            foreach (string d in partsData)
            {
                Debug.Log("d: " + d);
                if (d != "")
                {
                    string[] positions = d.Split("*");

                    float _x = float.Parse(positions[0]);
                    float _y = float.Parse(positions[1]);
                    Debug.Log(positions[2]);
                    float time = float.Parse(positions[2]);

                    KeyFrameData keyFrame = new KeyFrameData();
                    Vector2 pos = new Vector2(_x, _y);
                    keyFrame.pos = pos;
                    keyFrame.time = time;
                    movement.keyframes.Add(keyFrame);
                }
            }
        }
        Movement GetMovementForPart(CharacterData cd, string part)
        {
            foreach (Movement m in cd.movements)
                if (m.part == part)
                    return m;
            return null;
        }
    }

}