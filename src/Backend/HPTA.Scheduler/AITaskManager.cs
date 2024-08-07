﻿using Hangfire;
using HPTA.Services.Contracts;
using System.Collections.Concurrent;

namespace HPTA.Scheduler
{
    public class AITaskManager : IAITaskStatusUpdater
    {
        private static readonly ConcurrentDictionary<string, bool> jobs = new();
        public static void Enqueue(int surveyId, int? teamId, string email)
        {
            var key = CreateKey(surveyId, teamId);
            UpdateJobStatus(key, true);
            BackgroundJob.Enqueue<IAnswerService>(a => a.UpdateAIResponse(teamId, email, surveyId, key));
        }

        public static bool IsJobRunning(int surveyId, int? teamId)
        {
            var key = CreateKey(surveyId, teamId);
            if (jobs.TryGetValue(key, out bool value))
            {
                return value;
            }
            return false;
        }

        public void UpdateStatus(string key, bool status)
        {
            UpdateJobStatus(key, status);
        }

        private static void UpdateJobStatus(string key, bool status)
        {
            if (jobs.ContainsKey(key))
                jobs[key] = status;
            else
                jobs.TryAdd(key, status);
        }

        private static string CreateKey(int surveyId, int? teamId) => $"{surveyId}|{teamId ?? 0}";
    }
}
