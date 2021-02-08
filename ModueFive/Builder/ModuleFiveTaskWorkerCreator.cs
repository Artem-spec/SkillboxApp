using ModueFive.Model;
using ModueFive.TaskWorkers;
using SkillBox.Infrastructure.Builder;

namespace ModueFive.Builder
{
    internal class ModuleFiveTaskWorkerCreator : TaskWorkerCreator<TypeTask>
    {
        public override TaskWork CreateWorkerForTask(TypeTask type)
        {
            switch (type)
            {
                case TypeTask.TaskOne: return new ModuleFour.TaskWorkers.TaskThreeWorker();
                case TypeTask.TaskTwo: return new TaskTwoWorker();
                case TypeTask.TaskThree: return new TaskThreeWorker();
                case TypeTask.TaskFoure: return new TaskFoureWorker();
                case TypeTask.TaskFive: return new TaskFiveWorker();
                default: return null;
            }
        }
    }
}
