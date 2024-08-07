using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

namespace Box.Photon
{
    public class PhotonGameManager : MonoBehaviourPunCallbacks
    {

        static public PhotonGameManager Instance;
        private PhotonGameManager instance;
        PhotonView photonView;
        Player other;

        public bool Master()
        {
            return PhotonNetwork.LocalPlayer.IsMasterClient;
        }
        public bool TwoPlayersLoaded()
        {
            if(other != null)
                return true;
            return false;
        }
        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }
        void Start()
        {
            Instance = this;

            if (!PhotonNetwork.IsConnected)
            {
                //SceneManager.LoadScene("Launcher");
                return;
            }

            if (PhotonNetwork.InRoom)
            {
                Events.SetText("New LocalPlayer from " 
                    + SceneManagerHelper.ActiveSceneName 
                    + " id:" + PhotonNetwork.LocalPlayer.UserId
                    + " nick:" + PhotonNetwork.LocalPlayer.NickName
                    );
            }
            else
            {
                Events.SetText("Ignoring scene load for {0}" + SceneManagerHelper.ActiveSceneName);
            }
        }       
        public void SendMessageToOther(string message)
        {
            Events.SetText("Sending " + message + " to: " + other.UserId);
            photonView.RPC("ReceiveMessage", other, message);
        }
        [PunRPC]
        public void ReceiveMessage(string message)
        {
            Debug.Log("Message received: " + message);
            Events.SetText("Receiving " + message);
            Events.ReceiveMessage(message);
        }
        void Update()
        { 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitApplication();
            }
        }
        public override void OnJoinedRoom()
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Events.SetText("OnJoinedRoom"
                       + " id:" + PhotonNetwork.LocalPlayer.UserId
                    + " nick:" + PhotonNetwork.LocalPlayer.NickName);
                Player[] all = PhotonNetwork.PlayerList;
                foreach (Player p in all)
                {
                    Events.SetText("In ROOM are ID:" + p.UserId
                   + " nick:" + p.NickName);
                    if (p.UserId != PhotonNetwork.LocalPlayer.UserId)
                        other = p;
                }
            }
        }
        public override void OnPlayerEnteredRoom(Player other)
        {
            Events.SetText("OnPlayerEnteredRoom() " + other.NickName + " UserID: " + other.UserId + " IsMasterClient " + PhotonNetwork.IsMasterClient); // not seen if you're the player connecting
            this.other = other;
            //if (PhotonNetwork.IsMasterClient)
            //{
            //    Events.SetText("OnPlayerEnteredRoom IsMasterClient " + PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            //    LoadArena();
            //}
        }
        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.Log("OnPlayerLeftRoom() " + other.NickName + " UserID: " + other.UserId); // seen when other disconnects

            SceneManager.LoadScene("Register");

            //if (PhotonNetwork.IsMasterClient)
            //{
            //    Events.SetText("OnPlayerEnteredRoom IsMasterClient " + PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            //    LoadArena();
            //}
        }
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Register");
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void QuitApplication()
        {
            Application.Quit();
        }
        //void LoadArena()
        //{
        //    if (!PhotonNetwork.IsMasterClient)
        //    {
        //        Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        //        return;
        //    }

        //    Events.SetText("PhotonNetwork : Loading Level : " + PhotonNetwork.CurrentRoom.PlayerCount);

        //    PhotonNetwork.LoadLevel("PunBasics-Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        //}
    }
}