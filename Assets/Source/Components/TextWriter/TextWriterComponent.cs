using Assets.Source.Components.Base;
using Assets.Source.Input.Constants;
using Assets.Source.TextWriterStyle.Base;
using Assets.Source.TextWriterStyle.Factory;
using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace Assets.Source.Components.TextWriter
{
    public class TextWriterComponent : DelayedUpdateBaseComponent
    {
        // Resource Names
        private const string TEXT_OBJECT_NAME = "Text";
        private const int DEFAULT_DELAY = 100;

        private TextMeshProUGUI textMeshComponent;
        private TextWriterStyleFactory styleFactory;

        public override void ComponentAwake()
        {
            GameObject textObject = GetRequiredChild(TEXT_OBJECT_NAME);

            textMeshComponent = GetRequiredComponent<TextMeshProUGUI>(textObject);
            textMeshComponent.SetText(string.Empty);

            styleFactory = new TextWriterStyleFactory();

            base.ComponentAwake();
        }

        private bool isSkipped = false;

        /// <summary>
        /// If true, the text writer is finished
        /// </summary>
        public bool IsComplete { get; private set; } = false;

        /// <summary>
        /// The delay between typing characters
        /// </summary>
        public int Delay { get; set; } = DEFAULT_DELAY;

        // The text we are typing out right now
        //private string text = "{color:hex=#FF0000}Red {color:hex=#00FF00}Green {color:hex=#0000FF}Bleu.  {color:hex=#FFFFFF}Todays date is {currentDate}";
        private string text = "Todays date is {currentDate}";

        // our currently typed characters
        private StringBuilder chars = new StringBuilder();

        // current index
        private int charIndex = 0;

        public override void DelayedUpdate()
        {
            if (!string.IsNullOrEmpty(text))
            {
                IsComplete = (charIndex >= text.Length);

                if (!IsComplete)
                {
                    char nextChar = text[charIndex];

                    if (nextChar.Equals('{'))
                    {
                        // Begin reading the remaining text as a command until we reach the }
                        StringBuilder command = new StringBuilder();
                        bool foundEndTag = false;

                        while (charIndex < text.Length)
                        {
                            charIndex++;
                            Char commandChar = text[charIndex];

                            if (commandChar.Equals('}'))
                            {
                                foundEndTag = true;
                                break;
                            }
                            else if (commandChar.Equals('{'))
                            {
                                throw new UnityException("Unable to parse command string.  Expected ending } but found another opening {");
                            }
                            else
                            {
                                command.Append(text[charIndex]);
                            }
                        }

                        if (!foundEndTag)
                        {
                            throw new UnityException("Unable to parse command string.  Expected ending }");
                        }
                        else
                        {
                            // Execute the command
                            TextWriterStyleBase commandObject = styleFactory.Create(command.ToString());
                            string result = commandObject.Evaluate(textMeshComponent, chars, charIndex + 1, text);

                            if (!string.IsNullOrEmpty(result))
                            {
                                SpliceResult(result, charIndex);
                            }

                            charIndex--; //?
                        }
                    }
                    else // Just a normal character
                    {
                        chars.Append(nextChar);
                    }

                }
                charIndex++;
            }
        }


        // This runs every frame and doesn't give a FRICK about the frame delay
        public override void ComponentUpdate()
        {
            textMeshComponent.SetText(chars.ToString());

            if (!isSkipped)
            {
                FrameTimeDelay = Delay;
            }

            if (InputManager.IsKeyPressed(InputConstants.K_MENU_ENTER))
            {
                if (IsComplete)
                {
                    KillSelf();
                }
                else
                {
                    isSkipped = true;
                    Delay = 0;
                    FrameTimeDelay = 0;
                }
            }
            base.ComponentUpdate();
        }

        /// <summary>
        /// Subscribe to this event to attach a delegate that will be invoked 
        /// when the item finishes 
        /// </summary>
        public event EventHandler ItemCompleted;

        // Called when the item kills itself after finishing typing the text
        private void OnItemCompleted(EventArgs e)
        {
            EventHandler handler = ItemCompleted;
            ItemCompleted?.Invoke(this, e);
        }

        // don't do this irl
        private void KillSelf()
        {
            OnItemCompleted(null);
            Destroy(gameObject);
        }

        /// <summary>
        /// Manually type out a single string 
        /// </summary>
        /// <param name="textValue"></param>
        public void LoadText(string textValue)
        {
            if (textMeshComponent != null)
            {
                textMeshComponent.SetText(string.Empty);
            }
            chars.Clear();
            this.text = textValue ?? string.Empty;
        }

        /// <summary>
        /// Takes the result of a style and splices it into the current text.  This way the 
        /// result of our command is typed out as if it were part of the original text.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="text"></param>
        /// <param name="charIndex"></param>
        private void SpliceResult(string result, int charIndex)
        {
            string textPart1 = text.Substring(0, charIndex);
            string textPart2 = text.Substring(charIndex, text.Length - charIndex);
            text = $"{textPart1}{result}{textPart2}";
        }
    }
}
