using UnityEngine;
using UnityEngine.UI;

namespace FStudio.UI.Navigation {
    public static class NavigationScrollSnapper {
        /// <summary>
        /// Calculate a scroll rect position by snapTarget.
        /// Snap target should be a child of content area of scroll rect.
        /// Returns anchored position of content. But does not affect.
        /// Its just a calculator.
        /// </summary>
        /// <param name="scrollRect"></param>
        /// <param name="snapTarget"></param>
        /// <param name="isHorizontal"></param>
        /// <returns></returns>
        public static Vector2 CalculateSnapPosition(
            ScrollRect scrollRect, 
            Vector3 snapTargetWorldPosition, 
            bool isHorizontal) {

            var targetAnchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(scrollRect.content.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(snapTargetWorldPosition);

            if (isHorizontal)
                targetAnchoredPosition.y = scrollRect.content.anchoredPosition.y;
            else targetAnchoredPosition.x = scrollRect.content.anchoredPosition.x;

            return targetAnchoredPosition;
        }
    }
}