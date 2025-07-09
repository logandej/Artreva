using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ScenarioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static ScenarioManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public List<PlayableDirector> playableDirectors = new();
    private int directorIndex = 0;
    public bool WaitingForPlayer { get; private set; } = false;
    public void PauseTimeline()
    {
        if (!WaitingForPlayer)
        {
            Debug.Log("[ScenarioManager] Timeline paused.");
            // playableDirectors[directorIndex].timeUpdateMode = DirectorUpdateMode.Manual;
            playableDirectors[directorIndex].Pause();
            WaitingForPlayer = true;
            // Tu peux ici déclencher un événement, afficher un prompt, etc.
            LockSubtitles();
        }
    }

    private void LockSubtitles()
    {
        MyCustomSubtitle.eventLock?.Invoke();
    }

    private void UnlockSubtitles()
    {
        MyCustomSubtitle.eventUnlock?.Invoke();
    }

    public void ResumeTimelineIn(float time)
    {
        Invoke(nameof(ResumeTimeline), time);
    }

    public void ResumeTimeline()
    {
        if (WaitingForPlayer)
        {
            Debug.Log("[ScenarioManager] Timeline resumed.");
            WaitingForPlayer = false;
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

    public void PlayDirector(PlayableDirector playableDirector)
    {
        if (!playableDirectors.Contains(playableDirector)){
            Debug.LogWarning("The playable " + playableDirector.name + "is not in the Scenario Manager Sequence");
            return;
        }
        playableDirector.Play();
        directorIndex=playableDirectors.IndexOf(playableDirector);
        GetComponent<EventSequence>().SetGroupIndex(directorIndex);
    }

    public void NextTimeline()
    {
        UIManager.Instance.HideSettings();
        // Bien figer l'état de la précédente Timeline (optionnel)
        playableDirectors[directorIndex].Pause();
        playableDirectors[directorIndex].timeUpdateMode = DirectorUpdateMode.GameTime;

        directorIndex++;

        // Préparer la nouvelle Timeline
        playableDirectors[directorIndex].timeUpdateMode = DirectorUpdateMode.GameTime;
        PlayDirector(playableDirectors[directorIndex]);
    }

    public void ChangePlayerPosition(Transform transform)
    {
        StartCoroutine(ChangePlayerPositionCoroutine(transform));
    }
    IEnumerator ChangePlayerPositionCoroutine(Transform transform)
    {
        UIManager.Instance.HideSettings();

        SceneFader.Instance.LoadWhiteFade();
        yield return new WaitForSeconds(SceneFader.Instance.FadeTime);
        GameManager.Instance.Player.transform.position = transform.position;
        GameManager.Instance.Player.transform.rotation = transform.rotation;
        yield return new WaitForSeconds(1);
        SceneFader.Instance.UnloadFade();
    }

    public void Transition(GameObject obj, Transform transform, float duration)
    {
        TransitionManager.ChangeTransform(obj, transform, duration);
    }

    public void ShowInfo(Sprite sprite)
    {
        SceneFader.Instance.LoadBlackFade(.8f);
        UIManager.Instance.ShowInfo(sprite);
        SceneFader.Instance.UnloadFadeIn(5);
        //ResumeTimelineIn(10);
    }

    public void SetTextShowInfo(string key)
    {
        UIManager.Instance.SetTextByKey(key);
    }
}
