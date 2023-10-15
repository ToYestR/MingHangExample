using Im;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.TextCore.Text;
using StarterAssets;
using UnityEngine.InputSystem;
using Google.Protobuf.WellKnownTypes;
using System.Runtime.Serialization;

public class PlayerManager : MonoBehaviour
{
    GameObject player;
    private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        player = Resources.Load<GameObject>("Player");
    }


    private void Update()
    {

        if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.JoinRoomResponse")
        {
            HandleJoinRommResponse(pack.Msg.Unpack<JoinRoomResponse>());
            
        }
        else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.AddPlayer")
        {
            HandleAddPlayer(pack.Msg.Unpack<AddPlayer>());

        }
        else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.UpPos")
        {
            HandleUpPos(pack.Msg.Unpack<UpPos>());

        }
        else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.ExitRoom")
        {
            HandleExitRoom(pack.Msg.Unpack<ExitRoom>());

        }
        //处理账号密码的登陆结果
        else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.UserloginRsp")
        {
            HandleUserLoginRespond(pack.Msg.Unpack<UserloginRsp>());

        }
        //处理多人大厅的登陆结果
        else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.JoinRoomInLobbyResponse")
        {
            HandleJoinLobbyResponse(pack.Msg.Unpack<JoinLobbyRsp>());
        }
        //处理多人大厅的基础信息刷新结果
        else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.LobbyInfoRsp")
        {
            HandleLobbyInfoRsp(pack.Msg.Unpack<LobbyInfoRsp>());
        }
        //处理多人大厅的退出结果
        else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.LeaveLobbyRsp")
        {
            HandleLeaveLobbyRsp(pack.Msg.Unpack<LeaveLobbyRsp>());
        }
        //处理多人演练房间的加入返回结果
        else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.JoinRoomInLobbyResponse")
        {
            HandleJoinRoomInLobbyRspond(pack.Msg.Unpack<JoinRoomInLobbyResponse>());
        }

        //处理多人演练房间的退出结果
        else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.LeaveRoomResponse")
        {
            HandleLeaveRoomRspond(pack.Msg.Unpack<LeaveRoomResponse>());
        }
        //处理多人演练房间基础信息更新
        else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.OnRoomInfoResponse")
        {
            HandleRoomInfo(pack.Msg.Unpack<OnRoomInfoResponse>());
        }
        //处理一局回合的开始
        else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.StartGameRsp")
        {
            HandleStartGameRsp(pack.Msg.Unpack<StartGameRsp>());
        }
        //处理任务消息信息的同步
        else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.PushTaskInfoRsp")
        {
            HandlePushTaskInfoRespond(pack.Msg.Unpack<PushTaskInfoRsp>());
        }
    }

    public ImMsg pack;
    /// <summary>
    /// 处理用户登陆结果
    /// </summary>
    /// <param name="pack"></param>
    public void HandleUserLoginRespond(UserloginRsp pack)
    {
        print("用户登陆");
    }
    /// <summary>
    /// 处理加入大厅结果
    /// </summary>
    /// <param name="pack"></param>
    public void HandleJoinLobbyResponse(JoinLobbyRsp pack)
    {
        print("加入大厅");

    }
    /// <summary>
    /// 处理离开大厅的响应结果
    /// </summary>
    public void HandleLeaveLobbyRsp(LeaveLobbyRsp pack)
    {
        print("离开大厅");
    }
    /// <summary>
    /// 处理大厅信息查询结果
    /// </summary>
    public void HandleLobbyInfoRsp(LobbyInfoRsp pack)
    {
        print("大厅信息");

    }

    /// <summary>
    /// 返回大厅内房间登陆结果
    /// </summary>
    public void HandleJoinRoomInLobbyRspond(JoinRoomInLobbyResponse pack)
    {
        print("房间登陆");

    }
    //YYX
    /// <summary>
    /// 离开房间
    /// </summary>
    public void HandleLeaveRoomRspond(LeaveRoomResponse pack)
    {
        print("离开房间");

    }
    /// <summary>
    /// 返回房间信息
    /// </summary>
    /// <param name="pack"></param>
    public void HandleRoomInfo(OnRoomInfoResponse pack)
    {
        print("房间信息");

    }
    /// <summary>
    /// 处理开始游戏返回结果
    /// </summary>
    /// <param name="pack"></param>
    public void HandleStartGameRsp(StartGameRsp pack)
    {
        print("游戏开始");

    }
    /// <summary>
    /// 处理任务信息返回结果
    /// </summary>
    /// <param name="pack"></param>
    public void HandlePushTaskInfoRespond(PushTaskInfoRsp pack)
    {
        print("任务开始");

    }


    public void HandleJoinRommResponse(JoinRoomResponse pack)
    {
        this.pack = null;
        if (GameObject.Find("Canvas") != null)
        {
            GameObject.Find("Canvas").gameObject.SetActive(false);
        }

        Debug.Log("执行了");
        Debug.Log(pack.PlayerPack.Count);
        foreach (var p in pack.PlayerPack)
        {
            Debug.Log("添加角色" + p.Uid);

            //pos.x += (posindex += 2);
            GameObject g = Object.Instantiate(player, Vector3.zero, Quaternion.identity);
            if (p.Uid.Equals(ClientManager.Instance.uid))
            {
                //创建本地角色
                var controller= g.GetComponent<ThirdPersonController>();
                CharacterRistic characterRistic = g.GetComponent<CharacterRistic>();
                characterRistic.isLocal = true;
                characterRistic.username = p.Uid;
                g.AddComponent<UpStatusRequest>();
                g.AddComponent<UpdateStatus>();

                 
            }


            else
            {
                //创建其他客户端的角色
                CharacterRistic characterRistic = g.GetComponent<CharacterRistic>();
                characterRistic.isLocal = false;
                characterRistic.username = p.Uid;
                var controller = g.GetComponent<ThirdPersonController>();
                g.AddComponent<RemoteCharacter>();

                //Object.Destroy(g.GetComponentInChildren<Camera>().gameObject);

                Object.Destroy(controller.GetComponentInChildren<Camera>().gameObject);
                Object.Destroy(controller.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().gameObject);

                Object.Destroy(g.GetComponent<PlayerInput>());

            }
            players.Add(p.Uid, g);

        }

    }
    public void HandleAddPlayer(AddPlayer pack)
    {
        this.pack = null;
        GameObject g = Object.Instantiate(player, Vector3.zero, Quaternion.identity);
        //创建其他客户端的角色
        CharacterRistic characterRistic = g.GetComponent<CharacterRistic>();
        characterRistic.isLocal = false;
        characterRistic.username = pack.PlayerPack.Uid;
        var controller = g.GetComponent<ThirdPersonController>();
        g.AddComponent<RemoteCharacter>();

        //Object.Destroy(g.GetComponentInChildren<Camera>().gameObject);

        Object.Destroy(controller.GetComponentInChildren<Camera>().gameObject);
        Object.Destroy(controller.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().gameObject);

        Object.Destroy(g.GetComponent<PlayerInput>());
        players.Add(pack.PlayerPack.Uid, g);
    }

    public void HandleUpPos(UpPos pack)
    {
        var playerPack = pack.PlayerPack;
        var posPack = playerPack.PosPack;
        var animPack = playerPack.AnimPack;
        players.TryGetValue(playerPack.Uid, out GameObject g);
        g.GetComponent<RemoteCharacter>().SetState(new Vector3(posPack.X, posPack.Y, posPack.Z), posPack.RotY, animPack.Speed, animPack.Jump, animPack.Grounded,animPack.FreeFall);
    }

    public void HandleExitRoom(ExitRoom pack)
    {
        var playerPack = pack.PlayerPack;
        players.TryGetValue(playerPack.Uid, out GameObject g);
        DestroyImmediate(g);
    }

    private void OnDestroy()
    {

    }
}
