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
        public TMP_Text buttonlabel1;
        public TMP_Text buttonlabel2;
        public TMP_Text buttonlabel3;
        public int baker = 0;
        public int carpenter = 0;
        public int potter = 0;
        public int response_counter = 0; //-2 to deal with the double click bug at beginning
        public string clickedButton = "";
        public int button_num = 0;
        public string button_name = null;
        public bool isFocused = true;
        public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        tSINetworkInterface = TSImanager.GetComponent<TSINetworkInterface>();
                var allInteractables = GameObject.FindObjectsOfType<Interactable>();
            foreach (var i in allInteractables)
            {
           // i.OnClick.AddListener(() => Debug.Log(Time.time + ": " + i.gameObject.name + " was clicked"));
            //i.OnClick.AddListener(() =>button_name = i.gameObject.name);
            i.OnClick.AddListener(() => UpdateField(i));
            }
    }

    public void UpdateField(Microsoft.MixedReality.Toolkit.UI.Interactable index)
    {
        //button_name = index.gameObject.name;
        //set button number for clickresponse to use as switch case 
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
 /*
        var allInteractables = GameObject.FindObjectsOfType<Interactable>();
            foreach (var i in allInteractables)
            {
           // i.OnClick.AddListener(() => Debug.Log(Time.time + ": " + i.gameObject.name + " was clicked"));
            //i.OnClick.AddListener(() =>button_name = i.gameObject.name);
            i.OnClick.AddListener(() => UpdateField(i));
            i.OnClick.AddListener(() => playTurn(response_counter));
            //i.OnClick.AddListener(() => response_counter += 1);
            }
            */
            /*
        Debug.Log(button_name);
        response_counter += 1;
        Debug.Log(response_counter);
        Debug.Log("Button num: " + button_num);
        playTurn(response_counter, button_num);
        */
    }

    public void setButtonText(int turn_num){
        Debug.Log("button: "+ button_num);
        switch(turn_num){
            case 1:
            buttonlabel1.text = "Did Andrew do it?";
            buttonlabel2.text = "Who's James?";
            buttonlabel3.text = "Is Jagger sus?!?!?!?";
            break;
            case 2:
            buttonlabel1.text = "Cool";
            buttonlabel2.text = "Nice!";
            buttonlabel3.text = "Called it";
            break;
            case 3:
            buttonlabel1.text = "wha";
            buttonlabel2.text = "cool!";
            buttonlabel3.text = "knew it was coming";
            break;
            case 4:
            buttonlabel1.text = "Cool";
            buttonlabel2.text = "Nice!";
            buttonlabel3.text = "Called it";
            break;
            case 5:
            buttonlabel1.text = "wha";
            buttonlabel2.text = "cool!";
            buttonlabel3.text = "knew it was coming";
            break;
        }
    }

    public void playTurn(){
       switch(response_counter){
          case 1:
            switch(button_num){
                case 1:
                if(isFocused){
                jim.text = "FNAF is the best movie ever";
                setButtonText(response_counter);
                }else {
                jim.text = "FNAF is the worst movie ever";
                setButtonText(response_counter);
                }
                break;
                case 2:
                jim.text = "Skyrim is the best game ever";
                setButtonText(response_counter);
                break;    
                case 3:
                jim.text = "Jagger is always correct";
                setButtonText(response_counter);
                break;
               }
           break;
            case 2:
                switch(button_num){
                    case 1:
                    jim.text = "Andrew is innocent";
                    setButtonText(response_counter);
                    break;
                    case 2:
                    jim.text = "james is a baker. He bakes many good things like breads,cakes, and pastries. I especially like bagels with cream cheese it's just sooooooooooooooooo good";
                    setButtonText(response_counter);
                    break;    
                    case 3:
                    jim.text = "Jagger did it";
                    setButtonText(response_counter);
                    break;
                    }
           break;
            case 3:
                switch(button_num){
                    case 1:
                    jim.text = "Andrew is a wizard";
                    setButtonText(response_counter);
                    break;
                    case 2:
                    jim.text = "james is a baker.";
                    setButtonText(response_counter);
                    break;    
                    case 3:
                    jim.text = "Jagger sus";
                    setButtonText(response_counter);
                    break;
                    }
           break;
            case 4:
                switch(button_num){
                    case 1:
                    jim.text = "Andrew is a potter";
                    setButtonText(response_counter);
                    break;
                    case 2:
                    jim.text = "james is just here.";
                    setButtonText(response_counter);
                    break;    
                    case 3:
                    jim.text = "Jagger is a gamer";
                    setButtonText(response_counter);
                    break;
                    }
           break;
            case 5:
                switch(button_num){
                    case 1:
                    jim.text = "Andrew is cool";
                    setButtonText(response_counter);
                    break;
                    case 2:
                    jim.text = "james is good with bread knives.";
                    setButtonText(response_counter);
                    break;    
                    case 3:
                    jim.text = "Jagger hates all of us";
                    setButtonText(response_counter);
                    break;
                    }
           break;
}
response_counter += 1;
Debug.Log(response_counter);
}
}
}