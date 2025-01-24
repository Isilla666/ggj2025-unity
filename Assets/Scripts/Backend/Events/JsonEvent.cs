using Newtonsoft.Json;

namespace Backend.Events
{
    public abstract class JsonEvent<TOutput> : AbstractEvent<object, TOutput>
    {
        protected override TOutput ConvertToOutput(object input) =>
            JsonConvert.DeserializeObject<TOutput>(input.ToString());
    }
}