using Autofac;

namespace HangfireHost.Tasks
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