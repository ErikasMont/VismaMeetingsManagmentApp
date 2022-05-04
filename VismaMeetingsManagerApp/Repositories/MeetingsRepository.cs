using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.IO;
using VismaMeetingsManagerApp.Interfaces;
using VismaMeetingsManagerApp.Models;

namespace VismaMeetingsManagerApp.Repositories
{
    public class MeetingsRepository : IMeetingsRepository
    {
        public void Create(Meeting meeting)
        {
            List<Meeting> meetings = ReadFromJson("Meetings.json");
            meetings.Add(meeting);
            WriteToJson(meetings, "Meetings.json");
        }

        public List<Meeting> Read()
        {
            List<Meeting> meetings = ReadFromJson("Meetings.json");
            return meetings;
        }

        public void Delete(Meeting meeting)
        {
            List<Meeting> meetings = ReadFromJson("Meetings.json");
            meetings = meetings.Where(x => x.Name != meeting.Name).ToList();
            WriteToJson(meetings, "Meetings.json");
        }

        public void AddAttendee(Meeting meeting, Person person)
        {
            List<Meeting> meetings = ReadFromJson("Meetings.json");
            int index = meetings.IndexOf(meeting);
            meetings[index].Attendees.Add(person);
            WriteToJson(meetings, "Meetings.json");
        }

        public void RemoveAttendee(Meeting meeting, Person person)
        {
            List<Meeting> meetings = ReadFromJson("Meetings.json");
            int index = meetings.IndexOf(meeting);
            meetings[index].Attendees.Remove(person);
            WriteToJson(meetings, "Meetings.json");
        }

        private List<Meeting> ReadFromJson(string fileName)
        {
            List<Meeting> meetings = new List<Meeting>();
            string jsonString;

            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }

            jsonString = File.ReadAllText(fileName);

            if(jsonString != "")
            {
                meetings = JsonSerializer.Deserialize<List<Meeting>>(jsonString);
            }
            return meetings;
        }

        private void WriteToJson(List<Meeting> meetings, string fileName)
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
            string jsonString = JsonSerializer.Serialize(meetings);
            File.WriteAllText(fileName, jsonString);
        }
    }
}
