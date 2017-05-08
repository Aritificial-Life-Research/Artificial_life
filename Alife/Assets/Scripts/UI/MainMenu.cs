using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public void Start()
        {
            Button play = transform.Find("Panel/Play Easy").GetComponent<Button>();
            Button playHard = transform.Find("Panel/Play Hard").GetComponent<Button>();

            play.onClick.AddListener(LoadLevel);
            playHard.onClick.AddListener(LoadLevelHard);
        }

        public void LoadLevel()
        {
            Application.LoadLevel("Simple");
        }

        public void LoadLevelHard()
        {
            Application.LoadLevel("Hard");
        }
    }

}