using ModuleFour.Model;
using ModuleFour.TaskWorkers;
using SkillBox.Infrastructure.Builder;

namespace ModuleFour.Builder
{
    internal class ModuleFoureTaskWorkerCreator : TaskWorkerCreator<TypeTask>
    {
        public override TaskWork CreateWorkerForTask(TypeTask type)
        {
            switch (type)
            {
                case TypeTask.TaskOne: return new TaskOneWorker();
                case TypeTask.TaskTwo: return new TaskTwoWorker();
                case TypeTask.TaskThree: return new TaskThreeWorker();
                default: return null;
            }
        }
    }
}
