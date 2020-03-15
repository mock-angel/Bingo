// vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
using System.ComponentModel;
using UnityEngine;
using TMPro;

namespace Mirror
{
    /// <summary>
    /// An extension for the NetworkManager that displays a default HUD for controlling the network state of the game.
    /// <para>This component also shows useful internal state for the networking system in the inspector window of the editor. It allows users to view connections, networked objects, message handlers, and packet statistics. This information can be helpful when debugging networked games.</para>
    /// </summary>
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [HelpURL("https://mirror-networking.com/docs/Components/NetworkManagerHUD.html")]
    public class NetworkManagerMobileHUD : MonoBehaviour
    {
        NetworkManager manager;

        /// <summary>
        /// Whether to show the default control HUD at runtime.
        /// </summary>
        public bool showGUI = true;

        /// <summary>
        /// The horizontal offset in pixels to draw the HUD runtime GUI at.
        /// </summary>
        public int offsetX;

        /// <summary>
        /// The vertical offset in pixels to draw the HUD runtime GUI at.
        /// </summary>
        public int offsetY;
        
        public GUIStyle customButton;
        public GUIStyle customLabel;
        public GUIStyle customTextField;
        
        public GameObject ConnectionSelectionPanel;
        public GameObject ConnectingPanel;
        public GameObject ConnectedPanel;
        
        public TextMeshProUGUI ConnectingText;
        public TextMeshProUGUI ConnectedText;
        public TMP_InputField IPtext;
        
        
        void Start()
        {
            customButton = new GUIStyle("button");
            customButton.fontSize = 30;
            
            customLabel = new GUIStyle("label");
            customLabel.fontSize = 25;
            
            customTextField = new GUIStyle("textfield");
            customTextField.fontSize = 30;
        }
        
        void Awake()
        {
            manager = GetComponent<NetworkManager>();
        }
        
        void FixedUpdate(){
//            return;
            manager.networkAddress = IPtext.text;
            IPtext.text = manager.networkAddress;
            
            string str = "";
            if (!NetworkClient.isConnected && !NetworkServer.active)
            {
            
//                ConnectingPanel.SetActive(false);
                if (!NetworkClient.active)
                {
                    ConnectingPanel.SetActive(false);
                    ConnectionSelectionPanel.SetActive(true);
                }
                else
                {
                    ConnectingText.text = "Connecting to " + manager.networkAddress + "..";
                    
                    ConnectionSelectionPanel.SetActive(false);
                    ConnectingPanel.SetActive(true);
//                    manager.networkAddress = IPtext.text;
                }
            }
            else
            {
                ConnectingPanel.SetActive(false);
                ConnectionSelectionPanel.SetActive(false);
                
                if (NetworkServer.active)
                {
                    str += "Server: active. Transport: " + Transport.activeTransport + "\n";
                }
                
                if (NetworkClient.isConnected)
                {
                    str += "Client: address=" + manager.networkAddress;
                }
                
                ConnectedText.text = str;
            }
            
            // client ready
            if (NetworkClient.isConnected && !ClientScene.ready)
            {
                if (GUILayout.Button("Client Ready", customButton, GUILayout.Height(100)))
                {
                    ClientScene.Ready(NetworkClient.connection);

                    if (ClientScene.localPlayer == null)
                    {
                        ClientScene.AddPlayer();
                    }
                }
            }

            // stop
            if (NetworkServer.active || NetworkClient.isConnected)
            {
                ConnectedPanel.SetActive(true);
            }
            else ConnectedPanel.SetActive(false);
        }
        
        public void IPValueUpdate(){
//            manager.networkAddress = IPtext.text;
        }
        
        void OnGUI()
        {
            if (!showGUI)
                return;
            
            GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215*1.9f, 9999));
            if (!NetworkClient.isConnected && !NetworkServer.active)
            {
                if (!NetworkClient.active)
                {
                    // LAN Host
                    if (Application.platform != RuntimePlatform.WebGLPlayer)
                    {
                        if (GUILayout.Button("LAN Host", customButton, GUILayout.Height(100)))
                        {
                            manager.StartHost();
                        }
                    }

                    // LAN Client + IP
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("LAN Client", customButton, GUILayout.Height(100)))
                    {
                        manager.StartClient();
                    }
                    manager.networkAddress = GUILayout.TextField(manager.networkAddress, customTextField,  GUILayout.Height(100));
                    
                    GUILayout.EndHorizontal();

//                    // LAN Server Only
                    if (Application.platform == RuntimePlatform.WebGLPlayer)
                    {
                        // cant be a server in webgl build
                        GUILayout.Box("(  WebGL cannot be server  )", GUILayout.Height(30));
                    }
                    else
                    {
                        if (GUILayout.Button("LAN Server Only", customButton, GUILayout.Height(100))) manager.StartServer();
                    }
                }
                else
                {
                    // Connecting
                    GUILayout.Label("Connecting to " + manager.networkAddress + "..", customLabel, GUILayout.Height(40));
                    if (GUILayout.Button("Cancel Connection Attempt", customButton, GUILayout.Height(100)))
                    {
                        manager.StopClient();
                    }
                }
            }
            else
            {
                // server / client status message
                if (NetworkServer.active)
                {
                    GUILayout.Label("Server: active. Transport: " + Transport.activeTransport, customLabel, GUILayout.Height(40));
                }
                if (NetworkClient.isConnected)
                {
                    GUILayout.Label("Client: address=" + manager.networkAddress, customLabel, GUILayout.Height(40));
                }
            }

            // client ready
            if (NetworkClient.isConnected && !ClientScene.ready)
            {
                if (GUILayout.Button("Client Ready", customButton, GUILayout.Height(100)))
                {
                    ClientScene.Ready(NetworkClient.connection);

                    if (ClientScene.localPlayer == null)
                    {
                        ClientScene.AddPlayer();
                    }
                }
            }

            // stop
            if (NetworkServer.active || NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop", customButton, GUILayout.Height(100)))
                {
                    manager.StopHost();
                }
            }
            
            GUILayout.EndArea();
        }
    }
}
