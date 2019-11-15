using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.VR;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEditor;
 


namespace Prototype.NetworkLobby
{
    public class SceneController : MonoBehaviour
    {
        public UnityEngine.Video.VideoClip videoClip;
        public RenderTexture TexturaVideo;

        public double time;
        public double currentTime;

        private bool flag = false;

        private GameObject LobbyMan;
        public LobbyManager lobbyManager;

        public Texture2D virtualRealitySplashScreen;

        // Start is called before the first frame update
        void Start()
        {
            //
           
            LobbyMan = GameObject.Find("LobbyManager");

            if (LobbyMan != null)
            {
                lobbyManager = LobbyMan.GetComponent<LobbyManager>();
            }
            
            //cambiar de none a VR mode
            ToggleVR();
            
            //obtener componente video player para saber cuando esta activado
            var videoPlayer = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();

            videoPlayer.playOnAwake = false;
            videoPlayer.clip = videoClip;
            videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.RenderTexture;
            videoPlayer.targetTexture = TexturaVideo;

            time = gameObject.GetComponent<UnityEngine.Video.VideoPlayer>().clip.length;

            StartCoroutine(IniciarVideo(videoPlayer));
        }

        //rutina para inicia video
        IEnumerator IniciarVideo(UnityEngine.Video.VideoPlayer video)
        {
            yield return new WaitForSeconds(5);
            
            flag = true;
            video.Play();
            yield return new WaitForEndOfFrame();
        }

        //rutina para regresar al host
        IEnumerator BackToHost()
        {
            Debug.Log("Volviendo a client");
            yield return new WaitForEndOfFrame();

           
            StartCoroutine(LoadDevice("None"));
            
            flag = false;

            yield return new WaitForSeconds(2);
            //regresar al lobby
            LobbyManager.s_Singleton.ServerReturnToLobby();

        }

        public void ToggleVR()
        {

            if (XRSettings.loadedDeviceName == "cardboard")
            {
                StartCoroutine(LoadDevice("None"));
                //Debug.Log("None");
            }
            else
            {
                StartCoroutine(LoadDevice("cardboard"));
                //Debug.Log("cardboard");
            }
        }



        IEnumerator LoadDevice(string newDevice)
        {
            XRSettings.LoadDeviceByName(newDevice);
            yield return null;
            XRSettings.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {

            currentTime = gameObject.GetComponent<UnityEngine.Video.VideoPlayer>().time;
            if (currentTime >= time && flag.Equals(true) && lobbyManager != null)
            {
                StartCoroutine(BackToHost());
            }

        }
    }
}