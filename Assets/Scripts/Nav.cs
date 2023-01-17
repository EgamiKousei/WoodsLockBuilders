using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



// �_�b�V�������f�[�^�i�z��j
[System.Serializable]
public class DashData
{
	public dashData[] data;
}

// �_�b�V�������f�[�^�i�ϐ��j
[System.Serializable]
public class dashData
{
	public float sr;	// ����p�x�ŏ��l
	public float gr;	// ����p�x�ő�l
	public bool ch;		// ����p�x���L�^����Ȃ�true
}



[RequireComponent(typeof(NavMeshAgent))]	// NavMesh�g�p���鍇�}
public class Nav : MonoBehaviour
{
	Renderer targetRenderer; // ���肵�����I�u�W�F�N�g��renderer�ւ̎Q��

	string datapath;						// �_�b�V���f�[�^�܂ł̃p�X
	DashData dh;                            // �_�b�V���f�[�^�i�[�ϐ�

	string datapath2;                       // NPC�f�[�^�܂ł̃p�X
	NpcData npc;                            // NPC�f�[�^�i�[�ϐ�

	string datapath3;                       // NPC�f�[�^�܂ł̃p�X
	Npc1Data npc1;                          // NPC�f�[�^�i�[�ϐ�

	string datapath4;                       // NPC�f�[�^�܂ł̃p�X
	Npc2Data npc2;                          // NPC�f�[�^�i�[�ϐ�

	string datapath5;                       // NPC�f�[�^�܂ł̃p�X
	Npc3Data npc3;                          // NPC�f�[�^�i�[�ϐ�

	public NavMeshAgent m_Agent;			// NavMeshAgent�ϐ�
	[SerializeField]
	Rigidbody rb;                           // Rigidbody�ϐ�
	BoxCollider boxCollider;                // BoxCollider�ϐ�
	SphereCollider sphereCollider;			// SphereCollider�ϐ�

	GameObject player;						// AI�Q�[���I�u�W�F�N�g
	[SerializeField]
	GameObject[] targets;                   // �^�[�Q�b�g�I�u�W�F�N�g�i�[�z��
	[SerializeField]
	GameObject[] itemTargets;               // �A�C�e���I�u�W�F�N�g�i�[�z��
	[SerializeField]
	GameObject troTarget;					// �g���b�R�^�[�Q�b�g�I�u�W�F�N�g	
	[SerializeField]
	GameObject closeEnemy;                  // ��ԋ߂��^�[�Q�b�g�I�u�W�F�N�g 
	[SerializeField]
	GameObject closeItem;                   // ��ԋ߂��^�[�Q�b�g�I�u�W�F�N�g 
	[SerializeField]
	GameObject mineCart;                    // �̌@���̃J�[�g
	[SerializeField]
	GameObject damageLine;					// �̌@���̃_���[�W���C��

	int wd = 0;								// �؍ނ̐�
	int bw = 0;                             // �n�Ԃɐς܂�Ă���؍ނ̐�
	int rnd = 0;                            // �U���J�E���g
	int dPer = 0;                           // �_�b�V�����ǂ����̃����_������p�ϐ�
	int enduHeal = 0;                       // �ϋv�l�񕜊
	int i;
	[SerializeField]
	public int pTarget = 0;					// �D�悷��^�[�Q�b�g
	float old = 0;							// �_�b�V���p�x�i�[�ϐ�
	float oldHp = 30;                       // �_�b�V���OHP�i�[��
	float dCnt = 0;                         // �_�b�V���J�E���g
	float sCnt = 0;                         // �^�[�Q�b�g�m�F�J�E���g
	float aCnt = 0;                         // �^�[�Q�b�g�m�F�J�E���g
	float heal = 0;                         // �񕜂���HP
	float torchHeal = 0;                    // �����_������
	int sceneNo = 0;						// 0�F�̎�A1�F�̌@

	public int coin = 0;                    // �R�C������
	public int plHp = 100;                  // �v���C���[HP�T���v��
	public int endu = 100;                  // �ϋv�l�̃T���v��
	public int torch = 100;					// �����T���v��
	public int id = -1;                     // �U�����
	public int code = 0;                    // ���g�ɕt�^����Ă���NO
	public int lv;                          // ���x��
	public float speed;                     // �ړ��X�s�[�h
	public float upForce = 20f;             //������ɂ������
	public float forcePower = 20.0f;        // �������ɂ������
	public float closeDist = 1000;          // �G�Ƃ̋���
	public float cartDist;					// �̌@�̃g���b�R�ƃ_���[�W���C���̋���
	public float hp = 30;                   // HP
	public string tps = "";                 // �D�悷��G���ʒu�ɂ���Ď擾

	public bool isSwitch = true;			// �^�[�Q�b�g��ύX����Ȃ�true 
	public bool isItem = false;				// �A�C�e�����^�[�Q�b�g�ɂ���Ȃ�true
	public bool isGround = false;			// �n�ʂƐڐG���Ă���Ȃ�true
	public bool isWall = false;				// �ǂ���铮�쒆�Ȃ�true
	public bool isDash = false;				// �_�b�V����true
	//public bool isAtc = false;            // �U�����Ȃ�true
	public bool isArrow = true;             // ���u�U���Ȃ�true
	public bool isDmg = false;              // �_���[�W���肪���邩�ǂ���
	public bool isMain = false;             // mainTarget�ɂ���
	public bool isObstacle = false;         // ��Q�����^�[�Q�b�g�ɂ��邩�ǂ���
	public bool isChange = false;           // �^�[�Q�b�g�`�F���W
	public bool isZone = false;
	bool isInc = false;						// �s���s�\���ǂ���

	public Vector3 forceDirection;          // �͂̕���
	public Vector3 arrowDirection;          // ���u�U���̗͂̕���
	public Vector3 force;					// ��*����

	//pHp php;                              // HP�Ǘ��p�X�N���v�g

	public GameObject mainTarget;           // ���݂̃^�[�Q�b�g
	GameObject carTarget;                   // Enemy���̍ŏ��̃^�[�Q�b�g

	NpcManager npcManager;                  // NpcManager�X�N���v�g


