using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMulti : MonoBehaviour
{
    private GameObject playerPrefab = null;     // �v���C���[�̃��\�[�X(�v���n�u)

    // �S�v���C���[�̍s�����
    private Dictionary<string, PlayerActionData> PlayerActionMap;

    // �S�v���C���[�̃I�u�W�F�N�g���
    private readonly Dictionary<string, GameObject> playerObjectMap = new Dictionary<string, GameObject>();

    public static UnityAction<Dictionary<string, PlayerActionData>> recieveCompletedHandler;

    Rigidbody rb;

    /// ����X�V
    /// </summary>
    void Update()
    {
        // ���[�U�[�̍s����񂪂������瓯���������s���A���[�U�[�̍s������������
        if (PlayerActionMap != null)
        {
            Synchronaize();
            PlayerActionMap = null;
        }
    }

    void Start()
    {
        var otherNameText = transform.Find("TxtUserName").gameObject;
        otherNameText.GetComponent<TextMesh>().text = PlayerData.PlayerName;

        // WebSocket�̃��b�Z�[�W��M���\�b�h�̐ݒ�
        recieveCompletedHandler += OnReciveMessage;

        // ���v���C���[�̏�������WebSocket�ɑ��M
        Multicast.SendPlayerAction("connect", new Vector3(0,0.5f,-20f), transform.rotation.y);
    }

    /// <summary>
    ///  (���[�U�[�̍s�����)��M���\�b�h
    /// </summary>
    /// <param name="synchronizeData"></param>
    private void OnReciveMessage(Dictionary<string, PlayerActionData> PlayerActionMap)
    {
        // ���������擾
        this.PlayerActionMap = PlayerActionMap;
    }

    private void Synchronaize()
    {
        // �ޏo�������v���C���[�̌���
        /*List<string> otherPlayerNameList = new List<string>(playerObjectMap.Keys);
        foreach (var otherPlayerName in otherPlayerNameList)
        {
            // �ޏo�����v���C���[�̍폜
            if (!PlayerActionMap.ContainsKey(otherPlayerName))
            {
                Destroy(playerObjectMap[otherPlayerName]);
                playerObjectMap.Remove(otherPlayerName);
            }
        }*/
        // �v���C���[�̈ʒu���X�V
        foreach (var playerAction in PlayerActionMap.Values)
        {            // �����͈ړ��ς݂Ȃ̂ŃX���[
            if (PlayerData.PlayerName == playerAction.user)
            {
                continue;
            }
            // �������̑��v���C���[�̈ړ�
            if (playerObjectMap.ContainsKey(playerAction.user))
            {
                switch (playerAction.action)
                {
                    case "jump":
                        rb = playerObjectMap[playerAction.user].GetComponent<Rigidbody>();
                        rb.AddForce(transform.up * PlayerManager.JumpGravi, ForceMode.Impulse);
                        break;
                    default:
                        playerObjectMap[playerAction.user].transform.position = new Vector3(playerAction.pos_x, playerAction.pos_y, playerAction.pos_z);

                        //���[�e�[�V�����̒ǉ�
                        var tes = playerObjectMap[playerAction.user].transform.rotation;
                        tes.y = playerAction.rote_y;
                        playerObjectMap[playerAction.user].transform.rotation = tes;
                        break;
                }

                // �������������v���C���[�̐���
            }
            else
            {
                // ���v���C���[�̍쐬
                var player = MakePlayer(new Vector3(playerAction.pos_x, playerAction.pos_y, playerAction.pos_z), playerAction.user);

                // ���v���C���[���X�g�ւ̒ǉ�
                playerObjectMap.Add(playerAction.user, player);
            }
        }
    }

    /// <summary>
    /// �v���C���[���쐬
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="name"></param>
    private GameObject MakePlayer(Vector3 pos, string name)
    {
        // �v���C���[�̃��\�[�X(�v���n�u)���擾 ������̂�
        playerPrefab = playerPrefab ?? (GameObject)Resources.Load("SphPlayer");

        // �v���C���[�𐶐�
        var player = Instantiate(playerPrefab, pos, Quaternion.identity);

        // �v���C���[�̃l�[���v���[�g�̐ݒ�
        var otherNameText = player.transform.Find("TxtUserName").gameObject;
        otherNameText.GetComponent<TextMesh>().text = name;
        return player;
    }
}