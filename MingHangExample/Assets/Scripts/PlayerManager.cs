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
            //�����˺�����ĵ�½���
            else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.UserloginRsp")
            {
                HandleUserLoginRespond(pack.Msg.Unpack<UserloginRsp>());

            }
            //������˴����ĵ�½���
            else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.JoinLobbyRsp")
            {
                HandleJoinLobbyResponse(pack.Msg.Unpack<JoinLobbyRsp>());
            }
            //������˴����Ļ�����Ϣˢ�½��
            else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.LobbyInfoRsp")
            {
                HandleLobbyInfoRsp(pack.Msg.Unpack<LobbyInfoRsp>());
            }
            //������˴������˳����
            else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.LeaveLobbyRsp")
            {
                HandleLeaveLobbyRsp(pack.Msg.Unpack<LeaveLobbyRsp>());
            }
            //���������������ļ��뷵�ؽ��
            else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.JoinRoomInLobbyResponse")
            {
                HandleJoinRoomInLobbyRspond(pack.Msg.Unpack<JoinRoomInLobbyResponse>());
            }

            //�����������������˳����
            else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.LeaveRoomResponse")
            {
                HandleLeaveRoomRspond(pack.Msg.Unpack<LeaveRoomResponse>());
            }
            //��������������������Ϣ����
            else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.OnRoomInfoResponse")
            {
                HandleRoomInfo(pack.Msg.Unpack<OnRoomInfoResponse>());
            }
            //����һ�ֻغϵĿ�ʼ
            else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.StartGameRsp")
            {
                HandleStartGameRsp(pack.Msg.Unpack<StartGameRsp>());
            }
            //����������Ϣ��Ϣ��ͬ��
            else if (pack != null && pack.Msg.TypeUrl == "type.googleapis.com/Im.PushTaskInfoRsp")
            {
                HandlePushTaskInfoRespond(pack.Msg.Unpack<PushTaskInfoRsp>());
            }
        }
        //this.pack = null;
    }

    public ImMsg pack;
    /// <summary>
    /// �����û���½���
    /// </summary>
    /// <param name="pack"></param>
    public void HandleUserLoginRespond(UserloginRsp pack)
    {
        print("�û���½���:" +pack.Result+"˵����"+pack.Description);

    }
    /// <summary>
    /// �������������
    /// </summary>
    /// <param name="pack"></param>
    public void HandleJoinLobbyResponse(JoinLobbyRsp pack)
    {
        print("����������:"+pack.Result+"˵��:" + pack.Desctiption);
  
    }
    /// <summary>
    /// �����뿪��������Ӧ���
    /// </summary>
    public void HandleLeaveLobbyRsp(LeaveLobbyRsp pack)
    {
        print("�뿪����");

    }
    /// <summary>
    /// ���������Ϣ��ѯ���
    /// </summary>
    public void HandleLobbyInfoRsp(LobbyInfoRsp pack)
    {
        print("������Ϣ�б�");
        foreach(RoomInfo info in pack.MRoomInlobby)
        {
            print("��������" + info.RoomName
                + "�������������" + info.RoomMaxusers
                + "����������" + info.RoomPeopleMomber
                + "����������������б�:" + info.Namelist
                + "��������״̬��" + info.RoomStatus
                + "��ǰ�ۿ���Ա����:" + info.ViewersNumber
                + "��ǰ������Ա����:" + info.OperatorsNumber
                + "��ǰ������ɫ��Ӧ��:"+info.UserWithRole
                + "��ǰ������ţ�" + info.CurrentTaskIndex
                );
        }
    }

    /// <summary>
    /// ���ش����ڷ����½���
    /// </summary>
    public void HandleJoinRoomInLobbyRspond(JoinRoomInLobbyResponse pack)
    {
        print("�����½");
        print("���������½�����" + pack.Result + "���Ƿ��Ƿ���:" + pack.IsMaster + "��½���������" + pack.Description);

    }
    //YYX
    /// <summary>
    /// �뿪����
    /// </summary>
    public void HandleLeaveRoomRspond(LeaveRoomResponse pack)
    {
        
        print("�뿪����");
        print("�������"+pack.PlayerName);
        print("�Ƿ�Ϊ����:"+pack.IsMaster);
        for(int i=0;i<players.Count;i++)
        {
            KeyValuePair<string, GameObject> kv = players.ElementAt(i);
            DestroyImmediate(kv.Value);
            players.Remove(kv.Key);
        }
        m_canvas.SetActive(true);
    }
    /// <summary>
    /// ���ط�����Ϣ
    /// </summary>
    /// <param name="pack"></param>
    public void HandleRoomInfo(OnRoomInfoResponse pack)
    {
        print("������Ϣ");
        print(
                "��������" + pack.RoleInfo.RoomName
                + "�������������" + pack.RoleInfo.RoomMaxusers
                + "����������" + pack.RoleInfo.RoomPeopleMomber
                + "����������������б�:" + pack.RoleInfo.Namelist
                + "��ǰ���������״̬��" + pack.RoleInfo.RoomStatus
            + "��ǰ������Ա��Ա����:" + pack.RoleInfo.OperatorsNumber
            + "��ǰ�ۿ���Ա������" + pack.RoleInfo.ViewersNumber
            + "��ǰ������ɫ��Ӧ��:" + pack.RoleInfo.UserWithRole
            + "��ǰ�������:" + pack.RoleInfo.CurrentTaskIndex);
    }
    /// <summary>
    /// ����ʼ��Ϸ���ؽ��
    /// </summary>
    /// <param name="pack"></param>
    public void HandleStartGameRsp(StartGameRsp pack)
    {
        print("������ʼ");
        
    }
    /// <summary>
    /// ����������Ϣ���ؽ��
    /// </summary>
    /// <param name="pack"></param>
    public void HandlePushTaskInfoRespond(PushTaskInfoRsp pack)
    {
        print("������Ϣ��");
        print("�����Ӧ��ʱ��" + pack.Taskid.Time + "����С�½ں�" + pack.Taskid.Taskid + "����Ĵ�ģ���" + pack.Taskid.Chaterp + "playable��״̬" + pack.Taskid.Playablestatus);

    }


    public void HandleJoinRommResponse(JoinRoomResponse pack)
    {
        this.pack = null;
        if (GameObject.Find("Canvas") != null)
        {
            GameObject.Find("Canvas").gameObject.SetActive(false);
        }
        Debug.Log("ִ����");
        Debug.Log(pack.PlayerPack.Count);
        foreach (var p in pack.PlayerPack)
        {
            Debug.Log("��ӽ�ɫ" + p.Uid);
            //pos.x += (posindex += 2);
            GameObject g = Object.Instantiate(player, Vector3.zero, Quaternion.identity);
            if (p.Uid.Equals(ClientManager.Instance.uid))
            {
                //�������ؽ�ɫ
                var controller= g.GetComponent<ThirdPersonController>();
                CharacterRistic characterRistic = g.GetComponent<CharacterRistic>();
                characterRistic.isLocal = true;
                characterRistic.username = p.Uid;
                g.AddComponent<UpStatusRequest>();
                g.AddComponent<UpdateStatus>();

                 
            }
            else
            {
                //���������ͻ��˵Ľ�ɫ
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
        Debug.Log("ִ����");
        Debug.Log(pack.PlayerPack.Count);
        foreach (var p in pack.PlayerPack)
        {
            Debug.Log("��ӽ�ɫ" + p.Uid);
            //pos.x += (posindex += 2);
            GameObject g = Object.Instantiate(player, Vector3.zero, Quaternion.identity);
            if (p.Uid.Equals(ClientManager.Instance.uid))
            {
                //�������ؽ�ɫ
                var controller = g.GetComponent<ThirdPersonController>();
                CharacterRistic characterRistic = g.GetComponent<CharacterRistic>();
                characterRistic.isLocal = true;
                characterRistic.username = p.Uid;
                g.AddComponent<UpStatusRequest>();
                g.AddComponent<UpdateStatus>();
            }
            else
            {
                //���������ͻ��˵Ľ�ɫ
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
        //���������ͻ��˵Ľ�ɫ
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
