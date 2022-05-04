using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaMeetingsManagerApp.Models;

namespace VismaMeetingsManagerApp.Interfaces
{
    public interface IMeetingsRepository
    {
        void Create(Meeting meeting);
        List<Meeting> Read();
        void Delete(Meeting meeting);
        void AddAttendee(Meeting meeting, Person person);
        void RemoveAttendee(Meeting meeting, Person person);
    }
}
