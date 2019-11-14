using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


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
        // Start is called before the first frame update
        void Start()
        {
            //

            LobbyMan = GameObject.Find("LobbyManager");
            lobbyManager = LobbyMan.GetComponent<LobbyManager>();
            //11
            StartCoroutine(LoadDevice("cardboard", true));

            //obtener componente video player para saber cuando esta activado
            var videoPlayer = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();

            videoPlayer.playOnAwake = false;
            videoPlayer.clip = videoClip;
            videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.RenderTexture;
            videoPlayer.targetTexture = TexturaVideo;

            time = gameObject.GetComponent<UnityEngine.Video.VideoPlayer>().clip.length;

            StartCoroutine(IniciarVideo(videoPlayer));
        }

        IEnumerator IniciarVideo(UnityEngine.Video.VideoPlayer video)
        {
            yield return new WaitForSeconds(5);
            flag = true;
            video.Play();
        }

        IEnumerator BackToHost()
        {
            Debug.Log("Volviendo a client");
            yield return new WaitForEndOfFrame();
            flag = false;

            StartCoroutine(LoadDevice("None", true));
            yield return new WaitForSeconds(3);
            //regresar al lobby
            LobbyManager.s_Singleton.ServerReturnToLobby();
            //LobbyManager.s_Singleton.OnLobbyClientEnter();

      


        

       
            //OnClientReady(false);
            //SceneManager.LoadScene("client", LoadSceneMode.Single);

        }

        public void ToggleVR()
        {

            if (XRSettings.loadedDeviceName == "cardboard")
            {
                //StartCoroutine(LoadDevice("None"));
                Debug.Log("None");
            }
            else
            {
                //StartCoroutine(LoadDevice("cardboard"));
                Debug.Log("cardboard");
            }
        }



        IEnumerator LoadDevice(string newDevice, bool enable)
        {
            XRSettings.LoadDeviceByName(newDevice);
            yield return null;
            XRSettings.enabled = enable;
        }

        // Update is called once per frame
        void Update()
        {

            currentTime = gameObject.GetComponent<UnityEngine.Video.VideoPlayer>().time;
            if (currentTime >= time && flag.Equals(true))
            {
                StartCoroutine(BackToHost());
            }

        }
    }
}