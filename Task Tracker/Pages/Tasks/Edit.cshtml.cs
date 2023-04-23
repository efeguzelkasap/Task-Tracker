using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Task_Tracker.Pages.Tasks
{
    public class EditModel : PageModel
    {

        public TaskInfo taskInfo = new TaskInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {

            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql = "SELECT * FROM tasks WHERE id=@id";

                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                taskInfo.id = "" + reader.GetInt32(0);
                                taskInfo.project = reader.GetString(1);
                                taskInfo.title = reader.GetString(2);
                                taskInfo.issueType = reader.GetString(3);
                                taskInfo.description = reader.GetString(4);
                                taskInfo.createdAt = reader.GetDateTime(5).ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

        }

        public void OnPost()
        {
            taskInfo.id = Request.Form["id"];
            taskInfo.project = Request.Form["project"];
            taskInfo.title = Request.Form["title"];
            taskInfo.issueType = Request.Form["issueType"];
            taskInfo.description = Request.Form["description"];

            if (taskInfo.id.Length == 0 || taskInfo.project.Length == 0 || taskInfo.title.Length == 0 ||
                taskInfo.issueType.Length == 0 || taskInfo.description.Length == 0)
            {
                errorMessage = "All The Fields Are Required";
                return;
            }

            try
            {

                String connectionString = "Data Source=.\\sqlexpress;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql = "UPDATE tasks " +
                                 "SET project=@project, title=@title, issueType=@issueType, description=@description " +
                                 "WHERE id=@id";

                    using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@project", taskInfo.project);
                        sqlCommand.Parameters.AddWithValue("@title", taskInfo.title);
                        sqlCommand.Parameters.AddWithValue("@issueType", taskInfo.issueType);
                        sqlCommand.Parameters.AddWithValue("@description", taskInfo.description);
                        sqlCommand.Parameters.AddWithValue("@id", taskInfo.id);

                        sqlCommand.ExecuteNonQuery();
                    }

                }

            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
            }

            Response.Redirect("/Index");

        }

    }
}
