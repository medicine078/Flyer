using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movSpeed = 8.0f; //移動速度
    GameObject Player; //プレイヤー

    GameObject[] Muzzle; //レーザー射出口
    AudioSource myAudio; //自身の音源
    public GameObject LaserPrefab; //レーザープレハブ
    public AudioClip SeLaser; //レーザー射出音
    public float LaserSpeed = 500.0f; //レーザー速度って光速なんですけど。。。

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("MyShipRoot"); //プレイヤーを取得
        Muzzle = GameObject.FindGameObjectsWithTag("Muzzle"); //レーザー射出口を取得
        myAudio = GetComponent<AudioSource>(); //自身の音源を取得
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Energy <= 0)
        {
            return; //エネルギーゼロで操船不能
        }


        //レーザー射出処理
        if (Input.GetMouseButtonDown(0) ||
        OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            for (int idx = 0; idx < Muzzle.Length; idx++)
            { //射出口の数だけ繰り返す
                GameObject LZ = Instantiate(LaserPrefab); //レーザー生成
                LZ.transform.position = Muzzle[idx].transform.position;
                LZ.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, LaserSpeed);
            }
            myAudio.PlayOneShot(SeLaser); //レーザー射出音鳴動

            GameManager.Energy -= 4; //エネルギーを消費
        }

        //カメラの位置＆回転情報を取得
        Transform T = GetComponent<Transform>();
        //カメラの視線を作成
        Ray GazerRay = new Ray(T.position, T.TransformDirection(Vector3.forward));
        //視線が当たった物体の情報を入れる変数を定義
        RaycastHit hitInfo;
        //もしGayzerRayがMovableAreaに当たったら
        if (Physics.Raycast(GazerRay, out hitInfo))
        {
            if (hitInfo.collider.gameObject.name == "MovableArea")
            {
                Player.transform.position +=
                (hitInfo.point - Player.transform.position) * movSpeed * Time.deltaTime;
            }
        }
    }
}
