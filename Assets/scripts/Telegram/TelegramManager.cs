using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Box
{
    public class TelegramManager : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text debugText;
        public string initData = "";
        public string userData = "";
        public string referral = "";

        static TelegramManager mInstance = null;

        public static TelegramManager Instance
        {
            get  {  return mInstance;  }
        }
        void Awake()
        {
            if (mInstance == null) mInstance = this as TelegramManager;
            else if (mInstance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this);
            debugText.text = "check";
        }
        public void ReceiveUserData(string data) // comes from the js in the HTML
        {
            debugText.text += data + " ";
            this.userData = data;
        }
        public void ReceiveReferral(string data)// comes from the js in the HTML
        {
            print("ReceiveReferral " + data);
            debugText.text += data + " ";
            this.referral = data;
        }
        public void ReceiveInitData(string data)// comes from the js in the HTML
        {
            debugText.text += data + " ";
            this.initData = data;
        }
    }
}