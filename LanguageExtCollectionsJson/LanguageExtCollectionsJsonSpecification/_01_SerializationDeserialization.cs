using System.Collections.Immutable;
using FluentAssertions;
using LanguageExt;
using LanguageExtCollectionsJson.Converters;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace LanguageExtCollectionsJsonSpecification;

public class _01_SerializationDeserialization
{
    [Test]
    public void ShouldBeSerializedWithBothSerializersTheSameAsList()
    {
        var serializedSeq = JsonSerializer.Serialize(Seq<int>.Empty.Add(1).Add(2));
        var serializedArr = JsonSerializer.Serialize(Arr<int>.Empty.Add(1).Add(2), SystemTextJsonOptions.WithLanguageExtExtensions());
        var serializedList = JsonSerializer.Serialize(new List<int> { 1, 2 });

        serializedSeq.Should().Be(serializedList);
        serializedArr.Should().Be(serializedList);
    }
    
    [Test]
    public void ShouldBeDeserializedWithBothSerializersFromSerializedList()
    {
        var originalList = new List<int> { 1, 2 };
        var serializedList = JsonSerializer.Serialize(originalList);
        var deserializedSeq = JsonSerializer.Deserialize<Seq<int>>(serializedList, SystemTextJsonOptions.WithLanguageExtExtensions());
        var deserializedArr = JsonSerializer.Deserialize<Arr<int>>(serializedList, SystemTextJsonOptions.WithLanguageExtExtensions());

        deserializedSeq.Should().Equal(originalList);
        deserializedArr.ToArray().Should().Equal(originalList); //conversion needed
    }
    
    [Test]
    public void ShouldBeSerializedWithinDataStructureWithBothSerializersTheSameAsList()
    {
        var serializedSeqRecord = JsonSerializer.Serialize(new DataWithSeq(Seq<int>.Empty.Add(1).Add(2)), SystemTextJsonOptions.WithLanguageExtExtensions());
        var serializedArrRecord = JsonSerializer.Serialize(new DataWithArr(Arr<int>.Empty.Add(1).Add(2)), SystemTextJsonOptions.WithLanguageExtExtensions());
        var serializedListRecord = JsonSerializer.Serialize(new DataWithImmutableArray(ImmutableArray<int>.Empty.Add(1).Add(2)));

        serializedSeqRecord.Should().Be(serializedListRecord);
        serializedArrRecord.Should().Be(serializedListRecord);
    }
    
    [Test]
    public void ShouldBeSerializedWithinDataStructureWithAttributesWithSystemTextJsonSerializersTheSameAsList()
    {
        var serializedSeqRecordWithAttribute = JsonSerializer.Serialize(new DataWithSeqAttribute(Seq<int>.Empty.Add(1).Add(2)), SystemTextJsonOptions.WithLanguageExtExtensions());
        var serializedArrRecordWithAttribute = JsonSerializer.Serialize(new DataWithArrAttribute(Arr<int>.Empty.Add(1).Add(2)));
        var serializedListRecord = JsonSerializer.Serialize(new DataWithImmutableArray(ImmutableArray<int>.Empty.Add(1).Add(2)));

        serializedSeqRecordWithAttribute.Should().Be(serializedListRecord);
        serializedArrRecordWithAttribute.Should().Be(serializedListRecord);
    }
    
    [Test]
    public void ShouldBeDeserializedWithinDataStructureWithBothSerializersFromSerializedList()
    {
        var originalList = new DataWithImmutableArray(ImmutableArray<int>.Empty.Add(1).Add(2));
        var serializedList = JsonSerializer.Serialize(originalList);
        var deserializedSeq = JsonSerializer.Deserialize<DataWithSeq>(serializedList, SystemTextJsonOptions.WithLanguageExtExtensions());
        var deserializedArr = JsonSerializer.Deserialize<DataWithArr>(serializedList, SystemTextJsonOptions.WithLanguageExtExtensions());

        deserializedSeq.Ints.Should().Equal(originalList.Ints);
        deserializedArr.Ints.ToArray().Should().Equal(originalList.Ints);
    }

    public record DataWithImmutableArray(ImmutableArray<int> Ints);
    public record DataWithSeq(Seq<int> Ints);
    public record DataWithArr(Arr<int> Ints);
    
    public record DataWithSeqAttribute(
      [property: System.Text.Json.Serialization.JsonConverter(typeof(SeqConverter))] Seq<int> Ints);

    public record DataWithArrAttribute(
      [property: System.Text.Json.Serialization.JsonConverter(typeof(ArrConverter))] Arr<int> Ints);
}