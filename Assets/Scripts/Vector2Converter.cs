using System;
using Newtonsoft.Json;
using UnityEngine;

public class Vector2Converter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Vector2 valueVec2 = (Vector2)value;
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(valueVec2.x);
        writer.WritePropertyName("y");
        writer.WriteValue(valueVec2.y);
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        float x = 0, y = 0;
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var propName = reader.Value.ToString();
                reader.Read();
                if (propName == "x") x = Convert.ToSingle(reader.Value);
                else if (propName == "y") y = Convert.ToSingle(reader.Value);
            }
            else if (reader.TokenType == JsonToken.EndObject)
            {
                break;
            }
        }
        return new Vector2(x, y);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Vector2);
    }
}