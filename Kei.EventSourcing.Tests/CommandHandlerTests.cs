using Kei.EventSourcing.UnitTests.Helpers;
using Moq;
using System;
using Xunit;

namespace Kei.EventSourcing.UnitTests
{
    public class CommandHandlerTests
    {

        [Fact]
        public void CommandHandlerAvailable_ExecutesHandleOnce()
        {
            // Arrange
            var mockHandler = new Mock<ICommandHandler<FakeCommand>>();
            var command = new FakeCommand();

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(s => s.GetService(typeof(ICommandHandler<>).MakeGenericType(typeof(FakeCommand))))
                .Returns(mockHandler.Object);

            var commandHandler = new CommandHandler(mockServiceProvider.Object);

            // Act
            commandHandler.Handle(command);

            // Assert
            mockHandler.Verify(m => m.Handle(command), Times.Once);
        }

        [Fact]
        public void CommandHandlerNotAvailable_DoesntExecuteHandleAndDoesntThrowException()
        {
            // Arrange
            var mockHandler = new Mock<ICommandHandler<FakeCommand>>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockCommand = new FakeCommand();

            var commandHandler = new CommandHandler(mockServiceProvider.Object);

            // Act
            commandHandler.Handle(mockCommand);

            // Assert
            mockHandler.Verify(h => h.Handle(mockCommand), Times.Never);
        }
    }
}
