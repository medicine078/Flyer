using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsAction : MonoBehaviour
{

    public Renderer[] myRenderers = new Renderer[3];
    Transform Cam;
    float NowAlpha;
    float TargetAlpha;


    // Start is called before the first frame update
    void Start()
    {
        Cam = GameObject.Find("CenterEyeAnchor").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 nowCam = Vector3.forward;
        Vector3 flatCam = Vector3.ProjectOnPlane(Cam.forward, Vector3.up).normalized;
        float angleDelta = Vector3.Angle(nowCam, flatCam);
        //カメラが正面から60度以上開いたら、徐々にアルファ値が上がる。またその逆も。
        TargetAlpha = (angleDelta > 60.0f) ? 1.0f : 0.0f;
        NowAlpha = Mathf.MoveTowards(NowAlpha, TargetAlpha, 2.0f * Time.deltaTime);
        for (int idx = 0; idx < myRenderers.Length; idx++)
        {
            myRenderers[idx].material.SetFloat("_Alpha", NowAlpha);
        }
    }
}
