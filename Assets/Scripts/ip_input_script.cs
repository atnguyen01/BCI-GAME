using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;

public class ip_input_script : MonoBehaviour
{
    public TMP_Text local_ip_address;
    // theoretically, having a static var means it will not change and will always be accessable unless the entire game is rerun
    public static string global_ip_address;

    // connect button pressed
    /*public void Connect(){
        //Debug.Log(local_ip_address.text);

        global_ip_address = local_ip_address.text;
        Debug.Log(global_ip_address);
        Thread.Sleep(10000);
        SceneManager.LoadScene("TS-Unity");
    }
*/

public void Connect(){
    StartCoroutine(CoRoutine2());
    // StartCoroutine(order());
}

public IEnumerator CoRoutine2(){
    yield return CoRoutine1();
    SceneManager.LoadScene("TS-Unity");
}

public IEnumerator CoRoutine1(){
    global_ip_address = local_ip_address.text;
    yield return null;
}

public IEnumerator order() {
    global_ip_address = local_ip_address.text;
    yield return new WaitForSecondsRealtime(1);
    SceneManager.LoadScene("TS-Unity");
}

public string getIP(){
    return global_ip_address;
}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
