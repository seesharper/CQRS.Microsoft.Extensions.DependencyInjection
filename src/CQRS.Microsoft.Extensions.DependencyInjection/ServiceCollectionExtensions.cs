namespace CQRS.Microsoft.Extensions.DependencyInjection
{
    using System.Linq;
    using System.Reflection;
    using CQRS.Command.Abstractions;
    using CQRS.Execution;
    using CQRS.Query.Abstractions;
    using global::Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extends the <see cref="IServiceCollection"/> with methods for registering command and query handlers.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all command handlers found in the calling assembly to the <paramref name="serviceCollection"/> as scoped services.
        /// </summary>
        /// <param name="serviceCollection">The target <see cref="IServiceCollection"/>.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddCommandHandlers(this IServiceCollection serviceCollection)
        {
            return AddCommandHandlers(serviceCollection, Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Adds all command handlers found in the given <paramref name="assembly"/> to the <paramref name="serviceCollection"/> as scoped services.
        /// </summary>
        /// <param name="serviceCollection">The target <see cref="IServiceCollection"/>.</param>
        /// <param name="assembly">The assembly from which to add command handlers. </param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddCommandHandlers(this IServiceCollection serviceCollection, Assembly assembly)
        {
            var commandHanderDescriptions = assembly.GetCommandHandlerDescriptors();

            foreach (var commandHanderDescription in commandHanderDescriptions)
            {
                serviceCollection.AddScoped(commandHanderDescription.HandlerType, commandHanderDescription.ImplementingType);
            }

            if (!serviceCollection.Any(sd => sd.ServiceType == typeof(ICommandHandlerFactory)))
            {
                serviceCollection.AddScoped<ICommandHandlerFactory>(sp => new CommandHandlerFactory(sp));
            }

            if (!serviceCollection.Any(sd => sd.ServiceType == typeof(ICommandExecutor)))
            {
                serviceCollection.AddScoped<ICommandExecutor, CommandExecutor>();
            }

            return serviceCollection;
        }

        /// <summary>
        /// Adds all query handlers found in the calling assembly to the <paramref name="serviceCollection"/> as scoped services.
        /// </summary>
        /// <param name="serviceCollection">The target <see cref="IServiceCollection"/>.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddQueryHandlers(this IServiceCollection serviceCollection)
        {
            return AddQueryHandlers(serviceCollection, Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Adds all query handlers found in the given <paramref name="assembly"/> to the <paramref name="serviceCollection"/> as scoped services.
        /// </summary>
        /// <param name="serviceCollection">The target <see cref="IServiceCollection"/>.</param>
        /// <param name="assembly">The assembly from which to add query handlers. </param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddQueryHandlers(this IServiceCollection serviceCollection, Assembly assembly)
        {
            var queryHandlerDescriptions = assembly.GetQueryHandlerHandlerDescriptors();

            foreach (var queryHandlerDescription in queryHandlerDescriptions)
            {
                serviceCollection.AddScoped(queryHandlerDescription.HandlerType, queryHandlerDescription.ImplementingType);
            }

            if (!serviceCollection.Any(sd => sd.ServiceType == typeof(IQueryHandlerFactory)))
            {
                serviceCollection.AddScoped<IQueryHandlerFactory>(sp => new QueryHandlerFactory(sp));
            }

            if (!serviceCollection.Any(sd => sd.ServiceType == typeof(IQueryExecutor)))
            {
                serviceCollection.AddScoped<IQueryExecutor, QueryExecutor>();
            }

            return serviceCollection;
        }
    }
}
