using UnityEngine;
using UnityEngine.EventSystems;

public class ActionManager : MonoBehaviour
{
    int ItemNum=1;
    private Animator _animator;
    public static int attackParamHash;
    public GameObject setObject;

    public enum Player
    {
        Attack,
        Set,
        Destroy,
        Cannon,
    };
    public static Player playerScean;
    public GameObject Attack, Set, Break,ItemPanel;

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
                    Break.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
                    break;
                case Player.Destroy:
                    playerScean = Player.Attack;
                    Break.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
                    Attack.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
                    break;
            }
        }
        if (PlayerData.ItemBox.ContainsKey("‘•”õ")&& playerScean==Player.Attack)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _animator.SetBool(attackParamHash, true);
                Multicast.SendPlayerAction("Attack", transform.position, transform.rotation.y);
                Invoke("AttackEnd", 0.45f);   
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            switch (playerScean)
            {
                case Player.Set:
                    Debug.Log("”z’u");
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
        _animator.SetBool(attackParamHash, false);
    }
}