	// �v���C���[�ɍ��킹�Ă���l
	public GameObject AttackHantei;//�U������
	bool CanAttack = true;//�U���\��
	int woodAttack = 0; // �؂��U��������
	public bool HandHaving = false;//�����A�C�e���������Ă邩
	[SerializeField]
	int woodBlock = 0;
	public bool isWoodBlock = false;
	[SerializeField]
	GameObject Carigge; // �n��

	float MaxHP = 100;//hp

	public Slider HPbar;//HP�o�[
	public Image HealBar;//�񕜂���Ƃ��̃o�[

	public bool CanHeal = false;//�񕜂ł��邩
	public bool HealBarStart = false;//��UI�X�^�[�g
	public bool StartHeal = false;//�񕜃X�^�[�g

	public float HealInterval = 500;//���ɉ񕜉\�ɂȂ�܂ł̎��ԁi�t���[�����j

	public bool isFire = false; // ������_�΂���ۂ̔���
	public GameObject fireZone; // ������_�΂���ꏊ
	public GameObject healZone; // �񕜂���ꏊ
	public GameObject warpPoint;
	public bool isHeal = false;// �񕜂�����ۂ̔���
	public bool cariggeAttack = false;

	private void Awake()
	{
		datapath = Application.dataPath + "/Dash.json"; // �_�b�V���f�[�^�܂ł̃p�X
		datapath2 = Application.dataPath + "/Npc.json"; // NPC�f�[�^�܂ł̃p�X
		datapath3 = Application.dataPath + "/Npc1.json"; // NPC�f�[�^�܂ł̃p�X
		datapath4 = Application.dataPath + "/Npc2.json"; // NPC�f�[�^�܂ł̃p�X
		datapath5 = Application.dataPath + "/Npc3.json"; // NPC�f�[�^�܂ł̃p�X
	}

	void Start()
	{
		targetRenderer = GetComponent<Renderer>();

		m_Agent = GetComponent<NavMeshAgent>(); // NavMeshAgent���擾

		rb = this.GetComponent<Rigidbody>(); //���W�b�h�{�f�B���擾

		boxCollider = this.GetComponent<BoxCollider>(); // BoxCollider���擾
		sphereCollider = this.GetComponent<SphereCollider>();	// SphereCollider���擾

		//arrow = (GameObject)Resources.Load("Sphere");	// ���u�U���p�I�u�W�F�N�g�擾

		//php = GetComponent<pHp>();  // HP�Ǘ��p�X�N���v�g�擾

		code = this.gameObject.GetComponent<NpcCode>().npcNo;   // NPC�̃i���o�[�擾

		npcManager = GameObject.Find("GameManager").GetComponent<NpcManager>();

		dh = new DashData();		// �_�b�V���f�[�^�擾

		dh = LoadFile(datapath);	// �_�b�V���f�[�^���[�h

		SaveFile(dh);               // �_�b�V���f�[�^�Z�[�u

		npc = new NpcData();        // NPC�f�[�^�擾

		npc = LoadNPC(datapath2);   // NPC�f�[�^���[�h

		SaveNPC(npc);               // NPC�f�[�^�Z�[�u

		if (SceneManager.GetActiveScene().name == "Forest")
		{
			sceneNo = 0;
		}
		else if(SceneManager.GetActiveScene().name == "Mine")
        {
			sceneNo = 1;
			mineCart = GameObject.Find("Minecart");
			damageLine = GameObject.Find("PoisonMyst");
			mainTarget = mineCart;
		}

		if (code == 1)
		{
			npc1 = new Npc1Data();        // NPC1�f�[�^�擾
			npc1 = LoadNPC1(datapath3);   // NPC1�f�[�^���[�h
			SaveNPC1(npc1);               // NPC1�f�[�^�Z�[�u

			lv = npc1.lv;				  // ���x��
			tps = npc1.tpsEnemy;          // �D�悷��G���L�^
			heal = npc1.healHp;           // HP�񕜊
			torchHeal = npc1.torch;       // �����X���
			enduHeal = npc1.endu;         // �ϋv�l�񕜊
		}
		else if(code == 2)
        {
			npc2 = new Npc2Data();        // NPC2�f�[�^�擾
			npc2 = LoadNPC2(datapath4);   // NPC2�f�[�^���[�h
			SaveNPC2(npc2);               // NPC2�f�[�^�Z�[�u

			lv = npc2.lv;                 // ���x��
			tps = npc2.tpsEnemy;          // �D�悷��G���L�^
			heal = npc2.healHp;           // HP�񕜊
			torchHeal = npc2.torch;       // �����X���
			enduHeal = npc2.endu;		  // �ϋv�l�񕜊
		}
		else if(code == 3)
        {
			npc3 = new Npc3Data();        // NPC3�f�[�^�擾
			npc3 = LoadNPC3(datapath5);   // NPC3�f�[�^���[�h
			SaveNPC(npc);                 // NPC3�f�[�^�Z�[�u

			lv = npc3.lv;                 // ���x��
			tps = npc3.tpsEnemy;          // �D�悷��G���L�^
			heal = npc3.healHp;           // HP�񕜊
			torchHeal = npc3.torch;       // �����X���
			enduHeal = npc3.endu;         // �ϋv�l�񕜊
		}

		rb.constraints = RigidbodyConstraints.FreezeRotationZ;
		rb.constraints = RigidbodyConstraints.FreezeRotationX;
		rb.constraints = RigidbodyConstraints.FreezePositionY;

		//Switch();					// �^�[�Q�b�g�擾
	}

	// �_�b�V���f�[�^���[�h�֐�
	public DashData LoadFile(string dataPath)
	{
		StreamReader reader = new StreamReader(dataPath);
		string datastr = reader.ReadToEnd();
		reader.Close();

		return JsonUtility.FromJson<DashData>(datastr);
	}

	// �_�b�V���f�[�^�Z�[�u�֐�
	public void SaveFile(DashData dh)
	{
		string jsonstr = JsonUtility.ToJson(dh);
		StreamWriter wreiter = new StreamWriter(datapath, false);
		wreiter.WriteLine(jsonstr);
		wreiter.Flush();
		wreiter.Close();
	}

	// NPC�f�[�^���[�h�֐�
	public NpcData LoadNPC(string dataPath2)
	{
		StreamReader reader = new StreamReader(dataPath2);
		string datastr = reader.ReadToEnd();
		reader.Close();

		return JsonUtility.FromJson<NpcData>(datastr);
	}

