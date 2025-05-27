// Adicione estas usings no topo do seu arquivo UserTask.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System; // Para DateTime

namespace TaskManager.Models
{
    public class UserTask : INotifyPropertyChanged
    {
        // Backing fields
        private int _id;
        private int _userId;
        private User _user;
        private string _title;
        private string? _description;
        private DateTime _lastModifiedDate;
        private DateTime? _completionDate;
        private bool _isComplete;

        public int Id
        {
            get => _id;
            private set { _id = value; OnPropertyChanged(); }
        }

        public int UserId
        {
            get => _userId;
            set { if (_userId != value) { _userId = value; UpdateLastModifiedDate(); OnPropertyChanged(); } }
        }

        public User User
        {
            get => _user;
            private set { if (_user != value) { _user = value; UpdateLastModifiedDate(); OnPropertyChanged(); } }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    UpdateLastModifiedDate();
                    OnPropertyChanged();
                }
            }
        }

        public string? Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    UpdateLastModifiedDate();
                    OnPropertyChanged();
                }
            }
        }

        public DateTime LastModifiedDate
        {
            get => _lastModifiedDate;
            private set
            {
                if (_lastModifiedDate != value)
                {
                    _lastModifiedDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? CompletionDate
        {
            get => _completionDate;
            set
            {
                if (_completionDate != value)
                {
                    _completionDate = value;
                    UpdateLastModifiedDate();
                    OnPropertyChanged();
                }
            }
        }

        public bool IsComplete
        {
            get => _isComplete;
            set
            {
                if (_isComplete != value)
                {
                    _isComplete = value;
                    UpdateLastModifiedDate();
                    OnPropertyChanged();
                }
            }
        }

        // Construtor para EF Core e inicialização padrão
        public UserTask()
        {
            _user = null;
            _title = string.Empty;
            _lastModifiedDate = DateTime.Now;
        }

        // Construtor para criar novas tarefas
        public UserTask(User user, string title, string? description, DateTime? completionDate)
        {
            _user = user;
            _userId = user.Id;
            _title = title;
            _description = description;
            _completionDate = completionDate;
            _isComplete = false;
            _lastModifiedDate = DateTime.Now;
        }

        public UserTask(UserTask source)
        {
            this.Id = source.Id;
            this.UserId = source.UserId;
            this.User = source.User;
            this.Title = source.Title;
            this.Description = source.Description;
            this.CompletionDate = source.CompletionDate;
            this.IsComplete = source.IsComplete;
            this._lastModifiedDate = source.LastModifiedDate;
        }

        public void MarkAsComplete()
        {
            IsComplete = true;
        }

        private void UpdateLastModifiedDate()
        {
            LastModifiedDate = DateTime.Now;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}