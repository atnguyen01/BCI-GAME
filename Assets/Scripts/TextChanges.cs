using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using Unity.IO;
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
        //public string Path;
        public string[][] csvValues;
        public string[] labelRow; // 1st row with commands
        string[] strsplit; //for point changes
    // Start is called before the first frame update

    public void LoadFile(string path){
    var data = File.ReadAllLines(path).Select(l => l.Split(';')).ToArray().ToArray();
    csvValues = data;
    /*
    for(int i=0; i <csvValues.GetLength(0); i++){
        labelRow[i] = csvValues[i][0];
    }*/
    }
    void Start()
    {
        isFocused = true;
        //var resources = Resources.Load("story_script");
        //Path = Application.dataPath + @"\Resources\story_script.csv";
        var basePath = Path.Combine(Application.streamingAssetsPath, "story_script.csv");
        csvValues = File.ReadAllLines(basePath).Select(l => l.Split(';')).ToArray().ToArray();
        //Debug.Log(Path);
        tSINetworkInterface = TSImanager.GetComponent<TSINetworkInterface>();
        var allInteractables = GameObject.FindObjectsOfType<Interactable>();
            foreach (var i in allInteractables)
            {
           // i.OnClick.AddListener(() => Debug.Log(Time.time + ": " + i.gameObject.name + " was clicked"));
            //i.OnClick.AddListener(() =>button_name = i.gameObject.name);
            i.OnClick.AddListener(() => getButton(i));
            }
            //LoadFile(Path);
            Debug.Log(csvValues.GetLength(0)); //getting number of rows
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


//not done need to figure out point change for two characters at once
//lines 43-45 need to figure out (prob case for state choice) 
public void playTurn(){ 
    switch(labelRow[response_counter]){
        case "start_story":
            break;
        case "dialogue":
            jim.text = csvValues[response_counter][1];
                buttonlabel1.text = "";
                buttonlabel2.text = "Continue";
                buttonlabel3.text = "";
            if(labelRow[response_counter+1].Equals("choice_2")){
                buttonlabel1.text = csvValues[response_counter+1][1];
                buttonlabel2.text = "";
                buttonlabel3.text = csvValues[response_counter+1][2];
               // response_counter += 1; // skip choice in csv to show at the same time
            }
            if(labelRow[response_counter+1].Equals("choice_3")){
                buttonlabel1.text = csvValues[response_counter+1][1];
                buttonlabel2.text = csvValues[response_counter+1][2];
                buttonlabel3.text = csvValues[response_counter+1][3];
               // response_counter += 1; // skip choice in csv to show at the same time
            }
            if(labelRow[response_counter+1].Equals("state_choice_3") && isFocused){
                buttonlabel1.text = csvValues[response_counter+1][1];
                buttonlabel2.text = csvValues[response_counter+1][2];
                buttonlabel3.text = csvValues[response_counter+1][3];
            } else if(labelRow[response_counter+1].Equals("state_choice_3") && !isFocused){
                buttonlabel1.text = csvValues[response_counter+1][4];
                buttonlabel2.text = csvValues[response_counter+1][5];
                buttonlabel3.text = csvValues[response_counter+1][6];               
            }

            if(labelRow[response_counter+1].Equals("comment") && labelRow[response_counter+2].Equals("state") && labelRow[response_counter+3].Equals("comment")){
            response_counter += 3; //skip comment state and coment
            }else if(labelRow[response_counter+1].Equals("comment") && labelRow[response_counter+2].Equals("state")){
            response_counter += 2; //skip comment and state
            }else if(labelRow[response_counter+1].Equals("comment")){
            response_counter += 1; //skip comment
            }
            break;
        case "state_dialogue":
            if(isFocused){
                jim.text = csvValues[response_counter][1];
            }else{
                jim.text = csvValues[response_counter][2];
            }

            if(isFocused && labelRow[response_counter+1].Equals("comment") && labelRow[response_counter+2].Equals("state_choice_3") && labelRow[response_counter+3].Equals("point_change")){
            buttonlabel1.text = csvValues[response_counter+2][1];
            buttonlabel2.text = csvValues[response_counter+2][2];
            buttonlabel3.text = csvValues[response_counter+2][3];
            response_counter += 3; //skip comment and point change, ste choices at same time so skip
            }

            if(!isFocused && labelRow[response_counter+1].Equals("comment") && labelRow[response_counter+2].Equals("state_choice_3") && labelRow[response_counter+3].Equals("point_change")){
            buttonlabel1.text = csvValues[response_counter+2][4];
            buttonlabel2.text = csvValues[response_counter+2][5];
            buttonlabel3.text = csvValues[response_counter+2][6];
            response_counter += 3; //skip comment and point change, ste choices at same time so skip
            }
            break;
        case "path_2":
                switch(button_num){
                    case 1:
                    jim.text = csvValues[response_counter][1];
                    break;
                    case 2:
                    response_counter -=1; //so clicking will do nothing here
                    break;
                    case 3: 
                    jim.text = csvValues[response_counter][2];
                    break;
                }

                buttonlabel1.text = "";
                buttonlabel2.text = "Continue";
                buttonlabel3.text = "";
            break;
        case "state_path_3":
            switch(button_num){
                case 1:
                if(isFocused){
                   strsplit = csvValues[response_counter-1][1].Split(' ');
                   jim.text = csvValues[response_counter][1];
                } else {
                  strsplit = csvValues[response_counter-1][4].Split(' ');
                  jim.text = csvValues[response_counter][4];  
                }
                break;
                case 2:
                if(isFocused){
                   strsplit = csvValues[response_counter-1][2].Split(' ');
                   jim.text = csvValues[response_counter][2];
                } else {
                  strsplit = csvValues[response_counter-1][5].Split(' ');
                  jim.text = csvValues[response_counter][5];  
                }
                break;
                case 3:
                if(isFocused){
                   strsplit = csvValues[response_counter-1][3].Split(' ');
                   jim.text = csvValues[response_counter][3];
                } else {
                  strsplit = csvValues[response_counter-1][6].Split(' ');  
                  jim.text = csvValues[response_counter][6];
                } 
                break;
            }
                if(strsplit[0].Equals("Carpenter")){
                    carpenter += Int32.Parse(strsplit[1]);
                } else if(strsplit[0].Equals("Baker")){
                    baker += Int32.Parse(strsplit[1]);
                } else if(strsplit[0].Equals("Potter")){
                    potter += Int32.Parse(strsplit[1]);
                }

                if(labelRow[response_counter+1].Equals("comment")){
                response_counter += 1; //skip comment
                }
            
                //Debug.Log(carpenter);
                //Debug.Log(baker);
                //Debug.Log(potter);

            break;    
            case "wait":
                    jim.text = csvValues[response_counter][1];
                    buttonlabel1.text = csvValues[response_counter][2];
                    buttonlabel2.text = "";
                    buttonlabel3.text = csvValues[response_counter][3];

                    if(labelRow[response_counter+1].Equals("comment") && labelRow[response_counter+2].Equals("state")){
                        response_counter += 2;
                    }

                    if(button_num == 2 || button_num == 3){
                        response_counter -= 3; //stay where you are 
                    }
            break;        
    }
    response_counter +=1;
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