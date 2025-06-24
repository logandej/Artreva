using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;

public class SceneInfo : MonoBehaviour
{
    public XROrigin Player;
    public Transform cameraSubtitleTransform;
    public LightSwitcher lightSwitcher;
    public SensePackMR sensePackMR;

    public List<SceneDatas> sceneDatas;

    [Serializable]
    public class SceneDatas 
    {
        public GameManager.GameStates gameState;
        public List<GameObject> objectsToShow;
        public UnityEvent startevents = new(); 

        public void Begin()
        {
            objectsToShow.ForEach(c => c.SetActive(true));
            startevents.Invoke();
        }
    }

    void Start()
    {
        GameManager.Instance.RegisterSceneReferences(this);
        CheckSceneDatas();
    }

    private void CheckSceneDatas()
    {
        foreach(SceneDatas sceneDatas in sceneDatas)
        {
            if(GameManager.Instance.GameStatus == sceneDatas.gameState)
            {
                print("GameStatus Scene Info : "+GameManager.Instance.GameStatus);

                sceneDatas.Begin();
            }
        }
    }
}