	// NPC�f�[�^�Z�[�u�֐�
	public void SaveNPC(NpcData npc)
	{
		string jsonstr = JsonUtility.ToJson(npc);
		StreamWriter wreiter = new StreamWriter(datapath2, false);
		wreiter.WriteLine(jsonstr);
		wreiter.Flush();
		wreiter.Close();
	}

	// NPC�f�[�^���[�h�֐�
	public Npc1Data LoadNPC1(string dataPath3)
	{
		StreamReader reader = new StreamReader(dataPath3);
		string datastr = reader.ReadToEnd();
		reader.Close();

		return JsonUtility.FromJson<Npc1Data>(datastr);
	}

	// NPC�f�[�^�Z�[�u�֐�
	public void SaveNPC1(Npc1Data npc1)
	{
		string jsonstr = JsonUtility.ToJson(npc1);
		StreamWriter wreiter = new StreamWriter(datapath3, false);
		wreiter.WriteLine(jsonstr);
		wreiter.Flush();
		wreiter.Close();
	}

	// NPC�f�[�^���[�h�֐�
	public Npc2Data LoadNPC2(string dataPath4)
	{
		StreamReader reader = new StreamReader(dataPath4);
		string datastr = reader.ReadToEnd();
		reader.Close();

		return JsonUtility.FromJson<Npc2Data>(datastr);
	}

	// NPC�f�[�^�Z�[�u�֐�
	public void SaveNPC2(Npc2Data npc2)
	{
		string jsonstr = JsonUtility.ToJson(npc2);
		StreamWriter wreiter = new StreamWriter(datapath4, false);
		wreiter.WriteLine(jsonstr);
		wreiter.Flush();
		wreiter.Close();
	}

	// NPC�f�[�^���[�h�֐�
	public Npc3Data LoadNPC3(string dataPath5)
	{
		StreamReader reader = new StreamReader(dataPath5);
		string datastr = reader.ReadToEnd();
		reader.Close();

		return JsonUtility.FromJson<Npc3Data>(datastr);
	}

	// NPC�f�[�^�Z�[�u�֐�
	public void SaveNPC3(Npc3Data npc3)
	{
		string jsonstr = JsonUtility.ToJson(npc3);
		StreamWriter wreiter = new StreamWriter(datapath5, false);
		wreiter.WriteLine(jsonstr);
		wreiter.Flush();
		wreiter.Close();
	}

