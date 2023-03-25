using FluentAssertions;
using Kei.EventSourcing.UnitTests.Helpers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Kei.EventSourcing.UnitTests;

public class EventPublisherTests
{
    [Fact]
    public async Task SubscribedOneEvent_EventPublished_ActionInvoked()
    {
        // Arrange
        var publisher = new MemoryEventPublisher();
        var testEvent = new FakeEvent();
        Event publishedEvent = null;

        publisher.Subscribe(typeof(FakeEvent), (Event @event) => { publishedEvent = @event; });

        // Act
        await publisher.PublishAsync(testEvent);

        // Assert
        publishedEvent.Should().BeSameAs(testEvent).And.NotBeNull();
    }

    [Fact]
    public async Task SubscribedManyEventsOfSameType_EventPublished_ActionsInvoked()
    {
        // Arrange
        var publisher = new MemoryEventPublisher();
        var testEvent = new FakeEvent();
        Event publishedEvent = null;
        Event secondPublishedEvent = null;

        publisher.Subscribe(typeof(FakeEvent), (Event @event) => { publishedEvent = @event; })
            .Subscribe(typeof(FakeEvent), (Event @event) => { secondPublishedEvent = @event; });

        // Act
        await publisher.PublishAsync(testEvent);

        // Assert
        publishedEvent.Should().BeSameAs(testEvent).And.NotBeNull();
        secondPublishedEvent.Should().BeSameAs(secondPublishedEvent).And.NotBeNull();
    }

    [Fact]
    public async Task SubscribedDifferentEventTypes_EventPublished_OnlyPublishedTypeInvoked()
    {
        // Arrange
        var publisher = new MemoryEventPublisher();
        var testEvent = new FakeEvent();
        Event publishedEvent = null;
        Event secondPublishedEvent = null;

        publisher.Subscribe(typeof(FakeEvent), (Event @event) => { publishedEvent = @event; })
            .Subscribe(typeof(OtherFakeEvent), (Event @event) => { secondPublishedEvent = @event; });

        // Act
        await publisher.PublishAsync(testEvent);

        // Assert
        publishedEvent.Should().BeSameAs(testEvent).And.NotBeNull();
        secondPublishedEvent.Should().BeNull();
    }

    [Fact]
    public async Task NoSubscriptions_EventPublished_NoActionAndDoesNotThrow()
    {
        // Arrange
        var publisher = new MemoryEventPublisher();
        var testEvent = new FakeEvent();
        Event publishedEvent = null;
        
        // Act
        await publisher.PublishAsync(testEvent);

        // Assert
        publishedEvent.Should().BeNull();
    }

    [Fact]
    public void SubscribeToNonEvent_ThrowsArgumentException()
    {
        // Arrange
        var publisher = new MemoryEventPublisher();

        // Act
        Action subscribeAction = () => publisher.Subscribe(typeof(string), (Event @event) => { });

        // Assert
        subscribeAction.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void PublishNull_ThrowsArgumentNullException()
    {
        // Arrange
        var publisher = new MemoryEventPublisher();

        // Act
        Func<Task> publishNullAction = async () => await publisher.PublishAsync(null);

        // Assert
        publishNullAction.Should().ThrowAsync<ArgumentNullException>();
    }
}
