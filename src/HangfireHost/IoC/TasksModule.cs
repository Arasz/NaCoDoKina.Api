using Autofac;
using HangfireHost.Tasks;

namespace HangfireHost.IoC
{
    public class TasksModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(TasksModule).Assembly)
                .Where(type => type.IsAssignableTo<ITasksSchedule>())
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();
        }
    }
}