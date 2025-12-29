using Conways.Service.Domain.Boards;

using FluentAssertions;

namespace Conways.Service.Domain.Tests.Boards;

public sealed class BoardIdTests
{
    [Fact]
    public void New_ShouldCreateNonEmptyUniqueIdentifier()
    {
        // Arrange & Act
        var firstBoardId = BoardId.New();
        var secondBoardId = BoardId.New();

        // Assert
        firstBoardId.Value.Should().NotBe(Guid.Empty);
        secondBoardId.Value.Should().NotBe(Guid.Empty);
        firstBoardId.Should().NotBe(secondBoardId);
    }
}