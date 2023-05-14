using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rystem.OpenAi.Framework;

namespace Microsoft.Extensions.DependencyInjection
{
    public sealed class OpenAiFrameworkBuilder
    {
        private readonly IServiceCollection _services;
        private readonly string _integrationName;

        internal OpenAiFrameworkBuilder(IServiceCollection services, string integrationName)
        {
            _services = services;
            _integrationName = integrationName;
            if (OpenAiSystemPromptComposer.Composer.ContainsKey(_integrationName))
                throw new ArgumentException($"Integration {_integrationName} already installed during setup on startup. Please choose another integration name.");
            OpenAiSystemPromptComposer.Composer.Add(_integrationName, new());
            OpenAiSystemPromptComposer.Composer[_integrationName].Append("You are an assistant the helps user to solve every request. You can use only the commands listed in double quotes to improve your duty, Commands:");
            OpenAiSystemPromptComposer.Composer[_integrationName].Append($"You should only respond in JSON format as described below \nResponse Format: \n{s_defaultResponse}");
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
        public OpenAiFrameworkBuilder AddAction<TOpenAiAction>(int index, string actionDescription)
            where TOpenAiAction : class, IOpenAiAction
        {
            _services.AddTransient<IOpenAiAction, TOpenAiAction>();
            OpenAiSystemPromptComposer.Composer[_integrationName].Append($"\"{index}\": {actionDescription}.");
            return this;
        }
        public OpenAiFrameworkBuilder AddAction<TOpenAiAction, TMessage>(int index, string actionDescription)
            where TOpenAiAction : class, IOpenAiAction<TMessage>
            where TMessage : class
        {
            OpenAiSystemPromptComposer.Composer[_integrationName].Append($"\"{index}\": {actionDescription}.");
            _services.TryAddTransient<IOpenAiAction<TMessage>, TOpenAiAction>();
            return this;
        }
    }
}
