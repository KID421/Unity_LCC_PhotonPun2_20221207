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
        #region 資料
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

        [SerializeField, Header("房間最大人數"), Range(2, 20)]
        private byte maxPlayer = 10;

        private void Awake()
        {
            FindObject();

            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// 尋找物件
        /// </summary>
        private void FindObject()
        {
            inputFieldPlayerName = GameObject.Find("輸入欄位玩家名稱").GetComponent<TMP_InputField>();
            inputFieldCreateRoomName = GameObject.Find("輸入欄位創建房間名稱").GetComponent<TMP_InputField>();
            inputFieldJoinRoomName = GameObject.Find("輸入欄位加入房間名稱").GetComponent<TMP_InputField>();
            btnCreateRoom = GameObject.Find("按鈕創建房間").GetComponent<Button>();
            btnJoinRoom = GameObject.Find("按鈕加入房間").GetComponent<Button>();
            btnJoinRandomRoom = GameObject.Find("按鈕加入隨機房間").GetComponent<Button>();
            groupBlackAndTip = GameObject.Find("黑色底圖").GetComponent<CanvasGroup>();

            GetInputFieldText();
            ButtonClickEvent();
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
        private void ButtonClickEvent()
        {
            btnCreateRoom.onClick.AddListener(CreateRoom);
            btnJoinRoom.onClick.AddListener(JoinRoom);
            btnJoinRandomRoom.onClick.AddListener(JoinRandomRoom);
        }

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

        }

        /// <summary>
        /// 淡入
        /// </summary>
        private IEnumerator Fade()
        {
            for (int i = 0; i < 10; i++)
            {
                groupBlackAndTip.alpha -= 0.1f;
                yield return null;
            }

            groupBlackAndTip.interactable = false;
            groupBlackAndTip.blocksRaycasts = false;
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            print("<color=yellow>連線至伺服器。</color>");

            StartCoroutine(Fade());
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
        }
    }
}