	void Update()
	{
		cartDist= Vector3.Distance(transform.position, Carigge.transform.position);

		if(cartDist >= 50)
        {
			this.transform.position = warpPoint.transform.position;
			m_Agent.Warp(transform.position);   // �i�r���b�V���G�[�W�F���g�����݈ʒu�Ƀ��[�v

			npcManager.ChangeEnemy();
            
		}

		HealthBar();

		if (hp >= MaxHP)
        {
			hp = MaxHP;
		}

		if (hp >= 100)//hp100���ƃq�[���s��
		{
			StartHeal = false;
		}

		if (HealBar.fillAmount >= 1)
		{
			if (isHeal == true)
			{
				Carigge.GetComponent<CariggeManager>().FlowerMinus();
			}
			StartHeal = true;
			isHeal = false;
			HealBarStart = false;
			HealInterval = 0;
		}

		if(sceneNo == 1)
        {
			cartDist = Vector3.Distance(damageLine.transform.position, mineCart.transform.position);
			if(cartDist >= 11)
            {
				cariggeAttack = true;
            }
		}

        if (hp <= 30 && StartHeal == false && Carigge.GetComponent<CariggeManager>().HaveFlower >= 1)
        {
			isHeal = true;
        }

		if (CanHeal == true && HealInterval >= 500)
		{
			HealBarStart = true;
		}

		// �U���J�E���g���v���X
		rnd++;

		// �_�b�V���J�E���g���v���X
		dCnt += Time.deltaTime;

		// �^�[�Q�b�g�m�F�J�E���g���v���X
		sCnt += Time.deltaTime;

		// �_�b�V���J�E���g���v���X
		aCnt += Time.deltaTime;

		if (sCnt >= 3.0f && HandHaving == false)
		{
			npcManager.ChangeEnemy();
			sCnt = 0;
		}



		if (mainTarget != null)
		{
			if (HandHaving == false)
			{
				if (isObstacle == true)
				{
					m_Agent.SetDestination(npcManager.obstacle.transform.position);
				}
				else if (isFire == true)
				{
					m_Agent.SetDestination(fireZone.transform.position);
				}
				else if (isHeal == true)
				{
					m_Agent.SetDestination(healZone.transform.position);
				}
				else
				{
					m_Agent.SetDestination(mainTarget.transform.position);
				}

				if (sceneNo == 1 && cariggeAttack == true)
				{
					m_Agent.SetDestination(mineCart.transform.position);
				}
			}
		}

		/*
		// �^�[�Q�b�g��ǐՂ���
		if (m_Agent.enabled == true)
		{
			// �^�[�Q�b�g���G�l�~�[�̏ꍇ
			if (pTarget == 1 && closeEnemy != null)
			{
				m_Agent.SetDestination(closeEnemy.transform.position);
				mainTarget = closeEnemy;
			}
			// �^�[�Q�b�g���g���b�R�̏ꍇ
			else if(pTarget == 2)
            {
				m_Agent.SetDestination(troTarget.transform.position);
				mainTarget = troTarget;
			}
			// �^�[�Q�b�g���A�C�e���̏ꍇ
			else if(pTarget == 3)
            {
				m_Agent.SetDestination(itemTargets[0].transform.position);
				mainTarget = itemTargets[0];
            }
			else if(pTarget == 0)
            {
				isSwitch = true;
				mainTarget = null;

				if (lv == 1)
				{
					Switch();
				}
				else if(lv == 2)
                {
					Switch2();
                }
				else if(lv == 3)
                {
					Switch3();
                }
            }
            
		}
		*/

		if(HandHaving == true)
        {
			if(aCnt >= 1 && isZone == false)
            {
				m_Agent.speed = 6.5f;
            }
			pTarget = 4;
			m_Agent.SetDestination(Carigge.transform.position);
			//mainTarget = Carigge;
		}

		/*
		if(isWoodBlock == true)
        {
			Invoke("WoodBlockChange",0.2f);
			isWoodBlock = false;
        }
		*/

		/*
		// �_�b�V��
		if ((Input.GetKeyDown(KeyCode.Z) || php.isDmg == true) && isDash == false)
		{
			oldHp = php.hp; // �q�b�g�O��HP���L�^
			float rnd = Random.Range(dh.data[php.id].sr, dh.data[php.id].gr);   // ����p�x�������_���Őݒ�i�͈͂̓_�b�V���f�[�^���j
			old = rnd;  // ����p�x��ێ�
			m_Agent.enabled = false;    // �i�r���b�V���G�[�W�F���g���~

			//	�͂̌�����ݒ�
			float rad = -(this.gameObject.transform.localEulerAngles.y - rnd) * Mathf.Deg2Rad;
			float x = Mathf.Cos(rad);
			float y = 0f;
			float z = Mathf.Sin(rad);
			forceDirection = new Vector3(x, y, z);

			// �͂̌����Ƌ�����ݒ�
			force = forcePower * forceDirection;
			rb.AddForce(force, ForceMode.Impulse);

			isDash = true;  // �_�b�V����Ԃɂ���
			StartCoroutine("Frame");    // Frame�R���[�`�����X�^�[�g
		}
		*/

		/*// ���u�U��
		if (rnd >= 300 && isArrow == true)
		{
			m_Agent.enabled = false;	// �i�r���b�V���G�[�W�F���g���~

			//	�͂̌�����ݒ�
			float rad = -(this.gameObject.transform.localEulerAngles.y - 90.0f) * Mathf.Deg2Rad;
			float x = Mathf.Cos(rad);
			float y = 0f;
			float z = Mathf.Sin(rad);
			arrowDirection = new Vector3(x, y, z);

			// ���u�U���I�u�W�F�N�g����
			//Instantiate(arrow, new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z + 1.0f), Quaternion.identity);
			Instantiate(arrow, this.gameObject.transform);

			// Frame�R���[�`���𔭐�
			StartCoroutine("Frame");

			// �U���J�E���g��0��
			rnd = 0;
		
		}*/

		// NPC�̔ԍ��ɂ���ĎQ�Ƃ���f�[�^��ύX
		// �񕜍s��
		if (hp < heal)
		{
			hp = 100;
		}

		// �����̉񕜍s��
		if (torch < torchHeal)
		{
			torch = 100;
		}


		// �v���C���[�����s�� ������HP��0�ł͂Ȃ��v���C���[HP��0�ȉ��̏ꍇ
		if (plHp <= 0)
		{
			plHp = 30;  // �v���C���[HP��30�ɂ��ĕ���
		}

		// �_�b�V���s�����邩�ǂ����̃J�E���g


		/*
		// �_�b�V���s��
		if(dCnt >= 20)
        {
			dPer = Random.Range(1, 101);

			if(dPer < npc.freDash)
            {
				m_Agent.speed = 6.5f;
            }
            else
            {
				m_Agent.speed = 3.5f;
            }
        }
		*/

		// HP���O�ɂȂ����ꍇ�̏���
        if (hp <= 0)
        {
			isInc = true;
			m_Agent.enabled = false;
			boxCollider.enabled = false;
			sphereCollider.enabled = false;
        }

		/*
        // HP���ϓ������ۂ̏���
        if (hp != oldHp)
        {
			npcManager.changeHp();
        }

		if(mainTarget.GetComponent<Enemy>().isDes == true)
        {
			isMain = true;
        }

		if (isMain == true)
		{
			if (mainTarget != null)
			{
				Destroy(mainTarget.gameObject);
			}
			mainTarget = null;
			Switch();
			isMain = false;
		}
		*/
	}

	private void FixedUpdate()
	{
		HealBarPlus();
		FlowerHeal();
	}

	// �^�[�Q�b�g�ύX���ł���悤�ɂ���֐�
	void SwitchOn()
	{
		isSwitch = true;
	}

	// �^�[�Q�b�g�ݒ�֐�
	public void Switch()
    {
		m_Agent.enabled = true;
		m_Agent.speed = 6.5f;

		// �A�C�e���I�u�W�F�N�g�T��
		itemTargets = GameObject.FindGameObjectsWithTag("Item");
		if (itemTargets.Length != 0)
		{
			//Debug.Log("aaa");
			pTarget = 3;
		}

		/*
		if (GameObject.Find("GameManager").GetComponent<NpcManager>().isTro == false)
		{
			// �g���b�R�^�[�Q�b�g�I�u�W�F�N�g�T��
			troTarget = GameObject.FindGameObjectWithTag("Target");
			if (troTarget != null)
			{
				//Debug.Log("aaa");
				pTarget = 2;
			}
		}
		*/

		// �G�l�~�[�I�u�W�F�N�g�T��
		targets = GameObject.FindGameObjectsWithTag("Enemy");
		int npcLen = targets.Length;
		if (targets.Length != 0)
		{
			//Debug.Log("aaa");
			pTarget = 1;
		}

		if (pTarget == 1)
		{
			// �u�����l�v�̐ݒ�
			closeDist = 1000;

			foreach (GameObject y in targets)
			{
				// �R���\�[����ʂł̊m�F�p�R�[�h
				//print(Vector3.Distance(transform.position, t.transform.position));

				// ���̃I�u�W�F�N�g�i�C�e�j�ƓG�܂ł̋������v��
				float tDist = Vector3.Distance(transform.position, y.transform.position);

				// �������u�����l�v�����u�v�������G�܂ł̋����v�̕����߂��Ȃ�΁A
				if (closeDist > tDist)
				{
					// �ucloseDist�v���utDist�i���̓G�܂ł̋����j�v�ɒu��������B
					// ������J��Ԃ����ƂŁA��ԋ߂��G�������o�����Ƃ��ł���B
					closeDist = tDist;

					// ��ԋ߂��G�̏���closeEnemy�Ƃ����ϐ��Ɋi�[����i���j
					closeEnemy = y;

					mainTarget = closeEnemy;
				}

				// �C�e�����������0.5�b��ɁA��ԋ߂��G�Ɍ������Ĉړ����J�n����B
				//Invoke("SwitchOn", 0.5f);
				m_Agent.SetDestination(closeEnemy.transform.position);
			}
		}
		else if(pTarget == 3)
        {
			// �u�����l�v�̐ݒ�
			closeDist = 1000;

			foreach (GameObject y in itemTargets)
			{
				// �R���\�[����ʂł̊m�F�p�R�[�h
				//print(Vector3.Distance(transform.position, t.transform.position));

				// ���̃I�u�W�F�N�g�i�C�e�j�ƓG�܂ł̋������v��
				float tDist = Vector3.Distance(transform.position, y.transform.position);

				// �������u�����l�v�����u�v�������G�܂ł̋����v�̕����߂��Ȃ�΁A
				if (closeDist > tDist)
				{
					// �ucloseDist�v���utDist�i���̓G�܂ł̋����j�v�ɒu��������B
					// ������J��Ԃ����ƂŁA��ԋ߂��G�������o�����Ƃ��ł���B
					closeDist = tDist;

					// ��ԋ߂��G�̏���closeEnemy�Ƃ����ϐ��Ɋi�[����i���j
					closeItem = y;

					mainTarget = closeItem;
				}

				// �C�e�����������0.5�b��ɁA��ԋ߂��G�Ɍ������Ĉړ����J�n����B
				//Invoke("SwitchOn", 0.5f);
				m_Agent.SetDestination(closeItem.transform.position);
			}
		}
		i = 0;
	}

