using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player1 : MonoBehaviour
{
    //キャラクターの操作状態を管理するフラグ
    [SerializeField] public bool onGround = true;
    [SerializeField] public bool inJumping = false;

    //rigidbodyオブジェクト格納用変数
    Rigidbody rb;

    //移動速度の定義
    float speed = 6f;

    //ダッシュ速度の定義
    float sprintspeed = 9f;

    //移動の係数格納用変数
    float v;
    float h;

    // HP
    float hp = 100;
    float maxHp = 100;

    // 移動可能かどうか
    public bool isMove = true;

    // HPスライダー本体
    GameObject hpSlider;

    // HPスライダー
    Slider slider;

    // HPテキスト
    GameObject hpText;
    Text text;

    // 武器イメージ
    GameObject sword;
    GameObject bow;
    GameObject temp;
    Image swordImage;
    Image bowImage;
    Image tempImage;

    // 武器タイプ識別コード
    int weaponType = 1;

    // 攻撃中かどうか
    bool isAtc = false;

    // アイテム取得用コード
    public string code;

    // 遠隔攻撃用ターゲットとカメラ
    [SerializeField]
    private GameObject mainTarget;
    [SerializeField]
    private GameObject aimTarget;
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private float cameraSpeed;
    [SerializeField]
    private GameObject aimPlace;
    [SerializeField]
    private GameObject pivot;
    bool isMoveCamera = false;
    bool isChange = true;

    // 椅子のターゲット
    [SerializeField]
    private GameObject chairTarget;
    Vector3 oldPosition;

    // アイテムオブジェクト
    [SerializeField]
    private GameObject itemObj;

    // アイテムマネージャースクリプト
    ItemManager im;

    // ゲームマネージャースクリプト
    GameManager gm;

    // 配置状態のカメラ
    [SerializeField]
    private GameObject argCamera;
    public bool isArg = false;

    // グリッド
    [SerializeField]
    private GameObject grid;


    private void Start()
    {
        // HPスライダー取得
        hpSlider = GameObject.Find("HP");
        slider = hpSlider.GetComponent<Slider>();
        // HPテキスト取得
        hpText = GameObject.Find("HPText");
        text = hpText.GetComponent<Text>();
        // 武器イメージ取得
        sword = GameObject.Find("Sword");
        bow = GameObject.Find("Bow");
        temp = GameObject.Find("Temp");
        swordImage = sword.GetComponent<Image>();
        bowImage = bow.GetComponent<Image>();
        tempImage= temp.GetComponent<Image>();
        rb = GetComponent<Rigidbody>();

        // ゲームマネージャー取得
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //　アイテムマネージャー取得
        im = GameObject.Find("GameManager").GetComponent<ItemManager>();

        // 初期設定
        text.text = hp.ToString() + "/" + maxHp.ToString();
        slider.value = 1;
        code = null;
    }

    void Update() 
    {
        /*if (gm.isMenu == false)
        {*/
            // サンプルダメージ
            if (Input.GetKeyDown(KeyCode.L))
            {
                hp -= 30;
                slider.value = hp / 100;
                text.text = hp.ToString() + "/" + maxHp.ToString();
            }

            // サンプル武器入れ替え
            if (Input.GetKeyDown(KeyCode.Q) && isChange == true)
            {
                tempImage.sprite = bowImage.sprite;
                bowImage.sprite = swordImage.sprite;
                swordImage.sprite = tempImage.sprite;
                weaponType *= -1;
            }

            // 配置状態に移行
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (isArg == false)
                {
                    isArg = true;
                    argCamera.SetActive(true);
                    mainCamera.SetActive(false);
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f); // 0°に設定
                    pivot.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // 0°に設定
                    grid.SetActive(true);
                }
                else
                {
                    isArg = false;
                    mainCamera.SetActive(true);
                    argCamera.SetActive(false);
                    grid.SetActive(false);
                }
            }

            // オブジェクトを配置する処理
            if (im.kindNo == 2 && isAtc == false && isArg == true)
            {
                Ray ray = argCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                // 位置調整用変数
                float hitX = 0.5f;
                float hitZ = 0.5f;
                //Debug.Log(1);
                if (Input.GetMouseButtonDown(1))
                {
                    //Debug.Log(2);
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
                        Vector3 arPoint = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 0.6f, Mathf.Floor(hit.point.z) + 0.5f);
                        Debug.Log(arPoint);
                        Instantiate(itemObj, arPoint, Quaternion.identity);
                        im.GetItem(im.itemCode).DecInv(1);
                        // インベントリ内のアイテム数が0になったら
                        if (im.GetItem(im.itemCode).GetInvNum() == 0)
                        {
                            im.GetItem(im.itemCode).SetInvNo(0);
                            im.isInv = true;
                        }
                    }
                }
            }

            // 攻撃処理
            if ((Input.GetMouseButtonDown(0) || isAtc == true) && isArg == false)
            {
                // 攻撃中状態に
                isAtc = true;
                isChange = false;

                // 近接攻撃
                if (weaponType == 1)
                {
                    Debug.Log("近接攻撃");
                    isAtc = false;
                    isChange = true;
                }
                // 遠隔攻撃
                else if (weaponType == -1)
                {
                    // 溜め
                    if (Input.GetMouseButton(0))
                    {
                        mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, aimTarget.transform.position, cameraSpeed);
                        aimPlace.SetActive(true);
                        Debug.Log("溜め");
                    }
                    // 発射
                    else if (Input.GetMouseButtonUp(0))
                    {
                        Debug.Log("遠隔攻撃");
                    }

                    // 遠隔攻撃をやめる
                    if (Input.GetMouseButtonDown(1))
                    {
                        isAtc = false;
                        isMoveCamera = true;
                        aimPlace.SetActive(false);

                    }

                }
            }

            // カメラを元の位置に戻す処理
            if (isMoveCamera == true)
            {
                mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, mainTarget.transform.position, cameraSpeed);

                if (mainCamera.transform.position.x <= mainTarget.transform.position.x && mainCamera.transform.position.z <= mainTarget.transform.position.z)
                {
                    isMoveCamera = false;
                    isChange = true;
                }
            }

            if (isMove == true)
            {
                //Shift+上下キーでダッシュ、上下キーで通常移動,それ以外は停止
                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
                    v = Time.deltaTime * sprintspeed;
                else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
                    v = -Time.deltaTime * sprintspeed;
                else if (Input.GetKey(KeyCode.W))
                    v = Time.deltaTime * speed;
                else if (Input.GetKey(KeyCode.S))
                    v = -Time.deltaTime * speed;
                else
                    v = 0;

                //Shift+左右キーでダッシュ、左右キーで通常移動,それ以外は停止
                if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
                    h = Time.deltaTime * sprintspeed;
                else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
                    h = -Time.deltaTime * sprintspeed;
                else if (Input.GetKey(KeyCode.D))
                    h = Time.deltaTime * speed;
                else if (Input.GetKey(KeyCode.A))
                    h = -Time.deltaTime * speed;
                else
                    h = 0;

                //移動の実行
                if (!inJumping)//空中での移動を禁止
                {
                    transform.position += transform.forward * v;
                    transform.position += transform.right * h;
                }

                //スペースボタンでジャンプする
                if (onGround)
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        //ジャンプさせるため上方向に力を発生
                        rb.AddForce(transform.up * 8, ForceMode.Impulse);
                        //ジャンプ中は地面との接触判定をfalseにする
                        onGround = false;
                        inJumping = true;

                        //前後キーを押しながらジャンプしたときは前後方向の力も発生
                        if (Input.GetKey(KeyCode.W))
                        {
                            rb.AddForce(transform.forward * 6f, ForceMode.Impulse);
                        }
                        else if (Input.GetKey(KeyCode.S))
                        {
                            rb.AddForce(transform.forward * -3f, ForceMode.Impulse);
                        }
                        else if (Input.GetKey(KeyCode.D))
                        {
                            rb.AddForce(transform.right * 4f, ForceMode.Impulse);
                        }
                        else if (Input.GetKey(KeyCode.A))
                        {
                            rb.AddForce(transform.right * -4f, ForceMode.Impulse);
                        }
                    }
                }

            //}
        }
            
    }


    //地面に接触したときにはonGroundをtrue、injumpingをfalseにする
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            onGround = true;
            inJumping = false;
        }

        // アイテムにヒットした際
        if (col.gameObject.tag == "Item")
        {
            code = col.gameObject.GetComponent<ItemCode>().code;
            Destroy(col.gameObject);
            //Debug.Log(GetItem(code).GetInformation());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Chair")
        {
            Debug.Log("F");
            if (Input.GetKeyDown(KeyCode.F))
            {
                oldPosition = this.gameObject.transform.position;
                this.gameObject.transform.position = chairTarget.transform.position;
            }
        }
    }
}
