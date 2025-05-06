using UnityEngine;
using UnityEngine.XR.Hands;

public class CustomObjectHelp : MonoBehaviour
{
    public void Active()
    {
        ObjectHelper.ChangeColor(this.gameObject, Color.green);
    }

    public void Deactive()
    {
        ObjectHelper.ChangeColor(this.gameObject, Color.red);
    }



}
