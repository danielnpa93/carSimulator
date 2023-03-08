using FluentValidation.Results;

namespace DriverService.API.Domain.Utils
{
    public class Notification
    {
        public string Message { get; }
        public Notification(string message)
        {
            Message = message;
        }
    }
    public class NotificationContext
    {
        private readonly List<Notification> _notifications;
        public IReadOnlyCollection<Notification> Notifications => _notifications;
        public bool HasNotifications => _notifications.Any();
        public NotificationContext()
        {
            _notifications = new List<Notification>();
        }
        public void AddNotification(string message)
        {
            _notifications.Add(new Notification(message));
        }

        public void AddNotification(Notification notification)
        {
            _notifications.Add(notification);
        }

        public void AddNotifications(IEnumerable<string> messages)
        {
            var notifications = new List<Notification>();

            foreach (var m in messages)
            {
                notifications.Add(new Notification(m));
            }

            _notifications.AddRange(notifications);
        }

        public void AddNotifications(IReadOnlyCollection<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }

        public void AddNotifications(IList<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }

        public void AddNotifications(ICollection<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }

        public void AddNotifications(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddNotification(error.ErrorMessage);
            }
        }

    }
}
