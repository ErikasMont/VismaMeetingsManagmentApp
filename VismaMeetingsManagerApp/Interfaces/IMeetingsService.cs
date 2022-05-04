using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaMeetingsManagerApp.Models;

namespace VismaMeetingsManagerApp.Interfaces
{
    public interface IMeetingsService
    {
        string CreateMeeting(Meeting meeting);
        List<Meeting> GetAllMeetings();
        string DeleteMeeting(Person person, string name);
        string AddAttendee(Person person, string name);
        List<Person> GetAttendees(string name);
        string RemoveAttendee(Person person, string name);
        List<Meeting> GetMeetingsByFilter(Predicate<Meeting> lambda);
    }
}
