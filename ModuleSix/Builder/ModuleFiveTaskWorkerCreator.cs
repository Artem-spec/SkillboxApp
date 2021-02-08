using ModuleSix.Model;
using ModuleSix.TaskWorkers;
using SkillBox.Infrastructure.Builder;

namespace ModuleSix.Builder
{
    internal class ModuleFiveTaskWorkerCreator : TaskWorkerCreator<TypeTask>
    {
        public override TaskWork CreateWorkerForTask(TypeTask type)
        {
            switch (type)
            {
                case TypeTask.TaskOne: return new TaskOneWorker();
                default: return null;
            }
        }
    }
}