	// �^�[�Q�b�g�ݒ�֐�
	public void Switch2()
	{
		m_Agent.enabled = true;
		m_Agent.speed = 6.5f;


		// �A�C�e���I�u�W�F�N�g�T��
		itemTargets = GameObject.FindGameObjectsWithTag("Item");
		if (itemTargets.Length != 0)
		{
			//Debug.Log("aaa");
			pTarget = 3;
		}

		// �G�l�~�[�I�u�W�F�N�g�T��
		targets = GameObject.FindGameObjectsWithTag("Enemy");
		int npcLen = targets.Length;
		if (targets.Length != 0)
		{
			//Debug.Log("aaa");
			pTarget = 1;
		}

		/*
		if (GameObject.Find("GameManager").GetComponent<NpcManager>().isTro == false)
		{
			// �g���b�R�^�[�Q�b�g�I�u�W�F�N�g�T��
			troTarget = GameObject.FindGameObjectWithTag("Target");
			if (troTarget != null)
			{
				//Debug.Log("aaa");
				pTarget = 2;
			}
		}
		*/


		// �u�����l�v�̐ݒ�
		closeDist = 1000;
		// �G�̃^�[�Q�b�g���擾
		//carTarget = GameObject.FindGameObjectWithTag("eTarget");

		if (pTarget == 1)
		{
			foreach (GameObject y in targets)
			{
				// �R���\�[����ʂł̊m�F�p�R�[�h
				//print(Vector3.Distance(transform.position, t.transform.position));

				if (tps == "npc")
				{
					// ���̃I�u�W�F�N�g�i�C�e�j�ƓG�܂ł̋������v��
					float tDist = Vector3.Distance(transform.position, y.transform.position);

					// �������u�����l�v�����u�v�������G�܂ł̋����v�̕����߂��Ȃ�΁A
					if (closeDist > tDist)
					{
						// �ucloseDist�v���utDist�i���̓G�܂ł̋����j�v�ɒu��������B
						// ������J��Ԃ����ƂŁA��ԋ߂��G�������o�����Ƃ��ł���B
						closeDist = tDist;

						// ��ԋ߂��G�̏���closeEnemy�Ƃ����ϐ��Ɋi�[����i���j
						closeEnemy = y;

						mainTarget = closeEnemy;
					}
				}
				else if (tps == "car")
				{
					// ���̃I�u�W�F�N�g�i�C�e�j�ƓG�܂ł̋������v��
					float tDist = Vector3.Distance(Carigge.transform.position, y.transform.position) + Vector3.Distance(transform.position, y.transform.position);

					// �������u�����l�v�����u�v�������G�܂ł̋����v�̕����߂��Ȃ�΁A
					if (closeDist > tDist)
					{
						// �ucloseDist�v���utDist�i���̓G�܂ł̋����j�v�ɒu��������B
						// ������J��Ԃ����ƂŁA��ԋ߂��G�������o�����Ƃ��ł���B
						closeDist = tDist;

						// ��ԋ߂��G�̏���closeEnemy�Ƃ����ϐ��Ɋi�[����i���j
						closeEnemy = y;

						mainTarget = closeEnemy;
					}
				}
				// �C�e�����������0.5�b��ɁA��ԋ߂��G�Ɍ������Ĉړ����J�n����B
				//Invoke("SwitchOn", 0.5f);
				m_Agent.SetDestination(closeEnemy.transform.position);
			}
		}
		else if(pTarget == 3)
        {
			// �u�����l�v�̐ݒ�
			closeDist = 1000;

			foreach (GameObject y in itemTargets)
			{
				// �R���\�[����ʂł̊m�F�p�R�[�h
				//print(Vector3.Distance(transform.position, t.transform.position));

				// ���̃I�u�W�F�N�g�i�C�e�j�ƓG�܂ł̋������v��
				float tDist = Vector3.Distance(transform.position, y.transform.position);

				// �������u�����l�v�����u�v�������G�܂ł̋����v�̕����߂��Ȃ�΁A
				if (closeDist > tDist)
				{
					// �ucloseDist�v���utDist�i���̓G�܂ł̋����j�v�ɒu��������B
					// ������J��Ԃ����ƂŁA��ԋ߂��G�������o�����Ƃ��ł���B
					closeDist = tDist;

					// ��ԋ߂��G�̏���closeEnemy�Ƃ����ϐ��Ɋi�[����i���j
					closeItem = y;

					mainTarget = closeItem;
				}

				// �C�e�����������0.5�b��ɁA��ԋ߂��G�Ɍ������Ĉړ����J�n����B
				//Invoke("SwitchOn", 0.5f);
				m_Agent.SetDestination(closeItem.transform.position);
			}
		}
		i = 0;
	}



