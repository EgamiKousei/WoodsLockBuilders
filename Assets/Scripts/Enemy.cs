using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class Enemy : MonoBehaviour
{
	//�@�A�C�e���f�[�^�x�[�X
	[SerializeField]
	private EnemyDataBase enemyDataBase;

	private GameObject player;          // AI�Q�[���I�u�W�F�N�g
	[SerializeField]
	public GameObject[] m_Target;      // �^�[�Q�b�g�z��

	[SerializeField]
	GameObject target;					// �^�[�Q�b�g�I�u�W�F�N�g

	// �U���G�t�F�N�g�I�u�W�F�N�g
	public GameObject particleObject;
	public GameObject particleObject2;
	public GameObject particleObject3;
	public GameObject particleObject4;
	public GameObject particleObject5;

	public GameObject blowEffect;   // �̓�����G�t�F�N�g
	public GameObject circleEffect;   // �~��G�t�F�N�g
	public GameObject lineEffect;   // �����G�t�F�N�g
	public GameObject fanEffect;    // ���G�t�F�N�g
	public GameObject rotateEffect;	// ��]�G�t�F�N�g

	GameObject particle;

    //Nav nav;							// Nav�X�N���v�g

    PlayerManager PlayerManager;

	Rigidbody rb;                       // ���W�b�h�{�f�B

	[SerializeField]
	int kindEnemy = 0;					// �G�̎��

	[SerializeField]
	public int hp = 1000;				// HP
	int po = 0;                         // �U�����
	[SerializeField]
	int atc = 0;						// �U����
	[SerializeField]
	int dif = 1;						// ��Փx�i���j
	public float forcePower = 10.0f;    // �͂̑傫��
	[SerializeField]
	Vector3 force;						// �͂̑傫�����͂̌���

	public Vector3 forceDirection;      // �͂̌���

	int rnd = 0;						// �U���҂�����

	bool coroutineBool = false;         // �R���[�`�������ǂ���
	public bool isSwitch = false;       // �^�[�Q�b�g�ύX����Ȃ�true
	bool isAtc = false;                 // �U���\����
	bool isStay = true;                 // �U���̑҂�����
	bool isMax = true;
	bool isBattle = false;				// ��풆����

	private NavMeshAgent m_Agent;       // �i�r���b�V���G�[�W�F���g

	public bool isDes = false;
	public bool isTarget = false;

	NpcManager nm;

	public Animator animator;  // �A�j���[�^�[�R���|�[�l���g�擾�p

	private void Start()
    {
		hp = GetEnemy(kindEnemy).GetEnemyHp() * dif;
		atc = GetEnemy(kindEnemy).GetEnemyAtc() * dif;
		//player = GameObject.Find("Player");	// AI�I�u�W�F�N�g�擾
		//nav = player.GetComponent<Nav>();	// Nav�X�N���v�g�擾
		rb = this.GetComponent<Rigidbody>(); //���W�b�h�{�f�B���擾
		m_Agent = GetComponent<NavMeshAgent>(); // �i�r���b�V���G�[�W�F���g�擾
		if (kindEnemy == 0)
		{
			target = GameObject.FindGameObjectWithTag("Carigge");    // �^�[�Q�b�g�擾
		}
		else
        {
			target= GameObject.FindGameObjectWithTag("Player");
		}
		//m_Agent.SetDestination(m_Target[0].transform.position); // �^�[�Q�b�g��ǐ�

		nm = GameObject.Find("GameManager").GetComponent<NpcManager>();

	}

    private void Update()
    {
		rnd++;

		
		
		if(Input.GetKeyDown(KeyCode.Space))
        {
			animator.SetTrigger("Die");
		}
		


		// �^�[�Q�b�g��ǐՂ���E�ړ�
		if (m_Agent.enabled == true && m_Agent.speed != 0)
		{
			animator.SetFloat("Walk", 1); // "Float"�ɂ̓p�����[�^��������܂�
			if (target != null)
			{
				m_Agent.SetDestination(target.transform.position);
			}

			if(target.gameObject.tag == "Player")
			{
				if(target.GetComponent<PlayerManager2>().Hp <= 0)
                {
					target = GameObject.FindGameObjectWithTag("Carigge");    // �^�[�Q�b�g�擾
				}
            }
			else if (target.gameObject.tag == "Npc")
			{
				if (target.GetComponent<Nav>().hp <= 0)
				{
					target = GameObject.FindGameObjectWithTag("Carigge");    // �^�[�Q�b�g�擾
				}
			}
		}
        else
        {
			animator.SetFloat("Walk", 0); // "Float"�ɂ̓p�����[�^��������܂�
		}

		if (isDes == true)
		{
			if(target.gameObject.tag == "Npc")
            {
				target.GetComponent<Nav>().GetPoint("Enemy");
            }
			nm.StartCoroutine("ChangeDelay");
			Destroy(this.gameObject);
		}

		/*
		if (isBattle == true && target.GetComponent<PlayerManager>().Hp <= 0)
        {
			target = GameObject.FindGameObjectWithTag("Player");
			m_Agent.SetDestination(target.transform.position);
			isBattle = false;
		}
		*/

		
		/*
		if (Input.GetKeyDown(KeyCode.Return))
		{
			//m_Agent.enabled = false; // �i�r���b�V���G�[�W�F���g��K�p
			isBattle = true;
			//m_Target = GameObject.FindGameObjectsWithTag("Player");
			///*
			//target = other.gameObject;
			//if (target != null && isAtc == false && m_Agent.enabled == true)
			//{
				//m_Agent.SetDestination(target.transform.position);
			//}
			
			///*
			if (isStay == true && rnd >= 300)
			{

				isAtc = true;   // �A�^�b�N�\��
								//m_Agent.enabled = false;    // �i�r���b�V���G�[�W�F���g���~
				//transform.LookAt(other.gameObject.transform);

				// �G��ɂ���čU��������
				if (kindEnemy == 0)
				{
					po = 0;
				}
				else if (kindEnemy == 1)
				{
					po = 1;
				}
				else if (kindEnemy == 2)
				{
					//po = Random.Range(1, 5);    // �����_���ōU�����m��
					po = 1;
				}
				//po = 2; // �m�F�p

				// �G�t�F�N�g��������
				if (po == 1)
				{
					m_Agent.speed = 0;
					animator.SetTrigger("Jump");
					//Instantiate(particleObject, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
					particle = Instantiate(particleObject, this.gameObject.transform);
				}
				else if (po == 0)
				{
					animator.SetTrigger("SleepStart");
					forcePower = 10.0f;    // �͂̑傫��
					m_Agent.enabled = false; // �i�r���b�V���G�[�W�F���g��K�p
											 //Instantiate(particleObject2, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1.5f), Quaternion.identity);
											 //particle = Instantiate(particleObject2, this.gameObject.transform);
					float rad = -(gameObject.transform.localEulerAngles.y - 90.0f) * Mathf.Deg2Rad;
					float x = Mathf.Cos(rad);
					float y = 0f;
					float z = Mathf.Sin(rad);
					forceDirection = new Vector3(x, y, z);
					//Instantiate(particleObject3, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1.5f), Quaternion.identity);
					particle = Instantiate(particleObject2, this.gameObject.transform);
					// �͂̌����Ƌ�����ݒ�
					force = forcePower * forceDirection;
				}
				else if (po == 2)
				{
					animator.SetTrigger("SleepStart");
					forcePower = 50.0f;    // �͂̑傫��
					m_Agent.enabled = false; // �i�r���b�V���G�[�W�F���g��K�p
					float rad = -(gameObject.transform.localEulerAngles.y - 90.0f) * Mathf.Deg2Rad;
					float x = Mathf.Cos(rad);
					float y = 0f;
					float z = Mathf.Sin(rad);
					forceDirection = new Vector3(x, y, z);
					//Instantiate(particleObject3, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1.5f), Quaternion.identity);
					particle = Instantiate(particleObject3, this.gameObject.transform);
					// �͂̌����Ƌ�����ݒ�
					force = forcePower * forceDirection;
					//isDash = true;

				}
				else if (po == 3)
				{
					m_Agent.speed = 0;
					StartCoroutine("RageAnim");
					//Instantiate(particleObject4, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 0.5f), Quaternion.identity);
					particle = Instantiate(particleObject4, this.gameObject.transform);
					//target.GetComponent<Nav>().hp -= 30;	// �̂��ɃG�t�F�N�g�ɒǉ�
				}
				else if (po == 4)
				{
					animator.SetTrigger("Hit2");
					StartCoroutine("RotateAnim");
					m_Agent.speed = 0;
					//Instantiate(particleObject5, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
					particle = Instantiate(particleObject5, this.gameObject.transform);
					coroutineBool = true;

					//StartCoroutine("RightMove");
				}
				StartCoroutine("Effect");   // Effect�R���[�`������
											//isAtc = false;  // �A�^�b�N���ł͂Ȃ�

				StartCoroutine("Attack");   // Attack�R���[�`���𔭐�
				isStay = false; // �҂����ԏ�Ԃ�
				rnd = 0;

			}
			
		}
		*/	
	
	}

    // �U��
	private void OnTriggerStay(Collider other)
    {
		// �v���C���[���͈͂ɐN�������ꍇ
		if (other.gameObject.tag == "Player" || other.gameObject.tag == "Npc")
		{
            //m_Agent.enabled = false; // �i�r���b�V���G�[�W�F���g��K�p
            isBattle = true;
			//m_Target = GameObject.FindGameObjectsWithTag("Player");
			target = other.gameObject;
			if (target != null && isAtc == false && m_Agent.enabled == true)
			{
				m_Agent.SetDestination(target.transform.position);
			}

			if (isStay == true && rnd >= 300)
			{
               
                isAtc = true;   // �A�^�b�N�\��
				//m_Agent.enabled = false;    // �i�r���b�V���G�[�W�F���g���~
				transform.LookAt(other.gameObject.transform);

				// �G��ɂ���čU��������
				if (kindEnemy == 0)
				{
					po = 0;
				}
				else if (kindEnemy == 1)
				{
					po = 1;
				}
				else if (kindEnemy == 2)
				{
					//po = Random.Range(1, 5);    // �����_���ōU�����m��
					po = 3;
				}
				//po = 2; // �m�F�p

				// �G�t�F�N�g��������
				if (po == 1)
				{
					m_Agent.speed = 0;
					animator.SetTrigger("Jump");
					//Instantiate(particleObject, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
					particle = Instantiate(particleObject, this.gameObject.transform);
				}
				else if (po == 0)
				{
					animator.SetTrigger("SleepStart");
					m_Agent.enabled = false; // �i�r���b�V���G�[�W�F���g��K�p
					//Instantiate(particleObject2, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1.5f), Quaternion.identity);
					//particle = Instantiate(particleObject2, this.gameObject.transform);
					float rad = -(gameObject.transform.localEulerAngles.y - 90.0f) * Mathf.Deg2Rad;
					float x = Mathf.Cos(rad);
					float y = 0f;
					float z = Mathf.Sin(rad);
					forceDirection = new Vector3(x, y, z);
					//Instantiate(particleObject3, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1.5f), Quaternion.identity);
					particle = Instantiate(particleObject2, this.gameObject.transform);
					// �͂̌����Ƌ�����ݒ�
					force = forcePower * forceDirection;
				}
				else if (po == 2)
				{
					animator.SetTrigger("SleepStart");
					m_Agent.enabled = false; // �i�r���b�V���G�[�W�F���g��K�p
					float rad = -(gameObject.transform.localEulerAngles.y - 90.0f) * Mathf.Deg2Rad;
					float x = Mathf.Cos(rad);
					float y = 0f;
					float z = Mathf.Sin(rad);
					forceDirection = new Vector3(x, y, z);
					//Instantiate(particleObject3, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1.5f), Quaternion.identity);
					particle = Instantiate(particleObject3, this.gameObject.transform);
					// �͂̌����Ƌ�����ݒ�
					force = forcePower * forceDirection;
					//isDash = true;

				}
				else if (po == 3)
				{
					m_Agent.speed = 0;
					StartCoroutine("RageAnim");
					//Instantiate(particleObject4, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 0.5f), Quaternion.identity);
					particle = Instantiate(particleObject4, this.gameObject.transform);
					//target.GetComponent<Nav>().hp -= 30;	// �̂��ɃG�t�F�N�g�ɒǉ�
				}
				else if (po == 4)
				{
					animator.SetTrigger("Hit2");
					StartCoroutine("RotateAnim");
					m_Agent.speed = 0;
					//Instantiate(particleObject5, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
					particle = Instantiate(particleObject5, this.gameObject.transform);
					coroutineBool = true;
					//StartCoroutine("RightMove");
				}
				StartCoroutine("Effect");   // Effect�R���[�`������
				//isAtc = false;  // �A�^�b�N���ł͂Ȃ�

				StartCoroutine("Attack");   // Attack�R���[�`���𔭐�
				isStay = false; // �҂����ԏ�Ԃ�
				rnd = 0;
			}
		}
	}

    private void OnTriggerExit(Collider other)
    {
		/*
		// �v���C���[���͈͂��͈͊O�ɏo���ꍇ
		if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Npc") && isStay != true)
		{
			m_Agent.speed = 3.5f;
            m_Agent.enabled = true; // �i�r���b�V���G�[�W�F���g��K�p
        }
		*/
	}

    public IEnumerator Effect()
	{
		// 1�b������҂��܂��B
		yield return new WaitForSeconds(1.0f);
		Destroy(particle.gameObject);

		if (po == 2)
		{
			rb.AddForce(force, ForceMode.Impulse);
			lineEffect.SetActive(true);
			StartCoroutine("Frame");
		}
		else if(po == 0)
        {
			rb.AddForce(force, ForceMode.Impulse);
			blowEffect.SetActive(true);
			StartCoroutine("BodyBlow");
		}
		else if(po == 1)
        {
			circleEffect.SetActive(true);
			StartCoroutine("CircleAttack");
		}
		else if(po == 3)
        {
			fanEffect.SetActive(true);
			StartCoroutine("FanAttack");
		}
		else if(po == 4)
        {
			animator.SetFloat("Speed",0.7f);
			rotateEffect.SetActive(true);
			StartCoroutine("RightMove");
		}


	}

	// ��]�U���p
	IEnumerator RightMove()
	{
		for (int turn = 0; turn < 36; turn++)
		{
			transform.Rotate(0, -10, 0);
			yield return new WaitForSeconds(0.01f);
		}
		coroutineBool = false;

		rotateEffect.SetActive(false);
	}

	// ���i�U���p
	public IEnumerator Frame()
	{
		// 0.3�b������҂��܂��B
		yield return new WaitForSeconds(0.3f);

		lineEffect.SetActive(false);
		// ���W�b�h�{�f�B������Ȃ�����
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		
		m_Agent.Warp(transform.position);   // �i�r���b�V���G�[�W�F���g�����݈ʒu�Ƀ��[�v
		isAtc = false;
		//m_Agent.enabled = true; // �i�r���b�V���G�[�W�F���g��K�p


	}

	// �̓�����p
	public IEnumerator BodyBlow()
	{
		// 0.3�b������҂��܂��B
		yield return new WaitForSeconds(0.1f);

		blowEffect.SetActive(false);
		// ���W�b�h�{�f�B������Ȃ�����
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		m_Agent.Warp(transform.position);   // �i�r���b�V���G�[�W�F���g�����݈ʒu�Ƀ��[�v
		isAtc = false;
		//m_Agent.enabled = true; // �i�r���b�V���G�[�W�F���g��K�p


	}

	// �̓�����p
	public IEnumerator FanAttack()
	{
		// 0.3�b������҂��܂��B
		yield return new WaitForSeconds(0.3f);

		fanEffect.SetActive(false);
		// ���W�b�h�{�f�B������Ȃ�����
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		m_Agent.Warp(transform.position);   // �i�r���b�V���G�[�W�F���g�����݈ʒu�Ƀ��[�v
		isAtc = false;
		//m_Agent.enabled = true; // �i�r���b�V���G�[�W�F���g��K�p


	}

	// �~��͈͍U���p
	public IEnumerator CircleAttack()
	{
		// 0.3�b������҂��܂��B
		yield return new WaitForSeconds(1.0f);

		circleEffect.SetActive(false);
		// ���W�b�h�{�f�B������Ȃ�����
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		m_Agent.enabled = true; // �i�r���b�V���G�[�W�F���g��K�p
		m_Agent.speed = 3.5f;
		m_Agent.Warp(transform.position);   // �i�r���b�V���G�[�W�F���g�����݈ʒu�Ƀ��[�v
		isAtc = false;
		//m_Agent.enabled = true; // �i�r���b�V���G�[�W�F���g��K�p


	}

	public IEnumerator Attack()
    {
		yield return new WaitForSeconds(1.2f);

		isStay = true;
		//m_Agent.enabled = true;    // �i�r���b�V���G�[�W�F���g���J�n

	}

	public IEnumerator RotateAnim()
    {
		yield return new WaitForSeconds(0.1f);

		animator.SetFloat("Speed", 0.05f);

	}

	public IEnumerator RageAnim()
	{
		yield return new WaitForSeconds(0.5f);

		animator.SetTrigger("Rage");

	}

	//�@�R�[�h�ŃA�C�e�����擾
	public EnemyData GetEnemy(int searchKind)
	{
		return enemyDataBase.GetEnemyLists().Find(kindEnemy => kindEnemy.GetKindEnemy() == searchKind);
	}
}
