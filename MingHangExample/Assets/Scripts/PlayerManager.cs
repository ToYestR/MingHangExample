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
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    GameObject player;
    public GameObject m_canvas;
    private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    static PlayerManager _instance;
    public Queue<ImMsg> immsgQ = new Queue<ImMsg>();

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
        if (immsgQ.Count > 0)
        {
            pack = immsgQ.Dequeue();
            if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.JoinRoomResponse")
            {
                HandleJoinRommResponse(pack.Msg.Unpack<JoinRoomResponse>());

            }
            else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.CreatePlayerResponse")
            {
                HandleCreatePlayerResponse(pack.Msg.Unpack<CreatePlayerResponse>());

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
            else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.JoinLobbyRsp")
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
        //this.pack = null;
    }

    public ImMsg pack;
    /// <summary>
    /// 处理用户登陆结果
    /// </summary>
    /// <param name="pack"></param>
    public void HandleUserLoginRespond(UserloginRsp pack)
    {
        print("用户登陆结果:" +pack.Result+"说明："+pack.Description);

    }
    /// <summary>
    /// 处理加入大厅结果
    /// </summary>
    /// <param name="pack"></param>
    public void HandleJoinLobbyResponse(JoinLobbyRsp pack)
    {
        print("加入大厅结果:"+pack.Result+"说明:" + pack.Desctiption);
  
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
        print("房间信息列表：");
        foreach(RoomInfo info in pack.MRoomInlobby)
        {
            print("房间名：" + info.RoomName
                + "房间最大人数：" + info.RoomMaxusers
                + "房间人数：" + info.RoomPeopleMomber
                + "房间所有玩家名称列表:" + info.Namelist
                + "房间任务状态：" + info.RoomStatus
                + "当前观看人员数量:" + info.ViewersNumber
                + "当前操作人员数量:" + info.OperatorsNumber
                + "当前操作角色对应表:"+info.UserWithRole
                + "当前任务序号：" + info.CurrentTaskIndex
                );
        }
    }

    /// <summary>
    /// 返回大厅内房间登陆结果
    /// </summary>
    public void HandleJoinRoomInLobbyRspond(JoinRoomInLobbyResponse pack)
    {
        print("房间登陆");
        print("演练房间登陆结果：" + pack.Result + "我是否是房主:" + pack.IsMaster + "登陆结果描述：" + pack.Description);

    }
    //YYX
    /// <summary>
    /// 离开房间
    /// </summary>
    public void HandleLeaveRoomRspond(LeaveRoomResponse pack)
    {
        
        print("离开房间");
        print("玩家名："+pack.PlayerName);
        print("是否为房主:"+pack.IsMaster);
        for(int i=0;i<players.Count;i++)
        {
            KeyValuePair<string, GameObject> kv = players.ElementAt(i);
            DestroyImmediate(kv.Value);
            players.Remove(kv.Key);
        }
        m_canvas.SetActive(true);
    }
    /// <summary>
    /// 返回房间信息
    /// </summary>
    /// <param name="pack"></param>
    public void HandleRoomInfo(OnRoomInfoResponse pack)
    {
        print("房间信息");
        print(
                "房间名：" + pack.RoleInfo.RoomName
                + "房间最大人数：" + pack.RoleInfo.RoomMaxusers
                + "房间人数：" + pack.RoleInfo.RoomPeopleMomber
                + "房间所有玩家名称列表:" + pack.RoleInfo.Namelist
                + "当前房间的任务状态：" + pack.RoleInfo.RoomStatus
            + "当前操作人员人员数量:" + pack.RoleInfo.OperatorsNumber
            + "当前观看人员数量：" + pack.RoleInfo.ViewersNumber
            + "当前操作角色对应表:" + pack.RoleInfo.UserWithRole
            + "当前任务序号:" + pack.RoleInfo.CurrentTaskIndex);
    }
    /// <summary>
    /// 处理开始游戏返回结果
    /// </summary>
    /// <param name="pack"></param>
    public void HandleStartGameRsp(StartGameRsp pack)
    {
        print("演练开始");
        
    }
    /// <summary>
    /// 处理任务信息返回结果
    /// </summary>
    /// <param name="pack"></param>
    public void HandlePushTaskInfoRespond(PushTaskInfoRsp pack)
    {
        print("任务信息：");
        print("任务对应的时间" + pack.Taskid.Time + "任务小章节号" + pack.Taskid.Taskid + "任务的大模块号" + pack.Taskid.Chaterp + "playable的状态" + pack.Taskid.Playablestatus);

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

    public void HandleCreatePlayerResponse(CreatePlayerResponse pack)
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
                var controller = g.GetComponent<ThirdPersonController>();
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
        for (int i = 0; i < players.Count; i++)
        {
            KeyValuePair<string, GameObject> kv = players.ElementAt(i);
            DestroyImmediate(kv.Value);
            players.Remove(kv.Key);
        }
        m_canvas.SetActive(true);

    }

    private void OnDestroy()
    {

    }
}
