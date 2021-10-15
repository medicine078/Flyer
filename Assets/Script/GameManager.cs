using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIの利用に必要

public class GameManager : MonoBehaviour
{
    float Elapsed; //経過時間
    static public float Energy; //エネルギー
    int Score; //スコア
    public float MaxEnergy = 900.0f; //エネルギー最大値
    public float MaxTime = 90.0f; //最大時間
    public Text txtScore;
    public Image imgTimeFill;
    public Text txtTime;
    public Image imgEnergyFill;
    public Text txtEnergy;
    public Text txtMessage;
    public Image imgButton;
    public Text txtButton;
    public AudioClip BGM_Game;
    public AudioClip BGM_Menu;
    public AudioClip SE_TimeUp;
    AudioSource myAudio; //自身の音源

    int[] rank = new int[5];
    [SerializeField] Text[] txtRank = new Text[5];
    [SerializeField] GameObject[] meteoPrefabs = new GameObject[2];
    public enum MODE
    {
        TITLE, //タイトル画面
        PLAYING, //プレイ中
        TIMEUP //タイムアップ
    }
    MODE GameMode; //ゲームの状態

    //public GameObject[] MeteoPrefab = new GameObject[2]; //メテオプレハブ

    //float[] Rank = new float[6]; // 作業エリア
    //public Text[] txtRank;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("MeteoSpawner"); //メテオ生成処理

