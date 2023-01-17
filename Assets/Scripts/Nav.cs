using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



// ダッシュ方向データ（配列）
[System.Serializable]
public class DashData
{
	public dashData[] data;
}

// ダッシュ方向データ（変数）
[System.Serializable]
public class dashData
{
	public float sr;	// 回避角度最小値
	public float gr;	// 回避角度最大値
	public bool ch;		// 回避角度を記録するならtrue
}



[RequireComponent(typeof(NavMeshAgent))]	// NavMesh使用する合図
public class Nav : MonoBehaviour
{
	Renderer targetRenderer; // 判定したいオブジェクトのrendererへの参照

	string datapath;						// ダッシュデータまでのパス
	DashData dh;                            // ダッシュデータ格納変数

	string datapath2;                       // NPCデータまでのパス
	NpcData npc;                            // NPCデータ格納変数

	string datapath3;                       // NPCデータまでのパス
	Npc1Data npc1;                          // NPCデータ格納変数

	string datapath4;                       // NPCデータまでのパス
	Npc2Data npc2;                          // NPCデータ格納変数

	string datapath5;                       // NPCデータまでのパス
	Npc3Data npc3;                          // NPCデータ格納変数

	public NavMeshAgent m_Agent;			// NavMeshAgent変数
	[SerializeField]
	Rigidbody rb;                           // Rigidbody変数
	BoxCollider boxCollider;                // BoxCollider変数
	SphereCollider sphereCollider;			// SphereCollider変数

	GameObject player;						// AIゲームオブジェクト
	[SerializeField]
	GameObject[] targets;                   // ターゲットオブジェクト格納配列
	[SerializeField]
	GameObject[] itemTargets;               // アイテムオブジェクト格納配列
	[SerializeField]
	GameObject troTarget;					// トロッコターゲットオブジェクト	
	[SerializeField]
	GameObject closeEnemy;                  // 一番近いターゲットオブジェクト 
	[SerializeField]
	GameObject closeItem;                   // 一番近いターゲットオブジェクト 
	[SerializeField]
	GameObject mineCart;                    // 採掘時のカート
	[SerializeField]
	GameObject damageLine;					// 採掘時のダメージライン

	int wd = 0;								// 木材の数
	int bw = 0;                             // 馬車に積まれている木材の数
	int rnd = 0;                            // 攻撃カウント
	int dPer = 0;                           // ダッシュかどうかのランダム判定用変数
	int enduHeal = 0;                       // 耐久値回復基準
	int i;
	[SerializeField]
	public int pTarget = 0;					// 優先するターゲット
	float old = 0;							// ダッシュ角度格納変数
	float oldHp = 30;                       // ダッシュ前HP格納変
	float dCnt = 0;                         // ダッシュカウント
	float sCnt = 0;                         // ターゲット確認カウント
	float aCnt = 0;                         // ターゲット確認カウント
	float heal = 0;                         // 回復するHP
	float torchHeal = 0;                    // 松明点灯時間
	int sceneNo = 0;						// 0：採取、1：採掘

	public int coin = 0;                    // コイン枚数
	public int plHp = 100;                  // プレイヤーHPサンプル
	public int endu = 100;                  // 耐久値のサンプル
	public int torch = 100;					// 松明サンプル
	public int id = -1;                     // 攻撃種類
	public int code = 0;                    // 自身に付与されているNO
	public int lv;                          // レベル
	public float speed;                     // 移動スピード
	public float upForce = 20f;             //上方向にかける力
	public float forcePower = 20.0f;        // 横方向にかける力
	public float closeDist = 1000;          // 敵との距離
	public float cartDist;					// 採掘のトロッコとダメージラインの距離
	public float hp = 30;                   // HP
	public string tps = "";                 // 優先する敵を位置によって取得

