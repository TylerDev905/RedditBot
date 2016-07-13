using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RedditService
{
    class ConfigReader
    {
        public string Path { get; set; }

        public RedditConfig ConfigReddit { get; set; }

        public EmailConfig ConfigEmail { get; set; }

        public List<ScheduledTask> ScheduledTasks { get; set; }

        public enum TaskMode
        {
            DAILY,
            WEEKLY,
            MONTHLY,
            YEARLY
        }

        public enum TaskType
        {
            REDDIT,
            EMAIL
        }

        public class ScheduledTask
        {
            public string Name { get; set; }
            public TaskMode Mode { get; set; }
            public TaskType Type { get; set; }
            public DateTime Time { get; set; }
            public int? IntervalMinutes { get; set; }
            public string Path { get; set; }
            public string[] Args { get; set; }
        }

        public class EmailTask : ScheduledTask
        {
            public string Subject { get; set; }
            public string Body { get; set; }
            public string[] Recipiants { get; set; }
            public new TaskType Type { get; set; } = TaskType.EMAIL;
        }

        public class RedditConfig
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string SubReddit { get; set; }
        }

        public class EmailConfig
        {
            public string Sender { get; set; }
            public string Server { get; set; }
            public int Port { get; set; }
        }

        public ConfigReader(string path)
        {
            Path = path;
        }

        public void LoadConfig()
        {
            var xml = XDocument.Load(Path);

            var redditXml = xml.Root.Element("Config").Element("Reddit");
            ConfigReddit = new RedditConfig()
            {
                Username = redditXml.Element("Username").Value,
                Password = redditXml.Element("Password").Value,
                SubReddit = redditXml.Element("SubReddit").Value
            };

            var emailXml = xml.Root.Element("Config").Element("Email");
            ConfigEmail = new EmailConfig()
            {
                Server = emailXml.Element("Server").Value,
                Sender = emailXml.Element("Sender").Value,
                Port = int.Parse(emailXml.Element("Port").Value),
            };

            ScheduledTasks = new List<ScheduledTask>();
            var tasks = xml.Root.Descendants("Task");
            foreach (var task in tasks)
            {
                var mode = (TaskMode)Enum.Parse(typeof(TaskMode), task.Element("Mode").Value.ToUpper());
                var type = (TaskType)Enum.Parse(typeof(TaskType), task.Element("Type").Value.ToUpper());

                if (type == TaskType.REDDIT && mode == TaskMode.DAILY)
                {
                    ScheduledTasks.Add(new ScheduledTask()
                    {
                        Name = task.Element("Name").Value,
                        Mode = mode,
                        Time = DateTime.Parse(task.Element("Time").Value)
                    });
                }

                if (type == TaskType.EMAIL && mode == TaskMode.DAILY)
                {
                    ScheduledTasks.Add(new EmailTask()
                    {
                        Name = task.Element("Name").Value,
                        Mode = mode,
                        Time = DateTime.Parse(task.Element("Time").Value),
                        Subject = task.Element("Subject").Value,
                        Body = task.Element("Body").Value,
                        Recipiants = task.Element("Recipiants").Elements("Recipiant").Select(x => x.Value).ToArray()
                    });
                }
            }
        }
    }
}