	// �^�[�Q�b�g�ݒ�֐�
	public void Switch3()
	{
		if (HandHaving == false)
		{
			m_Agent.enabled = true;
			m_Agent.speed = 6.5f;

			if (mainTarget != null)
			{
				if (mainTarget.gameObject.tag == "Enemy")
				{
					mainTarget.GetComponent<Enemy>().isTarget = false;
				}
				else if (mainTarget.gameObject.tag == "Item")
				{
					//Debug.Log("aaa");
					mainTarget.GetComponent<ItemTarget>().isTarget = false;
				}
			}

			// �A�C�e���I�u�W�F�N�g�T��
			itemTargets = GameObject.FindGameObjectsWithTag("Item");
			if (itemTargets.Length != 0)
			{
				//Debug.Log("aaa");
				pTarget = 3;
			}


			// �G�l�~�[�I�u�W�F�N�g�T��
			targets = GameObject.FindGameObjectsWithTag("Enemy");
			int npcLen = targets.Length;
			if (targets.Length != 0)
			{
				//Debug.Log("aaa");
				pTarget = 1;
			}

			/*
			if (GameObject.Find("GameManager").GetComponent<NpcManager>().isTro == false)
			{
				// �g���b�R�^�[�Q�b�g�I�u�W�F�N�g�T��
				troTarget = GameObject.FindGameObjectWithTag("Target");
				if (troTarget != null)
				{
					//Debug.Log("aaa");
					pTarget = 2;
				}
			}
			*/

			if (pTarget == 1)
			{
				// �u�����l�v�̐ݒ�
				closeDist = 1000;
				// �G�̃^�[�Q�b�g���擾
				//carTarget = GameObject.FindGameObjectWithTag("eTarget");

				foreach (GameObject t in targets)
				{
					if (t.GetComponent<Enemy>().isTarget == false)
					{
						// �R���\�[����ʂł̊m�F�p�R�[�h
						//print(Vector3.Distance(transform.position, t.transform.position));

						if (tps == "npc")
						{
							// ���̃I�u�W�F�N�g�i�C�e�j�ƓG�܂ł̋������v��
							float tDist = Vector3.Distance(transform.position, t.transform.position);

							// �������u�����l�v�����u�v�������G�܂ł̋����v�̕����߂��Ȃ�΁A
							if (closeDist > tDist)
							{
								// �ucloseDist�v���utDist�i���̓G�܂ł̋����j�v�ɒu��������B
								// ������J��Ԃ����ƂŁA��ԋ߂��G�������o�����Ƃ��ł���B
								closeDist = tDist;

								// ��ԋ߂��G�̏���closeEnemy�Ƃ����ϐ��Ɋi�[����i���j
								closeEnemy = t;

								mainTarget = closeEnemy;
							}
						}
						else if (tps == "car")
						{
							// ���̃I�u�W�F�N�g�i�C�e�j�ƓG�܂ł̋������v��
							float tDist = Vector3.Distance(Carigge.transform.position, t.transform.position) + Vector3.Distance(transform.position, t.transform.position);

							// �������u�����l�v�����u�v�������G�܂ł̋����v�̕����߂��Ȃ�΁A
							if (closeDist > tDist)
							{
								// �ucloseDist�v���utDist�i���̓G�܂ł̋����j�v�ɒu��������B
								// ������J��Ԃ����ƂŁA��ԋ߂��G�������o�����Ƃ��ł���B
								closeDist = tDist;

								// ��ԋ߂��G�̏���closeEnemy�Ƃ����ϐ��Ɋi�[����i���j
								closeEnemy = t;

								mainTarget = closeEnemy;
							}
						}
						// �C�e�����������0.5�b��ɁA��ԋ߂��G�Ɍ������Ĉړ����J�n����B
						//Invoke("SwitchOn", 0.5f);
						if (closeEnemy != null)
						{
							m_Agent.SetDestination(closeEnemy.transform.position);
						}
					}
					i++;

					/*
					if (i == npcLen && closeEnemy.GetComponent<Enemy>().isTarget == true)
					{
						foreach (GameObject y in targets)
						{
							// �R���\�[����ʂł̊m�F�p�R�[�h
							//print(Vector3.Distance(transform.position, t.transform.position));

							if (tps == "npc")
							{
								// ���̃I�u�W�F�N�g�i�C�e�j�ƓG�܂ł̋������v��
								float tDist = Vector3.Distance(transform.position, y.transform.position);

								// �������u�����l�v�����u�v�������G�܂ł̋����v�̕����߂��Ȃ�΁A
								if (closeDist > tDist)
								{
									// �ucloseDist�v���utDist�i���̓G�܂ł̋����j�v�ɒu��������B
									// ������J��Ԃ����ƂŁA��ԋ߂��G�������o�����Ƃ��ł���B
									closeDist = tDist;

									// ��ԋ߂��G�̏���closeEnemy�Ƃ����ϐ��Ɋi�[����i���j
									closeEnemy = y;

									mainTarget = closeEnemy;
								}
							}
							else if (tps == "car")
							{
								// ���̃I�u�W�F�N�g�i�C�e�j�ƓG�܂ł̋������v��
								float tDist = Vector3.Distance(carTarget.transform.position, y.transform.position) + Vector3.Distance(transform.position, y.transform.position);

								// �������u�����l�v�����u�v�������G�܂ł̋����v�̕����߂��Ȃ�΁A
								if (closeDist > tDist)
								{
									// �ucloseDist�v���utDist�i���̓G�܂ł̋����j�v�ɒu��������B
									// ������J��Ԃ����ƂŁA��ԋ߂��G�������o�����Ƃ��ł���B
									closeDist = tDist;

									// ��ԋ߂��G�̏���closeEnemy�Ƃ����ϐ��Ɋi�[����i���j
									closeEnemy = y;

									mainTarget = closeEnemy;
								}
							}
							// �C�e�����������0.5�b��ɁA��ԋ߂��G�Ɍ������Ĉړ����J�n����B
							//Invoke("SwitchOn", 0.5f);
							m_Agent.SetDestination(closeEnemy.transform.position);
						}
					}
					*/
				}

				if (closeEnemy != null)
				{
					closeEnemy.GetComponent<Enemy>().isTarget = true;
				}
			}
			else if (pTarget == 3)
			{

				// �u�����l�v�̐ݒ�
				closeDist = 1000;


				foreach (GameObject y in itemTargets)
				{

					if (y.GetComponent<ItemTarget>().isTarget == false)
					{
						// �R���\�[����ʂł̊m�F�p�R�[�h
						//print(Vector3.Distance(transform.position, t.transform.position));

						// ���̃I�u�W�F�N�g�i�C�e�j�ƓG�܂ł̋������v��
						float tDist = Vector3.Distance(transform.position, y.transform.position);

						// �������u�����l�v�����u�v�������G�܂ł̋����v�̕����߂��Ȃ�΁A
						if (closeDist > tDist)
						{
							// �ucloseDist�v���utDist�i���̓G�܂ł̋����j�v�ɒu��������B
							// ������J��Ԃ����ƂŁA��ԋ߂��G�������o�����Ƃ��ł���B
							closeDist = tDist;

							// ��ԋ߂��G�̏���closeEnemy�Ƃ����ϐ��Ɋi�[����i���j
							closeItem = y;

							mainTarget = closeItem;
						}

						// �C�e�����������0.5�b��ɁA��ԋ߂��G�Ɍ������Ĉړ����J�n����B
						//Invoke("SwitchOn", 0.5f);
						m_Agent.SetDestination(closeItem.transform.position);
					}


				}
				closeItem.GetComponent<ItemTarget>().isTarget = true;
			}
			i = 0;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Ground") //Ground�^�O�̃I�u�W�F�N�g�ɐG�ꂽ�Ƃ�
		{
			isGround = true;    //isGround��true�ɂ���
			m_Agent.enabled = true; // �i�r���b�V���G�[�W�F���g�𔭐�
			rb.constraints = RigidbodyConstraints.FreezePositionY;  // ���W�b�h�{�f�B��y���ړ��𐧌�
		}



		if (other.gameObject.tag == "basya")    // basya�^�O�̃I�u�W�F�N�g�ɐG�ꂽ��
		{
			bw += wd;   // �莝���̖؍ނ̐���n�Ԃɐς܂�Ă���؍ނ̐��Ɂ{
			wd = 0; // �莝���̖؍ނ̐���0��
			Debug.Log(wd);
			Debug.Log(bw);
		}
	}

    private void OnCollisionStay(Collision collision)
    {
		if (collision.gameObject.tag == "Item")
		{
			//transform.LookAt(collision.gameObject.transform);
			if (collision.gameObject.GetComponent<ItemName>().name == "Tree") // Wood�^�O�̃I�u�W�F�N�g�ɐG�ꂽ��
			{
				if (CanAttack == true)
				{
					CanAttack = false;
					AttackHantei.SetActive(true);
					woodAttack++;
					Invoke("HanteiDel", 0.2f);
					Invoke("CanAttackTrue", 0.5f);
				}

				/*
					wd++;   // �莝���̖؍ނ̐����{
				targets = GameObject.FindGameObjectsWithTag("basya");   // �^�[�Q�b�g��n�ԂɕύX
				m_Agent.SetDestination(targets[0].transform.position);  // �^�[�Q�b�g�Ɉړ����J�n
				Debug.Log(wd);
				*/
			}

			if (collision.gameObject.GetComponent<ItemName>().name == "Metal") // Wood�^�O�̃I�u�W�F�N�g�ɐG�ꂽ��
			{
				if (CanAttack == true)
				{
					CanAttack = false;
					AttackHantei.SetActive(true);
					woodAttack++;
					Invoke("HanteiDel", 0.2f);
					Invoke("CanAttackTrue", 0.5f);
				}
			}

			if (collision.gameObject.GetComponent<ItemName>().name == "Coal") // Wood�^�O�̃I�u�W�F�N�g�ɐG�ꂽ��
			{
				if (CanAttack == true)
				{
					CanAttack = false;
					AttackHantei.SetActive(true);
					woodAttack++;
					Invoke("HanteiDel", 0.2f);
					Invoke("CanAttackTrue", 0.5f);
				}

				/*
					wd++;   // �莝���̖؍ނ̐����{
				targets = GameObject.FindGameObjectsWithTag("basya");   // �^�[�Q�b�g��n�ԂɕύX
				m_Agent.SetDestination(targets[0].transform.position);  // �^�[�Q�b�g�Ɉړ����J�n
				Debug.Log(wd);
				*/
			}
		}
	}

    public IEnumerator Frame()
	{
		// 0.3�b������҂��܂��B
		yield return new WaitForSeconds(0.3f);

		// ���W�b�h�{�f�B������Ȃ�����
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		m_Agent.Warp(transform.position);   // �i�r���b�V���G�[�W�F���g�����݈ʒu�Ƀ��[�v
		m_Agent.enabled = true; // �i�r���b�V���G�[�W�F���g��K�p

		if (isDash == true)
		{
			// 2�b������҂��܂��B
			yield return new WaitForSeconds(2.0f);
			// �_�b�V����ԂłȂ�����
			isDash = false;
		}

	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			//m_Agent.enabled = false;

			/*if (isGround == true)//���n���Ă���Ƃ�
			{
				m_Agent.enabled = false;
				//m_Agent.isStopped = true;
				isGround = false;//  isGround��false�ɂ���
				Debug.Log("aaa");
				rb.AddForce(new Vector3(0, upForce, 0),ForceMode.Impulse); //��Ɍ������ė͂�������
			}*/
		}

		if (other.gameObject.tag == "Weeds")
		{

			if (CanAttack == true)
			{
				CanAttack = false;
				AttackHantei.SetActive(true);
				woodAttack++;
				Invoke("HanteiDel", 0.2f);
				Invoke("CanAttackTrue", 0.5f);
			}
		}

		if (other.gameObject.name == "blowHantei")
		{
			hp -= 2;
		}

		if (other.gameObject.name == "CircleHantei")
		{
			hp -= 4;
		}

		if (other.gameObject.name == "LineHantei")
		{
			hp -= 3;
		}

		if (other.gameObject.name == "FanHantei")
		{
			hp -= 3;
		}

	}

