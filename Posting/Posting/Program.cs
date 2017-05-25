using System;
using System.Collections.Generic;
using ModuleLibrary.posting;
using ModuleLibrary;

namespace Posting
{
    public class Program
    {
        static void Main(string[] args)
        {
            string originalInput = "she squeezed his cheeks";
            Console.WriteLine($"originalInput: {originalInput}");

            var modules = new List<IModule<PreProcessEvent>>();
            modules.Add(new LoudPostingModule());

            // when the ProfanityPostingModule.dll is in the bin folder, we can access everything that assembly
            // by using reflection:
            // ultimately this can come from config files
            var profanityModulePath = "ProfanityPostingModule.ProfanityFilterModule,ProfanityPostingModule";
            var profanityModule = Activator.CreateInstance(Type.GetType(profanityModulePath)) as IModule<PreProcessEvent>;
            modules.Add(profanityModule);

            //only create one PreProcessEvent and then let all the modules register to it.
            var preprocessing = new PreProcessEvent();
            foreach (var module in modules)
            {
                module.Initialize(preprocessing);
            }

            PreprocessArgs preprocessingArgs = PreProcess(originalInput, preprocessing);
            
            Console.WriteLine($"preprocessed input: {preprocessingArgs.Input}");
            Console.WriteLine("now we can do some actual processing with this modified input");
            Console.WriteLine("note: the preprocessing can be simply changed by configuration and without modifying the Posting projects code");

            Console.ReadKey();
        }

        private static PreprocessArgs PreProcess(string originalInput, PreProcessEvent preprocessing)
        {
            var preprocessingArgs = new PreprocessArgs
            {
                Input = originalInput
            };

            preprocessing.ExecutePreprocessing?.Invoke(preprocessingArgs);

            return preprocessingArgs;
        }
    }

    // example module, the ultimate goal is to be able to create modules 
    // externally. That way we can plug in functionality without modifying or even rebuilding 
    // this project. For an example of this, see the ProfanityPostingModule project
    public class LoudPostingModule : IModule<PreProcessEvent>
    {
        public void Initialize(PreProcessEvent events)
        {
            events.ExecutePreprocessing += AddExlamations;
        }

        private void AddExlamations(PreprocessArgs args)
        {
            args.Input = args.Input + "!!!";
        }
    }
}
