using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaskBuddyClassLibrary.Models;

namespace TaskBuddyWinClient.Services.Old
{
    public class TaskService
    {
        private readonly HttpClient _httpClient;

        public TaskService()
        {
            _httpClient = new HttpClient();
        }

        public List<TaskBuddyTask> GetTasks(string url, int port)
        {
            try
            {
                var server = $"http://{url}:{port}/api/TaskBuddy";
                var response = _httpClient.GetAsync(server).Result;
                response.EnsureSuccessStatusCode();
                var content = response.Content.ReadAsStringAsync().Result;
                var tasks = JsonConvert.DeserializeObject<IEnumerable<TaskBuddyTask>>(content);
                return tasks.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool PutTask(string url, int port, TaskBuddyTask task)
        {
            try
            {
                var server = $"http://{url}:{port}/api/TaskBuddy/{task.Id}";
                var json = JsonConvert.SerializeObject(task);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = _httpClient.PutAsync(server, content).Result;
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool PostTask(string url, int port, TaskBuddyTask task)
        {
            try
            {
                var server = $"http://{url}:{port}/api/TaskBuddy/";
                var json = JsonConvert.SerializeObject(task);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = _httpClient.PostAsync(server, content).Result;
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteTask(string url, int port, TaskBuddyTask task)
        {
            try
            {
                var server = $"http://{url}:{port}/api/TaskBuddy/{task.Id}";
                var response = _httpClient.DeleteAsync(server).Result;
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
