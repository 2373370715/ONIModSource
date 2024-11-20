using Hjson;

namespace RsLib;

public class RsHjsonUtil {
    public static void PathAddString(JsonObject jsonObject, string path, string str) {
        var       split = path.Split('.');
        int       i;
        JsonValue value;
        for (i = 0; i < split.Length - 1; i++) {
            var s = split[i];
            if (jsonObject.TryGetValue(s, out value) && value.JsonType == JsonType.Object)
                jsonObject = (JsonObject)value;
            else {
                jsonObject[s] = new JsonObject();
                jsonObject    = (JsonObject)jsonObject[s];
            }
        }

        jsonObject[split[i]] = str;
    }
}