using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
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
        public int button_num = 0;
        public string button_name = null;
        public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        tSINetworkInterface = TSImanager.GetComponent<TSINetworkInterface>();
    }

    public void UpdateField(Microsoft.MixedReality.Toolkit.UI.Interactable index)
    {
        //button_name = index.gameObject.name;
           if(index.gameObject.name.Equals("Pressable Button (1)")){
                button_num = 1;
            } 
            if(index.gameObject.name.Equals("Pressable Button (2)")){
                button_num = 2;
            } 
            if(index.gameObject.name.Equals("Pressable Button (3)")){
                button_num = 3;
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickresponse(){
 //       button_name = GetComponent<Interactable>().name;
        var allInteractables = GameObject.FindObjectsOfType<Interactable>();
            foreach (var i in allInteractables)
            {
           // i.OnClick.AddListener(() => Debug.Log(Time.time + ": " + i.gameObject.name + " was clicked"));
            //i.OnClick.AddListener(() =>button_name = i.gameObject.name);
            i.OnClick.AddListener(() => UpdateField(i));
            //set button number for clickresponse to use as switch case 
            /*
            if(i.gameObject.name.Equals("Pressable Button (1)")){
                button_num = 1;
            } 
            if(i.gameObject.name.Equals("Pressable Button (2)")){
                button_num = 2;
            } 
            if(i.gameObject.name.Equals("Pressable Button (3)")){
                button_num = 3;
            }
            */
            }
        Debug.Log(button_name);
        response_counter += 1;
        Debug.Log(response_counter);
        Debug.Log("Button num: " + button_num);
        playTurn(response_counter, button_num);
    }

    public void playTurn(int turn_num, int button_number){
       // switch(turn_num){
          //  case 1:
            switch(button_num){
                case 1:
                jim.text = "FNAF is the best movie ever";
                break;
                case 2:
                jim.text = "Skyrim is the best game ever";
                break;    
                case 3:
                jim.text = "Jagger is always correct";
                break;
          //  }
           // break;
     //   }
    }
}
}
}