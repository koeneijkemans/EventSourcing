using System;

namespace Kei.EventSourcing;

/// <summary>
/// Generic handler for <see cref="Command" />
/// </summary>
public sealed class CommandHandler
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandler" /> class.
    /// </summary>
    /// <param name="serviceProvider">Instance of <see cref="IServiceProvider" /> used to lookup <see cref="ICommandHandler" /></param>
    public CommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Handles an incomming command by looking in the <see cref="IServiceProvider" />
    /// for a class, which has implemented the <see cref="ICommandHandler{T}" /> interface
    /// and can handle the <see cref="Command" />
    /// </summary>
    /// <typeparam name="T">Entity derived from <see cref="Command" /></typeparam>
    /// <param name="command">The command to handle</param>
    public void Handle<T>(T command) where T : Command
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        var handler = _serviceProvider.GetService(handlerType);

        (handler as ICommandHandler<T>)?.Handle(command);
    }
}
