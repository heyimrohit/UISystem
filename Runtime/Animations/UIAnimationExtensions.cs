using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.Animations
{
    public static class UIAnimationExtensions
    {
        public static UITweenAnimation FadeIn(this VisualElement element, float duration = 0.3f)
        {
            element.style.opacity = 0f;
            return new UITweenAnimation(element, duration, AnimationCurve.EaseInOut(0, 0, 1, 1),
                progress => element.style.opacity = progress);
        }

        public static UITweenAnimation FadeOut(this VisualElement element, float duration = 0.3f)
        {
            element.style.opacity = 1f;
            return new UITweenAnimation(element, duration, AnimationCurve.EaseInOut(0, 1, 1, 0),
                progress => element.style.opacity = 1f - progress);
        }

        public static UITweenAnimation SlideIn(this VisualElement element, Vector2 from, Vector2 to, float duration = 0.5f)
        {
            //return new UITweenAnimation(element, duration, AnimationCurve.EaseOutBack(0, 0, 1, 1),
            //    progress =>
            //    {
            //        var current = Vector2.Lerp(from, to, progress);
            //        element.style.left = current.x;
            //        element.style.top = current.y;
            //    });
            return null;
        }

        public static UITweenAnimation Scale(this VisualElement element, float fromScale, float toScale, float duration = 0.3f)
        {
            //return new UITweenAnimation(element, duration, AnimationCurve.EaseOutBounce(0, fromScale, 1, toScale),
            //    progress =>
            //    {
            //        var scale = Mathf.Lerp(fromScale, toScale, progress);
            //        element.transform.scale = new Vector3(scale, scale, 1f);
            //    });
            return null;
        }
    }
}