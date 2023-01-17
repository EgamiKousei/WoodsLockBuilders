using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class Enemy : MonoBehaviour
{
	//　アイテムデータベース
	[SerializeField]
	private EnemyDataBase enemyDataBase;

	private GameObject player;          // AIゲームオブジェクト
	[SerializeField]
	public GameObject[] m_Target;      // ターゲット配列

	[SerializeField]
	GameObject target;					// ターゲットオブジェクト

	// 攻撃エフェクトオブジェクト
	public GameObject particleObject;
	public GameObject particleObject2;
	public GameObject particleObject3;
	public GameObject particleObject4;
	public GameObject particleObject5;

	public GameObject blowEffect;   // 体当たりエフェクト
	public GameObject circleEffect;   // 円状エフェクト
	public GameObject lineEffect;   // 直線エフェクト
	public GameObject fanEffect;	// 扇状エフェクト

	GameObject particle;

    //Nav nav;							// Navスクリプト

    PlayerManager PlayerManager;

	Rigidbody rb;                       // リジッドボディ

	[SerializeField]
	int kindEnemy = 0;					// 敵の種別

	[SerializeField]
	public int hp = 1000;				// HP
	int po = 0;                         // 攻撃種類
	[SerializeField]
	int atc = 0;						// 攻撃力
	[SerializeField]
	int dif = 1;						// 難易度（仮）
	public float forcePower = 10.0f;    // 力の大きさ
	[SerializeField]
	Vector3 force;						// 力の大きさ＊力の向き

	public Vector3 forceDirection;      // 力の向き

	int rnd = 0;						// 攻撃待ち時間

	bool coroutineBool = false;         // コルーチン中かどうか
	public bool isSwitch = false;       // ターゲット変更するならtrue
	bool isAtc = false;                 // 攻撃可能判定
	bool isStay = true;                 // 攻撃の待ち判定
	bool isMax = true;
	bool isBattle = false;				// 交戦中判定

	private NavMeshAgent m_Agent;       // ナビメッシュエージェント

	public bool isDes = false;
	public bool isTarget = false;

	NpcManager nm;

	private void Start()
    {
		hp = GetEnemy(kindEnemy).GetEnemyHp() * dif;
		atc = GetEnemy(kindEnemy).GetEnemyAtc() * dif;
		//player = GameObject.Find("Player");	// AIオブジェクト取得
		//nav = player.GetComponent<Nav>();	// Navスクリプト取得
		rb = this.GetComponent<Rigidbody>(); //リジッドボディを取得
		m_Agent = GetComponent<NavMeshAgent>();	// ナビメッシュエージェント取得
		m_Target = GameObject.FindGameObjectsWithTag("Player");	// ターゲット取得
		//m_Agent.SetDestination(m_Target[0].transform.position); // ターゲットを追跡

		nm = GameObject.Find("GameManager").GetComponent<NpcManager>();

	}

    private void Update()
    {
		rnd++; 

		// ターゲットを追跡する・移動
		if (m_Agent.enabled == true)
		{
			if (target == null)
			{
				m_Agent.SetDestination(m_Target[0].transform.position);
			}
            else
            {
                m_Agent.SetDestination(target.transform.position);
            }
		}

		if (isDes == true)
		{
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
	}

    // 攻撃
	private void OnTriggerStay(Collider other)
    {
		// プレイヤーが範囲に侵入した場合
		if (other.gameObject.tag == "Player" || other.gameObject.tag == "Npc")
		{
            //m_Agent.enabled = false; // ナビメッシュエージェントを適用
            isBattle = true;
			//m_Target = GameObject.FindGameObjectsWithTag("Player");
			target = other.gameObject;
			if (target != null && isAtc == false && m_Agent.enabled == true)
			{
				m_Agent.SetDestination(target.transform.position);
			}

			if (isStay == true && rnd >= 300)
			{
               
                isAtc = true;   // アタック可能に
				//m_Agent.enabled = false;    // ナビメッシュエージェントを停止
				transform.LookAt(other.gameObject.transform);

				// 敵種によって攻撃を決定
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
					//po = Random.Range(1, 5);    // ランダムで攻撃を確定
					po = 4;
				}
				//po = 2; // 確認用

				// エフェクト発生処理
				if (po == 1)
				{
					m_Agent.speed = 0;
					//Instantiate(particleObject, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
					particle = Instantiate(particleObject, this.gameObject.transform);
				}
				else if (po == 0)
				{
					m_Agent.enabled = false; // ナビメッシュエージェントを適用
					//Instantiate(particleObject2, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1.5f), Quaternion.identity);
					//particle = Instantiate(particleObject2, this.gameObject.transform);
					float rad = -(gameObject.transform.localEulerAngles.y - 90.0f) * Mathf.Deg2Rad;
					float x = Mathf.Cos(rad);
					float y = 0f;
					float z = Mathf.Sin(rad);
					forceDirection = new Vector3(x, y, z);
					//Instantiate(particleObject3, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1.5f), Quaternion.identity);
					particle = Instantiate(particleObject2, this.gameObject.transform);
					// 力の向きと強さを設定
					force = forcePower * forceDirection;
				}
				else if (po == 2)
				{
					m_Agent.enabled = false; // ナビメッシュエージェントを適用
					float rad = -(gameObject.transform.localEulerAngles.y - 90.0f) * Mathf.Deg2Rad;
					float x = Mathf.Cos(rad);
					float y = 0f;
					float z = Mathf.Sin(rad);
					forceDirection = new Vector3(x, y, z);
					//Instantiate(particleObject3, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1.5f), Quaternion.identity);
					particle = Instantiate(particleObject3, this.gameObject.transform);
					// 力の向きと強さを設定
					force = forcePower * forceDirection;
					//isDash = true;

				}
				else if (po == 3)
				{
					m_Agent.speed = 0;
					//Instantiate(particleObject4, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 0.5f), Quaternion.identity);
					particle = Instantiate(particleObject4, this.gameObject.transform);
					//target.GetComponent<Nav>().hp -= 30;	// のちにエフェクトに追加
				}
				else if (po == 4)
				{
					m_Agent.speed = 0;
					//Instantiate(particleObject5, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
					particle = Instantiate(particleObject5, this.gameObject.transform);
					coroutineBool = true;
					//StartCoroutine("RightMove");
				}
				StartCoroutine("Effect");   // Effectコルーチン発生
				//isAtc = false;  // アタック中ではない

				StartCoroutine("Attack");   // Attackコルーチンを発生
				isStay = false; // 待ち時間状態に
				rnd = 0;
			}
		}
	}

    private void OnTriggerExit(Collider other)
    {
		// プレイヤーが範囲が範囲外に出た場合
		if (other.gameObject.tag == "Player" || other.gameObject.tag == "Npc")
		{
			m_Agent.speed = 3.5f;
			isAtc = false;  // アタック中ではない
            m_Agent.enabled = true; // ナビメッシュエージェントを適用
        }
	}

    public IEnumerator Effect()
	{
		// 1秒処理を待ちます。
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
			fanEffect.SetActive(true);
			StartCoroutine("RightMove");
		}


	}

	// 回転攻撃用
	IEnumerator RightMove()
	{
		for (int turn = 0; turn < 36; turn++)
		{
			transform.Rotate(0, 10, 0);
			yield return new WaitForSeconds(0.01f);
		}
		coroutineBool = false;

		fanEffect.SetActive(false);
	}

	// 直進攻撃用
	public IEnumerator Frame()
	{
		// 0.3秒処理を待ちます。
		yield return new WaitForSeconds(0.3f);

		lineEffect.SetActive(false);
		// リジッドボディを滑らなくする
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		
		m_Agent.Warp(transform.position);   // ナビメッシュエージェントを現在位置にワープ
		isAtc = false;
		//m_Agent.enabled = true; // ナビメッシュエージェントを適用


	}

	// 体当たり用
	public IEnumerator BodyBlow()
	{
		// 0.3秒処理を待ちます。
		yield return new WaitForSeconds(0.1f);

		blowEffect.SetActive(false);
		// リジッドボディを滑らなくする
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		m_Agent.Warp(transform.position);   // ナビメッシュエージェントを現在位置にワープ
		isAtc = false;
		//m_Agent.enabled = true; // ナビメッシュエージェントを適用


	}

	// 体当たり用
	public IEnumerator FanAttack()
	{
		// 0.3秒処理を待ちます。
		yield return new WaitForSeconds(0.3f);

		fanEffect.SetActive(false);
		// リジッドボディを滑らなくする
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		m_Agent.Warp(transform.position);   // ナビメッシュエージェントを現在位置にワープ
		isAtc = false;
		//m_Agent.enabled = true; // ナビメッシュエージェントを適用


	}

	// 円状範囲攻撃用
	public IEnumerator CircleAttack()
	{
		// 0.3秒処理を待ちます。
		yield return new WaitForSeconds(0.3f);

		circleEffect.SetActive(false);
		// リジッドボディを滑らなくする
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		m_Agent.Warp(transform.position);   // ナビメッシュエージェントを現在位置にワープ
		isAtc = false;
		//m_Agent.enabled = true; // ナビメッシュエージェントを適用


	}

	public IEnumerator Attack()
    {
		yield return new WaitForSeconds(1.2f);

		isStay = true;
		//m_Agent.enabled = true;    // ナビメッシュエージェントを開始

	}

	//　コードでアイテムを取得
	public EnemyData GetEnemy(int searchKind)
	{
		return enemyDataBase.GetEnemyLists().Find(kindEnemy => kindEnemy.GetKindEnemy() == searchKind);
	}
}
