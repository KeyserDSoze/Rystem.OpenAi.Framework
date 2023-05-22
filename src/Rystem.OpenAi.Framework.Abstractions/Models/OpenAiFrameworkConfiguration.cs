using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Rystem.OpenAi.Framework;

namespace Microsoft.Extensions.DependencyInjection
{
    public sealed class OpenAiFrameworkConfiguration
    {
        public static OpenAiFrameworkConfiguration Instance { get; } = new();
        internal Dictionary<string, List<Type>> MappedActions { get; } = new();
        private readonly Dictionary<string, string> _integrationComposition = new();
        private OpenAiFrameworkConfiguration() { }
        public string GetSystemMessage(string integrationName)
            => _integrationComposition[integrationName];
        internal void Compose(IEnumerable<IOpenAiAction> actions)
        {
            foreach (var integrationName in MappedActions.Keys)
            {
                var configuratedTypes = MappedActions[integrationName];
                if (!_integrationComposition.ContainsKey(integrationName))
                {
                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append("You are an assistant the helps user to solve every request. You can use only the commands listed in double quotes to improve your duty, Commands:");
                    foreach (var action in actions.Where(x => configuratedTypes.Contains(x.GetType())).GroupBy(x => x.Id).Select(x => x.First()))
                    {
                        stringBuilder.Append($"CommandId:\"{action.Id}\", {action.Description}.");
                    }
                    stringBuilder.Append($"You should only respond in JSON format as described below \nResponse Format: \n{s_defaultResponse}");
                    _integrationComposition.Add(integrationName, stringBuilder.ToString());
                }
            }
        }
        private static readonly string s_defaultResponse = new AgentAction()
        {
            Thought = new OpenAiThought
            {
                Text = "thought",
                Reason = "reasoning",
                Criticism = "constructive self-criticism",
                Speak = "thoughts summary to say to user",
                Title = "create a title for the request in ",
                Actions = new List<PlannedAction>
                 {
                     new PlannedAction
                     {
                         ActionToDo = "Action to do from a list of actions that conveys long-term plan",
                         CommandId = "The command id to use to solve the action"
                     }
                 }
            }
        }.ToJson();
    }
}