        myAudio = GetComponent<AudioSource>();
        Ready(); //準備処理
    }

    [ContextMenu("delete")]
    void Delete()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("データ領域を削除しました。");
    }

    //メテオ生成処理
    IEnumerator MeteoSpawner()
    {
        while (true)
        {
            float Ran = Random.Range(3.0f, 5.0f);
            yield return new WaitForSeconds(Ran);
            if (GameMode == MODE.PLAYING)
            {
                float Theta = Random.Range(0, Mathf.PI * 2.0f);
                float Radian = Random.value * 20.0f;
                Vector3 Pos = new Vector3(0, 0, 800);
                Pos.x = Mathf.Cos(Theta) * Radian;
                Pos.y = Mathf.Sin(Theta) * Radian;

                GameObject M = meteoPrefabs[Random.Range(0, meteoPrefabs.Length)];
                if (M.gameObject.tag == "Meteo")
                {
                    Instantiate(M, Pos, Random.rotation);
                }
                else if(M.gameObject.tag == "Gate")
                {
                    Instantiate(M, Pos, Quaternion.identity);
                }
                else if (M.gameObject.tag == "Flayer")
                {
                    Instantiate(M, Pos, Random.rotation);
                }
            }
        }
    }
    //スコア変動処理
    void ChangeScore(int Points)
    {
        Score += Points;
        txtScore.text = "SCORE: " + Score.ToString().PadLeft(5, '0');
    }

    //準備処理
    void Ready()
    {
        GameMode = MODE.TITLE;
        myAudio.clip = BGM_Menu;
        myAudio.Play(); //BGM鳴動
        Score = 0;
        Elapsed = 0.0f;
        Energy = MaxEnergy;
        txtMessage.text = "Flyer";
        txtButton.text = "START";
        imgButton.gameObject.SetActive(true);
        imgTimeFill.fillAmount = 1.0f;
        imgEnergyFill.fillAmount = 1.0f;
        txtTime.text = "TIME: " + MaxTime.ToString("f1");
        txtEnergy.text = "ENERGY: " + Energy.ToString("f1");
        txtScore.text = "SCORE: " + Score.ToString().PadLeft(5, '0');
        for (int i = 0; i < 5; i++)
        {
            txtRank[i].gameObject.SetActive(true);
        }
        Load();
    }

    //ゲーム開始処理
    void GameStart()
    {
        GameMode = MODE.PLAYING;
        txtMessage.text = "";
        for (int i = 0; i < 5; i++)
        {
            txtRank[i].gameObject.SetActive(false);
        }
        imgButton.gameObject.SetActive(false);
        myAudio.clip = BGM_Game;
        myAudio.Play(); //BGM鳴動
    }
    //ボタン銃撃処理
    void HitButton()
    {
        if (GameMode == MODE.TITLE)
        {
            GameStart(); //ゲーム開始を宣告
        }
        else if (GameMode == MODE.TIMEUP)
        {
            imgButton.gameObject.SetActive(false);
            txtMessage.text = "";
            txtEnergy.text = "";
            txtScore.text = "";
            txtTime.text = "";
            Invoke("Ready", 2.0f); //２秒後に初期化処理を実行
        }
    }

    //タイムアップ処理
    void TimeUp()
    {
        GameMode = MODE.TIMEUP;

        ChangeScore(Mathf.FloorToInt(Energy * 10)); //残りエネルギーの10倍をスコアに換算

        Save(Score);

        Energy = MaxEnergy;
        imgTimeFill.fillAmount = 0.0f;
        imgButton.gameObject.SetActive(true);
        txtButton.text = "BACK";
        txtTime.text = "TIME: 0.0";
        txtMessage.text = "TIME UP";

        GameObject[] Meteos = GameObject.FindGameObjectsWithTag("Meteo");
        foreach (GameObject stored in Meteos)
        {
            Destroy(stored); //残っていた全隕石の撤去
        }

        GameObject[] Gate = GameObject.FindGameObjectsWithTag("Gate");
        foreach (GameObject stored in Gate)
        {
            Destroy(stored); //残っていた全隕石の撤去
        }

        for (int i = 0; i < 5; i++)
        {
            txtRank[i].gameObject.SetActive(true);
        }
        Load();

        myAudio.Stop(); //BGM停止
        myAudio.PlayOneShot(SE_TimeUp); //タイムアップ鳴動
    }

    void Load()
    {
        // アプリのデータ領域が存在するか
        if (PlayerPrefs.HasKey("R1"))
        {
            for (int idx = 0; idx < 5; idx++)
            {
                rank[idx] = PlayerPrefs.GetInt("R" + idx); // データ領域読み込み
            }
            Debug.Log("データ領域を読み込みました。");
        }
        else
        {
            for (int idx = 0; idx < 5; idx++)
            {
                rank[idx] = 0/*float.MaxValue*/;
                PlayerPrefs.SetFloat("R" + idx, 0/*float.MaxValue*/); // 最大値を格納する
            }
            Debug.Log("データ領域を初期化しました。");
        }
        for (int idx = 0; idx < 5; idx++)
        {
            if (PlayerPrefs.GetInt("R" + idx) <= 0) 
            {
                txtRank[idx].text = (idx + 1) + "; ---";
            }
            else
            {
                txtRank[idx].text = (idx + 1).ToString() + ": " + PlayerPrefs.GetInt("R" + idx).ToString();
            }
        }
    }

    void Save(int Point)
    {
        int newRank = 10;
        for (int i = 0; i < 5; i++)
        {
            if (rank[i] < Point)
            {
                newRank = i;
                break;
            }
        }
        if (newRank != 10)
        {
            for (int i = 4; i > newRank; i--)
            {
                rank[i] = rank[i - 1];
            }
            rank[newRank] = Point;
            for (int i = 0; i < 5; i++)
            {
                PlayerPrefs.SetInt("R" + i, rank[i]);
            }
        }
    }
    //隕石衝突処理
   public void OnHitMeteo()
    {
        float Damage = Random.Range(80.0f, 120.0f); //ダメージ量はランダム値
        Energy -= Damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMode == MODE.PLAYING)
        {
            Elapsed += Time.deltaTime;
            if (Elapsed < MaxTime)
            {
                imgTimeFill.fillAmount = (MaxTime - Elapsed) / MaxTime;
                txtTime.text = "TIME: " + (MaxTime - Elapsed).ToString("f1");

                if (Energy > 0)
                {
                    Energy -= Time.deltaTime * 4; //ジェット推進だけでも毎秒４の減少
                    imgEnergyFill.fillAmount = Energy / MaxEnergy;
                }
                else
                {
                    Energy = 0.0f;
                    imgEnergyFill.fillAmount = 0.0f;
                }
                txtEnergy.text = "ENERGY: " + Energy.ToString("f1");
            }
            else
            {
                TimeUp(); //タイムアップを検出
            }
        }
    }
}
