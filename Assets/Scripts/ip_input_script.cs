using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class ip_input_script : MonoBehaviour
{
    public TMP_Text local_ip_address;
    // theoretically, having a static var means it will not change and will always be accessable unless the entire game is rerun
    public static TMP_Text global_ip_address;

    // connect button pressed
    public async void Connect(){
        global_ip_address.text = local_ip_address.text;
        SceneManager.LoadScene("TS-Unity");
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
