namespace PermissionManagement.CQRS
{
    public interface ICommandHandler<TCommand, TResult>
    {
        Task<TResult> HandleAsync(TCommand query);
    }
}
