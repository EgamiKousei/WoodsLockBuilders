using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    int ItemNum=1;
    private Animator _animator;
    public static int attackParamHash;
    public GameObject mainCamera, gridCamera,Grid,pivot, CreateMap,Script;
    GameObject itemObject=null,itemNow;
    public Sprite nullItem;

    public enum Player
    {
        Attack,
        Set,
        Field,
        Cannon,
    };
    public static Player playerScean;
    public GameObject Attack, Set, Break,ItemPanel,setItem;

    private void Awake()
    {
        playerScean = Player.Attack;
        Attack.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        transform.position = new Vector3(0, 0, -15);
        attackParamHash = Animator.StringToHash("Attack");
        itemNow = ItemPanel.transform.Find("Item" + ItemNum).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            switch (playerScean)
            {
                case Player.Attack:
                    playerScean = Player.Set;
                    ItemPanel.SetActive(true);
                    mainCamera.SetActive(false);
                    gridCamera.SetActive(true);
                    Grid.SetActive(true);
                    pivot.transform.eulerAngles = Vector3.zero;
                    Attack.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
                    Set.GetComponent<RectTransform>().sizeDelta= new Vector2(60, 60);
                    break;
                case Player.Set:
                    playerScean = Player.Field;
                    ItemPanel.SetActive(false);
                    mainCamera.SetActive(true);
                    gridCamera.SetActive(false);
                    Grid.SetActive(false);
                    Set.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
                    Break.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
                    Script.GetComponent<PlayerData>().SaveRoom();
                    break;
                case Player.Field:
                    playerScean = Player.Attack;
                    Break.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
                    Attack.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
                    break;
            }
        }
        if (playerScean==Player.Attack)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _animator.SetBool(attackParamHash, true);
                Multicast.SendPlayerAction("Attack", transform.position, transform.rotation.y);
                Invoke("AttackEnd", 0.45f);   
            }
        }

        if (Input.GetKeyDown(KeyCode.F)&& playerScean==Player.Set)
        {
            itemNow.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
            itemNow.transform.Find("Image").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
            ItemNum++;
            if (ItemNum > 8) 
                ItemNum = 1;
            itemNow = ItemPanel.transform.Find("Item" + ItemNum).gameObject;
            itemNow.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
            itemNow.transform.Find("Image").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        }

        //配置処理
        if(playerScean == Player.Set && Input.GetMouseButtonDown(0))
        {
            Ray ray = gridCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            // 位置調整用変数
            float hitX = 0.5f;
            float hitZ = 0.5f;

            if (Physics.Raycast(ray, out hit))
            {
                // 位置調整
                if (hit.point.x < 0)
                {
                    hitX *= -1;
                }
                if (hit.point.z < 0)
                {
                    hitZ *= -1;
                }
                float distance = hit.point.y;
                foreach (var i in PlayerData.ItemBox.Values)
                {
                    if (i.invno == ItemNum&&i.category=="家具"&&i.invnum>0)
                    {
                        itemObject = (GameObject)Resources.Load(i.name);
                        Vector3 arPoint;
                        if (i.name == "Box")
                             arPoint = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, distance+0.5f, Mathf.Floor(hit.point.z) + 0.5f);
                        else
                             arPoint = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, distance, Mathf.Floor(hit.point.z) + 0.5f);
                        GameObject obj = Instantiate(itemObject, arPoint, Quaternion.identity);
                        Vector3 rotationAngles = new Vector3(-90, 0, 0);
                        obj.transform.rotation = Quaternion.Euler(rotationAngles);
                        obj.transform.parent = CreateMap.transform;
                        RoomSet.objNum++;
                        obj.name = RoomSet.objNum.ToString();
                        var roomData = new roomData
                        {
                            name = i.name,
                            x = Mathf.Floor(hit.point.x) + 0.5f,
                            y = distance,
                            z = Mathf.Floor(hit.point.z) + 0.5f,
                            xr = -90,
                            num = RoomSet.objNum,
                        };
                        if (i.name == "Box")
                            roomData.y += 0.5f;
                        PlayerData.PlayMap.Add(RoomSet.objNum, roomData);
                        i.invnum --;
                        itemNow.transform.Find("Text (Legacy)").gameObject.GetComponent<Text>().text = i.invnum.ToString();
                        if (i.invnum == 0)
                        {
                            int invno=0;
                            foreach (var x in PlayerData.ItemBox.Values)
                            {
                                if (x.invno > invno)
                                    invno = x.invno;
                                if (x.invno > i.invno)
                                    x.invno--;
                            }
                            ItemSet.ItemPanel[invno-1].transform.Find("Image").gameObject.GetComponent<Image>().sprite = nullItem;
                            ItemSet.ItemPanel[invno - 1].transform.Find("Text (Legacy)").gameObject.GetComponent<Text>().text = "0";
                            i.invno = 0;
                            setItem.GetComponent<ItemSet>().SetItem();
                        }
                    }
                }
            }
        }
        //回転処理
        if (playerScean == Player.Set&&Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(1))
        {
            Ray ray = gridCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                if (name != "Box")
                {
                    Vector3 rotationAngles = hit.collider.gameObject.transform.rotation.eulerAngles;
                    rotationAngles.y = rotationAngles.y + 90;
                    hit.collider.gameObject.transform.rotation = Quaternion.Euler(rotationAngles);
                    foreach (var x in PlayerData.PlayMap.Values)
                    {
                        if (x.num == Int32.Parse(hit.collider.gameObject.name))
                            x.yr = rotationAngles.y;
                    }
                }
            }
        }


            //撤去処理
           else if (playerScean == Player.Set && Input.GetMouseButtonDown(1))
        {
            Ray ray = gridCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag!="Plane"&& hit.collider.gameObject.tag!=null&& hit.collider.gameObject.tag != "Untagged")
                {
                    if(hit.collider.gameObject!=null)
                    SetItem(hit.collider.gameObject.tag, hit.collider.gameObject);
                }
            }
        }
        }

    public void SetItem(string name,GameObject hit)
    {
        foreach (var i in PlayerData.ItemBox.Values)
        {
            if (i.name == name)
            {
                if (i.invnum > 0)
                    i.invnum++;
                else if (i.bagnum > 0)
                    i.bagnum++;
                else
                {
                    int bagno=1;
                    foreach (var x in PlayerData.ItemBox.Values)
                    {
                        if (x.bagno >= bagno)
                        {
                            bagno = x.bagno + 1;
                        }
                    }
                    i.bagno = bagno;
                    i.bagnum++;
                }
            }
        }
        setItem.GetComponent<ItemSet>().SetItem();
            if (RoomSet.objNum == Int32.Parse(hit.name))
                RoomSet.objNum--;
            PlayerData.PlayMap.Remove(key: Int32.Parse(hit.name));
            Destroy(hit);
    }


    private void AttackEnd()
    {
        _animator.SetBool(attackParamHash, false);
    }
}