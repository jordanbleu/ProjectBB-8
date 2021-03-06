﻿using Assets.Source.Components.Base;
using Assets.Source.Constants;
using Assets.Source.Input.Constants;
using Assets.Source.TextWriterStyle.Base;
using Assets.Source.TextWriterStyle.Factory;
using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace Assets.Source.Components.TextWriter
{
    public class TextWriterComponent : DelayComponentBase
    {
        // Resource Names
        private const string TEXT_OBJECT_NAME = "Text";
        
        private static readonly int DEFAULT_DELAY = 50;

        private TextMeshProUGUI textMeshComponent;
        private TextWriterStyleFactory styleFactory;

        private TextAvatarAnimatorComponent textAvatarAnimatorComponent;
        private Animator animator;

        private AudioSource audioSource;
        private AudioClip beep;

        // Animation Flags
        private bool isAnimationReady = false;
        
        public override void ComponentAwake()
        {
            audioSource = GetRequiredComponent<AudioSource>();
            beep = GetRequiredResource<AudioClip>($"{ResourcePaths.SoundFXFolder}/UI/DialogueBeeps/dialogueBeep_default");

            GameObject textObject = GetRequiredChild(TEXT_OBJECT_NAME);

            textMeshComponent = GetRequiredComponent<TextMeshProUGUI>(textObject);
            textMeshComponent.SetText(string.Empty);
            animator = GetRequiredComponent<Animator>();

            textAvatarAnimatorComponent = GetRequiredComponent<TextAvatarAnimatorComponent>(GetRequiredChild("Image"));

            styleFactory = new TextWriterStyleFactory();

            base.ComponentAwake();
        }

        // If true, the user pressed the key to skip typing
        private bool isSkipped = false;

        /// <summary>
        /// If true, the text writer is finished
        /// </summary>
        public bool IsDoneTyping { get; private set; } = false;

        /// <summary>
        /// The delay between typing characters
        /// </summary>
        public int Delay { get; set; } = DEFAULT_DELAY;

        // The text we are typing out right now
        private string text = "{color:hex=#FF0000}Red {color:hex=#00FF00}Green {color:hex=#0000FF}Bleu.  {color:hex=#FFFFFF}Todays date is {currentDate}.";
        //private string text = @"Todays date is: {currentDate}.";

        // our currently typed characters
        private StringBuilder chars = new StringBuilder();

        // current index
        private int charIndex = 0;

        public override void DelayedUpdate()
        {
            if (isAnimationReady)
            {
                if (!string.IsNullOrEmpty(text) && !IsDoneTyping)
                {
                    IsDoneTyping = (charIndex >= text.Length);

                    if (!IsDoneTyping)
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
                                char commandChar = text[charIndex];

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
                                string result = commandObject.Evaluate(this, textMeshComponent, chars, charIndex + 1, text);

                                if (!string.IsNullOrEmpty(result))
                                {
                                    SpliceResult(result, charIndex);
                                }
                            }
                        }
                        else // Just a normal character
                        {
                            chars.Append(nextChar);
                        }

                    }

                    if (beep != null)
                    {
                        audioSource.PlayOneShot(beep);
                    }
                    charIndex++;
                }
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
                if (IsDoneTyping)
                {
                    TriggerCloseAnimation();
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

        // Call this method to begin the closing animation, which will invoke the IsClosed callback method 
        // When its done 
        private void TriggerCloseAnimation() 
        {
            animator.SetTrigger("close");
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
            // This skips over the }
            string textPart1 = text.Substring(0, charIndex+1);
            string textPart2 = text.Substring(charIndex + 1, text.Length - charIndex - 1);

            text = $"{textPart1}{result}{textPart2}";
        }

        public void SetAvatar(TextAvatarAnimatorComponent.Avatars avatar) 
        {
            textAvatarAnimatorComponent.Avatar = avatar;
        }

        // This is called via animation event.
        public void OnReady()
        {
            isAnimationReady = true;
        }

        // This is called via an animation event
        private void OnClosed()
        {
            OnItemCompleted(null);
            Destroy(gameObject);
        }

    }
}
