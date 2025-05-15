using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    private class TransitionHelper : MonoBehaviour
    {
        private List<Transition> transitions = new List<Transition>();

        private void Update()
        {
            for (int i = transitions.Count - 1; i >= 0; i--)
            {
                if (transitions[i].UpdateTransition(Time.deltaTime))
                {
                    transitions.RemoveAt(i);
                }
            }
        }

        public void StartTransition(Transition transition)
        {
            // Remove any existing transition of the same type for the same target
            transitions.RemoveAll(t => t.Target == transition.Target && t.Type == transition.Type);
            transitions.Add(transition);
        }
    }

    private static TransitionHelper helper;

    private static TransitionHelper GetHelper()
    {
        if (helper == null)
        {
            GameObject obj = new GameObject("TransitionHelper");
            helper = obj.AddComponent<TransitionHelper>();
            Object.DontDestroyOnLoad(obj);
        }
        return helper;
    }

    public static void ChangeColor(GameObject target, Color targetColor, float duration)
    {
        GetHelper().StartTransition(new ColorTransition(target, targetColor, duration));
    }

    public static void ChangeSize(GameObject target, Vector3 targetSize, float duration)
    {
        GetHelper().StartTransition(new SizeTransition(target, targetSize, duration));
    }

    public static void ChangeRotation(GameObject target, Vector3 targetRotation, float duration)
    {
        GetHelper().StartTransition(new RotationTransition(target, targetRotation, duration));
    }

    public static void ChangeLocalPosition(GameObject target, Vector3 targetPosition, float duration, Vector3? curveDirection = null, float curvedStrenght=0)
    {
        // Utiliser Vector3.zero si curveDirection n'est pas défini
        Vector3 curveDir = curveDirection ?? Vector3.zero;
        GetHelper().StartTransition(new PositionTransition(target, targetPosition, duration,true,curveDir,curvedStrenght));
    }
    public static void ChangePosition(GameObject target, Vector3 targetPosition, float duration, Vector3? curveDirection = null, float curvedStrenght=0)
    {
        // Utiliser Vector3.zero si curveDirection n'est pas défini
        Vector3 curveDir = curveDirection ?? Vector3.zero;
        GetHelper().StartTransition(new PositionTransition(target, targetPosition, duration,false, curveDir, curvedStrenght));
    }

    public static void ChangeTransform(GameObject target, Transform targetTransform, float duration)
    {
        ChangePosition(target, targetTransform.position, duration);
        ChangeRotation(target, targetTransform.rotation.eulerAngles, duration);
        ChangeSize(target, targetTransform.localScale, duration);
    }

    private abstract class Transition
    {
        public GameObject Target { get; private set; }
        public string Type { get; private set; }
        protected float Duration { get; private set; }
        protected float ElapsedTime { get; private set; }

        public Transition(GameObject target, string type, float duration)
        {
            Target = target;
            Type = type;
            Duration = duration;
            ElapsedTime = 0f;
        }

        public bool UpdateTransition(float deltaTime)
        {
            ElapsedTime += deltaTime;
            float t = Mathf.Clamp01(ElapsedTime / Duration);
            ApplyTransition(t);
            return ElapsedTime >= Duration;
        }

        protected abstract void ApplyTransition(float t);
    }

    private class ColorTransition : Transition
    {
        private Color startColor;
        private Color targetColor;
        private SpriteRenderer spriteRenderer;

        public ColorTransition(GameObject target, Color targetColor, float duration) : base(target, "Color", duration)
        {
            spriteRenderer = target.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                startColor = spriteRenderer.color;
                this.targetColor = targetColor;
            }
        }

        protected override void ApplyTransition(float t)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.Lerp(startColor, targetColor, t);
            }
        }
    }

    private class SizeTransition : Transition
    {
        private Vector3 startScale;
        private Vector3 targetScale;
        private Transform targetTransform;

        public SizeTransition(GameObject target, Vector3 targetSize, float duration) : base(target, "Size", duration)
        {
            targetTransform = target.transform;
            startScale = targetTransform.localScale;
            targetScale = targetSize;
        }

        protected override void ApplyTransition(float t)
        {
            if (targetTransform != null)
            {
                targetTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            }
        }
    }

    private class RotationTransition : Transition
    {
        private Vector3 startRotation;
        private Vector3 targetRotation;
        private Transform targetTransform;

        public RotationTransition(GameObject target, Vector3 targetRotation, float duration) : base(target, "Rotation", duration)
        {
            targetTransform = target.transform;
            startRotation = targetTransform.localEulerAngles;
            this.targetRotation = targetRotation;
        }

        protected override void ApplyTransition(float t)
        {
            if (targetTransform != null)
            {
                targetTransform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, t);
            }
        }
    }

    private class PositionTransition : Transition
    {
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private Transform targetTransform;
        private bool local;
        private Vector3 controlPoint; // Point de contrôle pour la courbe

        public PositionTransition(GameObject target, Vector3 targetPosition, float duration, bool local, Vector3 curveDirection, float curveStrength)
            : base(target, "Position", duration)
        {
            this.local = local;
            targetTransform = target.transform;
            startPosition = local ? targetTransform.localPosition : targetTransform.position;
            this.targetPosition = targetPosition;

            

            // Définition du point de contrôle (milieu du chemin + direction de courbure)
            controlPoint = (startPosition + targetPosition) / 2 + curveDirection.normalized * curveStrength;
        }

        protected override void ApplyTransition(float t)
        {
            if (targetTransform != null)
            {
                // Interpolation quadratique de Bézier
                Vector3 curvedPosition =
                    Mathf.Pow(1 - t, 2) * startPosition +
                    2 * (1 - t) * t * controlPoint +
                    Mathf.Pow(t, 2) * targetPosition;

                if (local)
                    targetTransform.localPosition = curvedPosition;
                else
                    targetTransform.position = curvedPosition;
            }
        }
    }

}
