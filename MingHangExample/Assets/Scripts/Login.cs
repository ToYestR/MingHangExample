using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Im;
using System.Threading;
using Google.Protobuf.WellKnownTypes;

public class Login : MonoBehaviour
{
    public Button loginBtn, connectBtn, press, JoinRoom;
    public InputField serverIP, port, content,nickName, roomNum;
    [Header("�û���½")]
    public InputField username, password;
    [Header("������")]
    public InputField lobbyname;
    [Header("�����ķ�����")]
    public InputField roomnameExcute;
    public int threadNum;
   

    private void Start()
    {

        loginBtn.onClick.AddListener(OnLoginBtnClick);
        JoinRoom.onClick.AddListener(OnJoinBtnClick);
        connectBtn.onClick.AddListener(OnConnectBtnClick);
        //press.onClick.AddListener(Pressure);
        ClientManager.Instance.InitSocket("47.101.199.191", "10001");
        //��½+�������
        //LoginFuction();
    }
    /// <summary>
    /// �����û���½
    /// </summary>
    public void UserLogin()
    {
        ClientManager.Instance.Send(new UserloginRequest()
        {
            Username = username.text,
            Password =password.text
        }) ;
        Global.UserName = username.text;
    }

    public void UserJoinLobby()
    {
        ClientManager.Instance.Send(new JoinLobbyRequest()
        {
            Lobbyname= lobbyname.text
        });
    }
    public void LeaveLobbyRequest()
    {
        ClientManager.Instance.Send(new LeaveLobbyRequest()
        {
        });
    }
    public void JoinRoomInLobby()
    {
        ClientManager.Instance.Send(new JoiRoomInLobbyRequest()
        {
            RoomName = roomnameExcute.text,
            RoomMaxplayer = 50

        }) ;
    }
    public void LeaveRoomInLobby()
    {
        ClientManager.Instance.Send(new LeaveRoomRequest()
        {
            RoomName = roomnameExcute.text
        });
    }
    public void UpdateRoomInfo()
    {
        ClientManager.Instance.Send(new UpdateRoomInfoRequest()
        {
            RoomStatus = 1,OperatorsNumber=3,ViewersNumber=4,UserWithRole="Randoemstring"
            ,CurrentTaskIndex=5
            
        });
    }
    public void PushTaskInfo()
    {
        ClientManager.Instance.Send(new PushTaskInfoRequest()
        {
            Taskinfo = new TaskInfo { Time = 1.33, Taskid = 3, Chaterp = 22, Playablestatus = 32 }
        }); 
    }
    public void PushGameStart()
    {
        ClientManager.Instance.Send(new StartGameRequest()
        {
        });
    }
    public void Pressure()
    {
        for (int i = 0; i < threadNum; i++)
        {
            Thread thread = new Thread(PresureTest);
            thread.Start();
            Debug.Log("�����߳�" + i.ToString());
        }
    }
    /// <summary>
    /// ѹ������ ���е��÷���
    /// </summary>
    public void PresureTest()
    {
        ClientManager client = new ClientManager();
        client.InitSocket("54.223.80.84", "10001");
        client.Send( new LoginRequest() { Uid = "123456" });
    }


    private void OnConnectBtnClick()
    {
        ClientManager.Instance.InitSocket(serverIP.text, port.text);
    }
    public void OnLoginBtnClick()
    {

        ClientManager.Instance.Send(new LoginRequest() { Uid = content.text, PlayerPack = new PlayerPack() {NickName = nickName.text } });
        ClientManager.Instance.uid = content.text;
        Debug.Log("���͵�¼����uidΪ" + content.text);
    }
    public void OnJoinBtnClick()
    {

        ClientManager.Instance.Send(new JoinRoomRequest() { RoomNo = roomNum.text });
        Debug.Log("���ͼ��뷿������roomNOΪ " + roomNum.text);
    }
    public void OnLeaveBtnClick()
    {
        ClientManager.Instance.Send(new ExitRoomRequest() { RoomNo = roomNum.text });
        Debug.Log("�����뿪��������roomNOΪ " + roomNum.text);
    }
    public void OnCratePlayerClick()
    {
        ClientManager.Instance.Send(new CreatePlayerRequest() { PlayerPack= new PlayerPack { Uid=Global.UserName} });
    }
}
