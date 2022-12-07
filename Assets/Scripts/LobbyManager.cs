using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

namespace KID
{
    /// <summary>
    /// 大廳管理器
    /// </summary>
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        #region 大廳資料
        private TMP_InputField inputFieldPlayerName;
        private TMP_InputField inputFieldCreateRoomName;
        private TMP_InputField inputFieldJoinRoomName;
        private Button btnCreateRoom;
        private Button btnJoinRoom;
        private Button btnJoinRandomRoom;
        private CanvasGroup groupBlackAndTip;
        private string namePlayerName;
        private string nameCreateRoom;
        private string nameJoinRoom;
        #endregion

        private CanvasGroup groupRoom;
        private TextMeshProUGUI textRoomName;
        private Transform traPlayerList;
        private Button btnLeaveRoom;

        [SerializeField, Header("房間最大人數"), Range(2, 20)]
        private byte maxPlayer = 10;
        [SerializeField, Header("文字房間內玩家")]
        private GameObject goTextPlayerInRoom;

        private void Awake()
        {
            FindLobbyObject();
            FindRoomOjbect();

            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// 尋找房間物件
        /// </summary>
        private void FindRoomOjbect()
        {
            groupRoom = GameObject.Find("畫布房間").GetComponent<CanvasGroup>();
            textRoomName = GameObject.Find("文字房間名稱").GetComponent<TextMeshProUGUI>();
            traPlayerList = GameObject.Find("玩家名稱清單").transform;
            btnLeaveRoom = GameObject.Find("按鈕退出房間").GetComponent<Button>();
        }

        #region 大廳物件處理
        /// <summary>
        /// 尋找大廳物件
        /// </summary>
        private void FindLobbyObject()
        {
            inputFieldPlayerName = GameObject.Find("輸入欄位玩家名稱").GetComponent<TMP_InputField>();
            inputFieldCreateRoomName = GameObject.Find("輸入欄位創建房間名稱").GetComponent<TMP_InputField>();
            inputFieldJoinRoomName = GameObject.Find("輸入欄位加入房間名稱").GetComponent<TMP_InputField>();
            btnCreateRoom = GameObject.Find("按鈕創建房間").GetComponent<Button>();
            btnJoinRoom = GameObject.Find("按鈕加入房間").GetComponent<Button>();
            btnJoinRandomRoom = GameObject.Find("按鈕加入隨機房間").GetComponent<Button>();
            groupBlackAndTip = GameObject.Find("黑色底圖").GetComponent<CanvasGroup>();

            GetInputFieldText();
            LobbyButtonClickEvent();
        }

        /// <summary>
        /// 取得輸入欄位的文字
        /// </summary>
        private void GetInputFieldText()
        {
            inputFieldPlayerName.onEndEdit.AddListener((input) =>
            {
                namePlayerName = input;
                PhotonNetwork.NickName = namePlayerName;
                print($"<color=#ff0066>玩家名稱：{PhotonNetwork.NickName}</color>");
            });

            inputFieldCreateRoomName.onEndEdit.AddListener((input) => nameCreateRoom = input);
            inputFieldJoinRoomName.onEndEdit.AddListener((input) => nameJoinRoom = input);
        }

        /// <summary>
        /// 按鈕點擊事件
        /// </summary>
        private void LobbyButtonClickEvent()
        {
            btnCreateRoom.onClick.AddListener(CreateRoom);
            btnJoinRoom.onClick.AddListener(JoinRoom);
            btnJoinRandomRoom.onClick.AddListener(JoinRandomRoom);
        } 
        #endregion

        /// <summary>
        /// 建立房間
        /// </summary>
        private void CreateRoom()
        {
            RoomOptions ro = new RoomOptions();
            ro.MaxPlayers = maxPlayer;
            PhotonNetwork.CreateRoom(nameCreateRoom, ro);
        }

        /// <summary>
        /// 加入房間
        /// </summary>
        private void JoinRoom()
        {
            PhotonNetwork.JoinRoom(nameJoinRoom);
        }

        /// <summary>
        /// 加入隨機房間
        /// </summary>
        private void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        /// <summary>
        /// 淡入
        /// </summary>
        private IEnumerator Fade(CanvasGroup group, bool fadeIn = true)
        {
            float increase = fadeIn ? +0.1f : -0.1f;

            for (int i = 0; i < 10; i++)
            {
                group.alpha += increase;
                yield return null;
            }

            group.interactable = fadeIn;
            group.blocksRaycasts = fadeIn;
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            print("<color=yellow>連線至伺服器。</color>");
            StartCoroutine(Fade(groupBlackAndTip, false));
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            print($"<color=#6600ff>建立 {PhotonNetwork.CurrentRoom.Name} 房間成功。</color>");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            print($"<color=#6600ff>加入 {PhotonNetwork.CurrentRoom.Name} 房間成功。</color>");
            StartCoroutine(Fade(groupRoom, true));
        }
    }
}
