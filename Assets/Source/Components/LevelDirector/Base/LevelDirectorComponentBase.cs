using Assets.Source.Components.Base;
using Assets.Source.Components.TextWriter;
using Assets.Source.Constants;
using UnityEngine;

namespace Assets.Source.Components.LevelDirector.Base
{
    public abstract class LevelDirectorComponentBase : ComponentBase
    {
        
        public static void InitiateDialogueExchange(string stringsFile) 
        {
            // A prefab won't work here because we need to set properties before instantiating
            GameObject obj = new GameObject(GameObjects.TextWriterPipeline);
            TextWriterPipelineComponent pipeline = obj.AddComponent<TextWriterPipelineComponent>();
            pipeline.LoadText(stringsFile);
            Instantiate(obj);
        }
    }
}
