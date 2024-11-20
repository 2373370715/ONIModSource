using System;
using UnityEngine;
using UnityEngine.UI;

namespace RsTransferPort
{
    public class LineArrow : MonoBehaviour
    {
        public Graphic graphic;
        
        [SerializeField]
        private Vector3 end;

        [SerializeField]
        private Vector3 start;
        

        public void SetTwoPoint(Vector3 start, Vector3 end)
        {
            if (this.start == start && this.end == end) return;
            this.start = start;
            this.end = end;
            if (start == end) return;
            UpdateChange();
        }

        public void SetColor(Color color)
        {
            if (graphic != null && graphic.color != color) graphic.color = color;
        }

        private void UpdateChange()
        {
            var rectTransform = (RectTransform) transform;
            var parent = rectTransform.parent;
            float distance;
            if (parent != null)
            {
                distance = Vector2.Distance(parent.InverseTransformVector(start), parent.InverseTransformVector(end));
            }
            else
            {
                distance = Vector2.Distance((start), (end));
            }
            rectTransform.position = start;
            rectTransform.right = end - rectTransform.position;
            rectTransform.sizeDelta = new Vector2(distance, 0.2f);
        }

        private void OnValidate()
        {
            UpdateChange();
        }
    }
}