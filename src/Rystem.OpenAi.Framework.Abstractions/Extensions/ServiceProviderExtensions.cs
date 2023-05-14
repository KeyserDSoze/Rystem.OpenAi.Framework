using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Rystem.OpenAi.Framework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static IServiceProvider MapOpenAiFrameworkActions(this IServiceProvider serviceProvider)
        {
            var actions = serviceProvider.GetServices<IOpenAiAction>();
            OpenAiSystemPromptComposer.Composer.Append("You are an assistant the helps user to solve every request. You can use only the commands listed in double quotes to improve your duty, Commands:");
            foreach (var action in actions.GroupBy(x => x.Id).Select(x => x.First()))
            {
                OpenAiSystemPromptComposer.Composer.Append($"CommandId:\"{action.Id}\", {action.Description}.");
            }
            OpenAiSystemPromptComposer.Composer.Append($"You should only respond in JSON format as described below \nResponse Format: \n{s_defaultResponse}");
            OpenAiSystemPromptComposer.FinalComposition = OpenAiSystemPromptComposer.Composer.ToString();
            return serviceProvider;
        }
        private static readonly string s_defaultResponse = new OpenAiFrameworkResponse()
        {
            Thought = new OpenAiThought
            {
                Text = "thought",
                Reason = "reasoning",
                Criticism = "constructive self-criticism",
                Speak = "thoughts summary to say to user",
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
