using RolandK.RemoteFileStorage.Util;
using Xunit;

namespace RolandK.RemoteFileStorage.Tests.Util;

[Trait("Category", "NoDependencies")]
public class ReusableMemoryStreamTests
{
    [Fact]
    public void Check_UseAndGiveBack()
    {
        var memoryStream = ReusableMemoryStreams.Current.TakeMemoryStream();
        
        memoryStream.Write([1, 2, 3, 4]);
        
        ReusableMemoryStreams.Current.ReRegisterMemoryStream(memoryStream);
    }
    
    [Fact]
    public void Check_UseAndGiveBack_More_Times()
    {
        for (var loop = 0; loop < 10; loop++)
        {
            var memoryStream = ReusableMemoryStreams.Current.TakeMemoryStream();
        
            memoryStream.Write([1, 2, 3, 4]);
        
            ReusableMemoryStreams.Current.ReRegisterMemoryStream(memoryStream);
        }
    }
    
    [Fact]
    public void Check_UseAndGiveBack_With_Seek()
    {
        var memoryStream = ReusableMemoryStreams.Current.TakeMemoryStream();
        
        memoryStream.Write([1, 2, 3, 4]);
        memoryStream.Seek(0, SeekOrigin.Begin);
        
        ReusableMemoryStreams.Current.ReRegisterMemoryStream(memoryStream);
    }
    
    [Fact]
    public void Check_UseAndGiveBack_With_Seek_More_Times()
    {
        for (var loop = 0; loop < 5; loop++)
        {
            var memoryStream = ReusableMemoryStreams.Current.TakeMemoryStream();
        
            memoryStream.Write([1, 2, 3, 4]);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var targetArray = new byte[4];
            memoryStream.Read(targetArray);
        
            ReusableMemoryStreams.Current.ReRegisterMemoryStream(memoryStream);
        }
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    public void Check_UseAndGiveBack_With_Growth(int growthFactor)
    {
        var memoryStream = ReusableMemoryStreams.Current.TakeMemoryStream();

        for (var loop = 0; loop < growthFactor; loop++)
        {
            memoryStream.Write([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
        }
        
        ReusableMemoryStreams.Current.ReRegisterMemoryStream(memoryStream);
    }
    
    [Fact]
    public void Check_UseAndGiveBack_WriteNothing()
    {
        var memoryStream = ReusableMemoryStreams.Current.TakeMemoryStream();
        
        ReusableMemoryStreams.Current.ReRegisterMemoryStream(memoryStream);
    }
}