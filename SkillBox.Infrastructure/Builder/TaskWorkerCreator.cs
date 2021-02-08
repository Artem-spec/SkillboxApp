using System;

namespace SkillBox.Infrastructure.Builder
{
    public abstract class TaskWorkerCreator<T> where T : Enum
    {
        public abstract TaskWork CreateWorkerForTask(T type);
    }
}