	// �U��
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			//transform.LookAt(other.gameObject.transform);
			//�ړ�����]�����Ȃ��悤�ɂ���
			rb.constraints = RigidbodyConstraints.FreezeAll;

			if (CanAttack == true)
			{
				CanAttack = false;
				AttackHantei.SetActive(true);
				Invoke("HanteiDel", 0.2f);
				Invoke("CanAttackTrue", 0.5f);
				m_Agent.speed = 0;    // �i�r���b�V���G�[�W�F���g���~
				transform.LookAt(other.gameObject.transform);
				//Debug.Log("Attack!");
				//other.gameObject.GetComponent<Enemy>().hp -= 30;
				if(other.gameObject.GetComponent<Enemy>().hp <= 0)
                {
					mainTarget = null;
					other.gameObject.GetComponent<Enemy>().isDes = true;
					isMain = true;
					m_Agent.enabled = true;    // �i�r���b�V���G�[�W�F���g���Đ�
					if (lv == 1)
					{
						Switch();
					}
					else if (lv == 2)
					{
						Switch2();
					}
					else if (lv == 3)
					{
						Switch3();
					}
				}
				rnd = 0;
			}
		}


		if (other.gameObject.tag == "Item")
		{
			//transform.LookAt(other.gameObject.transform);

			if (other.gameObject.GetComponent<ItemName>().name == "Flower") // Wood�^�O�̃I�u�W�F�N�g�ɐG�ꂽ��
			{
				if (CanAttack == true)
				{
					CanAttack = false;
					AttackHantei.SetActive(true);
					woodAttack++;
					Invoke("HanteiDel", 0.2f);
					Invoke("CanAttackTrue", 0.5f);
				}
			}
		}

		if(other.gameObject.name == "HealZone" && isHeal == true)
        {
			CanHeal = true;
        }

		if(other.gameObject.name == "Minecart" && cartDist >= 11)
        {
			if (CanAttack == true)
			{
				CanAttack = false;
				AttackHantei.SetActive(true);
				woodAttack++;
				Invoke("HanteiDel", 0.2f);
				Invoke("CanAttackTrue", 0.5f);
			}
			cariggeAttack = false;
		}

		if (other.gameObject.tag == "Obstacle")
		{
			if (CanAttack == true)
			{
				CanAttack = false;
				AttackHantei.SetActive(true);
				woodAttack++;
				Invoke("HanteiDel", 0.2f);
				Invoke("CanAttackTrue", 0.5f);
			}
		}

		if(other.gameObject.name == "TrackZone")
		{
			isZone = true;
        }
	}

	// �ǂ���铮��
    IEnumerator MoveUp()
	{
		rb.isKinematic = true;
		for (int i = 0; i < 140; i++)
		{

			player.transform.Translate(0, 0.06f, 0);


			yield return new WaitForSeconds(0.01f);
		}

		rb.isKinematic = false;
		
	}

	
	private void OnTriggerExit(Collider other)
	{
		/*
		// �U���Ƀq�b�g���Ă��Ȃ��ꍇ
		if ((other.gameObject.tag == "En" || other.gameObject.tag == "Tai" || other.gameObject.tag == "Tyoku" || other.gameObject.tag == "Ougi" || other.gameObject.tag == "Kaiten") && dh.data[php.id].ch == true && oldHp == php.hp)
		{
			if ((old - 30.0f) >= 0.0f)	// �p�x��0���傫���Ȃ�
			{
				dh.data[php.id].sr = old - 30.0f;	// ��������p�x-30�x��ݒ�
            }
            else
            {
				dh.data[php.id].sr = 0.0f;	// 0�x��ݒ�

			}

			if ((old + 30.0f) <= 360.0f)	// �p�x��360��菬�����Ȃ�
			{
				dh.data[php.id].gr = old + 30.0f;	// ��������p�x�{30�x��ݒ�
			}
			else
			{
				dh.data[php.id].gr = 360.0f;	// 360�x��ݒ�

			}

			dh.data[php.id].ch = false;	// ����p�x��ύX���Ȃ��悤�ɐݒ�

			php.id = -1;	// �U�������Ă��Ȃ���ԂɕύX

			SaveFile(dh);	// �_�b�V���f�[�^���Z�[�u
		}*/

		// �G���痣�ꂽ��
        if (other.gameObject.tag == "Enemy")
        {
			m_Agent.enabled = true;    // �i�r���b�V���G�[�W�F���g�𕜋A
			rb.constraints = RigidbodyConstraints.None;
			rb.constraints = RigidbodyConstraints.FreezeRotationZ;
			rb.constraints = RigidbodyConstraints.FreezeRotationX;
			rb.constraints = RigidbodyConstraints.FreezePositionY;
		}


		if (other.gameObject.name == "HealZone")
		{
			CanHeal = false;
		}


		if (other.gameObject.name == "TrackZone")
		{
			isZone = false;
		}
	}

	void HanteiDel()
	{
		AttackHantei.SetActive(false);
	}

	void CanAttackTrue()
	{

		CanAttack = true;
	}

	public void WoodBlockChange()
    {
		woodBlock--;
		m_Agent.speed = 3.5f;
		if (woodBlock <= 0)
		{
			Switch();
		}
		else
		{
			itemTargets = GameObject.FindGameObjectsWithTag("WoodBlock");
			m_Agent.SetDestination(itemTargets[0].transform.position);
			mainTarget = itemTargets[0];
			pTarget = 3;
		}
		
	}

	void WoodBlockCheck()
    {
		itemTargets = GameObject.FindGameObjectsWithTag("WoodBlock");
		m_Agent.SetDestination(itemTargets[0].transform.position);
		woodBlock = itemTargets.Length;
		pTarget = 3;
		woodAttack = 0;
	}

	void HealthBar()//HP�o�[
	{
		HPbar.value = hp;
	}

	void FlowerHeal()//��
	{
		if (StartHeal == true && hp < 100)
		{
			hp += 0.1f;
			if (hp >= 100)
			{
				StartHeal = false;
			}
		}
		HealInterval += 1;
	}

	void HealBarPlus()//�񕜃o�[�i�ۂ���j
	{
		if (HealBarStart == true)
		{
			HealBar.fillAmount += 0.01f;
		}
		if (CanHeal == false)
		{
			HealBar.fillAmount = 0f;
		}
	}
	public void SwitchCanHeal()
	{
		CanHeal = true;
	}

}
