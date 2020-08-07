using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroText : MonoBehaviour
{
    string[] text = {
        "11:13PM\n11/07/1979",
        "A COUNTRY ROAD NEAR EDINBURGH\nSCOTLAND",
        "WHAT? COME ON!",
        "UCH",
        "START, YA WEE S*%T",
        "RIGHT",

    };

    private void Start()
    {
        StartCoroutine(PlayIntro());
    }

    public Text textBox;
    IEnumerator PlayIntro()
    {
        yield return new WaitForSeconds(3f);
        textBox.text = "11:13PM\n11/07/1979";
        yield return new WaitForSeconds(5f);
        textBox.text = "A COUNTRY ROAD NEAR EDINBURGH, SCOTLAND";
        yield return new WaitForSeconds(5f);
        textBox.text = "";
        yield return new WaitForSeconds(5f);
        textBox.text = "\"WHAT? COME ON!\"";
        yield return new WaitForSeconds(2f);
        // 20 seconds
        textBox.text = "\"UGH\"";
        yield return new WaitForSeconds(3f);
        //23 seconds
        textBox.text = "";
        yield return new WaitForSeconds(3f);
        // 26 seconds
        textBox.text = "\"START YA WEE TADGER\"";
        yield return new WaitForSeconds(4f);
        // 30 seconds
        textBox.text = "*SIGH*";
        yield return new WaitForSeconds(2f);
        textBox.text = "\"I MUST BE NEAR THE FORESTRY\"";
        yield return new WaitForSeconds(3f);
        textBox.text = "\"THEY LEAVE STUFF LYING AROUND ALL THE TIME\"";
        yield return new WaitForSeconds(5f);
        textBox.text = "\"MAYBE I CAN FIND SOME SPARE PARTS IN THE WOODS\"";
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
