using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Task_Tracker.Pages.Tasks
{
    public class CreateModel : PageModel
    {

        public TaskInfo taskInfo = new TaskInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            taskInfo.project = Request.Form["project"];
            taskInfo.title = Request.Form["title"];
            taskInfo.issueType = Request.Form["issueType"];
            taskInfo.description = Request.Form["description"];
            
            // Check if any are empty
            if (taskInfo.project.Length == 0 || taskInfo.title.Length == 0 ||
                taskInfo.issueType.Length == 0 || taskInfo.description.Length == 0)
            {
                errorMessage = "All The Fields Are Required";
                return;
            }

            // Save new task into database
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql = "INSERT INTO tasks " +
                                 "(project, title, issueType, description) VALUES " +
                                 "(@project, @title, @issueType, @description);";

                    using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@project", taskInfo.project);
                        sqlCommand.Parameters.AddWithValue("@title", taskInfo.title);
                        sqlCommand.Parameters.AddWithValue("@issueType", taskInfo.issueType);
                        sqlCommand.Parameters.AddWithValue("@description", taskInfo.description);

                        sqlCommand.ExecuteNonQuery();
                    }

                }
            } catch(Exception ex)
            { 
                errorMessage = ex.Message;
                return;
            }

            taskInfo.project = "";
            taskInfo.title = "";
            taskInfo.issueType = "";
            taskInfo.description = "";
            successMessage = "New Task Added Correctly";

            Response.Redirect("/Index");

        }
    }
}
