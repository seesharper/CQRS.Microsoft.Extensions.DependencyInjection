namespace CQRS.Microsoft.Extensions.DependencyInjection
{
    using System;
    using CQRS.Command.Abstractions;
    using CQRS.Execution;
    using global::Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// An <see cref="ICommandHandlerFactory"/> implementation that uses
    /// an <see cref="IServiceProvider"/> to create query handlers.
    /// </summary>
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlerFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> that is responsible for creating command handlers.</param>
        public CommandHandlerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public ICommandHandler<TCommand> CreateCommandHandler<TCommand>()
        {
            return serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        }
    }
}
