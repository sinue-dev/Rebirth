using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rebirth.Prototype
{
    public class UIMenu : UIBase
    {
        public void PlayOnClick()
        {
            StartCoroutine(GameManager.singleton.LoadLevel(2));

            GameManager.singleton.Hud.UIMainMenu.SetActive(false);
            GameManager.singleton.Hud.UIMenu.Hide();            
        }

        public void SettingsOnClick()
        {
			
        }

        public void QuitOnClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit;
#endif
        }
    }
}