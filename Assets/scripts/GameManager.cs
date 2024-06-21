using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Box
{
    public class GameManager : MonoBehaviour
    {
        static GameManager mInstance;
        static GameManager Instance { get { return mInstance; } }

        private void Awake()
        {
            mInstance = this;
        }
    }

}