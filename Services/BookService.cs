using BookCurationApp_V1.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;


namespace BookCurationApp_V1.Services;

public class BookService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public BookService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;

        
        // appsettings.jsonから設定を取得
        _apiBaseUrl = configuration["ApiSettings:BaseUrl"]
                     ?? "https://v3wbyayobb.execute-api.ap-northeast-1.amazonaws.com/default/api/v1";

        Console.WriteLine($"Using API Base URL: {_apiBaseUrl}");
    }

    public async Task<List<Book>> SearchBooksAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<Book>();

        try
        {
            var url = $"{_apiBaseUrl}/books?q={Uri.EscapeDataString(query)}";
            Console.WriteLine($"=== API Call Debug ===");
            Console.WriteLine($"Base URL: {_apiBaseUrl}");
            Console.WriteLine($"Full URL: {url}");
            Console.WriteLine($"Query: {query}");

            var response = await _httpClient.GetAsync(url);
            Console.WriteLine($"Response Status: {response.StatusCode}");

            var jsonContent = await response.Content.ReadAsStringAsync();
            var contentLength = jsonContent?.Length ?? 0;
            Console.WriteLine($"Response Content Length: {contentLength}");

            if (contentLength > 0)
            {
                var previewLength = Math.Min(500, contentLength);
                Console.WriteLine($"Response Content (first {previewLength} chars): {jsonContent.Substring(0, previewLength)}");
            }

            if (response.IsSuccessStatusCode)
            {
                if (string.IsNullOrEmpty(jsonContent))
                {
                    Console.WriteLine("ERROR: Response content is empty");
                    return new List<Book>();
                }

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                try
                {
                    var books = JsonSerializer.Deserialize<List<Book>>(jsonContent, jsonOptions);
                    var bookCount = books?.Count ?? 0;
                    Console.WriteLine($"Successfully parsed {bookCount} books");

                    if (books != null && books.Any())
                    {
                        for (int i = 0; i < Math.Min(3, books.Count); i++)
                        {
                            Console.WriteLine($"Book {i}: {books[i].Title} - {books[i].Authors?.FirstOrDefault()}");
                        }
                    }

                    return books ?? new List<Book>();
                }
                catch (JsonException jsonEx)
                {
                    Console.WriteLine($"JSON Parse Error: {jsonEx.Message}");
                    Console.WriteLine($"JSON Content: {jsonContent}");
                    return new List<Book>();
                }
            }
            else
            {
                Console.WriteLine($"API Error Response: {jsonContent}");
                return new List<Book>();
            }
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"HTTP Request Exception: {httpEx.Message}");
            return new List<Book>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General Exception: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            return new List<Book>();
        }
    }

    public Task AddToFavoritesAsync(string isbn)
    {
        Console.WriteLine($"addFavorite({isbn})");
        return Task.CompletedTask;
    }



}
