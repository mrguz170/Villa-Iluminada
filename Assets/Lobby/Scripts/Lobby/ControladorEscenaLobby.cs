using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

using UnityEngine.SceneManagement;

namespace Prototype.NetworkLobby
{
    public class ControladorEscenaLobby : MonoBehaviour
    {
        //Scene scene = SceneManager.GetActiveScene();
        private GameObject LobbyManager;
        private GameObject mainPanel;
        
        //menu2
        public LobbyManager lobbyManager;

        // Start is called before the first frame update
        public void Awake()
        {
            LobbyManager = GameObject.Find("LobbyManager");
            mainPanel = LobbyManager.transform.Find("MainPanel").gameObject;
            
            lobbyManager = LobbyManager.GetComponent<LobbyManager>();
            
            if (LobbyManager != null && mainPanel != null)
            {
                StartCoroutine(LoadDevice("None"));
                Debug.Log("App iniciada, NONE");
            }
            else
            {
                Debug.Log("null");
            }

        }


        IEnumerator LoadDevice(string newDevice)
        {
            XRSettings.LoadDeviceByName(newDevice);
            yield return null;
            XRSettings.enabled = true;
        }

        IEnumerator VerPanelMenu()
        {
            yield return new WaitForSeconds(0.2f);
            //Debug.Log("Se inicio la escena lobby");
            if (mainPanel != null)
            {
                //Debug.Log("El panel Menu NO es null");
                if (!mainPanel.gameObject.active)
                {
                    Debug.Log("El panel estaba desactivado y se ha activado!");
                    mainPanel.SetActive(true);
                }
            }
            else
            {
                Debug.Log("El panel Menu es null");
            }
        }

    }
}