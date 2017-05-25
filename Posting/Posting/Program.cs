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

            var profanityModulePath = "ProfanityPostingModule.ProfanityFilterModule,ProfanityPostingModule";
            var profanityModule = Activator.CreateInstance(Type.GetType(profanityModulePath)) as IModule<PreProcessEvent>;
            modules.Add(profanityModule);

            var preprocessing = new PreProcessEvent();
            foreach (var module in modules)
            {
                module.Initialize(preprocessing);
            }

            PreprocessArgs preprocessingArgs = PreProcess(originalInput, preprocessing);

            Console.WriteLine($"preprocessed input: {preprocessingArgs.Input}");

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
