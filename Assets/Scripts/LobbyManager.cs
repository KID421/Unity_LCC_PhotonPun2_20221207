using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

namespace KID
{
    /// <summary>
    /// �j�U�޲z��
    /// </summary>
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        #region ���
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

        [SerializeField, Header("�ж��̤j�H��"), Range(2, 20)]
        private byte maxPlayer = 10;

        private void Awake()
        {
            FindObject();

            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// �M�䪫��
        /// </summary>
        private void FindObject()
        {
            inputFieldPlayerName = GameObject.Find("��J��쪱�a�W��").GetComponent<TMP_InputField>();
            inputFieldCreateRoomName = GameObject.Find("��J���Ыةж��W��").GetComponent<TMP_InputField>();
            inputFieldJoinRoomName = GameObject.Find("��J���[�J�ж��W��").GetComponent<TMP_InputField>();
            btnCreateRoom = GameObject.Find("���s�Ыةж�").GetComponent<Button>();
            btnJoinRoom = GameObject.Find("���s�[�J�ж�").GetComponent<Button>();
            btnJoinRandomRoom = GameObject.Find("���s�[�J�H���ж�").GetComponent<Button>();
            groupBlackAndTip = GameObject.Find("�¦⩳��").GetComponent<CanvasGroup>();

            GetInputFieldText();
            ButtonClickEvent();
        }

        /// <summary>
        /// ���o��J��쪺��r
        /// </summary>
        private void GetInputFieldText()
        {
            inputFieldPlayerName.onEndEdit.AddListener((input) =>
            {
                namePlayerName = input;
                PhotonNetwork.NickName = namePlayerName;
                print($"<color=#ff0066>���a�W�١G{PhotonNetwork.NickName}</color>");
            });

            inputFieldCreateRoomName.onEndEdit.AddListener((input) => nameCreateRoom = input);
            inputFieldJoinRoomName.onEndEdit.AddListener((input) => nameJoinRoom = input);
        }

        /// <summary>
        /// ���s�I���ƥ�
        /// </summary>
        private void ButtonClickEvent()
        {
            btnCreateRoom.onClick.AddListener(CreateRoom);
            btnJoinRoom.onClick.AddListener(JoinRoom);
            btnJoinRandomRoom.onClick.AddListener(JoinRandomRoom);
        }

        /// <summary>
        /// �إߩж�
        /// </summary>
        private void CreateRoom()
        {
            RoomOptions ro = new RoomOptions();
            ro.MaxPlayers = maxPlayer;
            PhotonNetwork.CreateRoom(nameCreateRoom, ro);
        }

        /// <summary>
        /// �[�J�ж�
        /// </summary>
        private void JoinRoom()
        {
            PhotonNetwork.JoinRoom(nameJoinRoom);
        }

        /// <summary>
        /// �[�J�H���ж�
        /// </summary>
        private void JoinRandomRoom()
        {

        }

        /// <summary>
        /// �H�J
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

            print("<color=yellow>�s�u�ܦ��A���C</color>");

            StartCoroutine(Fade());
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            print($"<color=#6600ff>�إ� {PhotonNetwork.CurrentRoom.Name} �ж����\�C</color>");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            print($"<color=#6600ff>�[�J {PhotonNetwork.CurrentRoom.Name} �ж����\�C</color>");
        }
    }
}
