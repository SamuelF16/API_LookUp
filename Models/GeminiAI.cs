using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API_LookUp.Models;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Options;

public interface IGeminiService
{
    Task<byte[]> GeneratePreviewAsync(
        Stream userImage,
        string userImageMimeType,
        Stream productImage,
        string productImageMimeType,
        string? prompt = null);
}

public class GeminiService : IGeminiService
{
    private readonly Client _client;
    private readonly string _model;

    public GeminiService(IOptions<GeminiOptions> options)
    {
        var config = options.Value; 
        var apiKey = config.ApiKey;

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            apiKey = System.Environment.GetEnvironmentVariable("AQ.Ab8RN6JUa9SMPXFASSuJzGdBaBeLMr7cmxhcAy-SKV5skAbCAg") 
                        ?? System.Environment.GetEnvironmentVariable("AQ.Ab8RN6JUa9SMPXFASSuJzGdBaBeLMr7cmxhcAy-SKV5skAbCAg") 
                        ?? throw new InvalidOperationException(
                            @"A API Key do Gemini não foi configurada. Defina 'Gemini:ApiKey' no appsettings.json ou a variável de ambiente 'Gemini_API_Key' ou 'GEMINI_API_KEY'.");
        }

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                "A API Key do Gemini não foi configurada. Defina 'Gemini:ApiKey' ou a variável de ambiente 'Gemini_API_Key'.");
        }

        _client = new Client(apiKey: apiKey);
        _model = string.IsNullOrWhiteSpace(config.Model)
            ? "gemini-2.5-flash-image"
            : config.Model;
    }

    public async Task<byte[]> GeneratePreviewAsync(
        Stream userImage,
        string userImageMimeType,
        Stream productImage,
        string productImageMimeType,
        string? prompt = null)
    {
        var instruction = prompt;
        if (string.IsNullOrWhiteSpace(instruction))
        {
            instruction =
                @"Use as duas imagens fornecidas: a primeira é a pessoa e a segunda é o produto ocular. 
                Faça uma edição realista mantendo pose, expressão facial, roupas, iluminação, proporções e aparência 
                natural do produto.";
        }

        var contents = new List<Content>
        {
            new Content
            {
                Role = "user",
                Parts = new List<Part>
                {
                    new Part { Text = instruction },
                    new Part
                    {
                        InlineData = new Blob
                        {
                            MimeType = userImageMimeType,
                            Data = ReadBytes(userImage)
                        }
                    },
                    new Part
                    {
                        InlineData = new Blob
                        {
                            MimeType = productImageMimeType,
                            Data = ReadBytes(productImage)
                        }
                    }
                }
            }
        };

        var config = new GenerateContentConfig
        {
            Temperature = 0.2,
            ResponseModalities = new List<string> { "IMAGE", "TEXT" },
            SystemInstruction = new Content
            {
                Parts = new List<Part>
                {
                    new Part
                    {
                        Text =
                            "Se forem enviadas duas imagens, uma da pessoa e outra do produto ocular, faça um esboço realista de como a pessoa ficaria com esse produto, preservando pose, expressão facial, roupas, iluminação e proporções."
                    }
                }
            }
        };

        await foreach (var chunk in _client.Models.GenerateContentStreamAsync(_model, contents, config))
        {
            var parts = chunk.Candidates?.FirstOrDefault()?.Content?.Parts;
            if (parts == null)
            {
                continue;
            }

            foreach (var part in parts)
            {
                if (part.InlineData?.Data is { Length: > 0 } data)
                {
                    return data;
                }
            }
        }

        throw new Exception("Não foi possível gerar a imagem ou nenhum dado binário foi retornado.");
    }

    private static byte[] ReadBytes(Stream stream)
    {
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return memory.ToArray();
    }
}