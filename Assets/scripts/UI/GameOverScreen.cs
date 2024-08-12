using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Box.UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        [SerializeField] ButtonUI button;

        private void Start()
        {
            button.Init(Submit);
        }
        public void Init()
        {
            panel.SetActive(true);
            CharactersBar bars = GetComponent<CharactersBar>();
            button.field.text = "Ganó ";
            if (bars.player1.value > bars.player2.value)
                button.field.text += "RED";
            else button.field.text += "BLUE";
        }

        private void Submit(int obj)
        {
            SceneManager.LoadScene("Register");
        }

        public void Reset()
        {
            panel.SetActive(false);

        }
    }
}