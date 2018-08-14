using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rebirth.Prototype
{
    public class HUD : MonoBehaviour
    {
        public GameObject UIMainMenu;
        public UIMenu UIMenu;
        public GameObject UILogin;
        public UIActionPanel UIActionPanel;
        public UIBagPanel UIBagPanel;

        public Bag Bag;

        public GameObject MessagePanel;

        void Awake()
        {
            if(UIMenu != null)
            {
                UIMainMenu.SetActive(true);
                UIMenu.Show();
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        

        private bool mIsMessagePanelOpened = false;

        public bool IsMessagePanelOpened
        {
            get { return mIsMessagePanelOpened; }
        }

        public void OpenMessagePanel(InteractableItemBase item)
        {
            MessagePanel.SetActive(true);

            Text mpText = MessagePanel.transform.Find("Text").GetComponent<Text>();
            mpText.text = item.InteractText;


            mIsMessagePanelOpened = true;
        }

        public void CloseMessagePanel()
        {
            MessagePanel.SetActive(false);

            mIsMessagePanelOpened = false;
        }
    }
}