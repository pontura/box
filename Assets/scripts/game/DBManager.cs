using System.Collections.Generic;
using UnityEngine;
namespace Box
{
    public class DBManager : MonoBehaviour
    {
        public string dataSaved;
        public string dataSaved_2;

        public MovementData.CharacterData ch1;
        public MovementData.CharacterData ch2;

        public List<string> recorder_ch1;
        public List<string> recorder_ch2;

        string active_part;
        MovementData movementData;

        private void Awake()
        {
            movementData = new MovementData();
        }
        private void Start()
        {
            Events.ReceiveMessage += ReceiveMessage;
        }
        private void OnDestroy()
        {
            Events.ReceiveMessage -= ReceiveMessage;
        }
        void ReceiveMessage(string movement)
        {
            if (Settings.characterActive == 1)
                dataSaved_2 = movement;
            else
                dataSaved = movement;
        }
        public void SetRecorders(int round)
        {
            dataSaved = recorder_ch1[round];
            dataSaved_2 = recorder_ch2[round];
            OnParseMovements();
        }
        public void Reset()
        {
            dataSaved = dataSaved_2 = "";
            ch1 = new MovementData.CharacterData();
            ch2 = new MovementData.CharacterData();
        }
        public string GetMove(int playerID)
        {
            if (playerID == 1)   {
                if (dataSaved == "") dataSaved = "empty";
                return dataSaved;
            }
            else  {
                if (dataSaved_2 == "") dataSaved_2 = "empty";
                return dataSaved_2;
            }
        }
        public void SaveMovement(int playerID, string part, Vector2 newPos, float time)
        {
            if (playerID == 1)
            {
                if (dataSaved == "")
                    active_part = "";
                if (active_part != part)
                    dataSaved += "_" + part;
                dataSaved += "|" + newPos.x + "*" + newPos.y + "*" + time;
            }
            else
            {
                if (dataSaved_2 == "")
                    active_part = "";
                if (active_part != part)
                    dataSaved_2 += "_" + part;
                dataSaved_2 += "|" + newPos.x + "*" + newPos.y + "*" + time;
            }
            active_part = part;
        }
        public void OnParseMovements()
        {
            ch1 = movementData.SetDataToCharacter(dataSaved);
            ch2 = movementData.SetDataToCharacter(dataSaved_2);
        }
        public void RecordMovements()
        {
            recorder_ch1.Add(dataSaved);
            recorder_ch2.Add(dataSaved_2);
        }
        public bool MovementsDone()
        {
            if (dataSaved != "" && dataSaved_2 != "") return true;
            return false;
        }
    }
}
