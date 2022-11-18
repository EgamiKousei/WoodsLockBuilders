using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player1 : MonoBehaviour
{
    //�L�����N�^�[�̑����Ԃ��Ǘ�����t���O
    [SerializeField] public bool onGround = true;
    [SerializeField] public bool inJumping = false;

    //rigidbody�I�u�W�F�N�g�i�[�p�ϐ�
    Rigidbody rb;

    //�ړ����x�̒�`
    float speed = 6f;

    //�_�b�V�����x�̒�`
    float sprintspeed = 9f;

    //�ړ��̌W���i�[�p�ϐ�
    float v;
    float h;

    // HP
    float hp = 100;
    float maxHp = 100;

    // �ړ��\���ǂ���
    public bool isMove = true;

    // HP�X���C�_�[�{��
    GameObject hpSlider;

    // HP�X���C�_�[
    Slider slider;

    // HP�e�L�X�g
    GameObject hpText;
    Text text;

    // ����C���[�W
    GameObject sword;
    GameObject bow;
    GameObject temp;
    Image swordImage;
    Image bowImage;
    Image tempImage;

    // ����^�C�v���ʃR�[�h
    int weaponType = 1;

    // �U�������ǂ���
    bool isAtc = false;

    // �A�C�e���擾�p�R�[�h
    public string code;

    // ���u�U���p�^�[�Q�b�g�ƃJ����
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

    // �֎q�̃^�[�Q�b�g
    [SerializeField]
    private GameObject chairTarget;
    Vector3 oldPosition;

    // �A�C�e���I�u�W�F�N�g
    [SerializeField]
    private GameObject itemObj;

    // �A�C�e���}�l�[�W���[�X�N���v�g
    ItemManager im;

    // �Q�[���}�l�[�W���[�X�N���v�g
    GameManager gm;

    // �z�u��Ԃ̃J����
    [SerializeField]
    private GameObject argCamera;
    public bool isArg = false;

    // �O���b�h
    [SerializeField]
    private GameObject grid;


    private void Start()
    {
        // HP�X���C�_�[�擾
        hpSlider = GameObject.Find("HP");
        slider = hpSlider.GetComponent<Slider>();
        // HP�e�L�X�g�擾
        hpText = GameObject.Find("HPText");
        text = hpText.GetComponent<Text>();
        // ����C���[�W�擾
        sword = GameObject.Find("Sword");
        bow = GameObject.Find("Bow");
        temp = GameObject.Find("Temp");
        swordImage = sword.GetComponent<Image>();
        bowImage = bow.GetComponent<Image>();
        tempImage= temp.GetComponent<Image>();
        rb = GetComponent<Rigidbody>();

        // �Q�[���}�l�[�W���[�擾
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //�@�A�C�e���}�l�[�W���[�擾
        im = GameObject.Find("GameManager").GetComponent<ItemManager>();

        // �����ݒ�
        text.text = hp.ToString() + "/" + maxHp.ToString();
        slider.value = 1;
        code = null;
    }

    void Update() 
    {
        /*if (gm.isMenu == false)
        {*/
            // �T���v���_���[�W
            if (Input.GetKeyDown(KeyCode.L))
            {
                hp -= 30;
                slider.value = hp / 100;
                text.text = hp.ToString() + "/" + maxHp.ToString();
            }

            // �T���v���������ւ�
            if (Input.GetKeyDown(KeyCode.Q) && isChange == true)
            {
                tempImage.sprite = bowImage.sprite;
                bowImage.sprite = swordImage.sprite;
                swordImage.sprite = tempImage.sprite;
                weaponType *= -1;
            }

            // �z�u��ԂɈڍs
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (isArg == false)
                {
                    isArg = true;
                    argCamera.SetActive(true);
                    mainCamera.SetActive(false);
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f); // 0���ɐݒ�
                    pivot.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // 0���ɐݒ�
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

            // �I�u�W�F�N�g��z�u���鏈��
            if (im.kindNo == 2 && isAtc == false && isArg == true)
            {
                Ray ray = argCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                // �ʒu�����p�ϐ�
                float hitX = 0.5f;
                float hitZ = 0.5f;
                //Debug.Log(1);
                if (Input.GetMouseButtonDown(1))
                {
                    //Debug.Log(2);
                    if (Physics.Raycast(ray, out hit))
                    {
                        // �ʒu����
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
                        // �C���x���g�����̃A�C�e������0�ɂȂ�����
                        if (im.GetItem(im.itemCode).GetInvNum() == 0)
                        {
                            im.GetItem(im.itemCode).SetInvNo(0);
                            im.isInv = true;
                        }
                    }
                }
            }

            // �U������
            if ((Input.GetMouseButtonDown(0) || isAtc == true) && isArg == false)
            {
                // �U������Ԃ�
                isAtc = true;
                isChange = false;

                // �ߐڍU��
                if (weaponType == 1)
                {
                    Debug.Log("�ߐڍU��");
                    isAtc = false;
                    isChange = true;
                }
                // ���u�U��
                else if (weaponType == -1)
                {
                    // ����
                    if (Input.GetMouseButton(0))
                    {
                        mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, aimTarget.transform.position, cameraSpeed);
                        aimPlace.SetActive(true);
                        Debug.Log("����");
                    }
                    // ����
                    else if (Input.GetMouseButtonUp(0))
                    {
                        Debug.Log("���u�U��");
                    }

                    // ���u�U������߂�
                    if (Input.GetMouseButtonDown(1))
                    {
                        isAtc = false;
                        isMoveCamera = true;
                        aimPlace.SetActive(false);

                    }

                }
            }

            // �J���������̈ʒu�ɖ߂�����
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
                //Shift+�㉺�L�[�Ń_�b�V���A�㉺�L�[�Œʏ�ړ�,����ȊO�͒�~
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

                //Shift+���E�L�[�Ń_�b�V���A���E�L�[�Œʏ�ړ�,����ȊO�͒�~
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

                //�ړ��̎��s
                if (!inJumping)//�󒆂ł̈ړ����֎~
                {
                    transform.position += transform.forward * v;
                    transform.position += transform.right * h;
                }

                //�X�y�[�X�{�^���ŃW�����v����
                if (onGround)
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        //�W�����v�����邽�ߏ�����ɗ͂𔭐�
                        rb.AddForce(transform.up * 8, ForceMode.Impulse);
                        //�W�����v���͒n�ʂƂ̐ڐG�����false�ɂ���
                        onGround = false;
                        inJumping = true;

                        //�O��L�[�������Ȃ���W�����v�����Ƃ��͑O������̗͂�����
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


    //�n�ʂɐڐG�����Ƃ��ɂ�onGround��true�Ainjumping��false�ɂ���
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            onGround = true;
            inJumping = false;
        }

        // �A�C�e���Ƀq�b�g������
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
