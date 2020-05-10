using Assets.Source.Components.TextWriter;
using Assets.Source.TextWriterStyle.Base;
using Assets.Source.TextWriterStyle.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace Assets.Source.TextWriterStyle
{
    /// <summary>
    /// Usage: {Avatar:id=animation_nameId}
    /// </summary>
    public class AvatarStyle : TextWriterStyleBase
    {
        private TextAvatarAnimatorComponent.Avatars avatarArg;

        protected override void Initialize()
        {
            string enumString = GetArgumentValue("id");

            if (Enum.TryParse(enumString, true, out TextAvatarAnimatorComponent.Avatars avatar))
            {
                avatarArg = avatar;
            }
            else 
            {
                throw new StyleValidationException($"Unable to {enumString} as an Avatar enum value");
            }
            base.Initialize();
        }

        public override string Evaluate(TextWriterComponent textWriter, TextMeshProUGUI textMeshComponent, StringBuilder currentText, int currentIndex, string fullText)
        {
            textWriter.SetAvatar(avatarArg);
            
            return string.Empty;
        }
    }
}
