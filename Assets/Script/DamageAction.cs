using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAction : MonoBehaviour
{
    public GameObject DamagedShip; //ダメージ効果
    GameObject Manager; //マネージャー

    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.FindGameObjectWithTag("GameController"); //マネージャーを特定
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Meteo")
        {
            //マネージャーに隕石衝突を通達
            Manager.SendMessage("OnHitMeteo", SendMessageOptions.DontRequireReceiver);
            //ダメージ効果を設置
            GameObject Fx = Instantiate(DamagedShip,
            transform.position, Quaternion.identity) as GameObject;
            Destroy(Fx, 5.0f); //ダメージ効果を５秒後に撤去
        }
        if (other.gameObject.tag == "Flayer")
        {
            //マネージャーに隕石衝突を通達
            Manager.SendMessage("OnHitMeteo", SendMessageOptions.DontRequireReceiver);
            //ダメージ効果を設置
            GameObject Fx = Instantiate(DamagedShip,
            transform.position, Quaternion.identity) as GameObject;
            Destroy(Fx, 5.0f); //ダメージ効果を５秒後に撤去
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
