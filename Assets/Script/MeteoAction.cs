using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoAction : MonoBehaviour
{

    public GameObject ExplosionPrefab; //爆発エフェクト
    public GameObject RainbowExplode2; //爆発エフェクト
    public float MoveSpeed = -60.0f; //移動速度
    GameObject Manager; //マネージャー

    // Start is called before the first frame update
    void Start()
    {  
        if (gameObject.tag == "Meteo")
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, MoveSpeed); //速度を与える
            GetComponent<Rigidbody>().AddTorque(Random.rotation.eulerAngles); //自身の回転
        }
        else if(gameObject.tag == "Gate")
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, MoveSpeed * 2); //速度を与える
        }
        else if (gameObject.tag == "Flayer")
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, MoveSpeed * 2); //速度を与える
            GetComponent<Rigidbody>().AddTorque(Random.rotation.eulerAngles); //自身の回転
        }
        Manager = GameObject.FindGameObjectWithTag("GameController"); //マネージャーを特定
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Laser" || other.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Meteo")
            {
                GameObject Fx = Instantiate(ExplosionPrefab,
                transform.position, Random.rotation) as GameObject; //爆発エフェクトを生成
                //Z座標の1/10を得点として計上
                Manager.SendMessage("ChangeScore", Mathf.FloorToInt(transform.position.z / 10));
                Destroy(Fx, 5.0f); //エフェクトを５秒後に撤去
                Destroy(gameObject); //自身（隕石）を即撤去
            }
            else if(gameObject.tag == "Gate"&& other.gameObject.tag == "Player")
            {
                GameObject Fx = Instantiate(RainbowExplode2,
                transform.position, Random.rotation) as GameObject; //爆発エフェクトを生成
                Manager.SendMessage("ChangeScore",1000);
                Destroy(gameObject, 0.5f); //自身（隕石）を即撤去
            }
            if (gameObject.tag == "Flayer")
            {
                GameObject Fx = Instantiate(ExplosionPrefab,
                transform.position, Random.rotation) as GameObject; //爆発エフェクトを生成
                //Z座標の1/10を得点として計上
                Manager.SendMessage("ChangeScore", Mathf.FloorToInt(transform.position.z / 10));
                Destroy(Fx, 5.0f); //エフェクトを５秒後に撤去
                Destroy(gameObject); //自身（隕石）を即撤去
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= 0)
            Destroy(gameObject); //カメラの背後になれば自身（隕石）を撤去
    }
}
