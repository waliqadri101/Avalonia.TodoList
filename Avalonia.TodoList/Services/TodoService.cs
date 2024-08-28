﻿using Avalonia.TodoList.Data.Dtos;
using Avalonia.TodoList.Data.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<List<Todo>> GetAllTodosAsync()
        {
            var response = await _httpClient.GetAsync("api/todo/all");

            if (response.IsSuccessStatusCode)
            {
                var todos = await response.Content.ReadFromJsonAsync<List<Todo>>();
                return todos ?? new List<Todo>();
            }
            else
            {
                return new List<Todo>();
            }
        }

        // Method to update a Todo item
        public async Task<bool> UpdateTodoAsync(GetTodoDto todo)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/todo/update/{todo.Id}", todo);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error response here
                Debug.WriteLine($"Failed to update todo item with ID: {todo.Id}");
                return false;
            }
            else {
                Debug.WriteLine($"Successfully updated the ID: {todo.Id}");
                return true;
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

    }

}