	public bool isSwitch = true;			// ターゲットを変更するならtrue 
	public bool isItem = false;				// アイテムをターゲットにするならtrue
	public bool isGround = false;			// 地面と接触しているならtrue
	public bool isWall = false;				// 壁を上る動作中ならtrue
	public bool isDash = false;				// ダッシュ中true
	//public bool isAtc = false;            // 攻撃中ならtrue
	public bool isArrow = true;             // 遠隔攻撃ならtrue
	public bool isDmg = false;              // ダメージ判定があるかどうか
	public bool isMain = false;             // mainTargetについて
	public bool isObstacle = false;         // 障害物をターゲットにするかどうか
	public bool isChange = false;           // ターゲットチェンジ
	public bool isZone = false;
	bool isInc = false;						// 行動不能かどうか

	public Vector3 forceDirection;          // 力の方向
	public Vector3 arrowDirection;          // 遠隔攻撃の力の方向
	public Vector3 force;					// 力*向き

	//pHp php;                              // HP管理用スクリプト

	public GameObject mainTarget;           // 現在のターゲット
	GameObject carTarget;                   // Enemy側の最初のターゲット

	NpcManager npcManager;                  // NpcManagerスクリプト


	// プレイヤーに合わせている値
	public GameObject AttackHantei;//攻撃判定
	bool CanAttack = true;//攻撃可能か
	int woodAttack = 0; // 木を攻撃した数
	public bool HandHaving = false;//何かアイテムを持ってるか
	[SerializeField]
	int woodBlock = 0;
	public bool isWoodBlock = false;
	[SerializeField]
	GameObject Carigge; // 馬車

	float MaxHP = 100;//hp

	public Slider HPbar;//HPバー
	public Image HealBar;//回復するときのバー

	public bool CanHeal = false;//回復できるか
	public bool HealBarStart = false;//回復UIスタート
	public bool StartHeal = false;//回復スタート

	public float HealInterval = 500;//次に回復可能になるまでの時間（フレーム数）

	public bool isFire = false; // 松明を点火する際の判定
	public GameObject fireZone; // 松明を点火する場所
	public GameObject healZone; // 回復する場所
	public GameObject warpPoint;
	public bool isHeal = false;// 回復うする際の判定
	public bool cariggeAttack = false;

	private void Awake()
	{
		datapath = Application.dataPath + "/Dash.json"; // ダッシュデータまでのパス
		datapath2 = Application.dataPath + "/Npc.json"; // NPCデータまでのパス
		datapath3 = Application.dataPath + "/Npc1.json"; // NPCデータまでのパス
		datapath4 = Application.dataPath + "/Npc2.json"; // NPCデータまでのパス
		datapath5 = Application.dataPath + "/Npc3.json"; // NPCデータまでのパス
	}

	void Start()
	{
		targetRenderer = GetComponent<Renderer>();

		m_Agent = GetComponent<NavMeshAgent>(); // NavMeshAgentを取得

		rb = this.GetComponent<Rigidbody>(); //リジッドボディを取得

		boxCollider = this.GetComponent<BoxCollider>(); // BoxColliderを取得
		sphereCollider = this.GetComponent<SphereCollider>();	// SphereColliderを取得

		//arrow = (GameObject)Resources.Load("Sphere");	// 遠隔攻撃用オブジェクト取得

		//php = GetComponent<pHp>();  // HP管理用スクリプト取得

		code = this.gameObject.GetComponent<NpcCode>().npcNo;   // NPCのナンバー取得

		npcManager = GameObject.Find("GameManager").GetComponent<NpcManager>();

		dh = new DashData();		// ダッシュデータ取得

		dh = LoadFile(datapath);	// ダッシュデータロード

		SaveFile(dh);               // ダッシュデータセーブ

		npc = new NpcData();        // NPCデータ取得

		npc = LoadNPC(datapath2);   // NPCデータロード

		SaveNPC(npc);               // NPCデータセーブ

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
			npc1 = new Npc1Data();        // NPC1データ取得
			npc1 = LoadNPC1(datapath3);   // NPC1データロード
			SaveNPC1(npc1);               // NPC1データセーブ

			lv = npc1.lv;				  // レベル
			tps = npc1.tpsEnemy;          // 優先する敵を記録
			heal = npc1.healHp;           // HP回復基準
			torchHeal = npc1.torch;       // 松明店頭基準
			enduHeal = npc1.endu;         // 耐久値回復基準
		}
		else if(code == 2)
        {
			npc2 = new Npc2Data();        // NPC2データ取得
			npc2 = LoadNPC2(datapath4);   // NPC2データロード
			SaveNPC2(npc2);               // NPC2データセーブ

			lv = npc2.lv;                 // レベル
			tps = npc2.tpsEnemy;          // 優先する敵を記録
			heal = npc2.healHp;           // HP回復基準
			torchHeal = npc2.torch;       // 松明店頭基準
			enduHeal = npc2.endu;		  // 耐久値回復基準
		}
		else if(code == 3)
        {
			npc3 = new Npc3Data();        // NPC3データ取得
			npc3 = LoadNPC3(datapath5);   // NPC3データロード
			SaveNPC(npc);                 // NPC3データセーブ

			lv = npc3.lv;                 // レベル
			tps = npc3.tpsEnemy;          // 優先する敵を記録
			heal = npc3.healHp;           // HP回復基準
			torchHeal = npc3.torch;       // 松明店頭基準
			enduHeal = npc3.endu;         // 耐久値回復基準
		}

