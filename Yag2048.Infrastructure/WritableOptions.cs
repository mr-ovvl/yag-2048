using System.Buffers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Yag2048.Core.Infrastructure;

namespace Yag2048.Infrastructure;

public class WritableOptions<TOptions> : IWritableOptions<TOptions>
    where TOptions : class, new()
{
    private static readonly JsonWriterOptions _jsonWriterOptions = new ()
    {
        Indented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private readonly IConfiguration? _configuration;
    private readonly string _jsonFilePath;
    private readonly IOptionsMonitor<TOptions> _options;
    private readonly JsonEncodedText _section;

    public WritableOptions(
        string jsonFilePath,
        string section,
        IOptionsMonitor<TOptions> options,
        IConfiguration? configuration
    )
    {
        ArgumentNullException.ThrowIfNull(jsonFilePath);
        ArgumentNullException.ThrowIfNull(section);
        ArgumentNullException.ThrowIfNull(options);

        _jsonFilePath = jsonFilePath;
        _section = JsonEncodedText.Encode(section);
        _options = options;
        _configuration = configuration;
    }

    public TOptions Value => _options.CurrentValue;

    public TOptions CurrentValue => _options.CurrentValue;

    public TOptions Get(string? name) => _options.Get(name);

    public IDisposable? OnChange(Action<TOptions, string?> listener) => _options.OnChange(listener);

    public void Update(TOptions changedValue, bool reload = false)
    {
        using (var stream = new FileStream(_jsonFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
        {
            var buffer = ArrayPool<byte>.Shared.Rent((int)stream.Length);
            try
            {
                _ = stream.Read(buffer);
                var utf8Json = buffer.AsSpan();
                var utf8bom = Encoding.UTF8.Preamble;

                if (utf8Json.StartsWith(utf8bom))
                {
                    utf8Json = utf8Json[utf8bom.Length..];
                    _ = stream.Seek(utf8bom.Length, SeekOrigin.Begin);
                }
                else
                {
                    stream.Position = 0;
                }

                var reader = new Utf8JsonReader(utf8Json.Length > 0 ? utf8Json : "{}"u8);
                var currentJson = JsonElement.ParseValue(ref reader);
                var writer = new Utf8JsonWriter(stream, _jsonWriterOptions);

                writer.WriteStartObject();
                var isWritten = false;
                var serializedOptionsValue = JsonSerializer.SerializeToElement(changedValue);
                foreach (var element in currentJson.EnumerateObject())
                {
                    if (element.NameEquals(_section.EncodedUtf8Bytes))
                    {
                        writer.WritePropertyName(_section);
                        serializedOptionsValue.WriteTo(writer);
                        isWritten = true;
                    }
                    else
                    {
                        element.WriteTo(writer);
                    }
                }

                if (!isWritten)
                {
                    writer.WritePropertyName(_section);
                    serializedOptionsValue.WriteTo(writer);
                }

                writer.WriteEndObject();
                writer.Flush();
                stream.SetLength(stream.Position);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        if (reload && _configuration is IConfigurationRoot configurationRoot)
            configurationRoot.Reload();
    }
}