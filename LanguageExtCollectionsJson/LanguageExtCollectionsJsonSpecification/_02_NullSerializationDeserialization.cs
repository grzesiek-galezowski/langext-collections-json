using System.Collections.Immutable;
using FluentAssertions;
using LanguageExt;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace LanguageExtCollectionsJsonSpecification;

public class _02_NullSerializationDeserialization
{
    [Test]
    public void ShouldBeSerializedFromNull()
    {
        Seq<int>? seq = null;
        Arr<int>? arr = null;
        var serializedSeq = JsonSerializer.Serialize(seq);
        var serializedArr = JsonSerializer.Serialize(arr, SystemTextJsonOptions.WithLanguageExtExtensions());

        serializedSeq.Should().Be("null");
        serializedArr.Should().Be("null");
    }
    
    [Test]
    public void ShouldBeDeserializedFromNull()
    {
        var deserializedSeq = JsonSerializer.Deserialize<Seq<int>?>("null", SystemTextJsonOptions.WithLanguageExtExtensions());
        var deserializedArr = JsonSerializer.Deserialize<Arr<int>?>("null", SystemTextJsonOptions.WithLanguageExtExtensions());

        deserializedSeq.HasValue.Should().BeFalse();
        deserializedArr.HasValue.Should().BeFalse();
    }

    [Test]
    public void ShouldBeSerializedAsNullWithinDataStructureWithBothSerializersTheSameAsNullList()
    {
        var serializedSeqRecord = JsonSerializer.Serialize(new DataWithSeq(null));
        var serializedArrRecord = JsonSerializer.Serialize(new DataWithArr(null), SystemTextJsonOptions.WithLanguageExtExtensions());
        var serializedListRecord = JsonSerializer.Serialize(new DataWithImmutableArray(null), SystemTextJsonOptions.WithLanguageExtExtensions());

        serializedSeqRecord.Should().Be(serializedListRecord);
        serializedArrRecord.Should().Be(serializedListRecord);
    }

    [Test]
    public void ShouldBeDeserializedAsNullWithinDataStructureWithBothSerializersFromSerializedList()
    {
        var originalList = new DataWithImmutableArray(null);
        var serializedList = JsonSerializer.Serialize(originalList);
        var deserializedSeq = JsonSerializer.Deserialize<DataWithSeq>(serializedList, SystemTextJsonOptions.WithLanguageExtExtensions());
        var deserializedArr = JsonSerializer.Deserialize<DataWithArr>(serializedList, SystemTextJsonOptions.WithLanguageExtExtensions());

        deserializedSeq.Ints.HasValue.Should().BeFalse();
        deserializedArr.Ints.HasValue.Should().BeFalse();
    }

    // ...................................................................................//

    [Test]
    public void ShouldBeSerializedWithBothSerializersTheSameAsList()
    {
        var serializedSeq = JsonSerializer.Serialize(Seq<int>.Empty.Add(1).Add(2), SystemTextJsonOptions.WithLanguageExtExtensions());
        var serializedArr = JsonSerializer.Serialize(Arr<int>.Empty.Add(1).Add(2), SystemTextJsonOptions.WithLanguageExtExtensions());
        var serializedLst = JsonSerializer.Serialize(Lst<int>.Empty.Add(1).Add(2), SystemTextJsonOptions.WithLanguageExtExtensions());
        var serializedQue = JsonSerializer.Serialize(Que<int>.Empty.Enqueue(1).Enqueue(2), SystemTextJsonOptions.WithLanguageExtExtensions());
        var serializedHashSet = JsonSerializer.Serialize(LanguageExt.HashSet<int>.Empty.Add(1).Add(2), SystemTextJsonOptions.WithLanguageExtExtensions());
        var serializedSet = JsonSerializer.Serialize(Set<int>.Empty.Add(1).Add(2), SystemTextJsonOptions.WithLanguageExtExtensions());
        var serializedList = JsonSerializer.Serialize(new List<int> { 1, 2 });
        

        serializedSeq.Should().Be(serializedList);
        serializedArr.Should().Be(serializedList);
        serializedLst.Should().Be(serializedList);
        serializedQue.Should().Be(serializedList);
        serializedHashSet.Should().Be(serializedList);
        serializedSet.Should().Be(serializedList);
    }

    [Test]
    public void ShouldBeDeserializedWithBothSerializersFromSerializedList()
    {
        var originalList = new List<int> { 1, 2 };
        var serializedList = JsonSerializer.Serialize<List<int>?>(originalList);
        var deserializedSeq = JsonSerializer.Deserialize<Seq<int>?>(serializedList, SystemTextJsonOptions.WithLanguageExtExtensions());
        var deserializedArr = JsonSerializer.Deserialize<Arr<int>?>(serializedList, SystemTextJsonOptions.WithLanguageExtExtensions());

        deserializedSeq.Value.Should().Equal(originalList);
        deserializedArr.Value.ToArray().Should().Equal(originalList); //conversion needed
    }

    [Test]
    public void ShouldBeSerializedWithinDataStructureWithBothSerializersTheSameAsList()
    {
        var serializedSeqRecord = JsonSerializer.Serialize(new DataWithSeq(Seq<int>.Empty.Add(1).Add(2)));
        var serializedArrRecord = JsonSerializer.Serialize(new DataWithArr(Arr<int>.Empty.Add(1).Add(2)), SystemTextJsonOptions.WithLanguageExtExtensions());
        var serializedListRecord = JsonSerializer.Serialize(new DataWithImmutableArray(ImmutableArray<int>.Empty.Add(1).Add(2)), SystemTextJsonOptions.WithLanguageExtExtensions());

        serializedSeqRecord.Should().Be(serializedListRecord);
        serializedArrRecord.Should().Be(serializedListRecord);
    }

    [Test]
    public void ShouldBeDeserializedWithinDataStructureWithBothSerializersFromSerializedList()
    {
        var originalList = new DataWithImmutableArray(ImmutableArray<int>.Empty.Add(1).Add(2));
        var serializedList = JsonSerializer.Serialize(originalList);
        var deserializedSeq = JsonSerializer.Deserialize<DataWithSeq>(serializedList, SystemTextJsonOptions.WithLanguageExtExtensions());
        var deserializedArr = JsonSerializer.Deserialize<DataWithArr>(serializedList, SystemTextJsonOptions.WithLanguageExtExtensions());

        deserializedSeq.Ints.Value.Should().Equal(originalList.Ints);
        deserializedArr.Ints.Value.ToArray().Should().Equal(originalList.Ints);
    }

    public record DataWithImmutableArray(ImmutableArray<int>? Ints);
    public record DataWithSeq(Seq<int>? Ints);
    public record DataWithArr(Arr<int>? Ints);
}