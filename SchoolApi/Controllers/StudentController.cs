using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace SchoolApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly IMongoCollection<Student> _studentsCollection;

        public StudentController()
        {
            // MongoDB direct connection
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("school");
            _studentsCollection = database.GetCollection<Student>("students");
        }
        // 1. Get all students
        [HttpGet("all")]
        public ActionResult<List<Student>> GetAllStudents()
        {
            var students = _studentsCollection.Find(_ => true).ToList();
            return Ok(students);
        }

        // 2. Add single student
        [HttpPost("add")]
        public ActionResult AddStudent([FromBody] Student student)
        {
            _studentsCollection.InsertOne(student);
            return Ok("Student added successfully");
        }

        // 3. Add bulk students
        [HttpPost("add-bulk")]
        public ActionResult AddBulkStudents([FromBody] List<Student> students)
        {
            _studentsCollection.InsertMany(students);
            return Ok($"{students.Count} students added successfully");
        }

        // 4. Get top 10 students by marks
        [HttpGet("top")]
        public ActionResult<List<Student>> GetTopStudents(int limit = 10)
        {
            var topStudents = _studentsCollection
                .Find(s => s.Marks > 0)
                .SortByDescending(s => s.Marks)
                .Limit(limit)
                .ToList();

            return Ok(topStudents);
        }

        // 5. Get student by ID
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudentById(string id)
        {
            var student = _studentsCollection.Find(s => s.Id == id).FirstOrDefault();
            if (student == null) return NotFound("Student not found");
            return Ok(student);
        }

        // 6. Update student
        [HttpPut("update/{id}")]
        public ActionResult UpdateStudent(string id, [FromBody] Student updatedStudent)
        {
            var result = _studentsCollection.ReplaceOne(s => s.Id == id, updatedStudent);
            if (result.MatchedCount == 0) return NotFound("Student not found");
            return Ok("Student updated successfully");
        }

        // 7. Delete student
        [HttpDelete("delete/{id}")]
        public ActionResult DeleteStudent(string id)
        {
            var result = _studentsCollection.DeleteOne(s => s.Id == id);
            if (result.DeletedCount == 0) return NotFound("Student not found");
            return Ok("Student deleted successfully");
        }
    }
}

