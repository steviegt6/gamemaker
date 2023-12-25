using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tomat.GameBreaker;

internal sealed class GameBreakerSettings {
    [JsonProperty("settings")]
    private readonly Dictionary<string, object?> settings = new();

    // Well-known properties:
    [JsonIgnore]
    public bool UseNativeWindowing {
        get => Get("dev.tomat.gamebreaker.use_native_windowing", false);
        set => Set("dev.tomat.gamebreaker.use_native_windowing", value);
    }

    public bool TryGet<T>(string key, out T? value) {
        if (settings.TryGetValue(key, out var obj) && obj is T t) {
            value = t;
            return true;
        }

        value = default;
        return false;
    }

    public T? Get<T>(string key, T? defaultValue = default) {
        return settings.TryGetValue(key, out var obj) && obj is T t ? t : defaultValue;
    }

    public void Set<T>(string key, T value) {
        settings[key] = value;
    }
}
