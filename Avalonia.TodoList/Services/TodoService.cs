using Avalonia.TodoList.Data.Dtos;
using Avalonia.TodoList.Data.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Avalonia.TodoList.Services
{
    /// <summary>
    /// Class that hits the web api and contains all the CRUD methods
    /// </summary>
    public class TodoService
    {
        private readonly HttpClient _httpClient;

        public TodoService()
        {
            // Create a custom HttpClientHandler to bypass SSL certificate validation
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            };

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://10.0.0.100:7050") // Replace with your actual API URL
            };
        }

        /// <summary>
        /// A service method to hit the api and get all the todo list item, order by decending order of the CreateOn Date.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Todo>> GetAllTodosAsync()
        {
            var response = await _httpClient.GetAsync("api/todo/all");

            if (response.IsSuccessStatusCode)
            {
                var todos = await response.Content.ReadFromJsonAsync<List<Todo>>();
                

                // order the list with the recent CreatedOn on the top.
                if (todos != null && todos.Count > 1)
                {
                    todos = todos.OrderByDescending(todo => todo.CreatedOn).ToList();
                }

                return todos ?? new List<Todo>();
            }
            else
            {
                return new List<Todo>();
            }
        }
        
        public async Task<Todo?> CreateNewTodoAsync(CreateTodoDto todo)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/todo/create", todo);
            if (response.IsSuccessStatusCode)
            {
                var createdTodo = await response.Content.ReadFromJsonAsync<Todo>();
                Debug.WriteLine($"Successfully created a new todo with ID: {createdTodo?.Id}");
                return createdTodo;
            }
            else
            {
                Debug.WriteLine("Failed to create a new todo");
                return null;
            }
        }

        public async Task<bool> UpdateTodoAsync(GetTodoDto todo)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/todo/update/{todo.Id}", todo);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error response here
                Debug.WriteLine($"Failed to update todo item with ID: {todo.Id}");
                return false;
            }
            else
            {
                Debug.WriteLine($"Successfully updated the ID: {todo.Id}");
                return true;
            }
        }
    }
}
