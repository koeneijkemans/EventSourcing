using FluentAssertions;
using Kei.EventSourcing.UnitTests.Helpers;
using System;
using Xunit;

namespace Kei.EventSourcing.UnitTests
{
    public class EventPublisherTests
    {
        [Fact]
        public void SubscribedOneEvent_EventPublished_ActionInvoked()
        {
            // Arrange
            var publisher = new EventPublisher();
            var testEvent = new FakeEvent();
            Event publishedEvent = null;

            publisher.Subscribe(typeof(FakeEvent), (Event @event) => { publishedEvent = @event; });

            // Act
            publisher.Publish(testEvent);

            // Assert
            publishedEvent.Should().BeSameAs(testEvent).And.NotBeNull();
        }

        [Fact]
        public void SubscribedManyEventsOfSameType_EventPublished_ActionsInvoked()
        {
            // Arrange
            var publisher = new EventPublisher();
            var testEvent = new FakeEvent();
            Event publishedEvent = null;
            Event secondPublishedEvent = null;

            publisher.Subscribe(typeof(FakeEvent), (Event @event) => { publishedEvent = @event; })
                .Subscribe(typeof(FakeEvent), (Event @event) => { secondPublishedEvent = @event; });

            // Act
            publisher.Publish(testEvent);

            // Assert
            publishedEvent.Should().BeSameAs(testEvent).And.NotBeNull();
            secondPublishedEvent.Should().BeSameAs(secondPublishedEvent).And.NotBeNull();
        }

        [Fact]
        public void SubscribedDifferentEventTypes_EventPublished_OnlyPublishedTypeInvoked()
        {
            // Arrange
            var publisher = new EventPublisher();
            var testEvent = new FakeEvent();
            Event publishedEvent = null;
            Event secondPublishedEvent = null;

            publisher.Subscribe(typeof(FakeEvent), (Event @event) => { publishedEvent = @event; })
                .Subscribe(typeof(OtherFakeEvent), (Event @event) => { secondPublishedEvent = @event; });

            // Act
            publisher.Publish(testEvent);

            // Assert
            publishedEvent.Should().BeSameAs(testEvent).And.NotBeNull();
            secondPublishedEvent.Should().BeNull();
        }

        [Fact]
        public void NoSubscriptions_EventPublished_NoActionAndDoesNotThrow()
        {
            // Arrange
            var publisher = new EventPublisher();
            var testEvent = new FakeEvent();
            Event publishedEvent = null;
            
            // Act
            publisher.Publish(testEvent);

            // Assert
            publishedEvent.Should().BeNull();
        }

        [Fact]
        public void SubscribeToNonEvent_ThrowsArgumentException()
        {
            // Arrange
            var publisher = new EventPublisher();

            // Act
            Action subscribeAction = () => publisher.Subscribe(typeof(string), (Event @event) => { });

            // Assert
            subscribeAction.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void PublishNull_ThrowsArgumentNullException()
        {
            // Arrange
            var publisher = new EventPublisher();

            // Act
            Action publishNullAction = () => publisher.Publish(null);

            // Assert
            publishNullAction.Should().Throw<ArgumentNullException>();
        }
    }
}
