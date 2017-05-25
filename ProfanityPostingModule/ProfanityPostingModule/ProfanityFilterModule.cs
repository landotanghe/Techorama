using ModuleLibrary;
using ModuleLibrary.posting;

namespace ProfanityPostingModule
{
    public class ProfanityFilterModule : IModule<PreProcessEvent>
    {
        private const string FilthyWordReplacement = "%$#@";

        public void Initialize(PreProcessEvent events)
        {
            events.ExecutePreprocessing += ReplaceFilthyWords;
        }

        private void ReplaceFilthyWords(PreprocessArgs args)
        {
            var filthyWord = "cheek";
            args.Input = args.Input.Replace(filthyWord, FilthyWordReplacement);
        }
    }
}
