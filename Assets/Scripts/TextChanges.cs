using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
namespace TSI2Unity{
public class TextChanges : MonoBehaviour
{
        public GameObject TSImanager;
        private TSINetworkInterface tSINetworkInterface;
        public TMP_Text jim;
        public int baker = 0;
        public int carpenter = 0;
        public int potter = 0;
        public int response_counter = 0;
        public string clickedButton = "";

    public void clickresponse(){
        response_counter += 1;
        Debug.Log(response_counter);
    }
    // Start is called before the first frame update
    void Start()
    {
        tSINetworkInterface = TSImanager.GetComponent<TSINetworkInterface>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
}