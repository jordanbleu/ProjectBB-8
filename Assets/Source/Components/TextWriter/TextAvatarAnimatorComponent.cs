using Assets.Source.Components.Base;
using UnityEngine;

namespace Assets.Source.Components.TextWriter
{
    public class TextAvatarAnimatorComponent : ComponentBase
    {
        private TextWriterComponent textWriter;
        private Animator animator;

        public enum Avatars
        {
            None = 0,
            badFace = 1,
            sexyNurse_normalSpeak = 2,
            player_normalSpeak = 3,
            player_glassesSpeak = 4,
            major_normalSpeak = 5,
            technician_normalSpeak = 6
        }

        public Avatars Avatar { get; set; } = Avatars.None;
        public override void ComponentAwake()
        {
            // Grab a reference to the text writer, which we hook into and check if its typing
            textWriter = GetRequiredComponentInParent<TextWriterComponent>();

            animator = GetRequiredComponent<Animator>();
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            animator.SetInteger("avatar_id", (int)Avatar);

            if (textWriter.IsDoneTyping)
            {
                if (!Avatar.Equals(Avatars.None))
                {
                    // This is weird but it resets the animation and 
                    // then stops it immediately.  This ensures that the 
                    // animation ends on the first frame so the avatars face 
                    // doesn't get stuck like :O 
                    animator.Play(string.Empty, 0, 0f);
                    animator.StopPlayback();
                }
            }
            base.ComponentUpdate();
        }

    }
}
