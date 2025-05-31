using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;

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
        private bool _hasCompletionTime;

        // Construtor para EF Core e inicialização padrão
        public UserTask()
        {
            _title = string.Empty;
            _user = null!;
            _lastModifiedDate = DateTime.Now;
            _hasCompletionTime = false;
        }

        // Construtor para criar novas tarefas
        public UserTask(User user, string title, string? description, DateTime? completionDate, bool hasCompletionTime = false)
        {
            _user = user;
            _userId = user.Id;
            _title = title;
            _description = description;
            _hasCompletionTime = hasCompletionTime;

            if (completionDate.HasValue)
            {
                if (this.HasCompletionTime)
                    _completionDate = completionDate.Value;
                else
                    _completionDate = completionDate.Value.Date;
            }
            else
            {
                _completionDate = null;
                _hasCompletionTime = false;
            }

            _isComplete = false;
            _lastModifiedDate = DateTime.Now;
            OnPropertyChanged(nameof(LastModifiedDate));
        }

        // Construtor de Cópia
        public UserTask(UserTask source)
        {
            this._id = source.Id;
            this._userId = source.UserId;
            this._user = source.User;
            this._title = source.Title;
            this._description = source.Description;
            this._completionDate = source.CompletionDate;
            this._hasCompletionTime = source.HasCompletionTime;
            this._isComplete = source.IsComplete;
            this._lastModifiedDate = source.LastModifiedDate;
            OnPropertyChanged(nameof(LastModifiedDate));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id
        {
            get => _id;
            private set { _id = value; OnPropertyChanged(); }
        }

        public int UserId
        {
            get => _userId;
            set { 
                if (_userId != value) { 
                    _userId = value;
                    UpdateLastModifiedDate();
                    OnPropertyChanged(); 
                } 
            }
        }

        public User User
        {
            get => _user;
            private set {
                if (_user != value) {
                    _user = value;
                    UpdateLastModifiedDate();
                    OnPropertyChanged();
                }
            }
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
                    OnPropertyChanged(nameof(CompletionDateDisplay));
                }
            }
        }

        public bool HasCompletionTime
        {
            get => _hasCompletionTime;
            set
            {
                if (_hasCompletionTime != value)
                {
                    _hasCompletionTime = value;
                    UpdateLastModifiedDate();
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CompletionDateDisplay));
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

        // Propriedade formatada para exibição na Lista Principal
        public string? CompletionDateDisplay
        {
            get
            {
                if (!CompletionDate.HasValue)
                {
                    return string.Empty;
                }

                string prefix = "Concluir até: ";

                if (HasCompletionTime)
                    return prefix + CompletionDate.Value.ToString("dd/MM/yyyy HH:mm");
                else
                    return prefix + CompletionDate.Value.ToString("dd/MM/yyyy");
            }
        }


        public void MarkAsComplete()
        {
            IsComplete = true;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateLastModifiedDate()
        {
            LastModifiedDate = DateTime.Now;
        }
    }
}