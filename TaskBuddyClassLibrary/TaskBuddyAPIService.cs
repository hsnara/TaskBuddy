using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using TaskBuddyClassLibrary.Models;
using TaskBuddyClassLibrary.Exceptions;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Net;

namespace TaskBuddyClassLibrary.Services
{
    public class TaskService
    {
        private readonly HttpClient _httpClient;

        private string _url;
        private int _port;
        public int ServerPort
        {
            get { return _port; }
            set { _port = value; }
        }
        public string ServerAddress
        {
            get { return _url; }
            set
            { _url = value; }
        }

        public TaskService()
        {
            _httpClient = new HttpClient();
        }

        public TaskService(string url, int port)
        {
            _httpClient = new HttpClient();
            _url = url;
            _port = port;
        }

        public List<TaskBuddyTask> GetTasks()
        {
            try
            {
                var server = $"http://{_url}:{_port}/api/TaskBuddy";
                var response = _httpClient.GetAsync(server).Result;
                response.EnsureSuccessStatusCode();
                var content = response.Content.ReadAsStringAsync().Result;
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var tasks = JsonSerializer.Deserialize<List<TaskBuddyTask>>(content, options);
                
                return tasks.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool PutTask(TaskBuddyTask task)
        {
            try
            {
                var server = $"http://{_url}:{_port}/api/TaskBuddy/{task.Id}";
                var json = JsonSerializer.Serialize(task);
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
        public bool PostTask(TaskBuddyTask task)
        {
            try
            {
                var server = $"http://{_url}:{_port}/api/TaskBuddy/";
                var json = JsonSerializer.Serialize(task);
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

        public bool DeleteTask(TaskBuddyTask task)
        {
            try
            {
                var server = $"http://{_url}:{_port}/api/TaskBuddy/{task.Id}";
                var response = _httpClient.DeleteAsync(server).Result;
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<TaskBuddyTask>> GetTasksAsync()
        {
            var server = $"http://{_url}:{_port}/api/TaskBuddy";
            var response = await _httpClient.GetAsync(server);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var tasks = JsonSerializer.Deserialize<List<TaskBuddyTask>>(content, options);
            return tasks.ToList();
        }

        public async Task<bool> PutTaskAsync(TaskBuddyTask task)
        {
            var server = $"http://{_url}:{_port}/api/TaskBuddy/{task.Id}";
            var json = JsonSerializer.Serialize(task);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(server, content);
            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<bool> PostTaskAsync(TaskBuddyTask task)
        {
            var server = $"http://{_url}:{_port}/api/TaskBuddy";
            var json = JsonSerializer.Serialize(task);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(server, content);
            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<bool> DeleteTaskAsync(TaskBuddyTask task)
        {
            var server = $"http://{_url}:{_port}/api/TaskBuddy/{task.Id}";
            var response = await _httpClient.DeleteAsync(server);
            response.EnsureSuccessStatusCode();
            return true;
        }

    }
}
