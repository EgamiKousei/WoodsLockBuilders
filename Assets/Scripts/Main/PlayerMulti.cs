using System;
using System.Collections;
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

    // �S�v���C���[�̍��W���
    private Dictionary<string, Transform> playerTransform = new Dictionary<string, Transform>();

    public static UnityAction<Dictionary<string, PlayerActionData>> recieveCompletedHandler;

    Rigidbody rb;
    Animator anim;

    // ����X�V
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
        Multicast.SendPlayerAction("connect", new Vector3(0, 0.5f, -20f), transform.rotation.y);
    }

    //  (���[�U�[�̍s�����)��M���\�b�h
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
                anim = playerObjectMap[playerAction.user].GetComponent<Animator>();
                rb = playerObjectMap[playerAction.user].GetComponent<Rigidbody>();
                switch (playerAction.action)
                {
                    case "Jump":
                        anim.SetBool("Jump", true);
                        rb.AddForce(transform.up * PlayerManager.JumpGravi, ForceMode.Impulse);
                        StartCoroutine(JumpEnd(anim));
                        break;
                    case "MoveEnd":
                        anim.SetBool("Move", false);
                        break;
                    case "logout":
                        var otherColor = playerTransform[name].Find("Body_08b").gameObject;
                        Destroy(otherColor.GetComponent<SkinnedMeshRenderer>().materials[1].shader);
                        Destroy(playerObjectMap[playerAction.user]);
                        playerObjectMap.Remove(playerAction.user);
                        PlayerData.NameList.Remove(playerAction.user);
                        playerTransform.Remove(playerAction.user);
                        break;
                    case "Move":
                        //���[�e�[�V�����̒ǉ�
                        var tes = playerTransform[name].rotation;
                        tes.y = playerAction.rote_y;
                        playerTransform[name].rotation = tes;
                        playerTransform[name].position = new Vector3(playerAction.pos_x, playerAction.pos_y, playerAction.pos_z);
                        anim.SetBool("Move", true);
                        break;
                    case "Attack":
                        anim.SetBool("Attack", true);
                        StartCoroutine(AttackEnd(anim));
                        break;
                }

                // �������������v���C���[�̐���
            }
            else
            {
                // ���v���C���[�̍쐬
                var player = MakePlayer(new Vector3(playerAction.pos_x, playerAction.pos_y, playerAction.pos_z), playerAction.user, playerAction.color);

                // ���v���C���[���X�g�ւ̒ǉ�
                playerObjectMap.Add(playerAction.user, player);
                PlayerData.NameList.Add(playerAction.user);
            }
        }
    }
    private IEnumerator AttackEnd(Animator anim)
    {
        yield return new WaitForSeconds(0.45f);
        anim.SetBool("Attack", false);
    }
    private IEnumerator JumpEnd(Animator anim)
    {
        yield return new WaitForSeconds(0.8f);
        anim.SetBool("Jump", false);
    }

    // �v���C���[���쐬
    private GameObject MakePlayer(Vector3 pos, string name,string color)
    {
        // �v���C���[�̃��\�[�X(�v���n�u)���擾 ������̂�
        playerPrefab = playerPrefab ?? (GameObject)Resources.Load("OtherPlayer");

        // �v���C���[�𐶐�
        var player = Instantiate(playerPrefab, pos, Quaternion.identity);

        playerTransform.Add(name, player.transform);

        // �v���C���[�̃l�[���v���[�g�̐ݒ�
        var otherNameText = playerTransform[name].Find("TxtUserName").gameObject;
        otherNameText.GetComponent<TextMesh>().text = name;

        //�v���C���[�̐F�̐ݒ�
        var otherColor1 = playerTransform[name].Find("Body_08b").gameObject;
        var otherColor2 = playerTransform[name].Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/HEAD_CONTAINER/Head_08b").gameObject;
        var otherColor3 = playerTransform[name].Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand/L_shield_container/shield_12").gameObject;
        float r = (Convert.ToInt32(color, 16) >> 16) & 0xff;
        float g = (Convert.ToInt32(color, 16) >> 8) & 0xff;
        float b = Convert.ToInt32(color, 16) & 0xff;
        otherColor1.GetComponent<SkinnedMeshRenderer>().materials[1].color = new Color(r / 255, g / 255, b / 255);
        otherColor2.GetComponent<MeshRenderer>().materials[1].color = new Color(r / 255, g / 255, b / 255);
        otherColor3.GetComponent<MeshRenderer>().materials[1].color = new Color(r / 255, g / 255, b / 255);
        return player;
    }

    private void OnApplicationQuit()
    {
        Multicast.SendPlayerAction("logout", Vector3.zero, 0.0f);
        Resources.UnloadUnusedAssets();
    }
}