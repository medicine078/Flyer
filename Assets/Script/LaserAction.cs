using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z >= 1000)
            Destroy(gameObject); //1000の彼方を越えたら撤去
    }
}
