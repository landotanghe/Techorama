using System;

namespace ModuleLibrary.posting
{
    public class PreProcessEvent
    {
        public Action<PreprocessArgs> ExecutePreprocessing { get; set; }
    }
}
