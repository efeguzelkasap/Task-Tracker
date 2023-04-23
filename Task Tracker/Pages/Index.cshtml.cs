using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Task_Tracker.Pages
{
      public class IndexModel : PageModel
    {
        public List<TaskInfo> Tasks = new List<TaskInfo>();

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM tasks";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TaskInfo taskInfo = new TaskInfo();
                                taskInfo.id = "" + reader.GetInt32(0);
                                taskInfo.project = reader.GetString(1);
                                taskInfo.title = reader.GetString(2);
                                taskInfo.issueType = reader.GetString(3);
                                taskInfo.description = reader.GetString(4);
                                taskInfo.createdAt = reader.GetDateTime(5).ToString();

                                Tasks.Add(taskInfo);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exeption: " + ex.ToString());
                return;
            }

        }
    }
}

public class TaskInfo
{
    public String id;
    public String project;
    public String title;
    public String issueType;
    public String description;
    public String createdAt;
}


