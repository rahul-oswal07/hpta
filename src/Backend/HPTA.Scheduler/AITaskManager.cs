using Hangfire;
using HPTA.Services.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace HPTA.Scheduler
{
    public class AITaskManager : IAITaskStatusUpdater
    {
        private static readonly ConcurrentDictionary<string, bool> jobs = new();
        public static void Enqueue(int surveyId, int? teamId, string email)
        {
            var key = CreateKey(surveyId, teamId, email);
            BackgroundJob.Enqueue<IAnswerService>(a => a.UpdateAIResponse(teamId, email, surveyId, key));
        }

        public static bool IsJobRunning(int surveyId, int? teamId, string email)
        {
            var key = CreateKey(surveyId, teamId, email);
            if (jobs.TryGetValue(key, out bool value))
            {
                return value;
            }
            return false;
        }

        public void UpdateStatus(string key, bool status)
        {
            if (jobs.ContainsKey(key))
                jobs[key] = status;
            else
                jobs.TryAdd(key, status);
        }

        private static string CreateKey(int surveyId, int? teamId, string email) => $"{surveyId}|{teamId ?? 0}|{email}";
    }
}
