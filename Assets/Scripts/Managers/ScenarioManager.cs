using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ScenarioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("FirstEnigma")]
    [SerializeField] List<OrbEnigma> orbEnigma;

    public List<PlayableDirector> playableDirectors = new();
    private int directorIndex = 0;

    private bool waitingForPlayer = false;

    void Update()
    {
        //if (waitingForPlayer)
        //{
        //    playableDirectors[directorIndex].Evaluate(); // maintient la pose
        //}
    }

    public void PauseTimeline()
    {
        Debug.Log("[ScenarioManager] Timeline paused.");
       // playableDirectors[directorIndex].timeUpdateMode = DirectorUpdateMode.Manual;
        playableDirectors[directorIndex].Pause();
        waitingForPlayer = true;
        // Tu peux ici déclencher un événement, afficher un prompt, etc.
        LockSubtitles();
    }

    private void LockSubtitles()
    {
        MyCustomSubtitle.eventLock?.Invoke();
    }

    private void UnlockSubtitles()
    {
        MyCustomSubtitle.eventUnlock?.Invoke();
    }

    public void ResumeTimeline()
    {
        if (waitingForPlayer)
        {
            Debug.Log("[ScenarioManager] Timeline resumed.");
            waitingForPlayer = false;
           // playableDirectors[directorIndex].timeUpdateMode = DirectorUpdateMode.GameTime;
            playableDirectors[directorIndex].Play(); // ou Resume() selon ce que tu veux
            UnlockSubtitles();
        }
    }

    // Cette méthode peut être appelée par un trigger ou une interaction
    public void OnPlayerDidAction()
    {
        Debug.Log("[ScenarioManager] Player did expected action.");
        ResumeTimeline();
    }

    // Méthode appelée par un autre signal dans la Timeline
    public void TriggerSomething()
    {
        Debug.Log("[ScenarioManager] Triggered something.");
        // Fais ce que tu veux ici (effets, dialogues, autre Timeline…)
    }

    public void NextTimeline()
    {
        // Bien figer l'état de la précédente Timeline (optionnel)
        playableDirectors[directorIndex].Pause();
        playableDirectors[directorIndex].timeUpdateMode = DirectorUpdateMode.GameTime;

        directorIndex++;

        // Préparer la nouvelle Timeline
        playableDirectors[directorIndex].timeUpdateMode = DirectorUpdateMode.GameTime;
        playableDirectors[directorIndex].Play();
    }

    public void ChangePlayerPosition(Transform transform)
    {
        StartCoroutine(ChangePlayerPositionCoroutine(transform));
    }
    IEnumerator ChangePlayerPositionCoroutine(Transform transform)
    {
        SceneFader.Instance.LoadWhiteFade();
        yield return new WaitForSeconds(SceneFader.Instance.FadeTime);
        GameManager.Instance.Player.transform.position = transform.position;
        yield return new WaitForSeconds(1);
        SceneFader.Instance.UnloadFade();
    }

    public void Transition(GameObject obj, Transform transform, float duration)
    {
        TransitionManager.ChangeTransform(obj, transform, duration);
    }

    
}
