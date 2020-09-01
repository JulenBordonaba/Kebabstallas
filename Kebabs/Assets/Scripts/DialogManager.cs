using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public Image DialogBox;
    public TextMeshProUGUI textDisplay; //Texto de dialogos
    public bool endConversation; //fin de conversación
    public bool hasEnded = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Función para mostrar los dialogos por pantalla
    public IEnumerator Type(string myEvent)
    {
        hasEnded = false;
        DialogBox.enabled = true;
        textDisplay.text = "";
        endConversation = false;
        foreach (char letter in myEvent.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
        hasEnded = true;
        yield return new WaitUntil(() => endConversation == true);
        //Cuando acaba de hablar:
        textDisplay.text = "";
        DialogBox.enabled = false;

        
    }
}
