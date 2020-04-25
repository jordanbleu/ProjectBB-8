using System.Linq;
using System.Text;
using Assets.Source.TextWriterStyle.Base;
using Assets.Source.TextWriterStyle.Exception;
using TMPro;
using UnityEngine;

namespace Assets.Source.TextWriterStyle
{
    public class SetPositionStyle : TextWriterStyleBase
    {
        private enum Positions
        {
            Left,
            Center,
            Right
        }

        private Positions position = Positions.Center;

        protected override void Initialize()
        {
            if (ContainsArgument("right"))
            {
                position = Positions.Right;
            }
            else if (ContainsArgument("left"))
            {
                position = Positions.Left;
            }
            else if (ContainsArgument("center"))
            {
                position = Positions.Center;
            }
            else if(GetArgumentKeys().Any())
            {
                throw new StyleValidationException("setposition style received an incorrect argument for positioning");
            }

            base.Initialize();
        }

        public override string Evaluate(TextMeshProUGUI textMeshComponent, StringBuilder currentText, int currentIndex, string fullText)
        {
            RectTransform textWriterRectTransform = null;
            Transform textParentTransform = textMeshComponent.transform.parent;
            if(textParentTransform != null)
            {
                textWriterRectTransform = textParentTransform.GetComponent<RectTransform>();
            }

            if(textWriterRectTransform != null)
            {
                switch (position)
                {
                    case Positions.Center:
                        Debug.Log("Anchors set to center");
                        textWriterRectTransform.anchorMin = new Vector2(0.5f, 0.0f);
                        textWriterRectTransform.anchorMax = new Vector2(0.5f, 0.0f);
                        textWriterRectTransform.pivot = new Vector2(0.5f, 0.0f);
                        break;
                    case Positions.Left:
                        Debug.Log("Anchors set to left");
                        textWriterRectTransform.anchorMin = new Vector2(0.0f, 0.0f);
                        textWriterRectTransform.anchorMax = new Vector2(0.0f, 0.0f);
                        textWriterRectTransform.pivot = new Vector2(0.0f, 0.0f);
                        break;
                    case Positions.Right:
                        Debug.Log("Anchors set to right");
                        textWriterRectTransform.anchorMin = new Vector2(1.0f, 0.0f);
                        textWriterRectTransform.anchorMax = new Vector2(1.0f, 0.0f);
                        textWriterRectTransform.pivot = new Vector2(1.0f, 0.0f);
                        break;
                }

                textWriterRectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
            }

            // Returning a string would add it to the typed text
            return "";
        }
    }
}