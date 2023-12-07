using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
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
        public int response_counter = 0; 
        public int button_num = 0;
        public string button_name = null;
        public bool isFocused = true;
        public string Path;
        public string[][] csvValues;
        public string[] labelRow; // 1st row with commands
    // Start is called before the first frame update

    public void LoadFile(string path){
    var data = File.ReadAllLines(path).Select(l => l.Split(',')).ToArray().ToArray();
    csvValues = data;
    /*
    for(int i=0; i <csvValues.GetLength(0); i++){
        labelRow[i] = csvValues[i][0];
    }*/
    }
    void Start()
    {
        Path = Application.dataPath + @"\story_script.csv";
        //Debug.Log(Path);
        tSINetworkInterface = TSImanager.GetComponent<TSINetworkInterface>();
        var allInteractables = GameObject.FindObjectsOfType<Interactable>();
            foreach (var i in allInteractables)
            {
           // i.OnClick.AddListener(() => Debug.Log(Time.time + ": " + i.gameObject.name + " was clicked"));
            //i.OnClick.AddListener(() =>button_name = i.gameObject.name);
            i.OnClick.AddListener(() => getButton(i));
            }
            LoadFile(Path);
            //Debug.Log(csvValues.GetLength(0)); //getting number of rows
            for(int i=0; i <csvValues.GetLength(0); i++){
            //Debug.Log(csvValues[i][0]);
            labelRow[i] = csvValues[i][0];
            //Debug.Log(labelRow[i]);
            }   //shows row 1 for debugging and sanity purposes
    }

    public void getButton(Microsoft.MixedReality.Toolkit.UI.Interactable index)
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
    switch(labelRow[response_counter]){
        case "start_story":
            break;
        case "dialogue":
            break;
        case "":
            break;            
    }
}

//old play turn
/*

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
} */
}
}