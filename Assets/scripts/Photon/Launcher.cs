using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

namespace Box.Photon
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private GameObject controlPanel;

        [SerializeField]
        private Text feedbackText;

        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [Tooltip("The UI Loader Anime")]
        [SerializeField]
        private LoaderAnime loaderAnime;

        bool isConnecting;
        string gameVersion = "1";

        void Awake()
        {
            if (loaderAnime == null)
            {
                Debug.LogError("<Color=Red><b>Missing</b></Color> loaderAnime Reference.", this);
            }
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public void Connect()
        {
            feedbackText.text = "";
            isConnecting = true;

            controlPanel.SetActive(false);

            if (loaderAnime != null)
            {
                loaderAnime.StartLoaderAnimation();
            }

            if (PhotonNetwork.IsConnected)
            {
                LogFeedback("Joining Room...");
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                LogFeedback("Connecting...");
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = this.gameVersion;
            }
        }
        void LogFeedback(string message)
        {
            if (feedbackText == null)
                return;

            feedbackText.text += System.Environment.NewLine + message;
        }
        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");
                Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            LogFeedback("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            LogFeedback("<Color=Red>OnDisconnected</Color> " + cause);
            Debug.LogError("PUN Basics Tutorial/Launcher:Disconnected");

            loaderAnime.StopLoaderAnimation();

            isConnecting = false;
            controlPanel.SetActive(true);
        }

        public override void OnJoinedRoom()
        {
            LogFeedback("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");
                PhotonNetwork.LoadLevel("Game");

            }
        }
    }
}