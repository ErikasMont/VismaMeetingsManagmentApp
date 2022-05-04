using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaMeetingsManagerApp.Models
{
    public enum Category { CodeMonkey = 1, Hub, Short, TeamBuilding };
    public enum MeetingType { Live = 1, InPerson };
    public class Meeting
    {
        public string Name { get; set; }
        public Person ResponsiblePerson { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public MeetingType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Person> Attendees { get; set; }

        public Meeting(string name, Person responsiblePerson, string description, Category category,
            MeetingType type, DateTime startDate, DateTime endDate)
        {
            this.Name = name;
            this.ResponsiblePerson = responsiblePerson;
            this.Description = description;
            this.Category = category;
            this.Type = type;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Attendees = new List<Person>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Meeting))
            {
                return false;
            }
            Meeting meeting = obj as Meeting;

            return this.Name.Equals(meeting.Name);
        }

        public override string ToString()
        {
            return string.Format("| {0, -15} | {1, -25} | {2, -29} | {3, -20} | {4, -12} | {5, -8} | {6, -16} | {7, -16} | {8, 20} |", 
                Name, ResponsiblePerson.Name, ResponsiblePerson.Surname, Description, Category.ToString(), Type.ToString(), 
                StartDate.ToString("yyyy/MM/dd HH:mm"), EndDate.ToString("yyyy/MM/dd HH:mm"), Attendees.Count);
        }
    }
}
