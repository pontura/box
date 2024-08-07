using System;
using UnityEngine;
using UnityEngine.UI;

namespace Box.Photon
{
    public class MessageSender : MonoBehaviour
    {
        [SerializeField] ButtonUI button;
        [SerializeField] InputField inputField;
        [SerializeField] Text debugField;

        PhotonGameManager photonGameManager;

        private void Start()
        {
            photonGameManager = GetComponent<PhotonGameManager>();
            button.Init(Send);
            Events.SetText += SetText;
        }
        private void OnDestroy()
        {
            Events.SetText -= SetText;
        }

        private void SetText(string text)
        {
          //  print("SetText " + text);
            debugField.text += text + "\n";
        }

        void Send(int id)
        {
            string s = inputField.text;
           // print("SEND " + s);
            photonGameManager.SendMessageToOther(s);
        }
        void Receive(string s)
        {
            Debug(s);
        }
        void Debug(string text)
        {
            debugField.text += text;
        }
    }
}