using TaskManager.Models;
using TaskManager.Data;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.ViewModels
{
    public class TestViewModel
    {
        public void CriarUsuarioT()
        {
            string nome = "Vitor";
            using var context = new AppDbContext();
            context.Database.EnsureCreated();
            var usuarioExiste = context.Users.FirstOrDefault(u => u.Name == nome);

            if (usuarioExiste != null)
                return;

            var novoUsuario = new User(nome);
            context.Users.Add(novoUsuario);
            Console.WriteLine(nome);
            context.SaveChanges();
        }

        public void CriarTarefaT(int userId)
        {
            using var context = new AppDbContext();
            var usuario = context.Users.FirstOrDefault(u => u.Id == userId);
            if (usuario == null)
                return;

            string titulo = "sla";
            string? descricao = "sla";
            DateTime conclusao = DateTime.Now;
            UserTask task = new UserTask(usuario, titulo, descricao, conclusao);
            context.Tasks.Add(task);
            context.SaveChanges();
        }
        public List<UserTask> ObterTarefas()
        {
            using var context = new AppDbContext();
            return context.Tasks.Include(t => t.User).ToList();
        }

        public void LimparBanco()
        {
            using var context = new AppDbContext();
            context.Tasks.RemoveRange(context.Tasks.ToList());
            context.Users.RemoveRange(context.Users.ToList());
            context.SaveChanges();
        }
    }
}