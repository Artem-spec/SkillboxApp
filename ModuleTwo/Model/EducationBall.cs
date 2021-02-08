using System.Collections.Generic;

namespace ModuleTwo.Model
{
    public class EducationBall
    {
        public Subjects Subject { get; set; }
        public byte Mark { get; set; }

        public EducationBall(byte mark, Subjects subjects)
        {
            Mark = mark;
            Subject = subjects;
        }
    }

    public enum Subjects
    {
        History,
        Math,
        Rus
    }
}