		rb.constraints = RigidbodyConstraints.FreezeRotationZ;
		rb.constraints = RigidbodyConstraints.FreezeRotationX;
		rb.constraints = RigidbodyConstraints.FreezePositionY;

		//Switch();					// ターゲット取得
	}

	// ダッシュデータロード関数
	public DashData LoadFile(string dataPath)
	{
		StreamReader reader = new StreamReader(dataPath);
		string datastr = reader.ReadToEnd();
		reader.Close();

		return JsonUtility.FromJson<DashData>(datastr);
	}

	// ダッシュデータセーブ関数
	public void SaveFile(DashData dh)
	{
		string jsonstr = JsonUtility.ToJson(dh);
		StreamWriter wreiter = new StreamWriter(datapath, false);
		wreiter.WriteLine(jsonstr);
		wreiter.Flush();
		wreiter.Close();
	}

	// NPCデータロード関数
	public NpcData LoadNPC(string dataPath2)
	{
		StreamReader reader = new StreamReader(dataPath2);
		string datastr = reader.ReadToEnd();
		reader.Close();

		return JsonUtility.FromJson<NpcData>(datastr);
	}

	// NPCデータセーブ関数
	public void SaveNPC(NpcData npc)
	{
		string jsonstr = JsonUtility.ToJson(npc);
		StreamWriter wreiter = new StreamWriter(datapath2, false);
		wreiter.WriteLine(jsonstr);
		wreiter.Flush();
		wreiter.Close();
	}

	// NPCデータロード関数
	public Npc1Data LoadNPC1(string dataPath3)
	{
		StreamReader reader = new StreamReader(dataPath3);
		string datastr = reader.ReadToEnd();
		reader.Close();

		return JsonUtility.FromJson<Npc1Data>(datastr);
	}

	// NPCデータセーブ関数
	public void SaveNPC1(Npc1Data npc1)
	{
		string jsonstr = JsonUtility.ToJson(npc1);
		StreamWriter wreiter = new StreamWriter(datapath3, false);
		wreiter.WriteLine(jsonstr);
		wreiter.Flush();
		wreiter.Close();
	}

	// NPCデータロード関数
	public Npc2Data LoadNPC2(string dataPath4)
	{
		StreamReader reader = new StreamReader(dataPath4);
		string datastr = reader.ReadToEnd();
		reader.Close();

		return JsonUtility.FromJson<Npc2Data>(datastr);
	}

	// NPCデータセーブ関数
	public void SaveNPC2(Npc2Data npc2)
	{
		string jsonstr = JsonUtility.ToJson(npc2);
		StreamWriter wreiter = new StreamWriter(datapath4, false);
		wreiter.WriteLine(jsonstr);
		wreiter.Flush();
		wreiter.Close();
	}

	// NPCデータロード関数
	public Npc3Data LoadNPC3(string dataPath5)
	{
		StreamReader reader = new StreamReader(dataPath5);
		string datastr = reader.ReadToEnd();
		reader.Close();

		return JsonUtility.FromJson<Npc3Data>(datastr);
	}

	// NPCデータセーブ関数
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
			m_Agent.Warp(transform.position);   // ナビメッシュエージェントを現在位置にワープ

			npcManager.ChangeEnemy();
            
		}

		HealthBar();

		if (hp >= MaxHP)
        {
			hp = MaxHP;
		}

		if (hp >= 100)//hp100だとヒール不可
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

		// 攻撃カウントをプラス
		rnd++;

		// ダッシュカウントをプラス
		dCnt += Time.deltaTime;

		// ターゲット確認カウントをプラス
		sCnt += Time.deltaTime;

		// ダッシュカウントをプラス
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
		// ターゲットを追跡する
		if (m_Agent.enabled == true)
		{
			// ターゲットがエネミーの場合
			if (pTarget == 1 && closeEnemy != null)
			{
				m_Agent.SetDestination(closeEnemy.transform.position);
				mainTarget = closeEnemy;
			}
			// ターゲットがトロッコの場合
			else if(pTarget == 2)
            {
				m_Agent.SetDestination(troTarget.transform.position);
				mainTarget = troTarget;
			}
			// ターゲットがアイテムの場合
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
		// ダッシュ
		if ((Input.GetKeyDown(KeyCode.Z) || php.isDmg == true) && isDash == false)
		{
			oldHp = php.hp; // ヒット前のHPを記録
			float rnd = Random.Range(dh.data[php.id].sr, dh.data[php.id].gr);   // 回避角度をランダムで設定（範囲はダッシュデータより）
			old = rnd;  // 回避角度を保持
			m_Agent.enabled = false;    // ナビメッシュエージェントを停止

			//	力の向きを設定
			float rad = -(this.gameObject.transform.localEulerAngles.y - rnd) * Mathf.Deg2Rad;
			float x = Mathf.Cos(rad);
			float y = 0f;
			float z = Mathf.Sin(rad);
			forceDirection = new Vector3(x, y, z);

			// 力の向きと強さを設定
			force = forcePower * forceDirection;
			rb.AddForce(force, ForceMode.Impulse);

			isDash = true;  // ダッシュ状態にする
			StartCoroutine("Frame");    // Frameコルーチンをスタート
		}
		*/

		/*// 遠隔攻撃
		if (rnd >= 300 && isArrow == true)
		{
			m_Agent.enabled = false;	// ナビメッシュエージェントを停止

			//	力の向きを設定
			float rad = -(this.gameObject.transform.localEulerAngles.y - 90.0f) * Mathf.Deg2Rad;
			float x = Mathf.Cos(rad);
			float y = 0f;
			float z = Mathf.Sin(rad);
			arrowDirection = new Vector3(x, y, z);

			// 遠隔攻撃オブジェクト発生
			//Instantiate(arrow, new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z + 1.0f), Quaternion.identity);
			Instantiate(arrow, this.gameObject.transform);

			// Frameコルーチンを発生
			StartCoroutine("Frame");

			// 攻撃カウントを0に
			rnd = 0;
		
		}*/

		// NPCの番号によって参照するデータを変更
		// 回復行動
		if (hp < heal)
		{
			hp = 100;
		}

		// 松明の回復行動
		if (torch < torchHeal)
		{
			torch = 100;
		}


		// プレイヤー復活行動 自分のHPが0ではなくプレイヤーHPが0以下の場合
		if (plHp <= 0)
		{
			plHp = 30;  // プレイヤーHPを30にして復活
		}

		// ダッシュ行動するかどうかのカウント


		/*
		// ダッシュ行動
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

		// HPが０になった場合の処理
        if (hp <= 0)
        {
			isInc = true;
			m_Agent.enabled = false;
			boxCollider.enabled = false;
			sphereCollider.enabled = false;
        }

		/*
        // HPが変動した際の処理
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

	// ターゲット変更をできるようにする関数
	void SwitchOn()
	{
		isSwitch = true;
	}

	// ターゲット設定関数
	public void Switch()
    {
		m_Agent.enabled = true;
		m_Agent.speed = 6.5f;

		// アイテムオブジェクト探索
		itemTargets = GameObject.FindGameObjectsWithTag("Item");
		if (itemTargets.Length != 0)
		{
			//Debug.Log("aaa");
			pTarget = 3;
		}

		/*
		if (GameObject.Find("GameManager").GetComponent<NpcManager>().isTro == false)
		{
			// トロッコターゲットオブジェクト探索
			troTarget = GameObject.FindGameObjectWithTag("Target");
			if (troTarget != null)
			{
				//Debug.Log("aaa");
				pTarget = 2;
			}
		}
		*/

		// エネミーオブジェクト探索
		targets = GameObject.FindGameObjectsWithTag("Enemy");
		int npcLen = targets.Length;
		if (targets.Length != 0)
		{
			//Debug.Log("aaa");
			pTarget = 1;
		}

		if (pTarget == 1)
		{
			// 「初期値」の設定
			closeDist = 1000;

			foreach (GameObject y in targets)
			{
				// コンソール画面での確認用コード
				//print(Vector3.Distance(transform.position, t.transform.position));

				// このオブジェクト（砲弾）と敵までの距離を計測
				float tDist = Vector3.Distance(transform.position, y.transform.position);

				// もしも「初期値」よりも「計測した敵までの距離」の方が近いならば、
				if (closeDist > tDist)
				{
					// 「closeDist」を「tDist（その敵までの距離）」に置き換える。
					// これを繰り返すことで、一番近い敵を見つけ出すことができる。
					closeDist = tDist;

					// 一番近い敵の情報をcloseEnemyという変数に格納する（★）
					closeEnemy = y;

					mainTarget = closeEnemy;
				}

				// 砲弾が生成されて0.5秒後に、一番近い敵に向かって移動を開始する。
				//Invoke("SwitchOn", 0.5f);
				m_Agent.SetDestination(closeEnemy.transform.position);
			}
		}
		else if(pTarget == 3)
        {
			// 「初期値」の設定
			closeDist = 1000;

			foreach (GameObject y in itemTargets)
			{
				// コンソール画面での確認用コード
				//print(Vector3.Distance(transform.position, t.transform.position));

				// このオブジェクト（砲弾）と敵までの距離を計測
				float tDist = Vector3.Distance(transform.position, y.transform.position);

				// もしも「初期値」よりも「計測した敵までの距離」の方が近いならば、
				if (closeDist > tDist)
				{
					// 「closeDist」を「tDist（その敵までの距離）」に置き換える。
					// これを繰り返すことで、一番近い敵を見つけ出すことができる。
					closeDist = tDist;

					// 一番近い敵の情報をcloseEnemyという変数に格納する（★）
					closeItem = y;

					mainTarget = closeItem;
				}

				// 砲弾が生成されて0.5秒後に、一番近い敵に向かって移動を開始する。
				//Invoke("SwitchOn", 0.5f);
				m_Agent.SetDestination(closeItem.transform.position);
			}
		}
		i = 0;
	}

	// ターゲット設定関数
	public void Switch2()
	{
		m_Agent.enabled = true;
		m_Agent.speed = 6.5f;


		// アイテムオブジェクト探索
		itemTargets = GameObject.FindGameObjectsWithTag("Item");
		if (itemTargets.Length != 0)
		{
			//Debug.Log("aaa");
			pTarget = 3;
		}

		// エネミーオブジェクト探索
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
			// トロッコターゲットオブジェクト探索
			troTarget = GameObject.FindGameObjectWithTag("Target");
			if (troTarget != null)
			{
				//Debug.Log("aaa");
				pTarget = 2;
			}
		}
		*/


		// 「初期値」の設定
		closeDist = 1000;
		// 敵のターゲットを取得
		//carTarget = GameObject.FindGameObjectWithTag("eTarget");

		if (pTarget == 1)
		{
			foreach (GameObject y in targets)
			{
				// コンソール画面での確認用コード
				//print(Vector3.Distance(transform.position, t.transform.position));

				if (tps == "npc")
				{
					// このオブジェクト（砲弾）と敵までの距離を計測
					float tDist = Vector3.Distance(transform.position, y.transform.position);

					// もしも「初期値」よりも「計測した敵までの距離」の方が近いならば、
					if (closeDist > tDist)
					{
						// 「closeDist」を「tDist（その敵までの距離）」に置き換える。
						// これを繰り返すことで、一番近い敵を見つけ出すことができる。
						closeDist = tDist;

						// 一番近い敵の情報をcloseEnemyという変数に格納する（★）
						closeEnemy = y;

						mainTarget = closeEnemy;
					}
				}
				else if (tps == "car")
				{
					// このオブジェクト（砲弾）と敵までの距離を計測
					float tDist = Vector3.Distance(Carigge.transform.position, y.transform.position) + Vector3.Distance(transform.position, y.transform.position);

					// もしも「初期値」よりも「計測した敵までの距離」の方が近いならば、
					if (closeDist > tDist)
					{
						// 「closeDist」を「tDist（その敵までの距離）」に置き換える。
						// これを繰り返すことで、一番近い敵を見つけ出すことができる。
						closeDist = tDist;

						// 一番近い敵の情報をcloseEnemyという変数に格納する（★）
						closeEnemy = y;

						mainTarget = closeEnemy;
					}
				}
				// 砲弾が生成されて0.5秒後に、一番近い敵に向かって移動を開始する。
				//Invoke("SwitchOn", 0.5f);
				m_Agent.SetDestination(closeEnemy.transform.position);
			}
		}
		else if(pTarget == 3)
        {
			// 「初期値」の設定
			closeDist = 1000;

			foreach (GameObject y in itemTargets)
			{
				// コンソール画面での確認用コード
				//print(Vector3.Distance(transform.position, t.transform.position));

				// このオブジェクト（砲弾）と敵までの距離を計測
				float tDist = Vector3.Distance(transform.position, y.transform.position);

				// もしも「初期値」よりも「計測した敵までの距離」の方が近いならば、
				if (closeDist > tDist)
				{
					// 「closeDist」を「tDist（その敵までの距離）」に置き換える。
					// これを繰り返すことで、一番近い敵を見つけ出すことができる。
					closeDist = tDist;

					// 一番近い敵の情報をcloseEnemyという変数に格納する（★）
					closeItem = y;

					mainTarget = closeItem;
				}

				// 砲弾が生成されて0.5秒後に、一番近い敵に向かって移動を開始する。
				//Invoke("SwitchOn", 0.5f);
				m_Agent.SetDestination(closeItem.transform.position);
			}
		}
		i = 0;
	}



	// ターゲット設定関数
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

			// アイテムオブジェクト探索
			itemTargets = GameObject.FindGameObjectsWithTag("Item");
			if (itemTargets.Length != 0)
			{
				//Debug.Log("aaa");
				pTarget = 3;
			}


			// エネミーオブジェクト探索
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
				// トロッコターゲットオブジェクト探索
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
				// 「初期値」の設定
				closeDist = 1000;
				// 敵のターゲットを取得
				//carTarget = GameObject.FindGameObjectWithTag("eTarget");

				foreach (GameObject t in targets)
				{
					if (t.GetComponent<Enemy>().isTarget == false)
					{
						// コンソール画面での確認用コード
						//print(Vector3.Distance(transform.position, t.transform.position));

						if (tps == "npc")
						{
							// このオブジェクト（砲弾）と敵までの距離を計測
							float tDist = Vector3.Distance(transform.position, t.transform.position);

							// もしも「初期値」よりも「計測した敵までの距離」の方が近いならば、
							if (closeDist > tDist)
							{
								// 「closeDist」を「tDist（その敵までの距離）」に置き換える。
								// これを繰り返すことで、一番近い敵を見つけ出すことができる。
								closeDist = tDist;

								// 一番近い敵の情報をcloseEnemyという変数に格納する（★）
								closeEnemy = t;

								mainTarget = closeEnemy;
							}
						}
						else if (tps == "car")
						{
							// このオブジェクト（砲弾）と敵までの距離を計測
							float tDist = Vector3.Distance(Carigge.transform.position, t.transform.position) + Vector3.Distance(transform.position, t.transform.position);

							// もしも「初期値」よりも「計測した敵までの距離」の方が近いならば、
							if (closeDist > tDist)
							{
								// 「closeDist」を「tDist（その敵までの距離）」に置き換える。
								// これを繰り返すことで、一番近い敵を見つけ出すことができる。
								closeDist = tDist;

								// 一番近い敵の情報をcloseEnemyという変数に格納する（★）
								closeEnemy = t;

								mainTarget = closeEnemy;
							}
						}
						// 砲弾が生成されて0.5秒後に、一番近い敵に向かって移動を開始する。
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
							// コンソール画面での確認用コード
							//print(Vector3.Distance(transform.position, t.transform.position));

							if (tps == "npc")
							{
								// このオブジェクト（砲弾）と敵までの距離を計測
								float tDist = Vector3.Distance(transform.position, y.transform.position);

								// もしも「初期値」よりも「計測した敵までの距離」の方が近いならば、
								if (closeDist > tDist)
								{
									// 「closeDist」を「tDist（その敵までの距離）」に置き換える。
									// これを繰り返すことで、一番近い敵を見つけ出すことができる。
									closeDist = tDist;

									// 一番近い敵の情報をcloseEnemyという変数に格納する（★）
									closeEnemy = y;

									mainTarget = closeEnemy;
								}
							}
							else if (tps == "car")
							{
								// このオブジェクト（砲弾）と敵までの距離を計測
								float tDist = Vector3.Distance(carTarget.transform.position, y.transform.position) + Vector3.Distance(transform.position, y.transform.position);

								// もしも「初期値」よりも「計測した敵までの距離」の方が近いならば、
								if (closeDist > tDist)
								{
									// 「closeDist」を「tDist（その敵までの距離）」に置き換える。
									// これを繰り返すことで、一番近い敵を見つけ出すことができる。
									closeDist = tDist;

									// 一番近い敵の情報をcloseEnemyという変数に格納する（★）
									closeEnemy = y;

									mainTarget = closeEnemy;
								}
							}
							// 砲弾が生成されて0.5秒後に、一番近い敵に向かって移動を開始する。
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

				// 「初期値」の設定
				closeDist = 1000;


				foreach (GameObject y in itemTargets)
				{

					if (y.GetComponent<ItemTarget>().isTarget == false)
					{
						// コンソール画面での確認用コード
						//print(Vector3.Distance(transform.position, t.transform.position));

						// このオブジェクト（砲弾）と敵までの距離を計測
						float tDist = Vector3.Distance(transform.position, y.transform.position);

						// もしも「初期値」よりも「計測した敵までの距離」の方が近いならば、
						if (closeDist > tDist)
						{
							// 「closeDist」を「tDist（その敵までの距離）」に置き換える。
							// これを繰り返すことで、一番近い敵を見つけ出すことができる。
							closeDist = tDist;

							// 一番近い敵の情報をcloseEnemyという変数に格納する（★）
							closeItem = y;

							mainTarget = closeItem;
						}

						// 砲弾が生成されて0.5秒後に、一番近い敵に向かって移動を開始する。
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
		if (other.gameObject.tag == "Ground") //Groundタグのオブジェクトに触れたとき
		{
			isGround = true;    //isGroundをtrueにする
			m_Agent.enabled = true; // ナビメッシュエージェントを発生
			rb.constraints = RigidbodyConstraints.FreezePositionY;  // リジッドボディのy軸移動を制限
		}



		if (other.gameObject.tag == "basya")    // basyaタグのオブジェクトに触れた時
		{
			bw += wd;   // 手持ちの木材の数を馬車に積まれている木材の数に＋
			wd = 0; // 手持ちの木材の数を0に
			Debug.Log(wd);
			Debug.Log(bw);
		}
	}

    private void OnCollisionStay(Collision collision)
    {
		if (collision.gameObject.tag == "Item")
		{
			//transform.LookAt(collision.gameObject.transform);
			if (collision.gameObject.GetComponent<ItemName>().name == "Tree") // Woodタグのオブジェクトに触れた時
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
					wd++;   // 手持ちの木材の数を＋
				targets = GameObject.FindGameObjectsWithTag("basya");   // ターゲットを馬車に変更
				m_Agent.SetDestination(targets[0].transform.position);  // ターゲットに移動を開始
				Debug.Log(wd);
				*/
			}

			if (collision.gameObject.GetComponent<ItemName>().name == "Metal") // Woodタグのオブジェクトに触れた時
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

			if (collision.gameObject.GetComponent<ItemName>().name == "Coal") // Woodタグのオブジェクトに触れた時
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
					wd++;   // 手持ちの木材の数を＋
				targets = GameObject.FindGameObjectsWithTag("basya");   // ターゲットを馬車に変更
				m_Agent.SetDestination(targets[0].transform.position);  // ターゲットに移動を開始
				Debug.Log(wd);
				*/
			}
		}
	}

    public IEnumerator Frame()
	{
		// 0.3秒処理を待ちます。
		yield return new WaitForSeconds(0.3f);

		// リジッドボディを滑らなくする
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		m_Agent.Warp(transform.position);   // ナビメッシュエージェントを現在位置にワープ
		m_Agent.enabled = true; // ナビメッシュエージェントを適用

		if (isDash == true)
		{
			// 2秒処理を待ちます。
			yield return new WaitForSeconds(2.0f);
			// ダッシュ状態でなくする
			isDash = false;
		}

	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			//m_Agent.enabled = false;

			/*if (isGround == true)//着地しているとき
			{
				m_Agent.enabled = false;
				//m_Agent.isStopped = true;
				isGround = false;//  isGroundをfalseにする
				Debug.Log("aaa");
				rb.AddForce(new Vector3(0, upForce, 0),ForceMode.Impulse); //上に向かって力を加える
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

	// 攻撃
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			//transform.LookAt(other.gameObject.transform);
			//移動も回転もしないようにする
			rb.constraints = RigidbodyConstraints.FreezeAll;

			if (CanAttack == true)
			{
				CanAttack = false;
				AttackHantei.SetActive(true);
				Invoke("HanteiDel", 0.2f);
				Invoke("CanAttackTrue", 0.5f);
				m_Agent.speed = 0;    // ナビメッシュエージェントを停止
				transform.LookAt(other.gameObject.transform);
				//Debug.Log("Attack!");
				//other.gameObject.GetComponent<Enemy>().hp -= 30;
				if(other.gameObject.GetComponent<Enemy>().hp <= 0)
                {
					mainTarget = null;
					other.gameObject.GetComponent<Enemy>().isDes = true;
					isMain = true;
					m_Agent.enabled = true;    // ナビメッシュエージェントを再生
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

			if (other.gameObject.GetComponent<ItemName>().name == "Flower") // Woodタグのオブジェクトに触れた時
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

	// 壁を上る動き
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
		// 攻撃にヒットしていない場合
		if ((other.gameObject.tag == "En" || other.gameObject.tag == "Tai" || other.gameObject.tag == "Tyoku" || other.gameObject.tag == "Ougi" || other.gameObject.tag == "Kaiten") && dh.data[php.id].ch == true && oldHp == php.hp)
		{
			if ((old - 30.0f) >= 0.0f)	// 角度が0より大きいなら
			{
				dh.data[php.id].sr = old - 30.0f;	// 回避した角度-30度を設定
            }
            else
            {
				dh.data[php.id].sr = 0.0f;	// 0度を設定

			}

			if ((old + 30.0f) <= 360.0f)	// 角度が360より小さいなら
			{
				dh.data[php.id].gr = old + 30.0f;	// 回避した角度＋30度を設定
			}
			else
			{
				dh.data[php.id].gr = 360.0f;	// 360度を設定

			}

			dh.data[php.id].ch = false;	// 回避角度を変更しないように設定

			php.id = -1;	// 攻撃をしていない状態に変更

			SaveFile(dh);	// ダッシュデータをセーブ
		}*/

		// 敵から離れたら
        if (other.gameObject.tag == "Enemy")
        {
			m_Agent.enabled = true;    // ナビメッシュエージェントを復帰
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

	void HealthBar()//HPバー
	{
		HPbar.value = hp;
	}

	void FlowerHeal()//回復
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

	void HealBarPlus()//回復バー（丸いやつ）
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
