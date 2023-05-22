using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rystem.OpenAi.Framework
{
    public sealed class AgentTasks
    {
        [JsonPropertyName("tasks")]
        public List<AgentTask> Tasks { get; set; }
        internal static readonly string TasksAsJson = new AgentTasks
        {
            Tasks = new List<AgentTask>()
            {
                new AgentTask
                {
                    Title="Insert here a title for the task, max 4 words",
                    Description = "Insert here a detailed description for the task"
                }
            }
        }.ToJson();
    }
}
