using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManager.Models;
using TaskManager.Data;
using TaskManager.ViewModels;
using TaskManager.Exceptions;

namespace TaskManager.Views;

public partial class TaskViewWindow : Window
{
    private User _user;
    public TaskViewWindow(User user)
    {
        InitializeComponent();
        _user = user;
    }
}

