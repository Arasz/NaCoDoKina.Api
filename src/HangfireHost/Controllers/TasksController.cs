using HangfireHost.Models;
using HangfireHost.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HangfireHost.Controllers
{
    [Route("[controller]")]
    public class TasksController : Controller
    {
        private readonly IEnumerable<ITasksSchedule> _tasksSchedules;
        private readonly ILogger<TasksController> _logger;

        public TasksController(IEnumerable<ITasksSchedule> tasksSchedules, ILogger<TasksController> logger)
        {
            _tasksSchedules = tasksSchedules ?? throw new ArgumentNullException(nameof(tasksSchedules));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("")]
        public IActionResult Tasks()
        {
            return View(_tasksSchedules.Select(schedule => new ScheduleModel { Name = schedule.Name }));
        }

        [HttpGet("schedule/{scheduleName}/run")]
        public IActionResult RunTaskSchedule(string scheduleName)
        {
            var taskSchedule = _tasksSchedules
                .Single(schedule => schedule.Name == scheduleName);

            taskSchedule.Schedule();

            return View(new ScheduleModel { Name = taskSchedule.Name });
        }
    }
}