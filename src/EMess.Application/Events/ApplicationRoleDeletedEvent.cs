using MediatR;

namespace EMess.Application.Events
{
    public class ApplicationRoleDeletedEvent : INotification
    {
        public string RoleId { get; set; } = default!;
    }

    public class ApplicationRoleDeletedEventHandler : INotificationHandler<ApplicationRoleDeletedEvent>
    {
        public Task Handle(ApplicationRoleDeletedEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
