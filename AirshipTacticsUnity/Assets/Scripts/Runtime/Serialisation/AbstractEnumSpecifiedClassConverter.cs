using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public abstract class AbstractEnumSpecifiedClassConverter<TAbstractModel, TTypeEnum> : JsonConverter
        where TTypeEnum : System.Enum
{
    public class SpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(TAbstractModel).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }

    protected abstract Dictionary<TTypeEnum, Type> TypeDictionary { get; }
    protected string TypeKey = "type";
    static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new SpecifiedConcreteClassConverter() };
    static StringEnumConverter StringEnumConverter = new StringEnumConverter();

    public override bool CanConvert(Type objectType)
        => objectType.IsSubclassOf(typeof(TAbstractModel));

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        string typeString = jo[TypeKey].Value<string>();
        TTypeEnum typeEnum = (TTypeEnum)Enum.Parse(typeof(TTypeEnum), typeString, true);
        if (!TypeDictionary.ContainsKey(typeEnum))
        {
            throw new NotImplementedException();
        }
        Type type = TypeDictionary[typeEnum];
        string json = jo.ToString();
        return JsonConvert.DeserializeObject(json, type, SpecifiedSubclassConversion);
    }

    public override bool CanWrite
    {
        get { return true; }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        JToken token = JToken.FromObject(value);
        JObject jo = (JObject)token;
        Type type = value.GetType();
        jo.AddFirst(new JProperty(TypeKey, TypeDictionary.FirstOrDefault(t => t.Value == type).Key.ToString()));
        jo.WriteTo(writer);
    }
}
