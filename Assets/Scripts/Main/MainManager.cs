using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainManager : MonoBehaviour
{
    int ItemNum=1;
    private Animator _animator;

    enum Player
    {
        Attack,
        Set,
        Destroy
    };
    Player playerScean;
    public GameObject Attack, Set, Destroy,ItemPanel, mainCamera, aimPlace;

    private void Awake()
    {
        playerScean = Player.Attack;
        Attack.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
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
                    Attack.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
                    Set.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
                    break;
                case Player.Set:
                    playerScean = Player.Destroy;
                    ItemPanel.SetActive(false);
                    Set.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
                    Destroy.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
                    break;
                case Player.Destroy:
                    playerScean = Player.Attack;
                    Destroy.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
                    Attack.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
                    break;
            }
        }
        if (PlayerData.ItemBox.ContainsKey("ëïîı")&& playerScean==Player.Attack)
        {
            switch (PlayerData.ItemBox["ëïîı"].category)
            {
                case "åï":
                    if (Input.GetMouseButtonDown(0))
                    {
                        _animator.SetBool("Attack", true);
                        Multicast.SendPlayerAction("Attack", transform.position, transform.rotation.y);
                        Invoke("AttackEnd", 0.45f);
                    }
                    break;
                case "ã|":
                    if (Input.GetMouseButtonDown(0))
                    {
                        mainCamera.transform.localPosition = new Vector3(0f, 0f, 10f);
                    }
                    if (Input.GetMouseButton(0))
                    {
                        aimPlace.SetActive(true);
                        Debug.Log("ó≠Çﬂ");
                    }
                    // î≠éÀ
                    else if (Input.GetMouseButtonUp(0))
                    {
                        _animator.SetBool("Attack", true);
                        Invoke("AttackEnd", 1f);
                        Debug.Log("âìäuçUåÇ");
                        mainCamera.transform.localPosition = new Vector3(0f, 0f, 0f);
                        aimPlace.SetActive(false);
                    }
                    break;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            switch (playerScean)
            {
                case Player.Set:
                    Debug.Log("îzíu");
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.F)&& playerScean==Player.Set)
        {
            GameObject child = ItemPanel.transform.Find("Item" + ItemNum).gameObject;
            child.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
            ItemNum++;
            if (ItemNum <= 8)
            {
                child = ItemPanel.transform.Find("Item" + ItemNum).gameObject;
                child.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
            }
            else
            {
                ItemNum = 1;
                child = ItemPanel.transform.Find("Item" + ItemNum).gameObject;
                child.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
            }
        }
    }

    private void AttackEnd()
    {
        _animator.SetBool("Attack", false);
        Multicast.SendPlayerAction("AttackEnd", transform.position, transform.rotation.y);
    }
}