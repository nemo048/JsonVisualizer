using System.Text;
using JsonVisualizer.Helpers;

namespace JsonVisualizer.Tests;

public sealed class SampleTests
{
    [Fact]
    public async Task SimpleTest()
    {
        const string json = 
@"{
  ""nullValue"": null,
  ""booleanTrue"": true,
  ""booleanFalse"": false,
  ""numberInteger"": 42,
  ""numberFloat"": 3.14,
  ""stringValue"": ""Hello, World!"",
  ""emptyObject"": {},
  ""object"": {
    ""property1"": ""value1"",
    ""property2"": ""value2""
  },
  ""emptyArray"": [],
  ""array"": [
    ""element1"",
    ""element2""
  ],
  ""mixedArray"": [
    null,
    true,
    false,
    123,
    4.56,
    ""some text"",
    {},
    {
      ""nestedProperty"": ""nestedValue""
    },
    [],
    [
      ""nestedElement1"",
      ""nestedElement2""
    ]
  ]
}";
        using (MemoryStream stream = new(Encoding.UTF8.GetBytes(json)))
        using (JsonDrawer jsonVisualizer = new(stream))
        await using (Stream fileStream = File.Create($"test_{DateTime.Now:yyyyMMddHHmmss}.png"))
        {
            await jsonVisualizer.DrawJsonAsync(fileStream, CancellationToken.None);   
        }
    }
}