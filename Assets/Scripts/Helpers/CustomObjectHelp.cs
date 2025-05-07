using TMPro;
using UnityEngine;
using UnityEngine.XR.Hands;

public class CustomObjectHelp : MonoBehaviour
{
    [SerializeField] TMP_Text lefttext;
    [SerializeField] TMP_Text righttext;
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;

    private void Update()
    {
        lefttext.text = ObjectHelper.GetLocalRotationAbsoluteDifferenceFromTransform(leftHand).ToString("0");
        righttext.text = ObjectHelper.GetLocalRotationAbsoluteDifferenceFromTransform(rightHand).ToString("0"); 
    }



}
