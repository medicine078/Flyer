using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    GameObject Manager; //マネージャー

    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.FindGameObjectWithTag("GameController"); //マネージャーを特定
    }

    void OnTriggerEnter(Collider other)
    { //マネージャーにレーザー命中を通報
        if (other.gameObject.tag == "Laser")
        {
            Manager.SendMessage("HitButton", SendMessageOptions.DontRequireReceiver);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
