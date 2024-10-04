using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Box
{
    public class TelegramManager : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text debugText;
        public string initData = "";
        public string id = "";
        public string userName = "";
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
            ParseURL(data);
            debugText.text += userName;
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
        public void ParseURL(string text)
        {
            string[] arr;
            arr = text.Split("?id=");
            if (arr.Length < 2) return;
            arr = arr[1].Split("&first_name=");
            id = arr[0];
            if (arr.Length < 2) return;
            arr = arr[1].Split("&last_name=");
            if (arr.Length < 2) return;
            userName = arr[0];
            if (arr.Length > 1) userName += " " + arr[1];
        }
    }
}