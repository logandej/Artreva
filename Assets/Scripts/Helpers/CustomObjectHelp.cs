
using TMPro;
using UnityEngine;
using UnityEngine.XR.Hands;

public class CustomObjectHelp : MonoBehaviour
{


    public void ChangeColor(string col)
    {
        ObjectHelper.ChangeColor(this.gameObject, StringToColor(col));
    }

    public Color StringToColor(string name)
    {
        return name.ToLower() switch
        {
            "red" => Color.red,
            "green" => Color.green,
            "blue" => Color.blue,
            "black" => Color.black,
            "white" => Color.white,
            "yellow" => Color.yellow,
            "cyan" => Color.cyan,
            "magenta" => Color.magenta,
            "gray" => Color.gray,
            _ => Color.clear,
        };
    }



}
